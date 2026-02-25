using FluentValidation.Results;

namespace MF.OrderManagement.Application.Common.Exceptions;

public sealed class ValidationException : ApplicationExceptionBase
{
    public IReadOnlyList<ValidationFailure> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base("Validation failed.")
    {
        Errors = errors.ToList().AsReadOnly();
    }
}