using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monero.Common
{
    public class MoneroCheck
    {
        protected bool _isGood;
    
        public MoneroCheck(bool isGood)
        {
            _isGood = isGood;
        }

        public bool IsGood() { return _isGood; }

        public MoneroCheck SetIsGood(bool isGood)
        {
            _isGood = isGood;
            return this;
        }
    }
}
