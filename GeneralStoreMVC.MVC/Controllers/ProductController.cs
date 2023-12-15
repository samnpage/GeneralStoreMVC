using GeneralStoreMVC.Models.Product;
using GeneralStoreMVC.Models.Responses;
using GeneralStoreMVC.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStoreMVC.MVC.Controllers;
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetAllProductsAsync());
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    // GET: Product/Create
    public async Task<IActionResult> Create()
    {
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,QuantityInStock,Price")] ProductCreateVM product)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateNewProductAsync(product);
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await _productService.GetProductByIdAsync(id);

        ProductEditVM edit = new()
        {
            Name = product.Name,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock
        };
        if (edit == null)
            return NotFound();

        return View(edit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,QuantityInStock,Price")] ProductEditVM product)
    {
        if (id != product.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var edit = await _productService.EditProductAsync(id, product);
            if (edit)
                return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _productService.DeleteProductByIdAsync(id);
        if (response.message == $"Product #{id} does not exist")
        {
            TempData["ErrorMsg"] = $"Product #{id} does not exist";
            return RedirectToAction(nameof(Index));
        }
            
        if (response.message == $"Cannot delete Product #{id}")
        {
            TempData["ErrorMsg"] = $"Cannot delete Product #{id}";
        }

        return RedirectToAction(nameof(Index));
    }
}