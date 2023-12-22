//using Octokit;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//public class DictionariesStorage
//{
//    private IRepositoryContentsClient contentsClient;
//    private string repoOwner;
//    private string repoName;

//    public DictionariesStorage(string accessToken, string repositoryOwner, string repositoryName)
//    {
//        var github = new GitHubClient(new ProductHeaderValue("YourAppName"));
//        github.Credentials = new Credentials(accessToken);
//        contentsClient = github.Repository.Content;
//        repoOwner = repositoryOwner;
//        repoName = repositoryName;
//    }

//    public async Task<bool> UploadFile(string filePath, string commitMessage)
//    {
//        byte[] fileContents;

//        try
//        {
//            fileContents = File.ReadAllBytes(filePath);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Failed to read file: {ex.Message}");
//            return false;
//        }

//        var encodedContents = Convert.ToBase64String(fileContents);

//        var createResponse = await contentsClient.CreateFile(repoOwner, repoName, filePath, new CreateFileRequest(commitMessage, encodedContents));

//        if (createResponse != null)
//        {
//            // File created successfully
//            return true;
//        }
//        else
//        {
//            // File creation failed
//            return false;
//        }
//    }
//}