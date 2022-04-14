using Microsoft.AspNetCore.Http;

namespace ServiceCenter.WebFramework.API.Dtos.Files;

public class FileDto
{
    public IFormFile File { get; set; }
}

