using Microsoft.AspNetCore.Mvc;
using PicturesWebApi.Contracts;
using PicturesWebApi.Models.Requests;
using PicturesWebApi.Models.Responses;

namespace PicturesWebApi.Controllers;

[Route("api/images")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IImageService _imageService;

    public ImagesController(IConfiguration configuration, IImageService imageService) =>
        (_configuration, _imageService) = (configuration, imageService);

    [HttpPost("upload-by-url")]
    public async Task<IActionResult> UploadByUrl([FromForm] ImageRequest request)
    {
        var fileName = await _imageService.UploadImageByUrlAsync(request.Url);

        return Ok(new ImageResponse
        {
            Url = $"{Request.Scheme}://{Request.Host.Value}/{_configuration["FolderName"]}/{fileName}"
        });
    }

    [HttpPost("remove/{id}")]
    public async Task<IActionResult> RemoveImageById(string id)
    {
        await _imageService.RemoveImageByIdAsync(id);
        return Ok();
    }

    [HttpPost("get-url/{id}")]
    public async Task<IActionResult> GetUrlById(string id)
    {
        var fileName = await _imageService.GetImageByIdAsync(id);

        return Ok(new ImageResponse
        {
            Url = $"{Request.Scheme}://{Request.Host.Value}/{_configuration["FolderName"]}/{fileName}"
        });
    }

    [HttpPost("get-url/{id}/size/{size}")]
    public async Task<IActionResult> GetResizedImage(string id, string size)
    {
        var fileName = await _imageService.GetResizedImageAsync(id, size);

        return Ok(new ImageResponse
        {
            Url = $"{Request.Scheme}://{Request.Host.Value}/{_configuration["PreviewFolder"]}/{fileName}"
        });
    }
}
