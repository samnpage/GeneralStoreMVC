using GeneralStoreMVC.Models.Customer;
using GeneralStoreMVC.Services.Customer;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStoreMVC.MVC.Controllers;
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index()
    {
        List<CustomerIndexViewModel> customers = await _customerService.GetAllCustomersAsync();

        return View(customers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMsg"] = "Model State is invalid.";
            return View(model);
        }

        if (await _customerService.CreateCustomerAsync(model) is false)
        {
            TempData["ErrorMsg"] = "Unable to save to the database. Please try again later.";
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: customer/details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var customer = await _customerService.GetCustomerByIdAsync(id);

        if (customer is null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(customer);
    }

    // GET: customer/edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return RedirectToAction(nameof(Index));
        }
        
        var customer = await _customerService.GetEditCustomerByIdAsync(id);

        if (customer is null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CustomerEditViewModel model)
    {
        var customer = _customerService.EditCustomerByIdAsync(id, model);
        if (customer == null)
        {
            return NotFound();
        }

        if (customer != null)
        {
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMsg"] = "Unable to save to the database. Please try again later.";
        return View(model);
    }

    // GET: customer/delete/{id}
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _customerService.DeleteCustomerAsync(id);

        if (entity == false)
        {
            TempData["ErrorMsg"] = $"Customer #{id} does not exist";
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}