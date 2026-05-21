using ExamSystem.Enums;

namespace ExamSystem.Common;

public static class ErrorMessages
{
    public static class User
    {
        public static Error NotFound => new("User.NotFound", ErrorType.NotFound, "İstifadəçi tapılmadı.");
        public static Error EmailTaken => new("User.EmailTaken", ErrorType.Conflict, "Bu email artıq istifadə olunur.");
        public static Error InvalidCredentials => new("User.InvalidCredentials", ErrorType.Unauthorized, "Email və ya şifrə yanlışdır.");
        public static Error StudentNeedsGroup => new("User.StudentNeedsGroup", ErrorType.Validation, "Student üçün GroupId mütləqdir.");
        public static Error NotStudent => new("User.NotStudent", ErrorType.Validation, "İstifadəçi Student deyil.");
        public static Error NotTeacher => new("User.NotTeacher", ErrorType.Validation, "İstifadəçi Teacher deyil.");
        public static Error AlreadyInGroup => new("User.AlreadyInGroup", ErrorType.Conflict, "İstifadəçi artıq bu qrupdadır.");
        public static Error NotInGroup => new("User.NotInGroup", ErrorType.NotFound, "İstifadəçi bu qrupda deyil.");
    }

    public static class Exam
    {
        public static Error NotFound => new("Exam.NotFound", ErrorType.NotFound, "İmtahan tapılmadı.");
        public static Error NotActive => new("Exam.NotActive", ErrorType.Validation, "İmtahan aktiv deyil.");
        public static Error NotDraft => new("Exam.NotDraft", ErrorType.Validation, "Yalnız Draft statusunda olan imtahan dəyişdirilə bilər.");
        public static Error Unauthorized => new("Exam.Unauthorized", ErrorType.Unauthorized, "Bu imtahana icazəniz yoxdur.");
        public static Error AlreadyStarted => new("Exam.AlreadyStarted", ErrorType.Conflict, "Siz bu imtahana artıq başlamısınız.");
        public static Error AlreadySubmitted => new("Exam.AlreadySubmitted", ErrorType.Conflict, "İmtahan artıq təhvil verilib.");
        public static Error QuestionAlreadyExists => new("Exam.QuestionAlreadyExists", ErrorType.Conflict, "Bu sual artıq imtahana əlavə edilib.");
        public static Error QuestionNotFound => new("Exam.QuestionNotFound", ErrorType.NotFound, "Bu sual imtahanda mövcud deyil.");
    }

    public static class Question
    {
        public static Error NotFound => new("Question.NotFound", ErrorType.NotFound, "Sual tapılmadı.");
        public static Error Unauthorized => new("Question.Unauthorized", ErrorType.Unauthorized, "Bu sual üzərində redaktəyə icazəniz yoxdur.");
    }

    public static class Group
    {
        public static Error NotFound => new("Group.NotFound", ErrorType.NotFound, "Qrup tapılmadı.");
    }

    public static class Subject
    {
        public static Error NotFound => new("Subject.NotFound", ErrorType.NotFound, "Fənn tapılmadı.");
    }

    public static class Result
    {
        public static Error NotFound => new("Result.NotFound", ErrorType.NotFound, "Nəticə tapılmadı.");
    }
}
