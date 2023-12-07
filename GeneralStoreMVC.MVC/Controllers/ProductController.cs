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
}