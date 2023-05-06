using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TG.Backend.Features.Client.Command;
using TG.Backend.Models.Client;

namespace TG.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ISender _sender;

        public ClientController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            ClientResponseModel res = await _sender.Send(new GetClientsQuery());

            return res switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.NotFound } => NotFound(new { res.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(res.Clients),
                _ => BadRequest()
            };
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetClient([FromRoute] Guid id)
        {
            ClientResponseModel res = await _sender.Send(new GetClientQuery(new() { Id = id }));

            return res switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.NotFound } => NotFound(new { res.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.OK } => Ok(res.Clients?.FirstOrDefault()),
                _ => BadRequest()
            };
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteClient([FromRoute] Guid id)
        {
            ClientResponseModel res = await _sender.Send(new DeleteClientCommand(new() { Id = id }));

            return res switch
            {
                { IsSuccess: false, StatusCode: HttpStatusCode.NotFound } => NotFound(new { res.Messages }),
                { IsSuccess: true, StatusCode: HttpStatusCode.NoContent } => NoContent(),
                _ => BadRequest()
            };
        }
    }
}
