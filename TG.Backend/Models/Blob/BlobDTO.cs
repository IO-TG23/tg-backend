namespace TG.Backend.Models.Blob;

public class BlobDTO
{
    public required Stream Data { get; set; }
    public required string ContentType { get; set; }
}