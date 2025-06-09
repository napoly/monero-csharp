
namespace Monero.Common
{
    public class MoneroLwsConnection : MoneroConnection
    {
        public override bool CheckConnection(long timeoutMs)
        {
            throw new NotImplementedException();
        }

        public override MoneroConnection Clone()
        {
            throw new NotImplementedException();
        }

        public override bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}
