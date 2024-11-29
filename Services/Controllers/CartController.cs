//using Microsoft.AspNetCore.Mvc;
//using GOODIFY.Data;
//using GOODIFY.Entities;
//using Microsoft.AspNet.Identity;
//using GOODIFY.Context;
//using RATAISHOP.Context;

//namespace GOODIFY.Controllers
//{
//    public class CartController : Controller
//    {
//        private readonly RataiDbContext _context;

//        public CartController()
//        {
//            _context = new RataiDbContext();
//        }

//        public ActionResult Index()
//        {
//            var userId = User.Identity.GetUserId();
//            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();
//            return View(cartItems);
//        }

//        public ActionResult AddToCart(int productId)
//        {
//            var userId = User.Identity.GetUserId();
//            var cartItem = new CartItem { UserId = userId, ProductId = productId, Quantity = 1 };
//            _context.CartItems.Add(cartItem);
//            _context.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        public ActionResult RemoveFromCart(int id)
//        {
//            var cartItem = _context.CartItems.Find(id);
//            if (cartItem != null)
//            {
//                _context.CartItems.Remove(cartItem);
//                _context.SaveChanges();
//            }
//            return RedirectToAction("Index");
//        }
//    }

//}
