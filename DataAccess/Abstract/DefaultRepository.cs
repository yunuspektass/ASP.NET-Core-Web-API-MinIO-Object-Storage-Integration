using Core.Entities;
using DataAccess.Concrete;

namespace DataAccess.Abstract;

public class DefaultRepository<T> : BaseRepository<T> where T : BaseEntity
{
    public DefaultRepository(FileManagementDbContext context) : base(context)
    {

    }

}
