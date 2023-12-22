using System.IO;
using System.IO.Compression;
using System.Net;

namespace Learn_Words_Android
{
    public class GitHubStorage
    {
        static string DataFolderPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

        static public void CreateZip_And_SendToGit()
        {
            string zip_FileName = "Мои.zip";

            string folderToZip_Path = DataFolderPath + "/Мои";

            string zip_Path = $"{DataFolderPath}/{zip_FileName}";

            CreateZip(folderToZip_Path, zip_Path);

            //Upload to Git
            GitHubUploader uploader = new GitHubUploader();
            //uploader.UploadFileToGitHub(zip_Path);

            File.Delete(zip_Path);
        }

        static private void CreateZip(string folderPath, string zipPath)
        {
            ZipFile.CreateFromDirectory(folderPath, zipPath, CompressionLevel.Optimal, false);
        }

        static public void DownloadDictionaries()
        {
            string url = "https://github.com/YellowFlash1040/Dictionaries/raw/main/Мои.zip";

            DownloadAndExtract(url, DataFolderPath);
        }

        static private void DownloadAndExtract(string url, string dataFolderPath)
        {
            using (var client = new WebClient())
            {
                string zipPath = $"{dataFolderPath}\\Мои.zip";
                client.DownloadFile(url, zipPath);

                var unpackPath = $"{dataFolderPath}\\Мои";

                if (!Directory.Exists(unpackPath))
                {
                    Directory.CreateDirectory(unpackPath);
                }

                ZipFile.ExtractToDirectory(zipPath, unpackPath);

                File.Delete(zipPath);
            }
        }
    }
}