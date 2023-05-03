namespace TG.Backend.Data;

public class Blob : Entity
{
    public required string Name { get; set; }
    public required Guid OfferId { get; set; }
}