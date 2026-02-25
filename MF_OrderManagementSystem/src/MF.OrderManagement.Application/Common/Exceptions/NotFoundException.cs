namespace MF.OrderManagement.Application.Common.Exceptions;

public sealed class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException(string message) : base(message) { }
}