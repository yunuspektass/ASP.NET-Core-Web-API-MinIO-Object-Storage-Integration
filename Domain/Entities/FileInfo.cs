using System.Data.Common;
using Core.Entities;

namespace Domain.Entities;

public class FileInfo : BaseEntity
{
    public string FileName { get; set; }
    public long Size { get; set; }
    public string ContentType { get; set; }
    public string BucketName { get; set; }
    public string ObjectName { get; set; }

    public FileInfo()
    {
        ObjectName = Id.ToString();
    }
}
