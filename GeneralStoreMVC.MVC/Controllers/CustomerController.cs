using GeneralStoreMVC.Data;
using GeneralStoreMVC.Data.Entities;
using GeneralStoreMVC.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.MVC.Controllers;
public class CustomerController : Controller
{
    private readonly GeneralStoreDbContext _ctx;
    public CustomerController(GeneralStoreDbContext dbContext)
    {
        _ctx = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        List<CustomerIndexViewModel> customers = await _ctx.Customers
            .Select(customer => new CustomerIndexViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            })
            .ToListAsync();

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

        CustomerEntity entity = new()
        {
            Name = model.Name,
            Email = model.Email
        };

        _ctx.Customers.Add(entity);

        if (await _ctx.SaveChangesAsync() != 1)
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

        var entity = await _ctx.Customers.FindAsync(id);
        if (entity is null)
        {
            return RedirectToAction(nameof(Index));
        }

        CustomerDetailViewModel model = new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email
        };

        return View(model);
    }
}