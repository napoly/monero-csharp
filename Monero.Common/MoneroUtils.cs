using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monero.Common
{
    public static class MoneroUtils
    {
        public static bool WalletExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            return File.Exists(path);
        }
    }
}
