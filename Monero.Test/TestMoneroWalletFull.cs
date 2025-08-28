namespace Monero.Test;

public class TestMoneroWalletFull
{
    public TestMoneroWalletFull() { }

    [Fact]
    public void Test1()
    {
        var manager = Monero.Wallet.MoneroWalletManager.Instance;
        var walletHandler = manager.CreateWallet("test_wallet_csharp", "", "English", 1);
    }
}
