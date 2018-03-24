using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadManager
{
    public class DownloadWorker: IDownloadWorker
    {
        public async Task DownloadPageAsync(string uriPath)
        {
            var uri = new Uri(uriPath);
            var results = await GetDataAsync(uri);
            await GenerateFileAsync(uri, results);
        }

        private async Task<string> GetDataAsync(Uri uri)
        {
            string result;
            using (var client = new HttpClient())
            {
                result = await client.GetStringAsync(uri);
            }

            return result;
        }

        private async Task<string> GenerateFileAsync(Uri uri, string result)
        {
            var name = uri.Host;
            var fileName = name + ".html";
            fileName = fileName.Replace(Path.AltDirectorySeparatorChar.ToString(), string.Empty)
                .Replace(Path.DirectorySeparatorChar.ToString(), string.Empty);
            var folderPath = ConfigurationManager.AppSettings["FilePath"];
            var filePath = $@"{folderPath}{Path.DirectorySeparatorChar}{fileName}";
            
            File.Create(filePath).Close();

            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(result);
            }

            return filePath;
        }
    }
}
