using GeneralStoreMVC.Data;
using GeneralStoreMVC.Data.Entities;
using GeneralStoreMVC.Models.Product;
using GeneralStoreMVC.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.Services.Product;
public class ProductService : IProductService
{
    private readonly GeneralStoreDbContext _ctx;

    public ProductService(GeneralStoreDbContext dbContext)
    {
        _ctx = dbContext;
    }

    // Create
    public async Task<bool> CreateNewProductAsync(ProductCreateVM product)
    {
        ProductEntity entity = new()
        {
            Name = product.Name,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock
        };

        _ctx.Products.Add(entity);
        await _ctx.SaveChangesAsync();
        return true;
    }

    // Read All
    public async Task<List<ProductIndexVM>> GetAllProductsAsync()
    {
        var products = await _ctx.Products
            .Select(p => new ProductIndexVM
            {
                Id = p.Id,
                Name = p.Name,
                QuantityInStock = p.QuantityInStock
            })
            .ToListAsync();

        return products;
    }


    // Read By Id
    public async Task<ProductDetailVM> GetProductByIdAsync(int? id)
    {
        var product = await _ctx.Products.FindAsync(id);

        ProductDetailVM detail= new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock
        };

        return detail;
    }

    // Read By Id
    // public async Task<ProductEditVM> GetEditProductAsync(int? id)
    // {
    //     var product = await _ctx.Products.FindAsync(id);

    //     ProductEditVM model = new()
    //     {
    //         Id = product.Id,
    //         Name = product.Name,
    //         Price = product.Price,
    //         QuantityInStock = product.QuantityInStock
    //     };

    //     return model;
    // }

    // Update
    public async Task<bool> EditProductAsync(int id, ProductEditVM product)
    {
        var entity = await _ctx.Products.FindAsync(id);
        if (entity is null)
            return false;

        entity.Name = product.Name;
        entity.Price = product.Price;
        entity.QuantityInStock = product.QuantityInStock;

        _ctx.Products.Update(entity);
        await _ctx.SaveChangesAsync();

        return true;
    }

    // Delete
    public async Task<TextResponse> DeleteProductByIdAsync(int id)
    {
        var entity = await _ctx.Products
            .Include(c => c.Transactions)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            return new TextResponse($"Product #{id} does not exist");

        if (entity.Transactions.Count > 0)
            _ctx.Transactions.RemoveRange(entity.Transactions);

        _ctx.Products.Remove(entity);

        if (_ctx.SaveChanges() != 1 + entity.Transactions.Count)
            return new TextResponse($"Cannot delete Product #{id}");

        return new TextResponse("Product deleted successfully");
    }
}