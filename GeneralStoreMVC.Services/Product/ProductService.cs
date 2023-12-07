using GeneralStoreMVC.Data;

namespace GeneralStoreMVC.Services.Product;
public class ProductService : IProductService
{
    private readonly GeneralStoreDbContext _ctx;

    public ProductService(GeneralStoreDbContext dbContext)
    {
        _ctx = dbContext;
    }
}