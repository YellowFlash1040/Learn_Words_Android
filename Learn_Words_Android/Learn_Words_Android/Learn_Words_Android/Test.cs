using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Learn_Words_Android
{
    public class GitHubApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public GitHubApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        }

        public async Task UploadFileToRepository(string owner, string repository, string filePath, string branch, string fileAbsolutePath)
        {
            // Получение информации о содержимом репозитория
            string url = $"{_baseUrl}/repos/{owner}/{repository}/contents/{filePath}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            // Парсинг ответа
            var fileInfo = JsonConvert.DeserializeObject<FileContentResponse>(responseContent);
            string sha = fileInfo.sha;
            string fileUrl = fileInfo.url;

            // Чтение содержимого файла
            byte[] fileContentBytes = File.ReadAllBytes(fileAbsolutePath);
            string encodedContent = Convert.ToBase64String(fileContentBytes);

            // Создание содержимого файла
            var contentData = new
            {
                message = "Upload file",
                content = encodedContent,
                sha = sha,
                branch = branch
            };

            // Загрузка файла в репозиторий
            var contentJson = JsonConvert.SerializeObject(contentData);
            var contentString = new StringContent(contentJson, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PutAsync(fileUrl, contentString);
            response.EnsureSuccessStatusCode();
        }

        private class FileContentResponse
        {
            public string sha { get; set; }
            public string url { get; set; }
        }
    }
}