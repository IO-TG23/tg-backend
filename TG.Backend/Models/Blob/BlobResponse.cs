using Microsoft.AspNetCore.Mvc;

namespace TG.Backend.Models.Blob;

public class BlobResponse : ResponseModel
{
    public FileContentResult? Blob { get; set; }
}