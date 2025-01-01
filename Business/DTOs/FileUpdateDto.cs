using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class FileUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string FileName { get; set; }
}
