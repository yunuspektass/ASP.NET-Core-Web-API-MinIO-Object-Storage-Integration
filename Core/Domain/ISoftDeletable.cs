namespace Core.Domain;

public interface ISoftDeletable
{
    public bool Deleted { get; set; }
}
