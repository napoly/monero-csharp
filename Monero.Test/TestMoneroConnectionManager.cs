using Monero.Common;
using Monero.Test.Utils;
using Monero.Wallet;

namespace Monero.Test;

public class TestMoneroConnectionManager
{
    

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestConnectionManager()
    {
        List<MoneroWalletRpc> walletRpcs = [];
        MoneroConnectionManager? connectionManager = null;

        try
        {
            // start monero-wallet-rpc instances as test server connections (can also use monerod servers)
            for (int i = 0; i < 5; i++) walletRpcs.Add(TestUtils.StartWalletRpcProcess());

            // create connection manager
            connectionManager = new MoneroConnectionManager();

            // listen for changes
            ConnectionChangeCollector listener = new();
            connectionManager.AddListener(listener);

            // add prioritized connections
            connectionManager.AddConnection(walletRpcs[4].GetRpcConnection().SetPriority(1));
            connectionManager.AddConnection(walletRpcs[2].GetRpcConnection().SetPriority(2));
            connectionManager.AddConnection(walletRpcs[3].GetRpcConnection().SetPriority(2));
            connectionManager.AddConnection(walletRpcs[0].GetRpcConnection()); // default priority is lowest
            connectionManager.AddConnection(new MoneroRpcConnection(walletRpcs[1].GetRpcConnection().GetUri())); // test unauthenticated

            // test connections and order
            List<MoneroRpcConnection> orderedConnections = connectionManager.GetRpcConnections();
            Assert.That(orderedConnections[0] == walletRpcs[4].GetRpcConnection());
            Assert.That(orderedConnections[1] == walletRpcs[2].GetRpcConnection());
            Assert.That(orderedConnections[2] == walletRpcs[3].GetRpcConnection());
            Assert.That(orderedConnections[3] == walletRpcs[0].GetRpcConnection());
            Assert.That(orderedConnections[4].GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            foreach (MoneroRpcConnection c in orderedConnections) Assert.That(c.IsOnline() != null);

            // test getting connection by uri
            Assert.That(connectionManager.HasConnection(walletRpcs[0].GetRpcConnection().GetUri()));
            Assert.That(connectionManager.GetConnection(walletRpcs[0].GetRpcConnection().GetUri()) == walletRpcs[0].GetRpcConnection());

            // test unknown connection
            int numExpectedChanges = 0;
            connectionManager.SetConnection(orderedConnections[0]);
            Assert.That(null == connectionManager.IsConnected());
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);

            // auto connect to best available connection
            connectionManager.StartPolling((ulong)TestUtils.SYNC_PERIOD_IN_MS);
            Thread.Sleep(TestUtils.AUTO_CONNECT_TIMEOUT_MS);
            Assert.That(connectionManager.IsConnected() == true);
            MoneroRpcConnection? connection = connectionManager.GetConnection();
            Assert.That(connection.IsOnline() == true);
            Assert.That(connection == walletRpcs[4].GetRpcConnection());
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == connection);
            connectionManager.SetAutoSwitch(false);
            connectionManager.StopPolling();
            connectionManager.Disconnect();
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == null);

            // start periodically checking connection without auto switch
            connectionManager.StartPolling((ulong)TestUtils.SYNC_PERIOD_IN_MS, false, null, null, null);

            // connect to best available connection in order of priority and response time
            connection = connectionManager.GetBestAvailableConnection();
            connectionManager.SetConnection(connection);
            Assert.That(connection == walletRpcs[4].GetRpcConnection());
            Assert.That(connection.IsOnline() == true);
            Assert.That(connection.IsAuthenticated() == true);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == connection);

            // test connections and order
            orderedConnections = connectionManager.GetRpcConnections();
            Assert.That(orderedConnections[0] == walletRpcs[4].GetRpcConnection());
            Assert.That(orderedConnections[1] == walletRpcs[2].GetRpcConnection());
            Assert.That(orderedConnections[2] == walletRpcs[3].GetRpcConnection());
            Assert.That(orderedConnections[3] == walletRpcs[0].GetRpcConnection());
            Assert.That(orderedConnections[4].GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            for (int i = 1; i < orderedConnections.Count; i++) Assert.That(orderedConnections[i].IsOnline() == null);

            // shut down prioritized servers
            TestUtils.StopWalletRpcProcess(walletRpcs[2]);
            TestUtils.StopWalletRpcProcess(walletRpcs[3]);
            TestUtils.StopWalletRpcProcess(walletRpcs[4]);
            Thread.Sleep(TestUtils.SYNC_PERIOD_IN_MS + 100); // allow time to poll
            Assert.That(connectionManager.IsConnected() == false);
            Assert.That(connectionManager.GetConnection().IsOnline() == false);
            Assert.That(connectionManager.GetConnection().IsAuthenticated() == null);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == connectionManager.GetConnection());

            // test connection order
            orderedConnections = connectionManager.GetRpcConnections();
            Assert.That(orderedConnections[0] == walletRpcs[4].GetRpcConnection());
            Assert.That(orderedConnections[1] == walletRpcs[0].GetRpcConnection());
            Assert.That(orderedConnections[2].GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            Assert.That(orderedConnections[3] == walletRpcs[2].GetRpcConnection());
            Assert.That(orderedConnections[4] == walletRpcs[3].GetRpcConnection());

            // check all connections
            connectionManager.CheckConnections();

            // test connection order
            orderedConnections = connectionManager.GetRpcConnections();
            Assert.That(orderedConnections[0] == walletRpcs[4].GetRpcConnection());
            Assert.That(orderedConnections[1] == walletRpcs[0].GetRpcConnection());
            Assert.That(orderedConnections[2].GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            Assert.That(orderedConnections[3] == walletRpcs[2].GetRpcConnection());
            Assert.That(orderedConnections[4] == walletRpcs[3].GetRpcConnection());

            // test online and authentication status
            for (int i = 0; i < orderedConnections.Count; i++)
            {
                bool? IsOnline = orderedConnections[i].IsOnline();
                bool? IsAuthenticated = orderedConnections[i].IsAuthenticated();
                if (i == 1 || i == 2) Assert.That(IsOnline == true);
                else Assert.That(IsOnline == false);
                if (i == 1) Assert.That(IsAuthenticated == true);
                else if (i == 2) Assert.That(IsAuthenticated == false);
                else Assert.That(IsAuthenticated == null);
            }

            // test auto switch when disconnected
            connectionManager.SetAutoSwitch(true);
            Thread.Sleep(TestUtils.SYNC_PERIOD_IN_MS + 100);
            Assert.That(connectionManager.IsConnected() == true);
            connection = connectionManager.GetConnection();
            Assert.That(connection.IsOnline() == true);
            Assert.That(connection == walletRpcs[0].GetRpcConnection());
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == connection);

            // test connection order
            orderedConnections = connectionManager.GetRpcConnections();
            Assert.That(orderedConnections[0] == connection);
            Assert.That(orderedConnections[0] == walletRpcs[0].GetRpcConnection());
            Assert.That(orderedConnections[1].GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            Assert.That(orderedConnections[2] == walletRpcs[4].GetRpcConnection());
            Assert.That(orderedConnections[3] == walletRpcs[2].GetRpcConnection());
            Assert.That(orderedConnections[4] == walletRpcs[3].GetRpcConnection());

            // connect to specific endpoint without authentication
            connection = orderedConnections[1];
            Assert.That(connection.IsAuthenticated() == false);
            connectionManager.SetConnection(connection);
            Assert.That(connectionManager.IsConnected() == false);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);

            // connect to specific endpoint with authentication
            orderedConnections[1].SetCredentials("rpc_user", "abc123");
            connectionManager.CheckConnections();
            Assert.That(connectionManager.GetConnection().GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            Assert.That(connection.IsOnline() == true);
            Assert.That(connection.IsAuthenticated() == true);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == connection);

            // test connection order
            orderedConnections = connectionManager.GetRpcConnections();
            Assert.That(orderedConnections[0] == connectionManager.GetConnection());
            Assert.That(orderedConnections[0].GetUri() == walletRpcs[1].GetRpcConnection().GetUri());
            Assert.That(orderedConnections[1] == walletRpcs[0].GetRpcConnection());
            Assert.That(orderedConnections[2] == walletRpcs[4].GetRpcConnection());
            Assert.That(orderedConnections[3] == walletRpcs[2].GetRpcConnection());
            Assert.That(orderedConnections[4] == walletRpcs[3].GetRpcConnection());
            for (int i = 0; i < orderedConnections.Count - 1; i++) Assert.That(i <= 1 ? orderedConnections[i].IsOnline() == true : orderedConnections[i].IsOnline() != true);
            Assert.That(orderedConnections[4].IsOnline() == false);

            // set connection to existing uri
            connectionManager.SetConnection(walletRpcs[0].GetRpcConnection().GetUri());
            Assert.That(connectionManager.IsConnected() == true);
            Assert.That(walletRpcs[0].GetRpcConnection() == connectionManager.GetConnection());
            
            MoneroRpcConnection currentConnection = (MoneroRpcConnection)connectionManager.GetConnection();
            MoneroRpcConnection rpcConnection = new();

            if (rpcConnection.GetType().IsInstanceOfType(currentConnection))
            {
                rpcConnection = (MoneroRpcConnection)currentConnection;
                Assert.That(TestUtils.WALLET_RPC_USERNAME == rpcConnection.GetUsername());
                Assert.That(TestUtils.WALLET_RPC_PASSWORD == rpcConnection.GetPassword());
            }

            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == walletRpcs[0].GetRpcConnection());

            // set connection to new uri
            connectionManager.StopPolling();
            string uri = "http://localhost:49999";
            connectionManager.SetConnection(uri);
            Assert.That(uri == connectionManager.GetConnection().GetUri());
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(uri == listener.ChangedConnections[listener.ChangedConnections.Count - 1].GetUri());

            // set connection to empty string
            connectionManager.SetConnection("");
            Assert.That(null == connectionManager.GetConnection());
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);

            // check all connections and test auto switch
            connectionManager.CheckConnections();
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(connectionManager.IsConnected() == true);

            // remove current connection and test auto switch
            connectionManager.RemoveConnection(connectionManager.GetConnection().GetUri());
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(connectionManager.IsConnected() == false);
            connectionManager.CheckConnections();
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(connectionManager.IsConnected() == true);

            // test polling current connection
            connectionManager.SetConnection(null);
            Assert.That(connectionManager.IsConnected() == false);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            connectionManager.StartPolling((ulong)TestUtils.SYNC_PERIOD_IN_MS, null, null, MoneroConnectionManager.PollType.CURRENT, null);
            Thread.Sleep(TestUtils.AUTO_CONNECT_TIMEOUT_MS);
            Assert.That(connectionManager.IsConnected() == true);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);

            // test polling all connections
            connectionManager.SetConnection(null);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            connectionManager.StartPolling((ulong)TestUtils.SYNC_PERIOD_IN_MS, null, null, MoneroConnectionManager.PollType.ALL, null);
            Thread.Sleep(TestUtils.AUTO_CONNECT_TIMEOUT_MS);
            Assert.That(connectionManager.IsConnected() == true);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);

            // shut down all connections
            connection = connectionManager.GetConnection();
            foreach (MoneroWalletRpc walletRpc in walletRpcs) TestUtils.StopWalletRpcProcess(walletRpc);
            Thread.Sleep(TestUtils.SYNC_PERIOD_IN_MS + 100);
            Assert.That(connection.IsOnline() == false);
            Assert.That(++numExpectedChanges == listener.ChangedConnections.Count);
            Assert.That(listener.ChangedConnections[listener.ChangedConnections.Count - 1] == connection);

            // reset
            connectionManager.Reset();
            Assert.That(0 == connectionManager.GetConnections().Count);
            Assert.That(null == connectionManager.GetConnection());
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception occurred: {ex.Message}");
        }
        finally
        {
            if (connectionManager != null)
            {
                connectionManager.Reset();
            }

            foreach (MoneroWalletRpc wallet in walletRpcs)
            {
                try
                {
                    TestUtils.StopWalletRpcProcess(wallet);
                }
                catch { }
            }
        }
        Assert.Pass();
    }
}

internal class ConnectionChangeCollector : MoneroConnectionManagerListener
{
    public List<MoneroRpcConnection?> ChangedConnections = [];

    public void OnConnectionChanged(MoneroRpcConnection? connection)
    {
        ChangedConnections.Add(connection);
    }
  }