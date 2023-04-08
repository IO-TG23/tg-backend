using System.Net;

namespace TG.Backend.Models.Auth
{
    public class AuthResponseModel
    {
        public bool IsSuccess { get; set; }
        public string[] Messages { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}
