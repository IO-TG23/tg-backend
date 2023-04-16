namespace TG.Backend.Models.Offer;

public class GetOffersFilterDTO
{
    public string? Gearbox { get; set; }
    public string? Drive { get; set; }
    public decimal? PriceLow { get; set; }
    public decimal? PriceHigh { get; set; }
}