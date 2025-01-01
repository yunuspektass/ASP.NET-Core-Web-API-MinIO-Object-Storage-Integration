using Core.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using FileInfo = Domain.Entities.FileInfo;

namespace DataAccess;

public class FileManagementDbContext : DbContext
{
    public FileManagementDbContext(DbContextOptions<FileManagementDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddGlobalFilter();
    }

 public DbSet<FileInfo> Files { get; set; }
}
