namespace PicturesWebApi.Exceptions;

public class SizeOutOfAllowableLimitException : Exception
{
    public SizeOutOfAllowableLimitException(string message)
        : base(message)
    {
    }
}
