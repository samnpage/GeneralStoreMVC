using GeneralStoreMVC.Data;
using GeneralStoreMVC.Data.Entities;
using GeneralStoreMVC.Models.Customer;
using GeneralStoreMVC.Models.Transaction;
using GeneralStoreMVC.Services.Transaction;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.Services.Customer;
public class CustomerService : ICustomerService
{
    private readonly GeneralStoreDbContext _ctx;
    public CustomerService(GeneralStoreDbContext dbContext)
    {
        _ctx = dbContext;
    }

    public async Task<List<CustomerIndexViewModel>> GetAllCustomersAsync()
    {
        List<CustomerIndexViewModel> customers = await _ctx.Customers
            .Select(customer => new CustomerIndexViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            })
            .ToListAsync();

        return customers;
    }

    public async Task<bool> CreateCustomerAsync(CustomerCreateViewModel model)
    {
        CustomerEntity entity = new()
        {
            Name = model.Name,
            Email = model.Email
        };

        _ctx.Customers.Add(entity);

        return await _ctx.SaveChangesAsync() == 1;
    }

    // GET: customer/details/{id}
    public async Task<CustomerDetailViewModel> GetCustomerByIdAsync(int? id)
    {
        var entity = await _ctx.Customers
            .Include(c => c.Transactions)
            .ThenInclude(t => t.Product)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            return null;

        var transactions = entity.Transactions
            .Select(t => new TransactionListItem
            {
                ProductName = t.Product.Name,
                Quantity = t.Quantity,
                DateOfTransaction = t.DateOfTransaction,
                Price = t.Product.Price * t.Quantity
            }).ToList();

        CustomerDetailViewModel model = new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Transactions = transactions
        };

        await _ctx.SaveChangesAsync();
        return  model;
    }

    // GET: customer/edit/{id}
    public async Task<CustomerEditViewModel> GetEditCustomerByIdAsync(int? id)
    {
        var entity = await _ctx.Customers.FindAsync(id);

        CustomerEditViewModel model = new()
        {
            Name = entity.Name,
            Email = entity.Email
        };

        await _ctx.SaveChangesAsync();
        return  model;
    }


    public async Task<bool> EditCustomerByIdAsync(int id, CustomerEditViewModel model)
    {
        var entity = _ctx.Customers.Find(id);

        entity.Name = model.Name;
        entity.Email = model.Email;
        
        _ctx.Entry(entity).State = EntityState.Modified;

        return await _ctx.SaveChangesAsync() == 1;
    }

    // GET: customer/delete/{id}
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var entity = await _ctx.Customers
            .Include(c => c.Transactions)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
        {
            return false;
        }

        if (entity.Transactions.Count > 0)
        {
            _ctx.Transactions.RemoveRange(entity.Transactions);
        }

        _ctx.Customers.Remove(entity);
        
        if (_ctx.SaveChanges() != 1 + entity.Transactions.Count)
        {
            return false;
        }

        return true;
    }
}