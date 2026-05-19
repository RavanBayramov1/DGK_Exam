namespace ExamSystem.Enums;

public enum ExamStatus
{
    Draft,      // Hazırlanır, heç kim görə bilmir
    Scheduled,  // Vaxt gəlməyib hələ
    Active,     // İndi gedir
    Ended,      // Vaxt bitib, nəticələr işlənir
    Cancelled   // Ləğv edilib
}
