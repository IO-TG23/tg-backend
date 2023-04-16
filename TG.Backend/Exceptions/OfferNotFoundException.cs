namespace TG.Backend.Exceptions;

public class OfferNotFoundException : Exception
{
    public Guid Id { get; }
    public OfferNotFoundException(Guid id)
    {
        Id = id;
    }
}