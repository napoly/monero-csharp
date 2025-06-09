
namespace Monero.Daemon.Common
{
    public class MoneroDaemonUpdateDownloadResult : MoneroDaemonUpdateCheckResult
    {
        private string downloadPath;

        public MoneroDaemonUpdateDownloadResult(MoneroDaemonUpdateCheckResult checkResult): base(checkResult)
        {
        }

        public string getDownloadPath()
        {
            return downloadPath;
        }

        public void setDownloadPath(string downloadPath)
        {
            this.downloadPath = downloadPath;
        }
    }
}
