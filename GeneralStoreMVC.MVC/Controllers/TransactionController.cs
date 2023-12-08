using GeneralStoreMVC.Data;
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

    public async Task<IActionResult> Create(int customerId)
    {
        var products = await _productService.GetAllProductsAsync();
        List<SelectListItem> productSelectList = new();
        foreach (var product in products)
        {
            productSelectList.Add(new SelectListItem{Value = product.Id.ToString(), Text = $"{product.Name}"});
        }

        ViewData["Products"] = productSelectList;

        return View(await _transactionService.GetCreateTransactionAsync(customerId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(int customerId, TransactionCreateVM model)
    {
        if (customerId != model.CustomerId)
            return RedirectToAction("Index", "Customer");

        var response = await _transactionService.CreateTransactionAsync(customerId, model);

        if (response)
            return RedirectToAction("Details", "Customer", new { id = customerId});

        return View(model);
    }
}