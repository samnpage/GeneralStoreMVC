using GeneralStoreMVC.Models.Product;
using GeneralStoreMVC.Models.Responses;

namespace GeneralStoreMVC.Services.Product;
public interface IProductService
{
    // Create
    Task<bool> CreateNewProductAsync(ProductCreateVM product);

    // Read
    Task<List<ProductIndexVM>> GetAllProductsAsync();
    Task<ProductDetailVM> GetProductByIdAsync(int? id);
    Task<ProductEditVM> GetEditProductAsync(int? id);

    // Update
    Task<bool> EditProductAsync(int id, ProductEditVM product);

    // Delete
    Task<TextResponse> DeleteProductByIdAsync(int id);
}