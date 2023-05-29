namespace PicturesWebApi.Exceptions;

public class RootPathException : Exception
{
    public RootPathException(string message) 
        : base(message)
    {
    }
}
