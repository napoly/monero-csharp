
namespace Monero.Daemon.Common
{
    public class MoneroDaemonUpdateDownloadResult : MoneroDaemonUpdateCheckResult
    {
        private string downloadPath;

        public MoneroDaemonUpdateDownloadResult(MoneroDaemonUpdateCheckResult checkResult): base(checkResult)
        {
        }

        public string GetDownloadPath()
        {
            return downloadPath;
        }

        public void SetDownloadPath(string downloadPath)
        {
            this.downloadPath = downloadPath;
        }
    }
}
