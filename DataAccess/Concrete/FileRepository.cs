using FileInfo = Domain.Entities.FileInfo;

namespace DataAccess.Concrete;

public class FileRepository : BaseRepository<FileInfo>
{
    private FileManagementDbContext _db;

    public FileRepository(FileManagementDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<FileInfo> GetByObjectNameAsync(string objectName)
    {
        return await FirstOrDefault(f => f.ObjectName == objectName);
    }

}
