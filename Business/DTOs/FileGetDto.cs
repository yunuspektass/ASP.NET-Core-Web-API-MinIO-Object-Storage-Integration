namespace Business.DTOs;

public class FileGetDto
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; }
    public string ObjectName { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}
