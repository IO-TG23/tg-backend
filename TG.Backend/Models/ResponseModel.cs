namespace TG.Backend.Models;

public abstract class ResponseModel
{ 
    public required bool IsSuccess { get; set; }
    public string[]? Messages { get; set; }
    public required HttpStatusCode StatusCode { get; set; }
}