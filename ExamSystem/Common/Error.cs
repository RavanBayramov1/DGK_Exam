using ExamSystem.Enums;

namespace ExamSystem.Common;

public record Error(string Id, ErrorType Type, string Description);
