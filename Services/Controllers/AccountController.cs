//using GOODIFY.Authentication;
//using GOODIFY.Context;
//using GOODIFY.Data;
//using GOODIFY.Entities;
//using GOODIFY.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNetCore.Mvc;
//using RATAISHOP.Context;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;

//public class AccountController : Controller
//{
//    private readonly RataiDbContext _context;

//    public AccountController()
//    {
//        _context = new RataiDbContext();
//    }

//    [HttpPost]
//    public async Task<ActionResult> Register(RegisterViewModel model)
//    {
//        if (ModelState.IsValid)
//        {
//            var user = new ApplicationUser
//            {
//                UserName = model.Username,
//                Email = model.Email,
//                FullName = model.FullName,
//                PhoneNumber = model.PhoneNumber,
//                Address = model.Address,
//                IsSeller = model.IsSeller
//            };

//            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            var result = await userManager.CreateAsync(user, model.Password);

//            if (result.Succeeded)
//            {
//                var token = AuthService.GenerateToken(user.Id, user.UserName, user.Email, user.IsSeller);

//                return Json(new
//                {
//                    Success = true,
//                    Token = token,
//                    UserId = user.Id,
//                    IsSeller = user.IsSeller
//                });
//            }

//            return Json(new { Success = false, Errors = result.Errors });
//        }

//        return Json(new { Success = false, Message = "Invalid registration attempt" });
//    }

//    [HttpPost]
//    public async Task<ActionResult> Login(LoginViewModel model)
//    {
//        var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

//        ApplicationUser user = await userManager.FindByEmailAsync(model.Identifier) ?? await userManager.FindByNameAsync(model.Identifier);

//        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
//        {
//            var token = AuthService.GenerateToken(user.Id, user.UserName, user.Email, user.IsSeller);

//            return Json(new
//            {
//                Success = true,
//                Token = token,
//                UserId = user.Id,
//                IsSeller = user.IsSeller
//            });
//        }

//        return Json(new { Success = false, Message = "Invalid login attempt" });
//    }
//}
