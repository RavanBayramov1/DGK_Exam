namespace ExamSystem.Extensions;

public static class FileExtension
{
    public static bool IsValidType(this IFormFile file, string type) => file.ContentType.StartsWith(type);
    public static bool IsValidSizeMb(this IFormFile file , int mb) => file.Length <= mb *1024 * 1024;
}
