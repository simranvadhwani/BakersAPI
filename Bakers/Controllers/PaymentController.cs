using Bakers.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace Bakers.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly string keyId;
        private readonly string keySecret;
        public PaymentController(IConfiguration configuration)
        {
            keyId = configuration["Razorpay:KeyId"];
            keySecret = configuration["Razorpay:KeySecret"];
        }
        [HttpPost("order")]
        public async Task<string> CreateOrder(OrderVM od)
        {
            var client = new RazorpayClient(keyId, keySecret);
            var options = new Dictionary<string, object>
        {
            { "amount", od.amount * 100 }, // Razorpay accepts amount in paise
            { "currency", "INR" },
            // Add more options as needed
        };
            var order = client.Order.Create(options);
            return order["id"].ToString();
        }
    }
}
