using System.ComponentModel.DataAnnotations;

namespace ExamSystem.DTOs.GroupDtos;

public class UpdateGroupDto
{
    [Required(ErrorMessage = "Qrup adı boş ola bilməz.")]
    [MinLength(2, ErrorMessage = "Qrup adı minimum 2 simvol olmalıdır.")]
    public string Name { get; set; } = string.Empty;
}
