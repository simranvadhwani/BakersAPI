using Bakers.ContextClass;
using Bakers.Models;
using Bakers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bakers.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly BakersDbContext _dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public CartController(BakersDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;

        }

        [HttpGet]
        [Route("getCartData")]
        public IActionResult GetCartData()
        {
            try
            {
                var cartData = (from p in _dbContext.products
                                join c in _dbContext.carts on p.productId equals c.ProductId
                                select new { Product = p, Cart = c }).ToList();
                var productViewModels = new List<ProductViewModel>();
                if (cartData.Count > 0)
                {
                    foreach (var item in cartData)
                    {
                        var productViewModel = new ProductViewModel();
                        productViewModel.productId = item.Product.productId;
                        productViewModel.Price = item.Cart.Price;
                        productViewModel.Name = item.Product.Name;
                        productViewModel.Quantity = item.Cart.Quantity;
                        productViewModels.Add(productViewModel);
                    }
                    return Ok(productViewModels);
                }
                else
                {
                    return BadRequest("No Cart Products Available");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
