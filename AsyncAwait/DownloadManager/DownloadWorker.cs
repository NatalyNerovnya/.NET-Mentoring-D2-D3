using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadManager
{
    public class DownloadWorker: IDownloadWorker
    {
        public async Task DownloadPageAsync(string uriPath, CancellationToken ct)
        {
            var uri = new Uri(uriPath);
            var results = await GetDataAsync(uri, ct);
            await GenerateFileAsync(uri, results);
        }

        private async Task<string> GetDataAsync(Uri uri, CancellationToken ct)
        {
            HttpResponseMessage response;
            await Task.Delay(5000, ct); //To make this process longer
            using (var client = new HttpClient())
            {
                response = await client.GetAsync(uri, ct);
            }
            ct.ThrowIfCancellationRequested();

            return await response.Content.ReadAsStringAsync(); 
        }

        private async Task<string> GenerateFileAsync(Uri uri, string result)
        {
            var name = uri.Host;
            var fileName = $"{name}_{Guid.NewGuid()}.html";
            fileName = fileName.Replace(Path.AltDirectorySeparatorChar.ToString(), string.Empty)
                .Replace(Path.DirectorySeparatorChar.ToString(), string.Empty);
            var filePath = CreateFile(fileName);

            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(result);
            }

            return filePath;
        }

        private string CreateFile(string fileName)
        {
            var folderPath = ConfigurationManager.AppSettings["FilePath"];

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = $@"{folderPath}{Path.DirectorySeparatorChar}{fileName}";

            File.Create(filePath).Close();
            return filePath;
        }
    }
}
