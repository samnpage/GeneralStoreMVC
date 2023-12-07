using GeneralStoreMVC.Models.Customer;

namespace GeneralStoreMVC.Services.Customer;
public interface ICustomerService
{
    // Create
    Task<bool> CreateCustomerAsync(CustomerCreateViewModel model);

    // Read
    Task<List<CustomerIndexViewModel>> GetAllCustomersAsync();
    Task<CustomerDetailViewModel> GetCustomerByIdAsync(int? id);
    Task<CustomerEditViewModel> GetEditCustomerByIdAsync(int? id);

    // Edit
    Task<bool> EditCustomerByIdAsync(int id, CustomerEditViewModel model);

    // Delete
    Task<bool> DeleteCustomerAsync(int id);
}