using Bakers.ContextClass;
using Bakers.Models;
using Bakers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bakers.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly BakersDbContext _dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public CustomerController(BakersDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;

        }
        [HttpPost]
        [Route("addCustomerData")]
        public IActionResult AddCustomerData([FromBody] CustomerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var customerData = _dbContext.customer.FirstOrDefault(p => p.CustomerId == model.CustomerId);
                    if (customerData != null)
                    {
                        // Update the existing customer data
                        customerData.Name = model.Name;
                        customerData.Address = model.Address;
                        customerData.City = model.City;
                        customerData.State = model.State;
                        customerData.Zip = model.Zip;
                        customerData.Email = model.Email;
                        customerData.MobileNo = model.MobileNo;
                        model.EditedDate = DateTime.Now;
                        _dbContext.customer.Update(customerData);
                        _dbContext.SaveChanges();
                        return Ok(new
                        {
                            Message = "Customer updated."
                        });
                    }
                    else
                    {
                        customerData = new Customer
                        {
                            Name = model.Name,
                            Address = model.Address,
                            City = model.City,
                            State = model.State,
                            Zip = model.Zip,
                            Email = model.Email,
                            MobileNo = model.MobileNo,
                            AddedDate = DateTime.Now
                        };
                        _dbContext.customer.Add(customerData);
                        _dbContext.SaveChanges();

                        return Ok(new
                        {
                            Message = "Customer added."
                        });
                    }
                }
                else
                {
                    return BadRequest("Invalid customer data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
