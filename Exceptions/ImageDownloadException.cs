namespace PicturesWebApi.Exceptions;

public class ImageDownloadException : Exception
{
    public ImageDownloadException(string message)
        : base(message)
    {
    }
}
