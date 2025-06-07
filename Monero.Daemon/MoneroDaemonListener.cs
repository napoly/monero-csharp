using Monero.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monero.Daemon
{
    public class MoneroDaemonListener
    {
        private MoneroBlockHeader? _lastHeader;

        public void OnBlockHeader(MoneroBlockHeader header)
        {
            _lastHeader = header;
        }

        public MoneroBlockHeader? GetLastHeader() { return _lastHeader; }
    }
}
