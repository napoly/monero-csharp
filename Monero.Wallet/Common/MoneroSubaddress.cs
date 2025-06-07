using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Monero.Wallet.Common
{
    public class MoneroSubaddress
    {
        private int _accountIndex;
        private int _index;
        private string _address;
        private string _label;
        private long _balance;
        private long _unlockedBalance;
        private long _numUnspentOutputs;
        private bool _isUsed;
        private long _numBlocksToUnlock;


    }
}
