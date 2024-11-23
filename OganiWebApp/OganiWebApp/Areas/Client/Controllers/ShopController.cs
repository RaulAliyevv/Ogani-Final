using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database;
using OganiWebApp.Extensions;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("shop")]
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public ShopController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        [HttpGet("index", Name = "client-shop-index")]
        public IActionResult Index(int? CategoryId = null, int? SizeId = null , string? Search = null , int ? MaxPrice = null , int? MinPrice = null)
        {
            var product = _dataContext.Products
                .Include(p => p.PorductCategory)
                .AsEnumerable();

            if (CategoryId != null)
            {
                product = product.Where(p => p.PorductCategoryId == CategoryId);
            }
           
            else if (!string.IsNullOrWhiteSpace(Search))
            {
                product = product.Where(p => p.Name.Contains(Search) ||
                p.Shipping.Contains(Search) ||
                p.PorductCategory.Title.Contains(Search));
            }
            else if (!IsNullOrValueExtension.IsNullOrDefault(MinPrice) || !IsNullOrValueExtension.IsNullOrDefault(MaxPrice))
            {
                product = product.Where(p => p.Price != p.DiscountPrice ? p.DiscountPrice >= MinPrice && p.DiscountPrice <= MaxPrice : p.Price >= MinPrice && p.Price <= MaxPrice);
            }

            product = product.ToList();
          
            var model = new ShopViewModel
            {
                ProductSizes = _dataContext.Sizes.ToList(),
                ProductCategories = _dataContext.ProductCategories.ToList(),
                DiscountedProducts = _dataContext.Products.Where(x => x.Price > x.DiscountPrice)
                .Select(x => new ProductListViewModel(x.Id, x.MainImageName, _fileService.GetFileUrl(x.MainImageNameInFileSystem, Contracts.UploadDirectory.Proudct),
                x.Name, x.Shipping, x.Weight, x.Description, x.RemindCount, x.Price, x.DiscountPrice, x.PorductCategoryId)
               ).ToList(),
                Products = product
                .Select(x => new ProductListViewModel(x.Id, x.MainImageName, _fileService.GetFileUrl(x.MainImageNameInFileSystem, Contracts.UploadDirectory.Proudct),
                x.Name, x.Shipping, x.Weight, x.Description, x.RemindCount, x.Price, x.DiscountPrice, x.PorductCategoryId)
               ).ToList(),
            };
            return View(model);
        }
        
        [HttpGet("product-filter", Name = "client-product-filter")]
        public async Task<IActionResult> FilterAsync(ShopViewModel model, string Search = null)
        {
            return RedirectToRoute("client-shop-index", new { CategoryId = model.CategoryId,  Search = Search, MaxPrice = model.MaxPrice, MinPrice = model.MinPrice });
        }

        [HttpPost("product-search", Name = "client-product-search")]
        public async Task<IActionResult> SearchAsync(string? Search = null)
        {
            return RedirectToRoute("client-shop-index", new { Search = Search });
        }

        [HttpGet("item/{}", Name = "client-shop-index")]
        public async Task<IActionResult> Item(int id)
        {

        }
    }
}
