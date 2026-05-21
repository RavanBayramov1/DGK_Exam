using ExamSystem.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace ExamSystem.Hubs;

[Authorize]
public class ExamHub : Hub
{
    public async Task JoinExam(int examId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"exam{examId}");
    }
    public async Task JoinAsTeacher(int examId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"exam_{examId}_teacher");
    }
    public async Task AnswerSelected(int examId, int questionId, List<string> selectedOptions)
    {
        await Clients.Group($"exam_{examId}_teacher")
            .SendAsync("StudentAsnwered", new
            {
                StudentId = Context.UserIdentifier,
                QuestionId = questionId,
                SelectedOptions = selectedOptions
            });
    }

    public async Task ViewQuestion(int examId,int questionIndex)
    {
        await Clients.Group($"exam_{examId}_teacher")
            .SendAsync("StudentViewingQuestion", new
            {
                StudentId = Context.UserIdentifier,
                QuestionIndex = questionIndex
            });
    }
}
