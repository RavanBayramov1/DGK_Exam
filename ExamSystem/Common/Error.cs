using ExamSystem.Enums;

namespace ExamSystem.Common;

public record Error(string Id, ErrorType Type, string Description)
{
    public static Error NotFound(string description) =>
        new("NotFound", ErrorType.NotFound, description);

    public static Error Validation(string description) =>
        new("Validation", ErrorType.Validation, description);

    public static Error Unauthorized(string description) =>
        new("Unauthorized", ErrorType.Unauthorized, description);

    public static Error Conflict(string description) =>
        new("Conflict", ErrorType.Conflict, description);
}
