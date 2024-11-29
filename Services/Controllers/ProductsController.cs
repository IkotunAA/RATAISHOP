//using Microsoft.AspNetCore.Mvc;
//using GOODIFY.Entities;
//using Microsoft.AspNet.Identity;
//using GOODIFY.Context;

//namespace GOODIFY.Controllers
//{

//    public class ProductsController : Controller
//    {
//        private readonly GoodifyDbContext _context;

//        public ProductsController()
//        {
//            _context = new GoodifyDbContext();
//        }

//        public ActionResult Index()
//        {
//            var products = _context.Products.ToList();
//            return View(products);
//        }

//        [HttpGet]
//        public ActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Create(Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                product.SellerId = User.Identity.GetUserId();
//                _context.Products.Add(product);
//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(product);
//        }

//        [HttpGet]
//        public ActionResult Edit(int id)
//        {
//            var product = _context.Products.Find(id);
//            return View(product);
//        }

//        [HttpPost]
//        public ActionResult Edit(Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Entry(product).State = System.Data.Entity.EntityState.Modified;
//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(product);
//        }
//    }

//}
