using ACMWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ACMWeb.Controllers
{
        [Authorize("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static List<Product> products;
        public IWebHostEnvironment env;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _env)
        {
            env = _env;
            _logger = logger;
            if(products == null)
                products = Product.GetProduct();
        }
        public IActionResult Index()
        {
            ViewData["lastPoductList"] = products.Where(x => x.Id > 5).ToList();
            ViewBag.lastPoductList = products.Where(x => x.Id > 5).ToList();
            return View(products);
        }
        [HttpPost]
        public IActionResult Index(Product newProduct, IFormFile file)
        {
            if (file != null)
            {
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   
            }
            products.Add(newProduct);
            return RedirectToAction("Index");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        [AllowAnonymous]
        public IActionResult GetPartialTable()
        {
            return PartialView("~/Views/Shared/_ProductListTable.cshtml",products);
        }
        [HttpPost]
        public JsonResult CreateProduct(Product newProduct)
        {
            products.Add(newProduct);
            return Json(newProduct);
        }
        public IActionResult Update(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Update(Product updateProduct)
        {
            var product = products.FirstOrDefault(x => x.Id == updateProduct.Id);
            product.Name = updateProduct.Name;
            product.Quantity = updateProduct.Quantity;
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(x => x.Id == id);
            products.Remove(product);
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Store"),
            };
            // Learn more about ClaimsIdentity -> https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimsidentity?view=netframework-4.8

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // This is optional configuration
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(20)
            };
            await HttpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme,
              new ClaimsPrincipal(claimsIdentity),
              authProperties
            );

            await HttpContext.SignOutAsync();
            return View("/Dashboard");
        }


       
    
    }
}
