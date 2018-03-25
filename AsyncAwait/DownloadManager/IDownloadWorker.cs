using System.Threading;
using System.Threading.Tasks;

namespace DownloadManager
{
    public interface IDownloadWorker
    {
        Task DownloadPageAsync(string uriPath, CancellationToken ct);
    }
}
