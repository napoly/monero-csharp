using Monero.Common;
using System.Runtime.InteropServices;


namespace Monero.Wallet
{
    internal static class MoneroWallet2Api
    {
        private const string DllName = "monero_x86_64-w64-mingw32_libwallet2_api_c";

        #region Debug

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_DEBUG_test0();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_DEBUG_test1(bool x);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_DEBUG_test2(int x);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_DEBUG_test3(ulong x);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_DEBUG_test4(ulong x);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_DEBUG_test5();
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_DEBUG_test5_std();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_DEBUG_isPointerNull(IntPtr walletPtr);

        #endregion

        #region Pending Transaction

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_PendingTransaction_status(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_errorString(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_PendingTransaction_commit(IntPtr pendingTx_ptr, string filename, bool overwrite);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_commitUR(IntPtr pendingTx_ptr, int max_fragment_length);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_PendingTransaction_amount(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_PendingTransaction_dust(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_PendingTransaction_fee(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_txid(IntPtr pendingTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_PendingTransaction_txCount(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_subaddrAccount(IntPtr pendingTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_subaddrIndices(IntPtr pendingTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_multisigSignData(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_PendingTransaction_signMultisigTx(IntPtr pendingTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_signersKeys(IntPtr pendingTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_PendingTransaction_hex(IntPtr pendingTx_ptr, string separator);

        #endregion

        #region UnsingnedTransaction 


        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_UnsignedTransaction_status(IntPtr unsignedTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_errorString(IntPtr unsignedTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_amount(IntPtr unsignedTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_fee(IntPtr unsignedTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_mixin(IntPtr unsignedTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_confirmationMessage(IntPtr unsignedTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_paymentId(IntPtr unsignedTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_recipientAddress(IntPtr unsignedTx_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_UnsignedTransaction_minMixinCount(IntPtr unsignedTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_UnsignedTransaction_txCount(IntPtr unsignedTx_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_UnsignedTransaction_sign(IntPtr unsignedTx_ptr, string signedFileName);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_UnsignedTransaction_signUR(IntPtr unsignedTx_ptr, int max_fragment_length);

        #endregion

        #region Transaction Info

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_TransactionInfo_direction(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_TransactionInfo_isPending(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_TransactionInfo_isFailed(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_TransactionInfo_isCoinbase(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_amount(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_fee(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_blockHeight(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_TransactionInfo_description(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_TransactionInfo_subaddrIndex(IntPtr txInfo_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_TransactionInfo_subaddrAccount(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_TransactionInfo_label(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_confirmations(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_unlockTime(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_TransactionInfo_hash(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_timestamp(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_TransactionInfo_paymentId(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_TransactionInfo_transfers_count(IntPtr txInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_TransactionInfo_transfers_amount(IntPtr txInfo_ptr, int index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_TransactionInfo_transfers_address(IntPtr txInfo_ptr, int address);

        #endregion

        #region Transaction History

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_TransactionHistory_count(IntPtr txHistory_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_TransactionHistory_transaction(IntPtr txHistory_ptr, int index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_TransactionHistory_transactionById(IntPtr txHistory_ptr, string id);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_TransactionHistory_refresh(IntPtr txHistory_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_TransactionHistory_setTxNote(IntPtr txHistory_ptr, string txid, string note);

        #endregion

        #region Coins Info

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_CoinsInfo_blockHeight(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_CoinsInfo_hash(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_CoinsInfo_internalOutputIndex(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_CoinsInfo_globalOutputIndex(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_CoinsInfo_spent(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_CoinsInfo_frozen(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_CoinsInfo_spentHeight(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_CoinsInfo_amount(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_CoinsInfo_rct(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_CoinsInfo_keyImageKnown(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_CoinsInfo_pkIndex(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_CoinsInfo_subaddrIndex(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_CoinsInfo_subaddrAccount(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_CoinsInfo_address(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_CoinsInfo_addressLabel(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_CoinsInfo_keyImage(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_CoinsInfo_unlockTime(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_CoinsInfo_unlocked(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_CoinsInfo_pubKey(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_CoinsInfo_coinbase(IntPtr coinsInfo_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_CoinsInfo_description(IntPtr coinsInfo_ptr);

        #endregion

        #region Coins 

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Coins_count(IntPtr coins_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Coins_coin(IntPtr coins_ptr, int index);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Coins_getAll_size(IntPtr coins_ptr);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Coins_getAll_byIndex(IntPtr coins_ptr, int index);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Coins_refresh(IntPtr coins_ptr);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Coins_setFrozenByPublicKey(IntPtr coins_ptr, string public_key);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Coins_setFrozen(IntPtr coins_ptr, int index);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Coins_thaw(IntPtr coins_ptr, int index);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Coins_thawByPublicKey(IntPtr coins_ptr, string public_key);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Coins_isTransferUnlocked(IntPtr coins_ptr, ulong unlockTime, ulong blockHeight);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Coins_setDescription(IntPtr coins_ptr, string public_key, string description);

        #endregion

        #region Address Book Row

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_AddressBookRow_extra(IntPtr addressBookRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_AddressBookRow_getAddress(IntPtr addressBookRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_AddressBookRow_getDescription(IntPtr addressBookRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_AddressBookRow_getPaymentId(IntPtr addressBookRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_AddressBookRow_getRowId(IntPtr addressBookRow_ptr);

        #endregion

        #region Address Book

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_AddressBook_getAll_size(IntPtr addressBook_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_AddressBook_getAll_byIndex(IntPtr addressBook_ptr, int index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_AddressBook_addRow(IntPtr addressBook_ptr, string dst_addr, string payment_id, string description);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_AddressBook_deleteRow(IntPtr addressBook_ptr, uint rowId);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_AddressBook_setDescription(IntPtr addressBook_ptr, uint rowId, string description);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_AddressBook_refresh(IntPtr addressBook_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_AddressBook_errorString(IntPtr addressBook_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_AddressBook_errorCode(IntPtr addressBook_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_AddressBook_lookupPaymentID(IntPtr addressBook_ptr, string payment_id);

        #endregion

        #region Subaddress Row

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressRow_extra(IntPtr subaddressRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressRow_getAddress(IntPtr subaddressRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressRow_getLabel(IntPtr subaddressRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_SubaddressRow_getRowId(IntPtr subaddressRow_ptr);

        #endregion

        #region Subaddress

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Subaddress_getAll_size(IntPtr subaddress_ptr);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Subaddress_getAll_byIndex(IntPtr subaddress_ptr, int index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Subaddress_addRow(IntPtr subaddress_ptr, uint accountIndex, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Subaddress_setLabel(IntPtr subaddress_ptr, uint accountIndex, uint addressIndex, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Subaddress_refresh(IntPtr subaddress_ptr, uint accountIndex);

        #endregion

        #region Subaddress Account Row

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressAccountRow_extra(IntPtr subaddressAccountRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressAccountRow_getAddress(IntPtr subaddressAccountRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressAccountRow_getLabel(IntPtr subaddressAccountRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressAccountRow_getBalance(IntPtr subaddressAccountRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_SubaddressAccountRow_getUnlockedBalance(IntPtr subaddressAccountRow_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_SubaddressAccountRow_getRowId(IntPtr subaddressAccountRow_ptr);

        #endregion

        #region Subaddress Account

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_SubaddressAccount_getAll_size(IntPtr subaddressAccount_ptr);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_SubaddressAccount_getAll_byIndex(IntPtr subaddressAccount_ptr, int index);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_SubaddressAccount_addRow(IntPtr subaddressAccount_ptr, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_SubaddressAccount_setLabel(IntPtr subaddressAccount_ptr, uint accountIndex, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_SubaddressAccount_refresh(IntPtr subaddressAccount_ptr);

        #endregion

        #region Multisignature State

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_MultisigState_isMultisig(IntPtr multisigState_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_MultisigState_isReady(IntPtr multisigState_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_MultisigState_threshold(IntPtr multisigState_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_MultisigState_total(IntPtr multisigState_ptr);

        #endregion

        #region Device Progress

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_DeviceProgress_progress(IntPtr deviceProgress_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_DeviceProgress_indeterminate(IntPtr deviceProgress_ptr);

        #endregion

        #region Wallet2

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_seed(IntPtr wallet_ptr, string seed_offset);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getSeedLanguage(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setSeedLanguage(IntPtr wallet_ptr, string arg);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Wallet_status(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_errorString(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setPassword(IntPtr wallet_ptr, string password);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getPassword(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setDevicePin(IntPtr wallet_ptr, string pin);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setDevicePassphrase(IntPtr wallet_ptr, string passphrase);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_address(IntPtr wallet_ptr, ulong accountIndex, ulong addressIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_path(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Wallet_nettype(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern char MONERO_Wallet_useForkRules(IntPtr wallet_ptr, char version, ulong early_blocks);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_integratedAddress(IntPtr wallet_ptr, string payment_id);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_secretViewKey(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_publicViewKey(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_secretSpendKey(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_publicSpendKey(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_publicMultisigSignerKey(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_stop(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_store(IntPtr wallet_ptr, string path);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_filename(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_keysFilename(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_init(IntPtr wallet_ptr, string daemon_address, ulong upper_transaction_size_limit, string daemon_username, string daemon_password, bool use_ssl, bool lightWallet, string proxy_address);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_createWatchOnly(IntPtr wallet_ptr, string path, string password, string language);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setRefreshFromBlockHeight(IntPtr wallet_ptr, ulong refresh_from_block_height);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_getRefreshFromBlockHeight(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setRecoveringFromSeed(IntPtr wallet_ptr, bool recoveringFromSeed);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setRecoveringFromDevice(IntPtr wallet_ptr, bool recoveringFromDevice);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setSubaddressLookahead(IntPtr wallet_ptr, uint major, uint minor);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_connectToDaemon(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Wallet_connected(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setTrustedDaemon(IntPtr wallet_ptr, bool arg);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_trustedDaemon(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setProxy(IntPtr wallet_ptr, string address);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_balance(IntPtr wallet_ptr, uint accountIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_unlockedBalance(IntPtr wallet_ptr, uint accountIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_viewOnlyBalance(IntPtr wallet_ptr, uint accountIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_watchOnly(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_isDeterministic(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_blockChainHeight(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_approximateBlockChainHeight(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_estimateBlockChainHeight(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_daemonBlockChainHeight(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_daemonBlockChainTargetHeight(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_synchronized(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_displayAmount(ulong amount);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_amountFromString(string amount);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_amountFromDouble(double amount);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_genPaymentId();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_paymentIdValid(string paiment_id);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_addressValid(string str, int nettype);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_keyValid(string secret_key_string, string address_string, bool isViewKey, int nettype);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_keyValid_error(string secret_key_string, string address_string, bool isViewKey, int nettype);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_paymentIdFromAddress(string strarg, int nettype);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_maximumAllowedAmount();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_init3(IntPtr wallet_ptr, string argv0, string default_log_base_name, string log_path, bool console);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getPolyseed(IntPtr wallet_ptr, string passphrase);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_createPolyseed(string language);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_startRefresh(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_pauseRefresh(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_refresh(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_refreshAsync(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_rescanBlockchain(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_rescanBlockchainAsync(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setAutoRefreshInterval(IntPtr wallet_ptr, int millis);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Wallet_autoRefreshInterval(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_addSubaddressAccount(IntPtr wallet_ptr, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_numSubaddressAccounts(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_numSubaddresses(IntPtr wallet_ptr, uint accountIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_addSubaddress(IntPtr wallet_ptr, uint accountIndex, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getSubaddressLabel(IntPtr wallet_ptr, uint accountIndex, uint addressIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setSubaddressLabel(IntPtr wallet_ptr, uint accountIndex, uint addressIndex, string label);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_multisig(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getMultisigInfo(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_makeMultisig(IntPtr wallet_ptr, string info, string info_separator, uint threshold);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_exchangeMultisigKeys(IntPtr wallet_ptr, string info, string info_separator, bool force_update_use_with_caution);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_exportMultisigImages(IntPtr wallet_ptr, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_importMultisigImages(IntPtr wallet_ptr, string info, string info_separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_hasMultisigPartialKeyImages(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_restoreMultisigTransaction(IntPtr wallet_ptr, string signData);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_createTransactionMultDest(IntPtr wallet_ptr, string dst_addr_list, string dst_addr_list_separator, string payment_id, bool amount_sweep_all, string amount_list, string amount_list_separator, uint mixin_count, int pendingTransactionPriority, uint subaddr_account, string preferredInputs, string preferredInputs_separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_createTransaction(IntPtr wallet_ptr, string dst_addr, string payment_id, ulong amount, uint mixin_count, int pendingTransactionPriority, uint subaddr_account, string preferredInputs, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_loadUnsignedTx(IntPtr wallet_ptr, string unsigned_filename);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_loadUnsignedTxUR(IntPtr wallet_ptr, string input);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_submitTransaction(IntPtr wallet_ptr, string fileName);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_submitTransactionUR(IntPtr wallet_ptr, string input);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_hasUnknownKeyImages(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_exportKeyImages(IntPtr wallet_ptr, string filename, bool all);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_exportKeyImagesUR(IntPtr wallet_ptr, uint max_fragment_length, bool all) ;

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_importKeyImages(IntPtr wallet_ptr, string filename);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_importKeyImagesUR(IntPtr wallet_ptr, string input);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_exportOutputs(IntPtr wallet_ptr, string filename, bool all);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_exportOutputsUR(IntPtr wallet_ptr, uint max_fragment_length, bool all);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_importOutputs(IntPtr wallet_ptr, string filename);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_importOutputsUR(IntPtr wallet_ptr, string input);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setupBackgroundSync(IntPtr wallet_ptr, int background_sync_type, string wallet_password, string background_cache_password);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Wallet_getBackgroundSyncType(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_startBackgroundSync(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_stopBackgroundSync(IntPtr wallet_ptr, string wallet_password);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_isBackgroundSyncing(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_isBackgroundWallet(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_history(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_addressBook(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_coins(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_subaddress(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_Wallet_subaddressAccount(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_defaultMixin(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setDefaultMixin(IntPtr wallet_ptr, uint arg);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setCacheAttribute(IntPtr wallet_ptr, string key, string val);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getCacheAttribute(IntPtr wallet_ptr, string key);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_setUserNote(IntPtr wallet_ptr, string txid, string note);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getUserNote(IntPtr wallet_ptr, string txid);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_getTxKey(IntPtr wallet_ptr, string txid);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_signMessage(IntPtr wallet_ptr, string message, string address);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_verifySignedMessage(IntPtr wallet_ptr, string message, string address, string signature);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_rescanSpent(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setOffline(IntPtr wallet_ptr, bool offline);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_isOffline(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_segregatePreForkOutputs(IntPtr wallet_ptr, bool segregate);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_segregationHeight(IntPtr wallet_ptr, ulong height);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_keyReuseMitigation2(IntPtr wallet_ptr, bool mitigation);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_lockKeysFile(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_unlockKeysFile(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_isKeysFileLocked(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_Wallet_getDeviceType(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_coldKeyImageSync(IntPtr wallet_ptr, ulong spent, ulong unspent);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_Wallet_deviceShowAddress(IntPtr wallet_ptr, uint accountIndex, uint addressIndex);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_reconnectDevice(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_getBytesReceived(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_Wallet_getBytesSent(IntPtr wallet_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_getStateIsConnected();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern char MONERO_Wallet_getSendToDevice();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_getSendToDeviceLength();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern char MONERO_Wallet_getReceivedFromDevice();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint MONERO_Wallet_getReceivedFromDeviceLength();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_getWaitsForDeviceSend();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_Wallet_getWaitsForDeviceReceive();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setDeviceReceivedData(char data, uint len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_Wallet_setDeviceSendData(char data, uint len);

        #endregion

        #region Wallet Manager

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManagerFactory_getWalletManager();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_WalletManagerFactory_setLogLevel(int level);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_WalletManagerFactory_setLogCategories(string categories);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_createWallet(IntPtr wm, string path, string password, string language, int networkType);
        
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_openWallet(IntPtr wm, string path, string password, int networkType);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_recoveryWallet(IntPtr wm_ptr, string path, string password, string mnemonic, int networkType, ulong restoreHeight, ulong kdfRounds, string seedOffset);


        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_createWalletFromKeys(IntPtr wm, string path, string password, string language, int nettype, ulong restoreHeight, string addressString, string viewKeyString, string spendKeyString, ulong kdf_rounds);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_createDeterministicWalletFromSpendKey(IntPtr wm, string path, string password, string language, int nettype, ulong restoreHeight, string spendKeyString, ulong kdf_rounds);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_createWalletFromDevice(IntPtr wm_ptr, string path, string password, int nettype, string deviceName, ulong restoreHeight, string subaddressLookahead, string viewKeyString, string spendKeyString, ulong kdf_rounds);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MONERO_WalletManager_createWalletFromPolyseed(IntPtr wm, string path, string password, int nettype, string mnemonic, string passphrase, bool newWallet, ulong restore_height, ulong kdf_rounds);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_closeWallet(IntPtr wm, IntPtr walletPtr, bool store);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_walletExists(IntPtr wm_ptr, string path);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_verifyWalletPassword(IntPtr wm_ptr, string keys_file_name, string password, bool no_spend_key, ulong kdf_rounds);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MONERO_WalletManager_queryWalletDevice(IntPtr wm_ptr, string keys_file_name, string password, ulong kdf_rounds);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_WalletManager_findWallets(IntPtr wm_ptr, string path, string separator);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_WalletManager_errorString(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void MONERO_WalletManager_setDaemonAddress(IntPtr wm_ptr, string address);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_WalletManager_blockchainHeight(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_WalletManager_blockchainTargetHeight(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_WalletManager_networkDifficulty(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double MONERO_WalletManager_miningHashRate(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong MONERO_WalletManager_blockTarget(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_isMining(IntPtr wm_ptr);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_startMining(IntPtr wm_ptr, string address, uint threads, bool backgroundMining, bool ignoreBattery);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_stopMining(IntPtr wm_ptr, string address);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern string MONERO_WalletManager_resolveOpenAlias(IntPtr wm_ptr, string address, bool dnssec_valid);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool MONERO_WalletManager_setProxy(IntPtr wm_ptr, string address);

        #endregion


    }

    public class MoneroWalletManager {
        private readonly IntPtr _manager;
        private static MoneroWalletManager? _instance;

        public static MoneroWalletManager Instance
        {
            get
            {
                _instance ??= new MoneroWalletManager();

                return _instance;
            }
        }

        private MoneroWalletManager()
        {
            _manager = MoneroWallet2Api.MONERO_WalletManagerFactory_getWalletManager();
            if (_manager == IntPtr.Zero)
                throw new MoneroError("Could not get wallet manager instance");
        }

        public IntPtr CreateWallet(string path, string password, string language, int networkType)
        {
            IntPtr wallet = MoneroWallet2Api.MONERO_WalletManager_createWallet(_manager, path, password, language, networkType);
            if (wallet == IntPtr.Zero)
                throw new MoneroError("Could not create wallet handle");
            
            return wallet;
        }

        public IntPtr CreateWalletFromKeys(string path, string password, string language, int nettype, ulong restoreHeight, string addressString, string viewKeyString, string spendKeyString, ulong kdf_rounds) {
            IntPtr wallet = MoneroWallet2Api.MONERO_WalletManager_createWalletFromKeys(_manager, path, password, language, nettype, restoreHeight, addressString, viewKeyString, spendKeyString, kdf_rounds);

            if (wallet == IntPtr.Zero)
                throw new MoneroError("Could not create wallet handle");

            return wallet;
        }

        public IntPtr RecoveryWallet(string path, string password, string mnemonic, int networkType, ulong restoreHeight, ulong kdfRounds, string seedOffset)
        {
            IntPtr wallet = MoneroWallet2Api.MONERO_WalletManager_recoveryWallet(_manager, path, password, mnemonic, networkType, restoreHeight, kdfRounds, seedOffset);

            if (wallet == IntPtr.Zero)
                throw new MoneroError("Could not create wallet handle");

            return wallet;
        }
    }
}
