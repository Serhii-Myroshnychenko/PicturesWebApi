using PicturesWebApi.Exceptions;

namespace PicturesWebApi.Utils;

public static class PathHandler
{
    public static string GetRootPath()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        while (currentDirectory != null)
        {
            string[] solutionFiles = Directory.GetFiles(currentDirectory, "*.sln");

            if (solutionFiles.Length > 0)
            {
                return currentDirectory;
            }

            currentDirectory = Directory.GetParent(currentDirectory)!.FullName;
        }

        throw new RootPathException("Root path not found.");
    }
}
