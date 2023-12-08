using GeneralStoreMVC.Data;
using GeneralStoreMVC.Data.Entities;
using GeneralStoreMVC.Models.Transaction;
using GeneralStoreMVC.Services.Product;
using GeneralStoreMVC.Services.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.MVC.Controllers;
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly GeneralStoreDbContext _ctx;
    private readonly IProductService _productService;

    public TransactionController(ITransactionService transactionService, GeneralStoreDbContext dbContext, IProductService productService)
    {
        _transactionService = transactionService;
        _ctx = dbContext;
        _productService = productService;
    }

    // GET
    public async Task<IActionResult> Create(int customerId)
    {
        TransactionCreateVM model = new()
        {
            CustomerId = customerId
        };

        var productList = await _ctx.Products 
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Price:C})",
            })
            .ToListAsync();

        ViewData["Products"] = productList;

        return View(model);
    }

    // POST
    // [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] int customerId, TransactionCreateVM model)
    {
        if (customerId != model.CustomerId)
            return RedirectToAction("Index", "Customer");

        var response = await _transactionService.CreateTransactionAsync(customerId, model);

        if (response)
            return RedirectToAction("Details", "Customer", new { id = customerId});

        return View(model);
    }

    // GET
    // public async Task<IActionResult> Create(int customerId)
    // {
    //     TransactionCreateVM model = new()
    //     {
    //         CustomerId = customerId
    //     };

    //     var products = await _productService.GetAllProductsAsync();
    //     List<SelectListItem> productSelectList = new();
    //     foreach (var product in products)
    //     {
    //         productSelectList.Add(new SelectListItem
    //         {
    //             Value = product.Id.ToString(),
    //             Text = $"{product.Name}"
    //         });
    //     }

    //     ViewData["Products"] = productSelectList;

    //     return View(model);
    // }
    // public async Task<IActionResult> Create(int customerId, TransactionCreateVM model)
    // {
    //     if (customerId != model.CustomerId)
    //         return RedirectToAction("Index", "Customer");

    //     var entity = new TransactionEntity
    //     {
    //         ProductId = model.ProductId,
    //         CustomerId = model.CustomerId,
    //         Quantity = model.Quantity,
    //         DateOfTransaction = DateTime.Now
    //     };

    //     _ctx.Transactions.Add(entity);

    //     if (await _ctx.SaveChangesAsync() == 1)
    //         return RedirectToAction("Details", "Customer", new { id = customerId});

    //     return View(model);
    // }
}