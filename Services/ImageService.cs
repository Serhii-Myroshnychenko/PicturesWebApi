using PicturesWebApi.Contracts;
using PicturesWebApi.Exceptions;
using PicturesWebApi.Utils;

namespace PicturesWebApi.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IConfiguration _configuration;

    public ImageService(IImageRepository imageRepository, IConfiguration configuration) =>
        (_imageRepository, _configuration) = (imageRepository, configuration);


    public async Task<string> GetImageByIdAsync(string id)
    {
        return await _imageRepository.GetImageById(id);
    }

    public async Task<string> GetResizedImageAsync(string id, string size)
    {
        var fileName = await _imageRepository.GetImageById(id);

        string originalImagePath = Path.Combine(Path.Combine(PathHandler.GetRootPath(), _configuration["FolderName"]!), fileName);

        string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(originalImagePath).ToLower()}";

        string previewImagePath = Path.Combine(Path.Combine(PathHandler.GetRootPath(), _configuration["PreviewFolder"]!), newFileName);

        using var image = Image.Load(originalImagePath);

        var sizeArray = size.Split('x');

        if (sizeArray.Length != 2 || !int.TryParse(sizeArray[0], out int width) || !int.TryParse(sizeArray[1], out int height))
        {
            throw new InvalidFormatException("Invalid format. Use this format: widthxheight");
        }

        image.Mutate(x => x.Resize(width, height));

        image.Save(previewImagePath);

        return newFileName;
    }

    public async Task RemoveImageByIdAsync(string id)
    {
        var name = await _imageRepository.RemoveImageAsync(id);

        var fullPath = Path.Combine(Path.Combine(PathHandler.GetRootPath(), _configuration["FolderName"]!),name);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        else
        {
            throw new NotFoundEntityException("The file with this name was not found");
        }
    }

    public async Task<string> UploadImageByUrlAsync(string url)
    {
        string fileExtension = Path.GetExtension(url).ToLower();

        if (!FileExtensions.Extensions.Contains(fileExtension))
        {
            throw new InvalidFileExtensionException("The file extension is not allowed.");
        }

        string fileName = $"{Guid.NewGuid()}{fileExtension}";

        using HttpClient client = new();

        using HttpResponseMessage response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        using Stream contentStream = await response.Content.ReadAsStreamAsync();

        if (contentStream.Length > int.Parse(_configuration["MaxBytePictureSize"]!))
        {
            throw new SizeOutOfAllowableLimitException("The size of the downloaded image exceeds the maximum allowed size.");
        }

        using FileStream fileStream = new(Path.Combine(Path.Combine(PathHandler.GetRootPath(), _configuration["FolderName"]!, fileName)), FileMode.Create);

        await contentStream.CopyToAsync(fileStream);

        await CreateThumbnail(url, 100, 100);

        await CreateThumbnail(url, 300, 300);

        return await _imageRepository.AddImageAsync(fileName);
        
    }

    private async Task CreateThumbnail(string imageUrl, int width, int height)
    {
        string fileExtension = Path.GetExtension(imageUrl).ToLower();

        if (!FileExtensions.Extensions.Contains(fileExtension))
        {
            throw new InvalidFileExtensionException("The file extension is not allowed.");
        }

        string fileName = $"{Guid.NewGuid()}{fileExtension}";

        using var httpClient = new HttpClient();

        using var response = await httpClient.GetAsync(imageUrl);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();

        using var image = await Image.LoadAsync(stream);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(width, height),
            Mode = ResizeMode.Pad
        }));

        await image.SaveAsync(Path.Combine(Path.Combine(PathHandler.GetRootPath(), _configuration["ThumbnailFolder"]!), fileName));
    }
}
