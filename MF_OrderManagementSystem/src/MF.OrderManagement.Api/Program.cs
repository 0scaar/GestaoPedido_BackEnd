using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MF.OrderManagement.Api.DependencyInjection;
using MF.OrderManagement.Api.Middleware;
using MF.OrderManagement.Infrastructure;
using MF.OrderManagement.Observability.Mongo;
using MF.OrderManagement.Observability.Tracing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

const string appName = "orders-api";

// Mongo options
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoDbOptions>>().Value;
    return new MongoClient(options.ConnectionString);
});
builder.Services.AddSingleton<MongoLogChannel>();
builder.Services.AddHostedService<MongoLogBackgroundService>();

// Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    var env = context.HostingEnvironment.EnvironmentName;
    var channel = services.GetRequiredService<MongoLogChannel>();

    configuration
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", appName)
        .Enrich.WithProperty("Environment", env)
        .WriteTo.Console()
        .WriteTo.Sink(new MongoDbLogSink(channel, appName, env));
});

// OpenTelemetry traces
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: appName))
    .WithTracing(tracing =>
    {
        tracing
            .AddSource(TelemetryConstants.ActivitySourceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter();
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

// Swagger + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(scheme.Reference.Id, scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });
});

// JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// CORS opcional si lo necesitas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseCors("AllowAll");

//app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpMetrics();
app.MapMetrics("/metrics");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();