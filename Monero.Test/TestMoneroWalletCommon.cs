using Monero.Common;
using Monero.Daemon;
using Monero.Test.Utils;
using Monero.Wallet;
using Monero.Wallet.Common;


namespace Monero.Test
{
    public abstract class TestMoneroWalletCommon
    {
        // test constants
        protected static readonly bool LITE_MODE = false;
        protected static readonly bool TEST_NON_RELAYS = true;
        protected static readonly bool TEST_RELAYS = true;
        protected static readonly bool TEST_NOTIFICATIONS = true;
        protected static readonly bool TEST_RESETS = false;
        private static readonly int MAX_TX_PROOFS = 25; // maximum number of transactions to check for each proof, undefined to check all
        private static readonly int SEND_MAX_DIFF = 60;
        private static readonly int SEND_DIVISOR = 10;
        private static readonly int NUM_BLOCKS_LOCKED = 10;

        // instance variables
        protected MoneroWallet wallet = new MoneroWalletRpc("");        // wallet instance to test
        protected MoneroDaemonRpc daemon;     // daemon instance to test

        protected MoneroDaemonRpc GetTestDaemon() { return TestUtils.GetDaemonRpc(); }
        protected abstract MoneroWallet GetTestWallet();
        protected MoneroWallet OpenWallet(string path, string password) { return OpenWallet(new MoneroWalletConfig().SetPath(path).SetPassword(password)); }
        protected abstract MoneroWallet OpenWallet(MoneroWalletConfig config);
        protected abstract MoneroWallet CreateWallet(MoneroWalletConfig config);
        protected void CloseWallet(MoneroWallet wallet) { CloseWallet(wallet, false); }
        protected abstract void CloseWallet(MoneroWallet wallet, bool save);
        protected abstract List<string> GetSeedLanguages();

        [SetUp]
        public void Setup()
        {
        }

        #region Begin Tests

        // Can create a random wallet
        [Test]
        public void TestCreateWalletRandom()
        {
            Assert.That(TEST_NON_RELAYS);
            Exception? e1 = null;  // emulating Java "finally" but compatible with other languages
            try
            {

                // create random wallet
                MoneroWallet wallet = CreateWallet(new MoneroWalletConfig());
                string path = wallet.GetPath();
                Exception e2 = null;
                try
                {
                    MoneroUtils.ValidateAddress(wallet.GetPrimaryAddress(), TestUtils.NETWORK_TYPE);
                    MoneroUtils.ValidatePrivateViewKey(wallet.GetPrivateViewKey());
                    MoneroUtils.ValidatePrivateSpendKey(wallet.GetPrivateSpendKey());
                    MoneroUtils.ValidateMnemonic(wallet.GetSeed());
                    if (wallet.GetWalletType() != MoneroWalletType.RPC) Assert.That(MoneroWallet.DEFAULT_LANGUAGE == wallet.GetSeedLanguage());  // TODO monero-wallet-rpc: get seed language
                }
                catch (Exception e)
                {
                    e2 = e;
                }
                CloseWallet(wallet);
                if (e2 != null) throw e2;

                // attempt to create wallet at same path
                try
                {
                    CreateWallet(new MoneroWalletConfig().SetPath(path));
                    throw new Exception("Should have thrown error");
                }
                catch (Exception e)
                {
                    Assert.That("Wallet already exists: " + path == e.Message);
                }

                // attempt to create wallet with unknown language
                try
                {
                    CreateWallet(new MoneroWalletConfig().SetLanguage("english")); // TODO: support lowercase?
                    throw new Exception("Should have thrown error");
                }
                catch (Exception e)
                {
                    Assert.That("Unknown language: english" == e.Message);
                }
            }
            catch (Exception e)
            {
                e1 = e;
            }

            if (e1 != null) throw new Exception(e1.Message);
        }

        // Can create a wallet from a seed.
        [Test]
        public void TestCreateWalletFromSeed()
        {
            Assert.That(TEST_NON_RELAYS);
            Exception? e1 = null;  // emulating Java "finally" but compatible with other languages
            try
            {

                // save for comparison
                string primaryAddress = this.wallet.GetPrimaryAddress();
                string privateViewKey = this.wallet.GetPrivateViewKey();
                string privateSpendKey = this.wallet.GetPrivateSpendKey();

                // recreate test wallet from seed
                MoneroWallet wallet = CreateWallet(new MoneroWalletConfig().SetSeed(TestUtils.SEED).SetRestoreHeight(TestUtils.FIRST_RECEIVE_HEIGHT));
                string path = wallet.GetPath();
                Exception e2 = null;
                try
                {
                    Assert.That(primaryAddress == wallet.GetPrimaryAddress());
                    Assert.That(privateViewKey == wallet.GetPrivateViewKey());
                    Assert.That(privateSpendKey == wallet.GetPrivateSpendKey());
                    Assert.That(TestUtils.SEED == wallet.GetSeed());
                    if (wallet.GetWalletType() != MoneroWalletType.RPC) Assert.That(MoneroWallet.DEFAULT_LANGUAGE == wallet.GetSeedLanguage());
                }
                catch (Exception e)
                {
                    e2 = e;
                }
                CloseWallet(wallet);
                if (e2 != null) throw e2;

                // attempt to create wallet with two missing words
                try
                {
                    string invalidMnemonic = "memoir desk algebra inbound innocent unplugs fully okay five inflamed giant factual ritual toyed topic snake unhappy guarded tweezers haunted inundate giant";
                    wallet = CreateWallet(new MoneroWalletConfig().SetSeed(invalidMnemonic).SetRestoreHeight(TestUtils.FIRST_RECEIVE_HEIGHT));
                }
                catch (Exception e)
                {
                    Assert.That("Invalid mnemonic" == e.Message);
                }

                // attempt to create wallet at same path
                try
                {
                    CreateWallet(new MoneroWalletConfig().SetPath(path));
                    throw new Exception("Should have thrown error");
                }
                catch (Exception e)
                {
                    Assert.That("Wallet already exists: " + path == e.Message);
                }
            }
            catch (Exception e)
            {
                e1 = e;
            }

            if (e1 != null) throw new Exception(e1.Message);
        }

        // Can create a wallet from a seed with a seed offset
        [Test]
        public void TestCreateWalletFromSeedWithOffset()
        {
            Assert.That(TEST_NON_RELAYS);
            Exception e1 = null;  // emulating Java "finally" but compatible with other languages
            try
            {

                // create test wallet with offset
                MoneroWallet wallet = CreateWallet(new MoneroWalletConfig().SetSeed(TestUtils.SEED).SetRestoreHeight(TestUtils.FIRST_RECEIVE_HEIGHT).SetSeedOffset("my secret offset!"));
                Exception e2 = null;
                try
                {
                    MoneroUtils.ValidateMnemonic(wallet.GetSeed());
                    Assert.That(TestUtils.SEED == wallet.GetSeed());
                    MoneroUtils.ValidateAddress(wallet.GetPrimaryAddress(), TestUtils.NETWORK_TYPE);
                    Assert.That(TestUtils.ADDRESS != wallet.GetPrimaryAddress());
                    if (wallet.GetWalletType() != MoneroWalletType.RPC) Assert.That(MoneroWallet.DEFAULT_LANGUAGE == wallet.GetSeedLanguage());  // TODO monero-wallet-rpc: support
                }
                catch (Exception e)
                {
                    e2 = e;
                }
                CloseWallet(wallet);
                if (e2 != null) throw e2;
            }
            catch (Exception e)
            {
                e1 = e;
            }

            if (e1 != null) throw new Exception(e1.Message);
        }

        // Can create a wallet from keys
        [Test]
        public void TestCreateWalletFromKeys()
        {
            Assert.That(TEST_NON_RELAYS);
            Exception e1 = null; // emulating Java "finally" but compatible with other languages
            try
            {

                // save for comparison
                string primaryAddress = this.wallet.GetPrimaryAddress();
                string privateViewKey = this.wallet.GetPrivateViewKey();
                string privateSpendKey = this.wallet.GetPrivateSpendKey();

                // recreate test wallet from keys
                MoneroWallet wallet = CreateWallet(new MoneroWalletConfig().SetPrimaryAddress(primaryAddress).SetPrivateViewKey(privateViewKey).SetPrivateSpendKey(privateSpendKey).SetRestoreHeight(daemon.GetHeight()));
                string path = wallet.GetPath();
                Exception e2 = null;
                try
                {
                    Assert.That(primaryAddress == wallet.GetPrimaryAddress());
                    Assert.That(privateViewKey == wallet.GetPrivateViewKey());
                    Assert.That(privateSpendKey == wallet.GetPrivateSpendKey());
                    if (!wallet.IsConnectedToDaemon()) MoneroUtils.Log(0, "WARNING: wallet created from keys is not connected to authenticated daemon"); // TODO monero-project: keys wallets not connected
                    Assert.That(wallet.IsConnectedToDaemon(), "Wallet created from keys is not connected to authenticated daemon");
                    if (wallet.GetWalletType() != MoneroWalletType.RPC) {
                        MoneroUtils.ValidateMnemonic(wallet.GetSeed()); // TODO monero-wallet-rpc: cannot get seed from wallet created from keys?
                        Assert.That(MoneroWallet.DEFAULT_LANGUAGE == wallet.GetSeedLanguage());
                    }
                }
                catch (Exception e)
                {
                    e2 = e;
                }
                CloseWallet(wallet);
                if (e2 != null) throw e2;

                // recreate test wallet from spend key
                if (wallet.GetWalletType() != MoneroWalletType.RPC) { // TODO monero-wallet-rpc: cannot create wallet from spend key?
                    wallet = CreateWallet(new MoneroWalletConfig().SetPrivateSpendKey(privateSpendKey).SetRestoreHeight(daemon.GetHeight()));
                    e2 = null;
                    try
                    {
                        Assert.That(primaryAddress == wallet.GetPrimaryAddress());
                        Assert.That(privateViewKey == wallet.GetPrivateViewKey());
                        Assert.That(privateSpendKey == wallet.GetPrivateSpendKey());
                        if (!wallet.IsConnectedToDaemon()) MoneroUtils.Log(0, "WARNING: wallet created from keys is not connected to authenticated daemon"); // TODO monero-project: keys wallets not connected
                        Assert.That(wallet.IsConnectedToDaemon(), "Wallet created from keys is not connected to authenticated daemon");
                        if (wallet.GetWalletType() != MoneroWalletType.RPC) {
                            MoneroUtils.ValidateMnemonic(wallet.GetSeed()); // TODO monero-wallet-rpc: cannot get seed from wallet created from keys?
                            Assert.That(MoneroWallet.DEFAULT_LANGUAGE == wallet.GetSeedLanguage());
                        }
                    }
                    catch (Exception e)
                    {
                        e2 = e;
                    }
                    CloseWallet(wallet);
                    if (e2 != null) throw e2;
                }

                // attempt to create wallet at same path
                try
                {
                    CreateWallet(new MoneroWalletConfig().SetPath(path));
                    throw new Exception("Should have thrown error");
                }
                catch (Exception e)
                {
                    Assert.That("Wallet already exists: " + path == e.Message);
                }
            }
            catch (Exception e)
            {
                e1 = e;
            }

            if (e1 != null) throw new Exception(e1.Message);
        }

        // Can create wallets with subaddress lookahead
        [Test]
        public void TestSubaddressLookahead()
        {
            Assert.That(TEST_NON_RELAYS);
            Exception? e1 = null;  // emulating Java "finally" but compatible with other languages
            MoneroWallet? receiver = null;
            try
            {

                // create wallet with high subaddress lookahead
                receiver = CreateWallet(new MoneroWalletConfig().SetAccountLookahead(1).SetSubaddressLookahead(100000));

                // transfer funds to subaddress with high index
                wallet.CreateTx(new MoneroTxConfig()
                        .SetAccountIndex(0)
                        .AddDestination(receiver.GetSubaddress(0, 85000).GetAddress(), TestUtils.MAX_FEE)
                        .SetRelay(true));

                // observe unconfirmed funds
                Thread.Sleep(1000);
                receiver.Sync();
                Assert.That(receiver.GetBalance().CompareTo(0) > 0);
            }
            catch (Exception e)
            {
                e1 = e;
            }

            if (receiver != null) CloseWallet(receiver);
            if (e1 != null) throw new Exception(e1.Message);
        }

        // Can get the wallet's version
        [Test]
        public void TestGetVersion()
        {
            Assert.That(TEST_NON_RELAYS);
            MoneroVersion version = wallet.GetVersion();
            Assert.That(version.GetNumber() != null);
            Assert.That(version.GetNumber() > 0);
            Assert.That(version.IsRelease() != null);
        }

        // Can get the wallet's path
        [Test]
        public void TestGetPath()
        {
            Assert.That(TEST_NON_RELAYS);

            // create random wallet
            MoneroWallet wallet = CreateWallet(new MoneroWalletConfig());

            // set a random attribute
            string uuid = Guid.NewGuid().ToString();
            wallet.SetAttribute("uuid", uuid);

            // record the wallet's path then save and close
            string path = wallet.GetPath();
            CloseWallet(wallet, true);

            // re-open the wallet using its path
            wallet = OpenWallet(path, null);

            // test the attribute
            Assert.That(uuid == wallet.GetAttribute("uuid"));
            CloseWallet(wallet);
        }

        // Can set the daemon connection
        [Test]
        public void TestSetDaemonConnection()
        {
            // create random wallet with default daemon connection
            MoneroWallet wallet = CreateWallet(new MoneroWalletConfig());
            Assert.That(new MoneroRpcConnection(TestUtils.DAEMON_RPC_URI, TestUtils.DAEMON_RPC_USERNAME, TestUtils.DAEMON_RPC_PASSWORD).Equals(wallet.GetDaemonConnection()));
            Assert.That(wallet.IsConnectedToDaemon()); // uses default localhost connection

            // set empty server uri
            wallet.SetDaemonConnection("");
            Assert.That(null == wallet.GetDaemonConnection());
            Assert.That(false == wallet.IsConnectedToDaemon());

            // set offline server uri
            wallet.SetDaemonConnection(TestUtils.OFFLINE_SERVER_URI);
            Assert.That(new MoneroRpcConnection(TestUtils.OFFLINE_SERVER_URI, "", "").Equals(wallet.GetDaemonConnection()));
            Assert.That(false == wallet.IsConnectedToDaemon());

            // set daemon with wrong credentials
            wallet.SetDaemonConnection(TestUtils.DAEMON_RPC_URI, "wronguser", "wrongpass");
            Assert.That(new MoneroRpcConnection(TestUtils.DAEMON_RPC_URI, "wronguser", "wrongpass").Equals(wallet.GetDaemonConnection()));
            if ("".Equals(TestUtils.DAEMON_RPC_USERNAME) || TestUtils.DAEMON_RPC_USERNAME == null) Assert.That(wallet.IsConnectedToDaemon()); // TODO: monerod without authentication works with bad credentials?
            else Assert.That(false == wallet.IsConnectedToDaemon());

            // set daemon with authentication
            wallet.SetDaemonConnection(TestUtils.DAEMON_RPC_URI, TestUtils.DAEMON_RPC_USERNAME, TestUtils.DAEMON_RPC_PASSWORD);
            Assert.That(new MoneroRpcConnection(TestUtils.DAEMON_RPC_URI, TestUtils.DAEMON_RPC_USERNAME, TestUtils.DAEMON_RPC_PASSWORD).Equals(wallet.GetDaemonConnection()));
            Assert.That(wallet.IsConnectedToDaemon());

            // nullify daemon connection
            wallet.SetDaemonConnection((string)null);
            Assert.That(null == wallet.GetDaemonConnection());
            wallet.SetDaemonConnection(TestUtils.DAEMON_RPC_URI);
            Assert.That(new MoneroRpcConnection(TestUtils.DAEMON_RPC_URI).Equals(wallet.GetDaemonConnection()));
            wallet.SetDaemonConnection((MoneroRpcConnection)null);
            Assert.That(null == wallet.GetDaemonConnection());

            // set daemon uri to non-daemon
            wallet.SetDaemonConnection("www.Getmonero.org");
            Assert.That(new MoneroRpcConnection("www.Getmonero.org").Equals(wallet.GetDaemonConnection()));
            Assert.That(false == wallet.IsConnectedToDaemon());

            // set daemon to invalid uri
            wallet.SetDaemonConnection("abc123");
            Assert.That(false == wallet.IsConnectedToDaemon());

            // attempt to sync
            try
            {
                wallet.Sync();
                throw new Exception("Exception expected");
            }
            catch (MoneroError e)
            {
                Assert.That("Wallet is not connected to daemon" == e.Message);
            }
            finally
            {
                CloseWallet(wallet);
            }
        }

        // Can use a connection manager
        [Test]
        public void TestConnectionManager()
        {

            // create connection manager with monerod connections
            MoneroConnectionManager connectionManager = new MoneroConnectionManager();
            MoneroRpcConnection connection1 = new MoneroRpcConnection(TestUtils.GetDaemonRpc().GetRpcConnection()).SetPriority(1);
            MoneroRpcConnection connection2 = new MoneroRpcConnection("localhost:48081").SetPriority(2);
            connectionManager.SetConnection(connection1);
            connectionManager.AddConnection(connection2);

            // create wallet with connection manager
            MoneroWallet wallet = CreateWallet(new MoneroWalletConfig().SetServerUri("").SetConnectionManager(connectionManager));
            Assert.That(TestUtils.GetDaemonRpc().GetRpcConnection() == wallet.GetDaemonConnection());
            Assert.That(wallet.IsConnectedToDaemon());

            // set manager's connection
            connectionManager.SetConnection(connection2);
            Thread.Sleep(TestUtils.AUTO_CONNECT_TIMEOUT_MS);
            Assert.That(connection2 == wallet.GetDaemonConnection());

            // reopen wallet with connection manager
            string path = wallet.GetPath();
            CloseWallet(wallet);
            wallet = OpenWallet(new MoneroWalletConfig().SetServerUri("").SetConnectionManager(connectionManager).SetPath(path));
            Assert.That(connection2 == wallet.GetDaemonConnection());

            // disconnect
            connectionManager.SetConnection((string)null);
            Assert.That(null == wallet.GetDaemonConnection());
            Assert.That(false == wallet.IsConnectedToDaemon());

            // start polling connections
            connectionManager.StartPolling(TestUtils.SYNC_PERIOD_IN_MS);

            // test that wallet auto connects
            Thread.Sleep(TestUtils.AUTO_CONNECT_TIMEOUT_MS);
            Assert.That(connection1.Equals(wallet.GetDaemonConnection()));
            Assert.That(wallet.IsConnectedToDaemon());

            // test override with bad connection
            wallet.AddListener(new MoneroWalletListener());
            connectionManager.SetAutoSwitch(false);
            connectionManager.SetConnection("http://foo.bar.xyz");
            Assert.That("http://foo.bar.xyz" == wallet.GetDaemonConnection().GetUri());
            Assert.That(wallet.IsConnectedToDaemon() == false);
            Thread.Sleep(5000);
            Assert.That(wallet.IsConnectedToDaemon() == false);

            // set to another connection manager
            MoneroConnectionManager connectionManager2 = new MoneroConnectionManager();
            connectionManager2.SetConnection(connection2);
            wallet.SetConnectionManager(connectionManager2);
            Assert.That(connection2 == wallet.GetDaemonConnection());

            // unset connection manager
            wallet.SetConnectionManager(null);
            Assert.That(null == wallet.GetConnectionManager());
            Assert.That(connection2 == wallet.GetDaemonConnection());

            // stop polling and close
            connectionManager.StopPolling();
            CloseWallet(wallet);
        }

        // Can get the seed
        [Test]
        public void TestGetSeed()
        {
            Assert.That(TEST_NON_RELAYS);
            string seed = wallet.GetSeed();
            MoneroUtils.ValidateMnemonic(seed);
            Assert.That(TestUtils.SEED == seed);
        }

        // Can get the language of the seed
        [Test]
         public void TestGetSeedLanguage()
        {
            Assert.That(TEST_NON_RELAYS);
            string language = wallet.GetSeedLanguage();
            Assert.That(MoneroWallet.DEFAULT_LANGUAGE == language);
        }

        // Can get a list of supported languages for the seed
        [Test]
        public void TestGetSeedLanguages()
        {
            Assert.That(TEST_NON_RELAYS);
            List<string> languages = GetSeedLanguages();
            Assert.That(languages.Count > 0);
            foreach (string language in languages) Assert.That(language.Length > 0);
        }

        // Can get the private view key
        [Test]
        public void TestGetPrivateViewKey()
        {
            Assert.That(TEST_NON_RELAYS);
            string privateViewKey = wallet.GetPrivateViewKey();
            MoneroUtils.ValidatePrivateViewKey(privateViewKey);
        }

        // Can get the private spend key
        [Test]
        public void TestGetPrivateSpendKey()
        {
            Assert.That(TEST_NON_RELAYS);
            string privateSpendKey = wallet.GetPrivateSpendKey();
            MoneroUtils.ValidatePrivateSpendKey(privateSpendKey);
        }

        // Can get the public view key
        [Test]
        public void TestGetPublicViewKey()
        {
            Assert.That(TEST_NON_RELAYS);
            string publicViewKey = wallet.GetPublicViewKey();
            MoneroUtils.ValidatePrivateSpendKey(publicViewKey);
        }

        // Can get the public view key
        [Test]
        public void TestGetPublicSpendKey()
        {
            Assert.That(TEST_NON_RELAYS);
            string publicSpendKey = wallet.GetPublicSpendKey();
            MoneroUtils.ValidatePrivateSpendKey(publicSpendKey);
        }

        // Can get the primary address
        [Test]
        public void TestGetPrimaryAddress()
        {
            Assert.That(TEST_NON_RELAYS);
            string primaryAddress = wallet.GetPrimaryAddress();
            MoneroUtils.ValidateAddress(primaryAddress, TestUtils.NETWORK_TYPE);
            Assert.That(wallet.GetAddress(0, 0) == primaryAddress);
        }

        // Can get the address of a subaddress at a specified account and subaddress index
        [Test]
        public void TestGetSubaddressAddress()
        {
            Assert.That(TEST_NON_RELAYS);
            Assert.That(wallet.GetPrimaryAddress() == (wallet.GetSubaddress(0, 0)).GetAddress());
            foreach (MoneroAccount account in wallet.GetAccounts(true))
            {
                foreach (MoneroSubaddress subaddress in account.GetSubaddresses())
                {
                    Assert.That(subaddress.GetAddress() == wallet.GetAddress((uint)account.GetIndex(), (uint)subaddress.GetIndex()));
                }
            }
        }

        // Can get addresses out of range of used accounts and subaddresses
        [Test]
        public void TestGetSubaddressAddressOutOfRange()
        {
            Assert.That(TEST_NON_RELAYS);
            List<MoneroAccount> accounts = wallet.GetAccounts(true);
            uint accountIdx = (uint)accounts.Count - 1;
            uint subaddressIdx = (uint)accounts[accounts.Count - 1].GetSubaddresses().Count;
            string address = wallet.GetAddress(accountIdx, subaddressIdx);
            Assert.That(address != null);
            Assert.That(address.Length > 0);
        }

        #endregion
    }
}