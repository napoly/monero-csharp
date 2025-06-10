
namespace Monero.Common
{
    public interface MoneroConnectionManagerListener
    {
        void OnConnectionChanged(MoneroConnection? connection);
    }
}
