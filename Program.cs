using Microsoft.EntityFrameworkCore;
using PicturesWebApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using PicturesWebApi.Contracts;
using PicturesWebApi.Repositories;
using PicturesWebApi.Services;
using PicturesWebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Environment.CurrentDirectory, app.Configuration["FolderName"]!)),
    RequestPath = "/Images"
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
