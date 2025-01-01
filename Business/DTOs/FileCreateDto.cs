using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class FileCreateDto
{
    [Required]
    public string FileName { get; set; }
}
