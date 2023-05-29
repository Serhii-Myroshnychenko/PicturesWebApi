namespace PicturesWebApi.Contracts;

public interface IImageRepository
{
    Task<string> AddImageAsync(string fileName);
    Task<string> RemoveImageAsync(string id);
    Task<string> GetImageById(string id);
}
