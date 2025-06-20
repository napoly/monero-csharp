using Monero.Common;
using Monero.Daemon;
using Monero.Daemon.Common;
using Monero.Test.Utils;
using Monero.Wallet;
using Monero.Wallet.Common;
using System.Collections.ObjectModel;

namespace Monero.Test;

public class TestMoneroDaemonRpc
{
    private static MoneroDaemonRpc daemon;
    private static MoneroWalletRpc wallet;

    private static readonly bool LITE_MODE = false;
    private static readonly bool TEST_NON_RELAYS = true;
    private static readonly bool TEST_RELAYS = true; // creates and relays outgoing txs
    private static readonly bool TEST_NOTIFICATIONS = true;

    private static readonly TestContext BINARY_BLOCK_CTX = new();

    [SetUp]
    public void Setup()
    {
        BINARY_BLOCK_CTX.hasHex = false;
        BINARY_BLOCK_CTX.headerIsFull = false;
        BINARY_BLOCK_CTX.hasTxs = true;
        BINARY_BLOCK_CTX.txContext = new();
        BINARY_BLOCK_CTX.txContext.isPruned = false;
        BINARY_BLOCK_CTX.txContext.isConfirmed = true;
        BINARY_BLOCK_CTX.txContext.fromGetTxPool = false;
        BINARY_BLOCK_CTX.txContext.hasOutputIndices = false;
        BINARY_BLOCK_CTX.txContext.fromBinaryBlock = true;
        daemon = TestUtils.GetDaemonRpc();
        wallet = TestUtils.GetWalletRpc();
        TestUtils.WALLET_TX_TRACKER.Reset();
    }

    #region Non Relays Tests

    [Test]
    public void TestGetVersion()
    {
        Assert.That(TEST_NON_RELAYS == true);
        MoneroVersion version = daemon.GetVersion();
        Assert.That(version.GetNumber() != null);
        Assert.That(version.GetNumber() > 0);
        Assert.That(version.IsRelease() != null);
    }

    [Test]
    public void TestIsTrusted()
    {
        Assert.That(TEST_NON_RELAYS);
        daemon.IsTrusted();
    }

    // Can get the blockchain height
    [Test]
    public void TestGetHeight()
    {
        Assert.That(TEST_NON_RELAYS);
        ulong height = daemon.GetHeight();
        Assert.That(height > 0, "Height must be greater than 0");
    }

    // Can get a block hash by height
    [Test]
    public void TestGetBlockIdByHeight()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroBlockHeader lastHeader = daemon.GetLastBlockHeader();
        string hash = daemon.GetBlockHash((ulong)lastHeader.GetHeight());
        Assert.That(hash != null);
        Assert.That(64 == hash.Length);
    }

    // Can get a block template
    [Test]
    public void TestGetBlockTemplate()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroBlockTemplate template = daemon.GetBlockTemplate(TestUtils.ADDRESS, 2);
        TestBlockTemplate(template);
    }

    // Can get the last block's header
    [Test]
    public void TestGetLastBlockHeader()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroBlockHeader lastHeader = daemon.GetLastBlockHeader();
        TestBlockHeader(lastHeader, true);
    }

    // Can get a block header by hash
    [Test]
    public void TestGetBlockHeaderByHash()
    {
        Assert.That(TEST_NON_RELAYS);

        // retrieve by hash of last block
        MoneroBlockHeader lastHeader = daemon.GetLastBlockHeader();
        string hash = daemon.GetBlockHash((ulong)lastHeader.GetHeight());
        MoneroBlockHeader header = daemon.GetBlockHeaderByHash(hash);
        TestBlockHeader(header, true);
        Assert.That(lastHeader == header);

        // retrieve by hash of previous to last block
        hash = daemon.GetBlockHash((ulong)lastHeader.GetHeight() - 1);
        header = daemon.GetBlockHeaderByHash(hash);
        TestBlockHeader(header, true);
        Assert.That(lastHeader.GetHeight() - 1 == (ulong)header.GetHeight());
    }

    // Can get a block header by height
    [Test]
    public void TestGetBlockHeaderByHeight()
    {
        Assert.That(TEST_NON_RELAYS);

        // retrieve by height of last block
        MoneroBlockHeader lastHeader = daemon.GetLastBlockHeader();
        MoneroBlockHeader header = daemon.GetBlockHeaderByHeight((ulong)lastHeader.GetHeight());
        TestBlockHeader(header, true);
        Assert.That(lastHeader == header);

        // retrieve by height of previous to last block
        header = daemon.GetBlockHeaderByHeight((ulong)lastHeader.GetHeight() - 1);
        TestBlockHeader(header, true);
        Assert.That(lastHeader.GetHeight() - 1 == (ulong)header.GetHeight());
    }

    // Can get block headers by range
    // TODO: test start with no end, vice versa, inclusivity
    [Test]
    public void TestGetBlockHeadersByRange()
    {
        Assert.That(TEST_NON_RELAYS);

        // determine start and end height based on number of blocks and how many blocks ago
        ulong numBlocks = 100;
        ulong numBlocksAgo = 100;
        ulong currentHeight = daemon.GetHeight();
        ulong startHeight = currentHeight - numBlocksAgo;
        ulong endHeight = currentHeight - (numBlocksAgo - numBlocks) - 1;

        // fetch headers
        List<MoneroBlockHeader> headers = daemon.GetBlockHeadersByRange(startHeight, endHeight);

        // test headers
        Assert.That(numBlocks == (ulong)headers.Count);
        int j = 0;
        for (ulong i = 0; i < numBlocks; i++)
        {
            MoneroBlockHeader header = headers[j];
            Assert.That(startHeight + i == (ulong)header.GetHeight());
            TestBlockHeader(header, true);
            j++;
        }
    }

    // Can get a block by hash
    [Test]
    public void TestGetBlockByHash()
    {
        Assert.That(TEST_NON_RELAYS);

        // test config
        TestContext ctx = new TestContext();
        ctx.hasHex = true;
        ctx.hasTxs = false;
        ctx.headerIsFull = true;

        // retrieve by hash of last block
        MoneroBlockHeader lastHeader = daemon.GetLastBlockHeader();
        string hash = daemon.GetBlockHash((ulong)lastHeader.GetHeight());
        MoneroBlock block = daemon.GetBlockByHash(hash);
        TestBlock(block, ctx);
        Assert.That(daemon.GetBlockByHeight((ulong)block.GetHeight()) == block);
        Assert.That(null == block.GetTxs());

        // retrieve by hash of previous to last block
        hash = daemon.GetBlockHash((ulong)lastHeader.GetHeight() - 1);
        block = daemon.GetBlockByHash(hash);
        TestBlock(block, ctx);
        Assert.That(daemon.GetBlockByHeight((ulong)lastHeader.GetHeight() - 1) == block);
        Assert.That(null == block.GetTxs());
    }

    // Can get blocks by hash which includes transactions (binary)
    [Test]
    public void TestGetBlocksByHashBinary()
    {
        Assert.That(TEST_NON_RELAYS);
        throw new MoneroError("Not implemented");
    }

    // Can get a block by height
    [Test]
    public void TestGetBlockByHeight()
    {
        Assert.That(TEST_NON_RELAYS);

        // config for testing blocks
        TestContext ctx = new TestContext();
        ctx.hasHex = true;
        ctx.headerIsFull = true;
        ctx.hasTxs = false;

        // retrieve by height of last block
        MoneroBlockHeader lastHeader = daemon.GetLastBlockHeader();
        MoneroBlock block = daemon.GetBlockByHeight((ulong)lastHeader.GetHeight());
        TestBlock(block, ctx);
        Assert.That(daemon.GetBlockByHeight((ulong)block.GetHeight()) == block);

        // retrieve by height of previous to last block
        block = daemon.GetBlockByHeight((ulong)lastHeader.GetHeight() - 1);
        TestBlock(block, ctx);
        Assert.That(lastHeader.GetHeight() - 1 == (ulong)block.GetHeight());
    }

    // Can get blocks by height which includes transactions (binary)
    [Test]
    public void TestGetBlocksByHeightBinary()
    {
        Assert.That(TEST_NON_RELAYS);

        // set number of blocks to test
        int numBlocks = 100;

        // select random heights  // TODO: this is horribly inefficient way of computing last 100 blocks if not shuffling
        ulong currentHeight = daemon.GetHeight();
        List<ulong> allHeights = new List<ulong>();
        for (ulong i = 0; i < currentHeight - 1; i++) allHeights.Add(i);
        //GenUtils.shuffle(allHeights);
        List<ulong> heights = new List<ulong>();
        for (int i = allHeights.Count - numBlocks; i < allHeights.Count; i++) heights.Add(allHeights[i]);

        // fetch blocks
        List<MoneroBlock> blocks = daemon.GetBlocksByHeight(heights);

        // test blocks
        bool txFound = false;
        Assert.That(numBlocks == blocks.Count);
        for (int i = 0; i < heights.Count; i++)
        {
            MoneroBlock block = blocks[i];
            if (block.GetTxs().Count > 0) txFound = true;
            TestBlock(block, BINARY_BLOCK_CTX);
            Assert.That(block.GetHeight() == heights[i]);
        }
        Assert.That(txFound, "No transactions found to test");
    }

    // Can get blocks by range in a single request
    [Test]
    public void TestGetBlocksByRange()
    {
        Assert.That(TEST_NON_RELAYS);

        // get height range
        ulong numBlocks = 100;
        ulong numBlocksAgo = 190;
        Assert.That(numBlocks > 0);
        Assert.That(numBlocksAgo >= numBlocks);
        ulong height = daemon.GetHeight();
        Assert.That(height - numBlocksAgo + numBlocks - 1 < height);
        ulong startHeight = height - numBlocksAgo;
        ulong endHeight = height - numBlocksAgo + numBlocks - 1;

        // test known start and end heights
        TestGetBlocksRange(startHeight, endHeight, height, false);

        // test unspecified start
        TestGetBlocksRange(null, numBlocks - 1, height, false);

        // test unspecified end
        TestGetBlocksRange(height - numBlocks - 1, null, height, false);
    }

    // Can get blocks by range using chunked requests
    [Test]
    public void TestGetBlocksByRangeChunked()
    {
        Assert.That(TEST_NON_RELAYS && !LITE_MODE);

        // get ulong height range
        ulong numBlocks = Math.Min(daemon.GetHeight() - 2, 1440); // test up to ~2 days of blocks
        Assert.That(numBlocks > 0);
        ulong height = daemon.GetHeight();
        Assert.That(height - numBlocks - 1 < height);
        ulong startHeight = height - numBlocks;
        ulong endHeight = height - 1;

        // test known start and end heights
        TestGetBlocksRange(startHeight, endHeight, height, true);

        // test unspecified start
        TestGetBlocksRange(null, numBlocks - 1, height, true);

        // test unspecified end
        TestGetBlocksRange(endHeight - numBlocks - 1, null, height, true);
    }

    // Can get block hashes (binary)
    [Test]
    public void TestGetBlockIdsBinary()
    {
        Assert.That(TEST_NON_RELAYS);
        //get_hashes.bin
        throw new MoneroError("Not implemented");
    }

    // Can get a transaction by hash with and without pruning
    [Test]
    public void TestGetTxByHash()
    {
        Assert.That(TEST_NON_RELAYS);

        // fetch transaction hashes to test
        List<string> txHashes = GetConfirmedTxHashes(daemon);

        // context for testing txs
        TestContext ctx = new TestContext();
        ctx.isPruned = false;
        ctx.isConfirmed = true;
        ctx.fromGetTxPool = false;

        // fetch each tx by hash without pruning
        foreach (string txHash in txHashes)
        {
            MoneroTx tx = daemon.GetTx(txHash);
            TestTx(tx, ctx);
        }

        // fetch each tx by hash with pruning
        foreach (string txHash in txHashes)
        {
            MoneroTx tx = daemon.GetTx(txHash, true);
            ctx.isPruned = true;
            TestTx(tx, ctx);
        }

        // fetch invalid hash
        try
        {
            daemon.GetTx("invalid tx hash");
            throw new MoneroError("fail");
        }
        catch (MoneroError e)
        {
            Assert.That("Invalid transaction hash" == e.Message);
        }
    }

    // Can get transactions by hashes with and without pruning
    [Test]
    public void TestGetTxsByHashes()
    {
        Assert.That(TEST_NON_RELAYS);

        // fetch transaction hashes to test
        List<string> txHashes = GetConfirmedTxHashes(daemon);
        Assert.That(txHashes.Count > 0);

        // context for testing txs
        TestContext ctx = new TestContext();
        ctx.isPruned = false;
        ctx.isConfirmed = true;
        ctx.fromGetTxPool = false;

        // fetch txs by hash without pruning
        List<MoneroTx> txs = daemon.GetTxs(txHashes);
        Assert.That(txHashes.Count == txs.Count);
        foreach (MoneroTx _tx in txs)
        {
            TestTx(_tx, ctx);
        }

        // fetch txs by hash with pruning
        txs = daemon.GetTxs(txHashes, true);
        ctx.isPruned = true;
        Assert.That(txHashes.Count == txs.Count);
        foreach (MoneroTx _tx in txs)
        {
            TestTx(_tx, ctx);
        }

        // fetch missing hash
        MoneroTx tx = wallet.CreateTx(new MoneroTxConfig().SetAccountIndex(0).AddDestination(wallet.GetPrimaryAddress(), TestUtils.MAX_FEE));
        Assert.That(daemon.GetTx(tx.GetHash()) != null);
        txHashes.Add(tx.GetHash());
        int numTxs = txs.Count;
        txs = daemon.GetTxs(txHashes);
        Assert.That(numTxs == txs.Count);

        // fetch invalid hash
        txHashes.Add("invalid tx hash");
        try
        {
            daemon.GetTxs(txHashes);
            throw new MoneroError("fail");
        }
        catch (MoneroError e)
        {
            Assert.That("Invalid transaction hash" == e.Message);
        }
    }

    // Can get transactions by hashes that are in the transaction pool
    [Test]
    public void TestGetTxsByHashesInPool()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet); // wait for wallet's txs in the pool to clear to ensure reliable sync

        // submit txs to the pool but don't relay
        List<string> txHashes = new List<string>();
        for (uint i = 1; i < 3; i++)
        {
            MoneroTx tx = GetUnrelayedTx(wallet, i);
            MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
            TestSubmitTxResultGood(result);
            Assert.That(result.IsRelayed() == false);
            txHashes.Add(tx.GetHash());
        }

        // fetch txs by hash
        MoneroUtils.Log(0, "Fetching txs...");
        List<MoneroTx> txs = daemon.GetTxs(txHashes);
        MoneroUtils.Log(0, "done");

        // context for testing tx
        TestContext ctx = new TestContext();
        ctx.isPruned = false;
        ctx.isConfirmed = false;
        ctx.fromGetTxPool = false;

        // test fetched txs
        Assert.That(txHashes.Count == txs.Count);
        foreach (MoneroTx tx in txs)
        {
            TestTx(tx, ctx);
        }

        // clear txs from pool
        daemon.FlushTxPool(txHashes);
        wallet.Sync();
    }

    // Can get a transaction hex by hash with and without pruning
    [Test]
    public void TestGetTxHexByHash()
    {
        Assert.That(TEST_NON_RELAYS);

        // fetch transaction hashes to test
        List<string> txHashes = GetConfirmedTxHashes(daemon);

        // fetch each tx hex by hash with and without pruning
        List<string> hexes = new List<string>();
        List<string> hexesPruned = new List<string>();
        foreach (string txHash in txHashes)
        {
            hexes.Add(daemon.GetTxHex(txHash));
            hexesPruned.Add(daemon.GetTxHex(txHash, true));
        }

        // test results
        Assert.That(hexes.Count == txHashes.Count);
        Assert.That(hexesPruned.Count == txHashes.Count);
        for (int i = 0; i < hexes.Count; i++)
        {
            Assert.That(hexes[i] != null);
            Assert.That(hexesPruned[i] != null);
            Assert.That(hexesPruned.Count > 0);
            Assert.That(hexes[i].Length > hexesPruned[i].Length); // pruned hex is shorter
        }

        // fetch invalid hash
        try
        {
            daemon.GetTxHex("invalid tx hash");
            throw new MoneroError("fail");
        }
        catch (MoneroError e)
        {
            Assert.That("Invalid transaction hash" == e.Message);
        }
    }

    // Can get transaction hexes by hashes with and without pruning
    [Test]
    public void TestGetTxHexesByHashes()
    {
        Assert.That(TEST_NON_RELAYS);

        // fetch transaction hashes to test
        List<string> txHashes = GetConfirmedTxHashes(daemon);

        // fetch tx hexes by hash with and without pruning
        List<string> hexes = daemon.GetTxHexes(txHashes);
        List<string> hexesPruned = daemon.GetTxHexes(txHashes, true);

        // test results
        Assert.That(hexes.Count == txHashes.Count);
        Assert.That(hexesPruned.Count == txHashes.Count);
        for (int i = 0; i < hexes.Count; i++)
        {
            Assert.That(hexes[i] != null);
            Assert.That(hexesPruned[i] != null);
            Assert.That(hexesPruned.Count > 0);
            Assert.That(hexes[i].Length > hexesPruned[i].Length); // pruned hex is shorter
        }

        // fetch invalid hash
        txHashes.Add("invalid tx hash");
        try
        {
            daemon.GetTxHexes(txHashes);
            throw new MoneroError("fail");
        }
        catch (MoneroError e)
        {
            Assert.That("Invalid transaction hash" == e.Message);
        }
    }

    // Can get the miner transaction sum
    [Test]
    public void TestGetMinerTxSum()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroMinerTxSum sum = daemon.GetMinerTxSum(0l, Math.Min(50000l, daemon.GetHeight()));
        TestMinerTxSum(sum);
    }

    // Can get a fee estimate
    [Test]
    public void TestGetFeeEstimate()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroFeeEstimate feeEstimate = daemon.GetFeeEstimate();
        //TestUtils.testUnsignedBigInteger(feeEstimate.GetFee(), true);
        Assert.That(feeEstimate.GetFees().Count == 4); // slow, normal, fast, fastest
        //for (int i = 0; i < 4; i++) TestUtils.testUnsignedBigInteger(feeEstimate.GetFees()[i], true);
        //TestUtils.testUnsignedBigInteger(feeEstimate.GetQuantizationMask(), true);
    }

    // Can get all transactions in the transaction pool
    [Test]
    public void TestGetTxsInPool()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);

        // submit tx to pool but don't relay
        MoneroTx tx = GetUnrelayedTx(wallet, 1);
        MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
        TestSubmitTxResultGood(result);
        Assert.That(result.IsRelayed() == false);

        // fetch txs in pool
        List<MoneroTx> txs = daemon.GetTxPool();

        // context for testing tx
        TestContext ctx = new TestContext();
        ctx.isPruned = false;
        ctx.isConfirmed = false;
        ctx.fromGetTxPool = true;

        // test txs
        Assert.That(txs.Count > 0, "Test requires an unconfirmed tx in the tx pool");
        foreach (MoneroTx aTx in txs)
        {
            TestTx(aTx, ctx);
        }

        // flush the tx from the pool, gg
        daemon.FlushTxPool(tx.GetHash());
        wallet.Sync();
    }

    // Can get hashes of transactions in the transaction pool (binary)
    [Test]
    public void TestGetIdsOfTxsInPoolBin()
    {
        Assert.That(TEST_NON_RELAYS);
        // TODO: get_transaction_pool_hashes.bin
        throw new MoneroError("Not implemented");
    }

    // Can get the transaction pool backlog (binary)
    [Test]
    public void TestGetTxPoolBacklogBin()
    {
        Assert.That(TEST_NON_RELAYS);
        // TODO: get_txpool_backlog
        throw new MoneroError("Not implemented");
    }

    // Can get transaction pool statistics
    [Test]
    public void TestGetTxPoolStatistics()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);
        Exception err = null;
        Collection<string> txIds = new();
        try
        {

            // submit txs to the pool but don't relay
            for (uint i = 1; i < 3; i++)
            {

                // submit tx hex
                MoneroTx tx = GetUnrelayedTx(wallet, i);
                MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
                Assert.That(result.IsGood() == true);
                txIds.Add(tx.GetHash());

                // get tx pool stats
                MoneroTxPoolStats stats = daemon.GetTxPoolStats();
                Assert.That(stats.GetNumTxs() > i - 1);
                TestTxPoolStats(stats);
            }
        }
        catch (Exception e)
        {
            err = e;
        }

        // flush txs
        daemon.FlushTxPool(txIds.ToList());
        if (err != null) throw new MoneroError(err.Message);
    }

    // Can flush all transactions from the pool
    [Test]
    public void TestFlushTxsFromPool()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);

        // preserve original transactions in the pool
        List<MoneroTx> txPoolBefore = daemon.GetTxPool();

        // submit txs to the pool but don't relay
        for (uint i = 1; i < 3; i++)
        {
            MoneroTx tx = GetUnrelayedTx(wallet, i);
            MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
            TestSubmitTxResultGood(result);
        }
        Assert.That(txPoolBefore.Count + 2 == daemon.GetTxPool().Count);

        // flush tx pool
        daemon.FlushTxPool();
        Assert.That(0 == daemon.GetTxPool().Count);

        // re-submit original transactions
        foreach (MoneroTx tx in txPoolBefore)
        {
            MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), tx.IsRelayed() == true);
            TestSubmitTxResultGood(result);
        }

        // pool is back to original state
        Assert.That(txPoolBefore.Count == daemon.GetTxPool().Count);

        // sync wallet for next test
        wallet.Sync();
    }

    // Can flush a transaction from the pool by hash
    [Test]
    public void TestFlushTxFromPoolByHash()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);

        // preserve original transactions in the pool
        List<MoneroTx> txPoolBefore = daemon.GetTxPool();

        // submit txs to the pool but don't relay
        List<MoneroTx> txs = [];
        for (uint i = 1; i < 3; i++)
        {
            MoneroTx tx = GetUnrelayedTx(wallet, i);
            MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
            TestSubmitTxResultGood(result);
            txs.Add(tx);
        }

        // remove each tx from the pool by hash and test
        for (int i = 0; i < txs.Count; i++)
        {

            // flush tx from pool
            daemon.FlushTxPool(txs[i].GetHash());

            // test tx pool
            List<MoneroTx> poolTxs = daemon.GetTxPool();
            Assert.That(txs.Count - i - 1 == poolTxs.Count);
        }

        // pool is back to original state
        Assert.That(txPoolBefore.Count == daemon.GetTxPool().Count);

        // sync wallet for next test
        wallet.Sync();
    }

    // Can flush transactions from the pool by hashes
    [Test]
    public void TestFlushTxsFromPoolByHashes()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);

        // preserve original transactions in the pool
        List<MoneroTx> txPoolBefore = daemon.GetTxPool();

        // submit txs to the pool but don't relay
        List<string> txHashes = [];
        for (uint i = 1; i < 3; i++)
        {
            MoneroTx tx = GetUnrelayedTx(wallet, i);
            MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
            TestSubmitTxResultGood(result);
            txHashes.Add(tx.GetHash());
        }
        Assert.That(txPoolBefore.Count + txHashes.Count == daemon.GetTxPool().Count);

        // remove all txs by hashes
        daemon.FlushTxPool(txHashes);

        // pool is back to original state
        Assert.That(txPoolBefore.Count == daemon.GetTxPool().Count);
        wallet.Sync();
    }

    // Can get the spent status of key images
    [Test]
    public void TestGetSpentStatusOfKeyImages()
    {
        Assert.That(TEST_NON_RELAYS);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);

        // submit txs to the pool to collect key images then flush them
        List<MoneroTx> txs = [];
        for (uint i = 1; i < 3; i++)
        {
            MoneroTx tx = GetUnrelayedTx(wallet, i);
            daemon.SubmitTxHex(tx.GetFullHex(), true);
            txs.Add(tx);
        }
        List<string> keyImages = [];
        List<string> txHashes = [];
        foreach (MoneroTx tx in txs) txHashes.Add(tx.GetHash());
        foreach (MoneroTx tx in daemon.GetTxs(txHashes))
        {
            foreach (MoneroOutput input in tx.GetInputs()) keyImages.Add(input.GetKeyImage().GetHex());
        }
        daemon.FlushTxPool(txHashes);

        // key images are not spent
        TestSpentStatuses(keyImages, MoneroKeyImage.SpentStatus.NOT_SPENT);

        // submit txs to the pool but don't relay
        foreach (MoneroTx tx in txs) daemon.SubmitTxHex(tx.GetFullHex(), true);

        // key images are in the tx pool
        TestSpentStatuses(keyImages, MoneroKeyImage.SpentStatus.TX_POOL);

        // collect key images of confirmed txs
        keyImages = [];
        txs = GetConfirmedTxs(daemon, 10);
        foreach (MoneroTx tx in txs)
        {
            foreach (MoneroOutput input in tx.GetInputs()) keyImages.Add(input.GetKeyImage().GetHex());
        }

        // key images are all spent
        TestSpentStatuses(keyImages, MoneroKeyImage.SpentStatus.CONFIRMED);

        // flush this test's txs from pool
        daemon.FlushTxPool(txHashes);
    }

    // Can get output indices given a list of transaction hashes (binary)
    [Test]
    public void TestGetOutputIndicesFromTxIdsBinary()
    {
        Assert.That(TEST_NON_RELAYS);
        throw new Exception("Not implemented"); // get_o_indexes.bin
    }

    // Can get outputs given a list of output amounts and indices (binary)
    [Test]
    public void TestGetOutputsFromAmountsAndIndicesBinary()
    {
        Assert.That(TEST_NON_RELAYS);
        throw new Exception("Not implemented"); // get_outs.bin
    }

    // Can get an output histogram (binary)
    [Test]
    public void TestGetOutputHistogramBinary()
    {
        Assert.That(TEST_NON_RELAYS);
        List<MoneroOutputHistogramEntry> entries = daemon.GetOutputHistogram(null, null, null, null, null);
        Assert.That(entries.Count > 0);
        foreach (MoneroOutputHistogramEntry entry in entries)
        {
            TestOutputHistogramEntry(entry);
        }
    }

    // Can get an output distribution (binary)
    [Test]
    public void TestGetOutputDistributionBinary()
    {
        Assert.That(TEST_NON_RELAYS);
        List<ulong> amounts = [];
        amounts.Add(0);
        amounts.Add(1);
        amounts.Add(10);
        amounts.Add(100);
        amounts.Add(1000);
        amounts.Add(10000);
        amounts.Add(100000);
        amounts.Add(1000000);
        List<MoneroOutputDistributionEntry> entries = daemon.GetOutputDistribution(amounts);
        foreach (MoneroOutputDistributionEntry entry in entries)
        {
            TestOutputDistributionEntry(entry);
        }
    }

    // Can get general information
    [Test]
    public void TestGetGeneralInformation()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroDaemonInfo info = daemon.GetInfo();
        TestInfo(info);
    }

    // Can get sync information
    [Test]
    public void TestGetSyncInformation()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroDaemonSyncInfo syncInfo = daemon.GetSyncInfo();
        TestSyncInfo(syncInfo);
    }

    // Can get hard fork information
    [Test]
    public void TestGetHardForkInformation()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroHardForkInfo hardForkInfo = daemon.GetHardForkInfo();
        TestHardForkInfo(hardForkInfo);
    }

    // Can get alternative chains
    [Test]
    public void TestGetAlternativeChains()
    {
        Assert.That(TEST_NON_RELAYS);
        List<MoneroAltChain> altChains = daemon.GetAltChains();
        foreach (MoneroAltChain altChain in altChains)
        {
            TestAltChain(altChain);
        }
    }

    // Can get alternative block hashes
    [Test]
    public void TestGetAlternativeBlockIds()
    {
        Assert.That(TEST_NON_RELAYS);
        List<string> altBlockIds = daemon.GetAltBlockHashes();
        foreach (string altBlockId in altBlockIds)
        {
            Assert.That(altBlockId != null);
            Assert.That(64 == altBlockId.Length);  // TODO: common validation
        }
    }

    // Can get, set, and reset a download bandwidth limit
    [Test]
  public void TestSetDownloadBandwidth()
    {
        Assert.That(TEST_NON_RELAYS);
        int initVal = daemon.GetDownloadLimit();
        Assert.That(initVal > 0);
        int setVal = initVal * 2;
        daemon.SetDownloadLimit(setVal);
        Assert.That(setVal == daemon.GetDownloadLimit());
        int resetVal = daemon.ResetDownloadLimit();
        Assert.That(initVal == resetVal);

        // test invalid limits
        try
        {
            daemon.SetDownloadLimit(0);
            throw new MoneroError("Should have thrown error on invalid input");
        }
        catch (MoneroError e)
        {
            Assert.That("Download limit must be an integer greater than 0" == e.Message);
        }
        Assert.That(daemon.GetDownloadLimit() == initVal);
    }

    // Can get, set, and reset an upload bandwidth limit
    [Test]
    public void TestSetUploadBandwidth()
    {
        Assert.That(TEST_NON_RELAYS);
        int initVal = daemon.GetUploadLimit();
        Assert.That(initVal > 0);
        int setVal = initVal * 2;
        daemon.SetUploadLimit(setVal);
        Assert.That(setVal == daemon.GetUploadLimit());
        int resetVal = daemon.ResetUploadLimit();
        Assert.That(initVal == resetVal);

        // test invalid limits
        try
        {
            daemon.SetUploadLimit(0);
            throw new Exception("Should have thrown error on invalid input");
        }
        catch (MoneroError e)
        {
            Assert.That("Upload limit must be an integer greater than 0" == e.Message);
        }
        Assert.That(initVal == daemon.GetUploadLimit());
    }

    // Can get peers with active incoming or outgoing connections
    [Test]
    public void TestGetPeers()
    {
        Assert.That(TEST_NON_RELAYS);
        List<MoneroPeer> peers = daemon.GetPeers();
        Assert.That(peers.Count > 0, "Daemon has no incoming or outgoing peers to test");
        foreach (MoneroPeer peer in peers)
        {
            TestPeer(peer);
        }
    }

    // Can get all known peers which may be online or offline
    [Test]
    public void TestGetKnownPeers()
    {
        Assert.That(TEST_NON_RELAYS);
        List<MoneroPeer> peers = daemon.GetKnownPeers();
        Assert.That(peers.Count > 0, "Daemon has no known peers to test");
        foreach (MoneroPeer peer in peers)
        {
            TestKnownPeer(peer, false);
        }
    }

    // Can limit the number of outgoing peers
    [Test]
    public void TestSetOutgoingPeerLimit()
    {
        Assert.That(TEST_NON_RELAYS);
        daemon.SetOutgoingPeerLimit(0);
        daemon.SetOutgoingPeerLimit(8);
        daemon.SetOutgoingPeerLimit(10);
    }

    // Can limit the number of incoming peers
    [Test]
    public void TestSetIncomingPeerLimit()
    {
        Assert.That(TEST_NON_RELAYS);
        daemon.SetIncomingPeerLimit(0);
        daemon.SetIncomingPeerLimit(8);
        daemon.SetIncomingPeerLimit(10);
    }

    // Can ban a peer
    [Test]
    public void TestBanPeer()
    {
        Assert.That(TEST_NON_RELAYS);

        // set ban
        MoneroBan ban = new MoneroBan();
        ban.SetHost("192.168.1.51");
        ban.SetIsBanned(true);
        ban.SetSeconds((long)60);
        daemon.SetPeerBan(ban);

        // test ban
        List<MoneroBan> bans = daemon.GetPeerBans();
        bool found = false;
        foreach (MoneroBan aBan in bans)
        {
            TestMoneroBan(aBan);
            if ("192.168.1.51".Equals(aBan.GetHost())) found = true;
        }
        Assert.That(found);
    }

    // Can ban peers
    [Test]
    public void TestBanPeers()
    {
        Assert.That(TEST_NON_RELAYS);

        // set bans
        MoneroBan ban1 = new MoneroBan();
        ban1.SetHost("192.168.1.52");
        ban1.SetIsBanned(true);
        ban1.SetSeconds((long)60);
        MoneroBan ban2 = new MoneroBan();
        ban2.SetHost("192.168.1.53");
        ban2.SetIsBanned(true);
        ban2.SetSeconds((long)60);
        List<MoneroBan> bans = [];
        bans.Add(ban1);
        bans.Add(ban2);
        daemon.SetPeerBans(bans);

        // test bans
        bans = daemon.GetPeerBans();
        bool found1 = false;
        bool found2 = false;
        foreach (MoneroBan aBan in bans)
        {
            TestMoneroBan(aBan);
            if ("192.168.1.52".Equals(aBan.GetHost())) found1 = true;
            if ("192.168.1.53".Equals(aBan.GetHost())) found2 = true;
        }
        Assert.That(found1);
        Assert.That(found2);
    }

    // Can start and stop mining
    [Test]
    public void TestMining()
    {
        Assert.That(TEST_NON_RELAYS);

        // stop mining at beginning of test
        try { daemon.StopMining(); }
        catch (MoneroError e) { }

        // generate address to mine to
        string address = wallet.GetPrimaryAddress();

        // start mining
        daemon.StartMining(address, 2, false, true);

        // stop mining
        daemon.StopMining();
    }

    // Can get mining status
    [Test]
    public void TestGetMiningStatus()
    {
        Assert.That(TEST_NON_RELAYS);

        try
        {

            // stop mining at beginning of test
            try { daemon.StopMining(); }
            catch (MoneroError e) { }

            // test status without mining
            MoneroMiningStatus status = daemon.GetMiningStatus();
            Assert.That(false == status.IsActive());
            Assert.That(status.GetAddress() == null);
            Assert.That(0 == (long)status.GetSpeed());
            Assert.That(0 == (int)status.GetNumThreads());
            Assert.That(status.IsBackground() == null);

            // test status with mining
            string address = wallet.GetPrimaryAddress();
            ulong threadCount = 3;
            bool isBackground = false;
            daemon.StartMining(address, threadCount, isBackground, true);
            status = daemon.GetMiningStatus();
            Assert.That(true == status.IsActive());
            Assert.That(address == status.GetAddress());
            Assert.That(status.GetSpeed() >= 0);
            Assert.That(threadCount == status.GetNumThreads());
            Assert.That(isBackground == status.IsBackground());
        }
        catch (MoneroError e)
        {
            throw e;
        }
        finally
        {

            // stop mining at end of test
            try { daemon.StopMining(); }
            catch (MoneroError e) { }
        }
    }

    // Can submit a mined block to the network
    [Test]
    public void TestSubmitMinedBlock()
    {
        Assert.That(TEST_NON_RELAYS);

        // get template to mine on
        MoneroBlockTemplate template = daemon.GetBlockTemplate(TestUtils.ADDRESS);

        // TODO monero rpc: way to get mining nonce when found in order to submit?

        // try to submit block hashing blob without nonce
        try
        {
            daemon.SubmitBlock(template.GetBlockTemplateBlob());
            throw new Exception("Should have thrown error");
        }
        catch (MoneroRpcError e)
        {
            Assert.That(-7 == e.GetCode());
            Assert.That("Block not accepted" == e.Message);
        }
    }

    // Can prune the blockchain
    [Test]
    public void TestPruneBlockchain()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroPruneResult result = daemon.PruneBlockchain(true);
        if (result.IsPruned())
        {
            Assert.That(result.GetPruningSeed() > 0);
        }
        else
        {
            Assert.That(0 == result.GetPruningSeed());
        }
    }

    // Can check for an update
    [Test]
    public void TestCheckForUpdate()
    {
        Assert.That(TEST_NON_RELAYS);
        MoneroDaemonUpdateCheckResult result = daemon.CheckForUpdate();
        TestUpdateCheckResult(result);
    }

    // Can download an update
    [Test]
    [Ignore("Disabled")]
    public void TestDownloadUpdate()
    {
        Assert.That(TEST_NON_RELAYS);

        // download to default path
        MoneroDaemonUpdateDownloadResult result = daemon.DownloadUpdate();
        TestUpdateDownloadResult(result, null);

        // download to defined path
        string path = "test_download_" + DateTime.Now.ToString() + ".tar.bz2";
        result = daemon.DownloadUpdate(path);
        TestUpdateDownloadResult(result, path);

        // test invalid path
        if (result.IsUpdateAvailable())
        {
            try
            {
                result = daemon.DownloadUpdate("./ohhai/there");
                throw new Exception("Should have thrown error");
            }
            catch (MoneroRpcError e)
            {
                Assert.That(e.Message != "Should have thrown error");
                Assert.That(500 == e.GetCode());  // TODO monerod: this causes a 500 in daemon rpc
            }
        }
    }

    // Can be stopped
    [Test]
    [Ignore("Disabled")] // test is disabled to not interfere with other tests
    public void TestStop()
    {
        Assert.That(TEST_NON_RELAYS);
    
        // stop the daemon
        daemon.Stop();

        // give the daemon time to shut down
        Thread.Sleep(TestUtils.SYNC_PERIOD_IN_MS);
        // try to interact with the daemon
        try {
          daemon.GetHeight();
          throw new Exception("Should have thrown error");
        } catch (MoneroError e) {
            Assert.That("Should have thrown error" != e.Message);
        }
    }

    #endregion

    #region Relay Tests

    // Can submit a tx in hex format to the pool and relay in one call
    [Test]
    public void TestSubmitAndRelayTxHex()
    {
        Assert.That(TEST_RELAYS && !LITE_MODE);

        // wait one time for wallet txs in the pool to clear
        // TODO monero-project: update from pool does not prevent creating double spend tx
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);

        // create 2 txs, the second will double spend outputs of first
        MoneroTx tx1 = GetUnrelayedTx(wallet, 2); // TODO: this test requires tx to be from/to different accounts else the occlusion issue (#4500) causes the tx to not be recognized by the wallet at all
        MoneroTx tx2 = GetUnrelayedTx(wallet, 2);

        // submit and relay tx1
        MoneroSubmitTxResult result = daemon.SubmitTxHex(tx1.GetFullHex());
        Assert.That(true == result.IsRelayed());
        TestSubmitTxResultGood(result);

        // tx1 is in the pool
        List<MoneroTx> txs = daemon.GetTxPool();
        bool found = false;
        foreach (MoneroTx aTx in txs)
        {
            if (aTx.GetHash().Equals(tx1.GetHash()))
            {
                Assert.That(true == aTx.IsRelayed());
                found = true;
                break;
            }
        }
        Assert.That(found, "Tx1 was not found after being submitted to the daemon's tx pool");

        // tx1 is recognized by the wallet
        wallet.Sync();
        wallet.GetTx(tx1.GetHash());

        // submit and relay tx2 hex which double spends tx1
        result = daemon.SubmitTxHex(tx2.GetFullHex());
        Assert.That(true == result.IsRelayed());
        TestSubmitTxResultDoubleSpend(result);

        // tx2 is in not the pool
        txs = daemon.GetTxPool();
        found = false;
        foreach (MoneroTx aTx in txs)
        {
            if (aTx.GetHash().Equals(tx2.GetHash()))
            {
                found = true;
                break;
            }
        }
        Assert.That(!found, "Tx2 should not be in the pool because it double spends tx1 which is in the pool");

        // all wallets will need to wait for tx to confirm in order to properly sync
        TestUtils.WALLET_TX_TRACKER.Reset();
    }

    // Can submit a tx in hex format to the pool then relay
    [Test]
    public void TestSubmitThenRelayTxHex()
    {
        Assert.That(TEST_RELAYS && !LITE_MODE);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);
        MoneroTx tx =GetUnrelayedTx(wallet, 1);
        TestSubmitThenRelay([tx]);
    }

    // Can submit txs in hex format to the pool then relay
    [Test]
    public void TestSubmitThenRelayTxHexes()
    {
        Assert.That(TEST_RELAYS && !LITE_MODE);
        TestUtils.WALLET_TX_TRACKER.WaitForWalletTxsToClearPool(wallet);
        List<MoneroTx> txs = [];
        txs.Add(GetUnrelayedTx(wallet, 1));
        txs.Add(GetUnrelayedTx(wallet, 2));  // TODO: accounts cannot be re-used across send tests else isRelayed is true; wallet needs to update?
        TestSubmitThenRelay(txs);
    }

    #endregion

    #region Notification Tests

    // Can notify listeners when a new block is added to the chain
    [Test]
    public void TestBlockListener()
    {
        Assert.That(!LITE_MODE && TEST_NOTIFICATIONS);

        try
        {
            // start mining if possible to help push the network along
            string address = wallet.GetPrimaryAddress();
            try { daemon.StartMining(address, 8, false, true); }
            catch (MoneroError e) { }

            // register a listener
            MoneroDaemonListener listener = new MoneroDaemonListener();
            daemon.AddListener(listener);

            // wait for next block notification
            MoneroBlockHeader header = daemon.WaitForNextBlockHeader();
            daemon.RemoveListener(listener); // unregister listener so daemon does not keep polling
            TestBlockHeader(header, true);

            // test that listener was called with equivalent header
            Assert.That(header == listener.GetLastBlockHeader());
        }
        catch (MoneroError e)
        {
            throw e;
        }
        finally
        {

            // stop mining
            try { daemon.StopMining(); }
            catch (MoneroError e) { }
        }
    }

    #endregion

    #region Test Helpers

    private static void TestBlockHeader(MoneroBlockHeader? header, bool isFull)
    {
        Assert.That(header != null);
        Assert.That(header.GetHeight() >= 0);
        Assert.That(header.GetMajorVersion() > 0);
        Assert.That(header.GetMinorVersion() >= 0);
        if (header.GetHeight() == 0) Assert.That(header.GetTimestamp() == 0);
        else Assert.That(header.GetTimestamp() > 0);
        Assert.That(header.GetPrevHash() != null);
        Assert.That(header.GetNonce() != null);
        if (header.GetNonce() == 0) MoneroUtils.Log(0, "WARNING: header nonce is 0 at height " + header.GetHeight()); // TODO (monero-project): why is header nonce 0?
        else Assert.That(header.GetNonce() > 0);
        Assert.That(header.GetPowHash() == null);  // never seen defined
        if (isFull)
        {
            Assert.That(header.GetSize() > 0);
            Assert.That(header.GetDepth() >= 0);
            Assert.That(header.GetDifficulty() > 0);
            Assert.That(header.GetCumulativeDifficulty() > 0);
            Assert.That(64 == header.GetHash().Length);
            Assert.That(64 == header.GetMinerTxHash().Length);
            Assert.That(header.GetNumTxs() >= 0);
            Assert.That(header.GetOrphanStatus() != null);
            Assert.That(header.GetReward() != null);
            Assert.That(header.GetWeight() != null);
            Assert.That(header.GetWeight() > 0);
        }
        else
        {
            Assert.That(header.GetSize() == null);
            Assert.That(header.GetDepth() == null);
            Assert.That(header.GetDifficulty() == null);
            Assert.That(header.GetCumulativeDifficulty() == null);
            Assert.That(header.GetHash() == null);
            Assert.That(header.GetMinerTxHash() == null);
            Assert.That(header.GetNumTxs() == null);
            Assert.That(header.GetOrphanStatus() == null);
            Assert.That(header.GetReward() == null);
            Assert.That(header.GetWeight() == null);
        }
    }

    private static void TestGetBlocksRange(ulong? startHeight, ulong? endHeight, ulong chainHeight, bool chunked)
    {

        // fetch blocks by range
        ulong realStartHeight = startHeight == null ? 0 : (ulong)startHeight;
        ulong realEndHeight = endHeight == null ? chainHeight - 1 : (ulong)endHeight;
        List<MoneroBlock> blocks = chunked ? daemon.GetBlocksByRangeChunked((ulong)startHeight, (ulong)endHeight) : daemon.GetBlocksByRange((ulong)startHeight, (ulong)endHeight);
        Assert.That(realEndHeight - realStartHeight + 1 == (ulong)blocks.Count);

        // test each block
        for (int i = 0; i < blocks.Count; i++)
        {
            Assert.That(realStartHeight + (ulong)i == (ulong)blocks[i].GetHeight());
            TestBlock(blocks[i], BINARY_BLOCK_CTX);
        }
    }

    private static List<string> GetConfirmedTxHashes(MoneroDaemon daemon)
    {
        int numTxs = 5;
        List<string> txHashes = new List<string>();
        ulong height = daemon.GetHeight();
        while (txHashes.Count < numTxs && height > 0)
        {
            MoneroBlock block = daemon.GetBlockByHeight(--height);
            foreach (string txHash in block.GetTxHashes()) txHashes.Add(txHash);
        }
        return txHashes;
    }

    private static MoneroTx GetUnrelayedTx(MoneroWallet wallet, uint accountIdx)
    {
        Assert.That(accountIdx > 0, "Txs sent from/to same account are not properly synced from the pool");  // TODO monero-project
        MoneroTxConfig config = new MoneroTxConfig().SetAccountIndex(accountIdx).SetAddress(wallet.GetPrimaryAddress()).SetAmount(TestUtils.MAX_FEE);
        MoneroTx tx = wallet.CreateTx(config);
        Assert.That(tx.GetFullHex().Length > 0);
        Assert.That(tx.GetRelay() == false);
        return tx;
    }

    private static void TestBlockTemplate(MoneroBlockTemplate template)
    {
        Assert.That(template != null);
        Assert.That(template.GetBlockTemplateBlob() != null);
        Assert.That(template.GetBlockHashingBlob() != null);
        Assert.That(template.GetDifficulty() != null);
        Assert.That(template.GetExpectedReward() != null);
        Assert.That(template.GetHeight() != null);
        Assert.That(template.GetPrevHash() != null);
        Assert.That(template.GetReservedOffset() != null);
        Assert.That(template.GetSeedHeight() != null);
        Assert.That(template.GetSeedHeight() > 0);
        Assert.That(template.GetSeedHash() != null);
        Assert.That(template.GetSeedHash().Length > 0);
        // next seed hash can be null or initialized  // TODO: test circumstances for each
    }

    // TODO: test block deep copy
    private static void TestBlock(MoneroBlock block, TestContext ctx)
    {

        // test required fields
        Assert.That(block != null);
        TestMinerTx(block.GetMinerTx());  // TODO: miner tx doesn't have as much stuff, can't call TestTx?
        TestBlockHeader(block, ctx.headerIsFull == true);

        if (ctx.hasHex == true)
        {
            Assert.That(block.GetHex() != null);
            Assert.That(block.GetHex().Length > 1);
        }
        else
        {
            Assert.That(block.GetHex() != null);
        }

        if (ctx.hasTxs == true)
        {
            Assert.That(ctx.txContext != null);
            foreach (MoneroTx tx in block.GetTxs())
            {
                Assert.That(block == tx.GetBlock());
                TestTx(tx, ctx.txContext);
            }
        }
        else
        {
            Assert.That(ctx.txContext != null);
            Assert.That(block.GetTxs() == null);
        }
    }

    private static void TestMinerTx(MoneroTx? minerTx)
    {
        Assert.That(minerTx != null);
        Assert.That(minerTx.IsMinerTx() != null);
        Assert.That(minerTx.GetVersion() >= 0);
        Assert.That(minerTx.GetExtra() != null);
        Assert.That(minerTx.GetExtra().Length > 0);
        Assert.That(minerTx.GetUnlockTime() >= 0);

        //    // TODO: miner tx does not have hashes in binary requests so this will fail, need to derive using prunable data
        //    TestContext ctx = new TestContext();
        //    ctx.hasJson = false;
        //    ctx.isPruned = true;
        //    ctx.isFull = false;
        //    ctx.isConfirmed = true;
        //    ctx.isMiner = true;
        //    ctx.fromGetTxPool = true;
        //    testTx(minerTx, ctx);
    }

    private static void TestTx(MoneroTx? tx, TestContext? ctx)
    {
        // check inputs
        Assert.That(tx != null);
        Assert.That(ctx != null);
        Assert.That(ctx.isPruned != null);
        Assert.That(ctx.isConfirmed != null);
        Assert.That(ctx.fromGetTxPool != null);

        // standard across all txs
        Assert.That(64 == tx.GetHash().Length);
        if (tx.IsRelayed() == null) Assert.That(tx.InTxPool() == true);  // TODO monerod: add relayed to get_transactions
        else Assert.That(tx.IsRelayed() != null);
        Assert.That(tx.IsConfirmed() != null);
        Assert.That(tx.InTxPool() != null);
        Assert.That(tx.IsMinerTx() != null);
        Assert.That(tx.IsDoubleSpendSeen() != null);
        Assert.That(tx.GetVersion() >= 0);
        Assert.That(tx.GetUnlockTime() >= 0);
        Assert.That(tx.GetInputs() != null);
        Assert.That(tx.GetOutputs() != null);
        Assert.That(tx.GetExtra().Length > 0);
        //TestUtils.testUnsignedBigInteger(tx.GetFee(), true);

        // test presence of output indices
        // TODO: change this over to outputs only
        if (tx.IsMinerTx() == true) Assert.That(tx.GetOutputIndices() == null); // TODO: how to get output indices for miner transactions?
        if (tx.InTxPool() == true || ctx.fromGetTxPool == true || ctx.hasOutputIndices == false) Assert.That(null == tx.GetOutputIndices());
        else Assert.That(tx.GetOutputIndices() != null);
        if (tx.GetOutputIndices() != null) Assert.That(tx.GetOutputIndices().Count > 0);

        // test confirmed ctx
        if (ctx.isConfirmed == true) Assert.That(true == tx.IsConfirmed());
        if (ctx.isConfirmed == false) Assert.That(false == tx.IsConfirmed());

        // test confirmed
        if (tx.IsConfirmed() == true)
        {
            Assert.That(tx.GetBlock() != null);
            Assert.That(tx.GetBlock().GetTxs().Contains(tx));
            Assert.That(tx.GetBlock().GetHeight() > 0);
            Assert.That(tx.GetBlock().GetTxs().Contains(tx));
            Assert.That(tx.GetBlock().GetHeight() > 0);
            Assert.That(tx.GetBlock().GetTimestamp() > 0);
            Assert.That(true == tx.GetRelay());
            Assert.That(true == tx.IsRelayed());
            Assert.That(false == tx.IsFailed());
            Assert.That(false == tx.InTxPool());
            Assert.That(false == tx.IsDoubleSpendSeen());
            if (ctx.fromBinaryBlock == true) Assert.That(tx.GetNumConfirmations() == null);
            else Assert.That(tx.GetNumConfirmations() > 0);
        }
        else
        {
            Assert.That(null == tx.GetBlock());
            Assert.That(0 == (long)tx.GetNumConfirmations());
        }

        // test in tx pool
        if (tx.InTxPool() == true)
        {
            Assert.That(tx.IsConfirmed() == false);
            Assert.That(tx.IsDoubleSpendSeen() == false);
            Assert.That(tx.GetLastFailedHeight() == null);
            Assert.That(tx.GetLastFailedHash() == null);
            Assert.That(tx.GetReceivedTimestamp() > 0);
            if (ctx.fromGetTxPool == true)
            {
                Assert.That(tx.GetSize() > 0);
                Assert.That(tx.GetWeight() > 0);
                Assert.That(tx.IsKeptByBlock() != null);
                Assert.That(tx.GetMaxUsedBlockHeight() >= 0);
                Assert.That(tx.GetMaxUsedBlockHash() != null);
            }
            Assert.That(null == tx.GetLastFailedHeight());
            Assert.That(null == tx.GetLastFailedHash());
        }
        else
        {
            Assert.That(tx.GetLastRelayedTimestamp() == null);
        }

        // test miner tx
        if (tx.IsMinerTx() == true)
        {
            Assert.That(0 == tx.GetFee());
            Assert.That(null == tx.GetInputs());
            Assert.That(tx.GetSignatures() == null);
        }
        else
        {
            if (tx.GetSignatures() != null) Assert.That(tx.GetSignatures().Count > 0);
        }

        // test failed  // TODO: what else to test associated with failed
        if (tx.IsFailed() == true)
        {
            Assert.That(tx.GetReceivedTimestamp() > 0);
        }
        else
        {
            if (tx.IsRelayed() == null) Assert.That(null == tx.GetRelay()); // TODO monerod: add relayed to get_transactions
            else if (tx.IsRelayed() == true) Assert.That(false == tx.IsDoubleSpendSeen());
            else
            {
                Assert.That(false == tx.IsRelayed());
                if (ctx.fromGetTxPool == true)
                {
                    Assert.That(false == tx.GetRelay());
                    Assert.That(tx.IsDoubleSpendSeen() != null);
                }
            }
        }
        Assert.That(tx.GetLastFailedHeight() == null);
        Assert.That(tx.GetLastFailedHash() == null);

        // received time only for tx pool or failed txs
        if (tx.GetReceivedTimestamp() != null)
        {
            Assert.That(tx.InTxPool() == true || tx.IsFailed() == true);
        }

        // test inputs and outputs
        if (tx.IsMinerTx() == false) Assert.That(tx.GetInputs().Count > 0);
        foreach (MoneroOutput input in tx.GetInputs())
        {
            Assert.That(tx == input.GetTx());
            TestInput(input, ctx);
        }
        Assert.That(tx.GetOutputs().Count > 0);
        foreach (MoneroOutput output in tx.GetOutputs())
        {
            Assert.That(tx == output.GetTx());
            TestOutput(output, ctx);
        }

        // test pruned vs not pruned
        if (ctx.fromGetTxPool == true || ctx.fromBinaryBlock == true) Assert.That(tx.GetPrunableHash() == null);   // TODO monerod: tx pool txs do not have prunable hash, TODO: GetBlocksByHeight() has inconsistent client-side pruning
        else Assert.That(tx.GetPrunableHash() != null);
        if (ctx.isPruned == true)
        {
            Assert.That(tx.GetRctSigPrunable() == null);
            Assert.That(tx.GetSize() == null);
            Assert.That(tx.GetLastRelayedTimestamp() == null);
            Assert.That(tx.GetReceivedTimestamp() == null);
            Assert.That(tx.GetFullHex() == null);
            Assert.That(tx.GetPrunedHex() != null);
        }
        else
        {
            Assert.That(tx.GetPrunedHex() == null);
            if (ctx.fromBinaryBlock == true) Assert.That(tx.GetFullHex() == null);         // TODO: GetBlocksByHeight() has inconsistent client-side pruning
            else Assert.That(tx.GetFullHex().Length > 0);
            if (ctx.fromBinaryBlock == true) Assert.That(tx.GetRctSigPrunable() == null);  // TODO: GetBlocksByHeight() has inconsistent client-side pruning
                                                                                               //else Assert.That(null != (tx.GetRctSigPrunable()); // TODO: define and test this
            Assert.That(tx.IsDoubleSpendSeen() == false);
            if (tx.IsConfirmed() == true)
            {
                Assert.That(tx.GetLastRelayedTimestamp() == null);
                Assert.That(tx.GetReceivedTimestamp() == null);
            }
            else
            {
                if (tx.IsRelayed() == true) Assert.That(tx.GetLastRelayedTimestamp() > 0);
                else Assert.That(tx.GetLastRelayedTimestamp() == null);
                Assert.That(tx.GetReceivedTimestamp() > 0);
            }
        }

        if (tx.IsFailed() == true)
        {
            // TODO: implement this
        }

        // test deep copy
        if (true != ctx.doNotTestCopy) TestTxCopy(tx, ctx);
    }

    private static void TestInput(MoneroOutput input, TestContext ctx)
    {
        TestOutput(input);
        TestKeyImage(input.GetKeyImage(), ctx);
        Assert.That(input.GetRingOutputIndices().Count > 0);
    }

    private static void TestKeyImage(MoneroKeyImage image, TestContext ctx)
    {
        Assert.That(image.GetHex().Length > 0);
        if (image.GetSignature() != null)
        {
            Assert.That(image.GetSignature() != null);
            Assert.That(image.GetSignature().Length > 0);
        }
    }

    private static void TestOutput(MoneroOutput output, TestContext ctx)
    {
        TestOutput(output);
        if (output.GetTx().InTxPool() == true || ctx.hasOutputIndices == false) Assert.That(null == output.GetIndex());
        else Assert.That(output.GetIndex() >= 0);
        Assert.That(64 == output.GetStealthPublicKey().Length);
    }

    private static void TestOutput(MoneroOutput output)
    {
        //TestUtils.testUnsignedBigInteger(output.GetAmount());
    }

    private static void TestTxCopy(MoneroTx tx, TestContext ctx)
    {

        // copy tx and assert deep equality
        MoneroTx copy = tx.Clone();
        //Assert.That(copy instanceof MoneroTx);
        Assert.That(copy.GetBlock() == null);
        if (tx.GetBlock() != null) copy.SetBlock(tx.GetBlock().Clone().SetTxs([copy]));
        Assert.That(tx.ToString() == copy.ToString());
        Assert.That(copy != tx);

        // test different input references
        if (copy.GetInputs() == null) Assert.That(tx.GetInputs() == null);
        else
        {
            Assert.That(copy.GetInputs() != tx.GetInputs());
            for (int i = 0; i < copy.GetInputs().Count; i++)
            {
                Assert.That(tx.GetInputs()[i].GetAmount().Equals(copy.GetInputs()[i].GetAmount()));
            }
        }

        // test different output references
        if (copy.GetOutputs() == null) Assert.That(null == tx.GetOutputs());
        else
        {
            Assert.That(copy.GetOutputs() != tx.GetOutputs());
            for (int i = 0; i < copy.GetOutputs().Count; i++)
            {
                Assert.That(tx.GetOutputs()[i].GetAmount().Equals(copy.GetOutputs()[i].GetAmount()));
            }
        }

        // test copied tx
        ctx = new TestContext(ctx);
        ctx.doNotTestCopy = true; // to prevent infinite recursion
        if (tx.GetBlock() != null) copy.SetBlock(tx.GetBlock().Clone().SetTxs([copy])); // copy block for testing
        TestTx(copy, ctx);

        // test merging with copy
        //oneroTx merged = copy.Merge(copy.Clone());
        //Assert.That(tx.tostring(), merged.tostring());
    }

    private static void TestMinerTxSum(MoneroMinerTxSum txSum)
    {
        //TestUtils.testUnsignedBigInteger(txSum.GetEmissionSum(), true);
        //TestUtils.testUnsignedBigInteger(txSum.GetFeeSum(), true);
    }

    private static void TestTxPoolStats(MoneroTxPoolStats? stats)
    {
        Assert.That(stats != null);
        Assert.That(stats.GetNumTxs() >= 0);
        if (stats.GetNumTxs() > 0)
        {
            if (stats.GetNumTxs() == 1) Assert.That(stats.GetHisto() == null);
            else
            {
                Dictionary<ulong, int> histo = stats.GetHisto();
                Assert.That(histo != null);
                Assert.That(histo.Count > 0);
                foreach (var kv in histo)
                {
                    Assert.That(kv.Value >= 0);
                }
            }
            Assert.That(stats.GetBytesMax() > 0);
            Assert.That(stats.GetBytesMed() > 0);
            Assert.That(stats.GetBytesMin() > 0);
            Assert.That(stats.GetBytesTotal() > 0);
            Assert.That(stats.GetHisto98pc() == null || stats.GetHisto98pc() > 0);
            Assert.That(stats.GetOldestTimestamp() > 0);
            Assert.That(stats.GetNum10m() >= 0);
            Assert.That(stats.GetNumDoubleSpends() >= 0);
            Assert.That(stats.GetNumFailing() >= 0);
            Assert.That(stats.GetNumNotRelayed() >= 0);
        }
        else
        {
            Assert.That(null == stats.GetBytesMax());
            Assert.That(null == stats.GetBytesMed());
            Assert.That(null == stats.GetBytesMin());
            Assert.That(0 == (long)stats.GetBytesTotal());
            Assert.That(null == stats.GetHisto98pc());
            Assert.That(null == stats.GetOldestTimestamp());
            Assert.That(0 == (int)stats.GetNum10m());
            Assert.That(0 == (int)stats.GetNumDoubleSpends());
            Assert.That(0 == (int)stats.GetNumFailing());
            Assert.That(0 == (int)stats.GetNumNotRelayed());
            Assert.That(null == stats.GetHisto());
        }
    }

    // helper function to check the spent status of a key image or array of key images
    private static void TestSpentStatuses(List<string> keyImages, MoneroKeyImage.SpentStatus expectedStatus)
    {

        // test image
        foreach (string keyImage in keyImages)
        {
            Assert.That(expectedStatus == daemon.GetKeyImageSpentStatus(keyImage));
        }

        // test array of images
        List<MoneroKeyImage.SpentStatus> statuses = keyImages.Count == 0 ? new List<MoneroKeyImage.SpentStatus>() : daemon.GetKeyImageSpentStatuses(keyImages);
        Assert.That(keyImages.Count == statuses.Count);
        foreach (MoneroKeyImage.SpentStatus status in statuses) Assert.That(expectedStatus == status);
    }

    private static List<MoneroTx> GetConfirmedTxs(MoneroDaemon daemon, int numTxs)
    {
        List<MoneroTx> txs = new List<MoneroTx>();
        ulong numBlocksPerReq = 50;
        for (ulong startIdx = daemon.GetHeight() - numBlocksPerReq - 1; startIdx >= 0; startIdx -= numBlocksPerReq)
        {
            List<MoneroBlock> blocks = daemon.GetBlocksByRange(startIdx, startIdx + numBlocksPerReq);
            foreach (MoneroBlock block in blocks)
            {
                if (block.GetTxs() == null) continue;
                foreach (MoneroTx tx in block.GetTxs())
                {
                    txs.Add(tx);
                    if (txs.Count == numTxs) return txs;
                }
            }
        }
        throw new MoneroError("Could not get " + numTxs + " confirmed txs");
    }

    private static void TestOutputDistributionEntry(MoneroOutputDistributionEntry entry)
    {
        //TestUtils.testUnsignedBigInteger(entry.GetAmount());
        Assert.That(entry.GetBase() >= 0);
        Assert.That(entry.GetDistribution().Count > 0);
        Assert.That(entry.GetStartHeight() >= 0);
    }

    private static void TestInfo(MoneroDaemonInfo info)
    {
        Assert.That(info.GetVersion() != null);
        Assert.That(info.GetNumAltBlocks() >= 0);
        Assert.That(info.GetBlockSizeLimit() > 0);
        Assert.That(info.GetBlockSizeMedian() > 0);
        Assert.That(info.GetBootstrapDaemonAddress() == null || info.GetBootstrapDaemonAddress().Length > 0);
        //TestUtils.testUnsignedBigInteger(info.GetCumulativeDifficulty());
        //TestUtils.testUnsignedBigInteger(info.GetFreeSpace());
        Assert.That(info.GetNumOfflinePeers() >= 0);
        Assert.That(info.GetNumOnlinePeers() >= 0);
        Assert.That(info.GetHeight() >= 0);
        Assert.That(info.GetHeightWithoutBootstrap() > 0);
        Assert.That(info.GetNumIncomingConnections() >= 0);
        Assert.That(info.GetNetworkType() != null);
        Assert.That(info.IsOffline() != null);
        Assert.That(info.GetNumOutgoingConnections() >= 0);
        Assert.That(info.GetNumRpcConnections() >= 0);
        Assert.That(info.GetStartTimestamp() > 0);
        Assert.That(info.GetAdjustedTimestamp() > 0);
        Assert.That(info.GetTarget() > 0);
        Assert.That(info.GetTargetHeight() >= 0);
        Assert.That(info.GetNumTxs() >= 0);
        Assert.That(info.GetNumTxsPool() >= 0);
        Assert.That(info.GetWasBootstrapEverUsed() != null);
        Assert.That(info.GetBlockWeightLimit() > 0);
        Assert.That(info.GetBlockWeightMedian() > 0);
        Assert.That(info.GetDatabaseSize() > 0);
        Assert.That(info.GetUpdateAvailable() != null);
        //TestUtils.testUnsignedBigInteger(info.GetCredits(), false); // 0 credits
        Assert.That(info.GetTopBlockHash().Length > 0);
        Assert.That(info.IsBusySyncing() != null);
        Assert.That(info.IsSynchronized() != null);
    }

    private static void TestSyncInfo(MoneroDaemonSyncInfo syncInfo)
    { // TODO: consistent naming, daemon in name?
        MoneroDaemonSyncInfo testObj = new();

        Assert.That(testObj.GetType().IsInstanceOfType(syncInfo));
        Assert.That(syncInfo.GetHeight() >= 0);
        if (syncInfo.GetPeers() != null)
        {
            Assert.That(syncInfo.GetPeers().Count > 0);
            foreach (MoneroPeer connection in syncInfo.GetPeers())
            {
                TestPeer(connection);
            }
        }
        if (syncInfo.GetSpans() != null)
        {  // TODO: test that this is being hit, so far not used
            Assert.That(syncInfo.GetSpans().Count > 0);
            foreach (MoneroConnectionSpan span in syncInfo.GetSpans())
            {
                TestConnectionSpan(span);
            }
        }
        Assert.That(syncInfo.GetNextNeededPruningSeed() >= 0);
        Assert.That(syncInfo.GetOverview() == null);
        //TestUtils.testUnsignedBigInteger(syncInfo.GetCredits(), false); // 0 credits
        Assert.That(syncInfo.GetTopBlockHash() == null);
    }

    private static void TestConnectionSpan(MoneroConnectionSpan? span)
    {
        Assert.That(span != null);
        Assert.That(span.GetConnectionId() != null);
        Assert.That(span.GetConnectionId().Length > 0);
        Assert.That(span.GetStartHeight() > 0);
        Assert.That(span.GetNumBlocks() > 0);
        Assert.That(span.GetRemoteAddress() == null || span.GetRemoteAddress().Length > 0);
        Assert.That(span.GetRate() > 0);
        Assert.That(span.GetSpeed() >= 0);
        Assert.That(span.GetSize() > 0);
    }

    private static void TestHardForkInfo(MoneroHardForkInfo hardForkInfo)
    {
        Assert.That(null != hardForkInfo.GetEarliestHeight());
        Assert.That(null != hardForkInfo.IsEnabled());
        Assert.That(null != hardForkInfo.GetState());
        Assert.That(null != hardForkInfo.GetThreshold());
        Assert.That(null != hardForkInfo.GetVersion());
        Assert.That(null != hardForkInfo.GetNumVotes());
        Assert.That(null != hardForkInfo.GetVoting());
        Assert.That(null != hardForkInfo.GetWindow());
        //TestUtils.testUnsignedBigInteger(hardForkInfo.GetCredits(), false); // 0 credits
        Assert.That(null == hardForkInfo.GetTopBlockHash());
    }

    private static void TestMoneroBan(MoneroBan ban)
    {
        Assert.That(null != ban.GetHost());
        Assert.That(null != ban.GetIp());
        Assert.That(null != ban.GetSeconds());
    }

    private static void TestAltChain(MoneroAltChain altChain)
    {
        Assert.That(null != (altChain));
        Assert.That(altChain.GetBlockHashes().Count > 0);
        //TestUtils.testUnsignedBigInteger(altChain.GetDifficulty(), true);
        Assert.That(altChain.GetHeight() > 0);
        Assert.That(altChain.GetLength() > 0);
        Assert.That(64 == altChain.GetMainChainParentBlockHash().Length);
    }

    private static void TestPeer(MoneroPeer peer)
    {
        //Assert.That(peer instanceof MoneroPeer);
        TestKnownPeer(peer, true);
        Assert.That(peer.GetHash().Length > 0);
        Assert.That(peer.GetAvgDownload() >= 0);
        Assert.That(peer.GetAvgUpload() >= 0);
        Assert.That(peer.GetCurrentDownload() >= 0);
        Assert.That(peer.GetCurrentUpload() >= 0);
        Assert.That(peer.GetHeight() >= 0);
        Assert.That(peer.GetLiveTime() >= 0);
        Assert.That(null != peer.IsLocalIp());
        Assert.That(null != peer.IsLocalHost());
        Assert.That(peer.GetNumReceives() >= 0);
        Assert.That(peer.GetReceiveIdleTime() >= 0);
        Assert.That(peer.GetNumSends() >= 0);
        Assert.That(peer.GetSendIdleTime() >= 0);
        Assert.That(null != peer.GetState());
        Assert.That(peer.GetNumSupportFlags() >= 0);
        Assert.That(null != peer.GetType());
    }

    private static void TestKnownPeer(MoneroPeer peer, bool fromConnection)
    {
        Assert.That(null != peer);
        Assert.That(peer.GetId().Length > 0);
        Assert.That(peer.GetHost().Length > 0);
        Assert.That(peer.GetPort() > 0);
        Assert.That(peer.GetRpcPort() == null || peer.GetRpcPort() >= 0);
        Assert.That(null != peer.IsOnline());
        //if (peer.GetRpcCreditsPerHash() != null) TestUtils.testUnsignedBigInteger(peer.GetRpcCreditsPerHash());
        if (fromConnection) Assert.That(null == peer.GetLastSeenTimestamp());
        else
        {
            if (peer.GetLastSeenTimestamp() < 0) MoneroUtils.Log(0, "Last seen timestamp is invalid: " + peer.GetLastSeenTimestamp());
            Assert.That(peer.GetLastSeenTimestamp() >= 0);
        }
        Assert.That(peer.GetPruningSeed() == null || peer.GetPruningSeed() >= 0);
    }

    private static void TestUpdateCheckResult(MoneroDaemonUpdateCheckResult result)
    {
        //Assert.That(result instanceof MoneroDaemonUpdateCheckResult);
        Assert.That(result.IsUpdateAvailable() != null);
        if (result.IsUpdateAvailable() == true)
        {
            Assert.That(result.GetAutoUri().Length > 0, "No auto uri; is daemon online?");
            Assert.That(result.GetUserUri().Length > 0);
            Assert.That(result.GetVersion().Length > 0);
            Assert.That(result.GetHash().Length > 0);
            Assert.That(64 == result.GetHash().Length);
        }
        else
        {
            Assert.That(result.GetAutoUri() == null);
            Assert.That(result.GetUserUri() == null);
            Assert.That(result.GetVersion() == null);
            Assert.That(result.GetHash() == null);
        }
    }

    private static void TestUpdateDownloadResult(MoneroDaemonUpdateDownloadResult result, string path)
    {
        TestUpdateCheckResult(result);
        if (result.IsUpdateAvailable() == true)
        {
            if (path != null) Assert.That(path == result.GetDownloadPath());
            else Assert.That(result.GetDownloadPath() != null);
        }
        else
        {
            Assert.That(result.GetDownloadPath() == null);
        }
    }

    private static void TestSubmitTxResultGood(MoneroSubmitTxResult result)
    {
        TestSubmitTxResultCommon(result);
        try
        {
            Assert.That(false == result.IsDoubleSpend(), "tx submission is double spend.");
            Assert.That(false == result.IsFeeTooLow());
            Assert.That(false == result.IsMixinTooLow());
            Assert.That(false == result.HasInvalidInput());
            Assert.That(false == result.HasInvalidOutput());
            Assert.That(false == result.HasTooFewOutputs());
            Assert.That(false == result.IsOverspend());
            Assert.That(false == result.IsTooBig());
            Assert.That(false == result.GetSanityCheckFailed());
            //TestUtils.testUnsignedBigInteger(result.getCredits(), false); // 0 credits
            Assert.That(result.GetTopBlockHash() == null);
            Assert.That(false == result.IsTxExtraTooBig());
            Assert.That(true == result.IsGood());
            Assert.That(false == result.IsNonzeroUnlockTime());
        }
        catch (Exception e)
        {
            MoneroUtils.Log(0, "Submit result is not good");
            throw e;
        }
    }

    private static void TestSubmitTxResultDoubleSpend(MoneroSubmitTxResult result)
    {
        TestSubmitTxResultCommon(result);
        Assert.That(false == result.IsGood());
        Assert.That(true == result.IsDoubleSpend());
        Assert.That(false == result.IsFeeTooLow());
        Assert.That(false == result.IsMixinTooLow());
        Assert.That(false == result.HasInvalidInput());
        Assert.That(false == result.HasInvalidOutput());
        Assert.That(false == result.IsOverspend());
        Assert.That(false== result.IsTooBig());
    }

    private static void TestSubmitTxResultCommon(MoneroSubmitTxResult result)
    {
        Assert.That(result.IsGood() != null);
        Assert.That(result.IsRelayed() != null);
        Assert.That(result.IsDoubleSpend() != null);
        Assert.That(result.IsFeeTooLow() != null);
        Assert.That(result.IsMixinTooLow() != null);
        Assert.That(result.HasInvalidInput() != null);
        Assert.That(result.HasInvalidOutput() != null);
        Assert.That(result.IsOverspend() != null);
        Assert.That(result.IsTooBig() != null);
        Assert.That(result.GetSanityCheckFailed() != null);
        Assert.That(result.GetReason() == null || result.GetReason().Length > 0);
    }

    private static void TestOutputHistogramEntry(MoneroOutputHistogramEntry entry)
    {
        //TestUtils.testUnsignedBigInteger(entry.getAmount());
        Assert.That(entry.GetNumInstances() >= 0);
        Assert.That(entry.GetNumUnlockedInstances() >= 0);
        Assert.That(entry.GetNumRecentInstances() >= 0);
    }

    private static void TestSubmitThenRelay(List<MoneroTx> txs)
    {
        // submit txs hex but don't relay
        List<string> txHashes = [];
        foreach (MoneroTx tx in txs)
        {
            txHashes.Add(tx.GetHash());
            MoneroSubmitTxResult result = daemon.SubmitTxHex(tx.GetFullHex(), true);
            TestSubmitTxResultGood(result);
            Assert.That(false == result.IsRelayed());

            // ensure tx is in pool
            List<MoneroTx> _poolTxs = daemon.GetTxPool();
            bool found = false;
            foreach (MoneroTx aTx in _poolTxs)
            {
                if (aTx.GetHash().Equals(tx.GetHash()))
                {
                    Assert.That(false == aTx.IsRelayed());
                    found = true;
                    break;
                }
            }
            Assert.That(found, "Tx was not found after being submitted to the daemon's tx pool");

            // fetch tx by hash and ensure not relayed
            MoneroTx fetchedTx = daemon.GetTx(tx.GetHash());
            Assert.That(false == fetchedTx.IsRelayed());
        }

        // relay the txs
        try
        {
            if (txHashes.Count == 1) daemon.RelayTxByHash(txHashes[0]);
            else daemon.RelayTxsByHash(txHashes);
        }
        catch (Exception e)
        {
            daemon.FlushTxPool(txHashes); // flush txs when relay fails to prevent double spends in other tests
            throw e;
        }

        // wait for txs to be relayed // TODO (monero-project): all txs should be relayed: https://github.com/monero-project/monero/issues/8523
        try { Thread.Sleep(1000); } catch (Exception e) { throw new Exception(e.Message); }

        // ensure txs are relayed
        List<MoneroTx> poolTxs = daemon.GetTxPool();
        foreach (MoneroTx tx in txs)
        {
            bool found = false;
            foreach (MoneroTx aTx in poolTxs)
            {
                if (aTx.GetHash().Equals(tx.GetHash()))
                {
                    Assert.That(true == aTx.IsRelayed());
                    found = true;
                    break;
                }
            }
            Assert.That(found, "Tx was not found after being submitted to the daemon's tx pool");
        }

        // wallets will need to wait for tx to confirm in order to properly sync
        TestUtils.WALLET_TX_TRACKER.Reset();
    }

    #endregion
}

internal class TestContext
{
    public bool? hasJson;
    public bool? isPruned;
    public bool? isFull;
    public bool? isConfirmed;
    public bool? isMinerTx;
    public bool? fromGetTxPool;
    public bool? fromBinaryBlock;
    public bool? hasOutputIndices;
    public bool? doNotTestCopy;
    public bool? hasTxs;
    public bool? hasHex;
    public bool? headerIsFull;
    public TestContext? txContext;

    public TestContext() { }
    
    public TestContext(TestContext ctx)
    {
        hasJson = ctx.hasJson;
        isPruned = ctx.isPruned;
        isFull = ctx.isFull;
        isConfirmed = ctx.isConfirmed;
        isMinerTx = ctx.isMinerTx;
        fromGetTxPool = ctx.fromGetTxPool;
        fromBinaryBlock = ctx.fromBinaryBlock;
        hasOutputIndices = ctx.hasOutputIndices;
        doNotTestCopy = ctx.doNotTestCopy;
        hasTxs = ctx.hasTxs;
        hasHex = ctx.hasHex;
        headerIsFull = ctx.headerIsFull;
    }
}