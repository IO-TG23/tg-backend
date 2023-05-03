using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TG.Backend.Features.Offer.CreateOffer;
using TG.Backend.Features.Offer.DeleteOffer;
using TG.Backend.Features.Offer.EditOffer;
using TG.Backend.Features.Offer.GetOfferById;
using TG.Backend.Features.Offer.GetOffers;
using TG.Backend.Models.Offer;

namespace TG.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class OfferController : ControllerBase
{
    private readonly ISender _sender;

    public OfferController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetOffers([FromQuery] GetOffersFilterDTO filter)
    {
        var getOffersResponse = await _sender.Send(new GetOffersQuery(filter));

        if (getOffersResponse is { IsSuccess: true, StatusCode: HttpStatusCode.OK })
            return Ok(getOffersResponse.Offers);

        return StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetOfferById([FromRoute] Guid id)
    {
        var getOfferResponse = await _sender.Send(new GetOfferByIdQuery(id));

        if (getOfferResponse is { IsSuccess: true, StatusCode: HttpStatusCode.OK })
            return Ok(getOfferResponse.Offer);

        return StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Employee,Client")]
    public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDTO createOfferDto)
    {
        var createOfferResponse = await _sender.Send(new CreateOfferCommand(createOfferDto));

        if (createOfferResponse is { IsSuccess: true, StatusCode: HttpStatusCode.NoContent })
            return NoContent();

        return StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Employee,Client")]
    public async Task<IActionResult> DeleteOffer([FromRoute] Guid id)
    {
        var deleteOfferResponse = await _sender.Send(new DeleteOfferCommand(id));

        if (deleteOfferResponse is { IsSuccess: true, StatusCode: HttpStatusCode.NoContent })
            return NoContent();

        return StatusCode(StatusCodes.Status503ServiceUnavailable);
    }

    [HttpPut("{id:Guid}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,Employee,Client")]
    public async Task<IActionResult> EditOffer([FromRoute] Guid id, [FromBody] EditOfferDTO editOfferDto)
    {
        var editOfferResponse = await _sender.Send(new EditOfferCommand(editOfferDto, id));

        if (editOfferResponse is { IsSuccess: true, StatusCode: HttpStatusCode.NoContent })
            return NoContent();

        return StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}