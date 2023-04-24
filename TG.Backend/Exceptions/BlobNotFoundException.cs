namespace TG.Backend.Exceptions;

public class BlobNotFoundException : NotFoundException
{
    public BlobNotFoundException(Guid id) : base(id) {}
}