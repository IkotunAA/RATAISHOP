//using Microsoft.AspNetCore.Mvc;
//using GOODIFY.Data;
//using Newtonsoft.Json.Linq;
//using GOODIFY.Context;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace GOODIFY.Controllers
//{

//    public class CheckoutController : Controller
//    {
//        private readonly GoodifyDbContext _context;

//        public CheckoutController()
//        {
//            _context = new GoodifyDbContext();
//        }

//        [HttpPost]
//        public async Task<ActionResult> Checkout(string paymentReference)
//        {
//            var verificationResult = await VerifyPayment(paymentReference);

//            if (verificationResult != null && verificationResult["data"]["status"].ToString() == "success")
//            {
//                var cartItems = _context.CartItems.ToList();
//                _context.CartItems.RemoveRange(cartItems);
//                _context.SaveChanges();

//                return RedirectToAction("OrderConfirmation");
//            }

//            TempData["Error"] = "Payment verification failed. Please try again.";
//            return RedirectToAction("Cart");
//        }

//        private async Task<JObject> VerifyPayment(string reference)
//        {
//            var secretKey = System.Configuration.ConfigurationManager.AppSettings["PaystackSecretKey"];

//            using (var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", secretKey);
//                var response = await client.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");

//                if (response.IsSuccessStatusCode)
//                {
//                    var jsonString = await response.Content.ReadAsStringAsync();
//                    return JObject.Parse(jsonString);
//                }
//            }

//            return null;
//        }

//        public ActionResult OrderConfirmation()
//        {
//            return View();
//        }
//    }

//}
