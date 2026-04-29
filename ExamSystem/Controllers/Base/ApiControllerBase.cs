using ExamSystem.Common;
using ExamSystem.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.Controllers.Base;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleFailure(ServiceResult result)
    {
        if (result.Error == null)
            return StatusCode(500, "Naməlum xəta baş verdi!");

        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(result.Error.Description),
            ErrorType.Validation => BadRequest(result.Error.Description),
            ErrorType.Unauthorized => Unauthorized(result.Error.Description),
            ErrorType.Conflict => Conflict(result.Error.Description),
            _ => StatusCode(500, result.Error.Description)
        };
    }
}
