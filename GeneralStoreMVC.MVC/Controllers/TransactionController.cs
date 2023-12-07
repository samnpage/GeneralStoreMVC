using GeneralStoreMVC.Data;
using GeneralStoreMVC.Models.Transaction;
using GeneralStoreMVC.Services.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreMVC.MVC.Controllers;
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    private readonly GeneralStoreDbContext _ctx;

    public TransactionController(ITransactionService transactionService, GeneralStoreDbContext dbContext)
    {
        _transactionService = transactionService;
        _ctx = dbContext;
    }

    public async Task<IActionResult> Create(int customerId)
    {
        var productList = await _ctx.Products
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} ({p.Price:C})",
            })
            .ToListAsync();

        ViewData["Products"] = productList;

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