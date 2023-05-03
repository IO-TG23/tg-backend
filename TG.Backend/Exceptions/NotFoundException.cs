namespace TG.Backend.Exceptions;

public class NotFoundException : Exception
{
    public Guid Id { get; }

    public NotFoundException(Guid id)
    {
        Id = id;
    }
}