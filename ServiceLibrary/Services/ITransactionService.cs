using ModelLibrary.ViewModels;

namespace ServiceLibrary.Services
{
    public interface ITransactionService
    {
        public Task<List<TransactionViewModel>> GetTransactionsAsync(int id);
        public List<TransactionViewModel> GetTransactions(int id);
        public Task<string> MakeTransaction(int id, string type, decimal amount, string transferId, DateTime transactionDate, string comment);
    }
}
