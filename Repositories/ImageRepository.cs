using Microsoft.EntityFrameworkCore;
using PicturesWebApi.Contracts;
using PicturesWebApi.Data;
using PicturesWebApi.Exceptions;
using Image = PicturesWebApi.Models.Image;

namespace PicturesWebApi.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _context;

    public ImageRepository(ApplicationDbContext context) =>
        _context = context;

    public async Task<string> AddImageAsync(string fileName)
    {
        await _context.AddAsync(new Image { Name = fileName, Id = Guid.NewGuid().ToString() });
        await _context.SaveChangesAsync();

        return fileName;
    }

    public async Task<string> GetImageById(string id)
    {
        var image = await _context.Images
            .FirstOrDefaultAsync(i => i.Id == id) ?? throw new NotFoundEntityException($"An entity with {id} was not found ");
        return image.Name;
    }

    public async Task<string> RemoveImageAsync(string id)
    {
        var image = await _context.Images
            .FirstOrDefaultAsync(i => i.Id == id) ?? throw new NotFoundEntityException($"An entity with {id} was not found ");

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();

        return image.Name;
    }
}
