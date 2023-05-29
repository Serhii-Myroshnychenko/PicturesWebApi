namespace PicturesWebApi.Exceptions;

public class InvalidFileExtensionException : Exception
{
    public InvalidFileExtensionException(string message)
        : base(message)
    {  
    }
}
