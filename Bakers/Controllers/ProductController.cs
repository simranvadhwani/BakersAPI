using Bakers.ContextClass;
using Bakers.Models;
using Bakers.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bakers.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BakersDbContext _dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public ProductController(BakersDbContext dbContext,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) 
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;

        }

        [HttpGet]
        [Route("products")]
        public IActionResult GetProducts()
            {
            try
            {
                var products = _dbContext.products.ToList();
                var productViewModels = new List<ProductViewModel>();
                foreach (var item in products)
                {
                    var productViewModel = new ProductViewModel
                    {
                        productId = item.productId,
                        Name = item.Name,
                        Discription = item.Discription,
                        Price = item.Price
                    };
                    productViewModels.Add(productViewModel);
                }
                return Ok(productViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }
    }
}
