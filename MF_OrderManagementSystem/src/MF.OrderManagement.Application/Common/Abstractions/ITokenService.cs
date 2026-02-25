namespace MF.OrderManagement.Application.Common.Abstractions;

public interface ITokenService
{
    string GenerateToken(string username, IEnumerable<string>? roles = null);
}