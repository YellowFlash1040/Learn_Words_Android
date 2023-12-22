using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class GitHubUploader
{
    private const string repositoryOwner = "YellowFlash1040";
    private const string repositoryName = "Dictionaries";
    private const string accessToken = "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";

    public async Task UploadFileToGitHub(string filePath)
    {
        using (var httpClient = new HttpClient())
        {
            var apiUrl = $"https://api.github.com/repos/{repositoryOwner}/{repositoryName}/contents/{Path.GetFileName(filePath)}";
            var fileContent = Convert.ToBase64String(File.ReadAllBytes(filePath));

            var content = new StringContent($"{{\"message\": \"test upload\", \"content\": \"{fileContent}\"}}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string result = response.StatusCode.ToString();
            }
            else
            {
                string result = response.StatusCode.ToString();
            }
        }
    }
}