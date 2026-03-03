# Sistema de Gestão de Pedidos 

A solução foi construída com foco em **boas práticas de arquitetura, testabilidade, escalabilidade e separação de responsabilidades**, utilizando .NET, SQL Server e RabbitMQ.

---

# Tecnologias Utilizadas

## Back-end
- .NET 8/9
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- RabbitMQ
- FluentValidation
- JWT Authentication
- xUnit + Moq (testes unitários)

## Infraestrutura
- Docker & Docker Compose
- SQL Server (container)
- RabbitMQ (container)

---

# Arquitetura da Solução

A solução segue princípios de Clean Architecture e DDD:
```bash
Orders.Domain          → Entidades e regras de negócio
Orders.Application     → Use cases e validações
Orders.Infrastructure  → EF Core, RabbitMQ, repositórios
Orders.Api             → Controllers e autenticação
Orders.Worker          → Processamento assíncrono
```

---

# Como Executar o Projeto

## 1️⃣ Pré-requisitos
- Docker Desktop
- .NET SDK 8 ou superior

# Acessos

API (Swagger):

```bash
http://localhost:8080/swagger
```

RabbitMQ:

```bash
http://localhost:15672
user: guest
password: guest
```
