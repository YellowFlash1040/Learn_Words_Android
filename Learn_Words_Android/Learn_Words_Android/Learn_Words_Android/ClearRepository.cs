//using System;
//using System.Threading.Tasks;
//using Octokit;

//public class ClearRepository
//{
//    private readonly GitHubClient _github;

//    public ClearRepository(string personalAccessToken)
//    {
//        _github = new GitHubClient(new ProductHeaderValue("MyApp"));
//        _github.Credentials = new Credentials(personalAccessToken);
//    }

//    public async Task DeleteFile(string owner, string repo, string path, string commit)
//    {
//        try
//        {
//            await _github.Repository.Content.DeleteFile(owner, repo, path, new DeleteFileRequest("Delete file", commit));
//            Console.WriteLine("File deleted successfully.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Failed to delete file: {ex.Message}");
//        }
//    }
//}