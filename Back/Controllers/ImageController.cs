using Microsoft.AspNetCore.Mvc;
using Back.Services;
using System.Reflection.Metadata.Ecma335;
using Back.Dtos;


namespace Back.Controllers;

[ApiController]
[Route("images")]
public class ImagesController : ControllerBase
{
    private readonly ImageService imageService;

    public ImagesController(ImageService imageService)
    {
        this.imageService = imageService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] PostImageDTO model)
    {
        var file = model.File;

        if (file == null || file.Length == 0)return BadRequest("Файл не выбран");

        using var stream = file.OpenReadStream();
        var id = await imageService.UploadAsync(file.FileName, file.ContentType, stream);

        return Ok(new { id = id.ToString() });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var stream = await imageService.DownloadAsync(id);
            var fileInfo = await imageService.GetFileInfoAsync(id);
            var contentType = fileInfo.Metadata != null && fileInfo.Metadata.Contains("contentType")
                ? fileInfo.Metadata["contentType"].AsString
                : "application/octet-stream";

            return File(stream, contentType);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}

