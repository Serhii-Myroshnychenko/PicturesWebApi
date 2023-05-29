namespace PicturesWebApi.Contracts;

public interface IImageService
{
    Task<string> UploadImageByUrlAsync(string url);
    Task RemoveImageByIdAsync(string id);
    Task<string> GetImageByIdAsync(string id);
    Task<string> GetResizedImageAsync(string id, string size);
}
