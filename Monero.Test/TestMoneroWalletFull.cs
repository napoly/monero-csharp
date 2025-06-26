namespace Monero.Test;

public class TestMoneroWalletFull
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var manager = Monero.Wallet.MoneroWalletManager.Instance;
        var walletHandler = manager.CreateWallet("test_wallet_csharp", "", "English", 1);
    }
}
