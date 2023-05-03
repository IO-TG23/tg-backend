namespace TG.Backend.Exceptions;

public class OfferNotFoundException : NotFoundException
{
    public OfferNotFoundException(Guid id) : base(id) {}
}