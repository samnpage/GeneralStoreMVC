using GeneralStoreMVC.Models.Transaction;

namespace GeneralStoreMVC.Services.Transaction;
public interface ITransactionService
{
    // Create
    Task<bool> CreateTransactionAsync(int customerId, TransactionCreateVM model);
}