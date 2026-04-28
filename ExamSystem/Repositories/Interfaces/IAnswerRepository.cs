using ExamSystem.Models;

namespace ExamSystem.Repositories.Interfaces;

public interface IAnswerRepository : IGenericRepository<Answer>
{
    Task<List<Answer>> GetByResultIdAsync(int resultId);
}
