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
        public ProductController(BakersDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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

        [HttpGet]
        [Route("productGetById")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var getProducts = _dbContext.products.Where(p => p.productId == id).FirstOrDefault();
                if (getProducts != null)
                {
                    var productViewModel = new ProductViewModel();
                    productViewModel.productId = getProducts.productId;
                    productViewModel.Price = getProducts.Price;
                    productViewModel.Name = getProducts.Name;
                    productViewModel.Discription = getProducts.Discription;
                    return Ok(productViewModel);
                }
                else
                {
                    return NotFound("Product with ID " + id + " does not exist.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);

            }
        }
        [HttpPost]
        [Route("addProductToCart")]
        public IActionResult AddProductToCart([FromBody] ProductViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var cartData = _dbContext.carts.FirstOrDefault(p => p.ProductId == model.productId);
                    if (cartData != null)
                    {
                        // Update the existing cart data
                        cartData.Quantity += model.Quantity;
                        cartData.Price += model.Price * model.Quantity;
                        cartData.CreatedDate = DateTime.Now;
                        _dbContext.carts.Update(cartData);
                        _dbContext.SaveChanges();

                        // Retrieve total count of items in the cart
                        int cartItemCount = _dbContext.carts.Count(); // Modify this according to your data model

                        return Ok(new
                        {
                            Message = "Product updated in cart.",
                            CartId = cartData.Id,
                            CartLength = cartItemCount
                        });
                    }
                    else
                    {
                        // Add a new cart entry
                        cartData = new Cart
                        {
                            ProductId = model.productId,
                            Quantity = model.Quantity,
                            Price = model.Price * model.Quantity,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.carts.Add(cartData);
                        _dbContext.SaveChanges();

                        // Retrieve total count of items in the cart
                        int cartItemCount = _dbContext.carts.Count(); 

                        return Ok(new
                        {
                            Message = "Product added to cart.",
                            CartId = cartData.Id,
                            CartLength = cartItemCount
                        });
                    }
                }
                else
                {
                    return BadRequest("Invalid product data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet]
        [Route("getCartProducts")]
        public IActionResult GetCartProducts()
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
                        productViewModel.Price = item.Product.Price;
                        productViewModel.Name = item.Product.Name;
                        productViewModel.Discription = item.Product.Discription;
                        productViewModel.Quantity = item.Cart.Quantity;
                        productViewModels.Add(productViewModel);
                    }
                    return Ok(productViewModels); 
                }
                else
                {
                    return BadRequest("No Products Available");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


    }
}
