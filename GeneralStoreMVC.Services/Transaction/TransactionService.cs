using GeneralStoreMVC.Data;
using GeneralStoreMVC.Data.Entities;
using GeneralStoreMVC.Models.Transaction;

namespace GeneralStoreMVC.Services.Transaction;
public class TransactionService : ITransactionService
{
    private readonly GeneralStoreDbContext _ctx;

    public TransactionService(GeneralStoreDbContext dbContext)
    {
        _ctx = dbContext;
    }

    public async Task<bool> CreateTransactionAsync(int customerId, TransactionCreateVM model)
    {
        TransactionEntity entity = new()
        {
            ProductId = model.ProductId,
            CustomerId = model.CustomerId,
            Quantity = model.Quantity,
            DateOfTransaction = DateTime.Now
        };

        _ctx.Transactions.Add(entity);
        
        return await _ctx.SaveChangesAsync() == 1;
    }
}