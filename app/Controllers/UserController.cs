using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using app.Models;
using freelancerzy.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System;

namespace app.Controllers
{
    public class UserController : Controller
    {
        private readonly cb2020freedbContext _context;

        //private readonly ILogger<UserController> _logger;
        public UserController(cb2020freedbContext context)
        {
            _context = context;
        }

        //public UserController(ILogger<UserController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Login(string ReturnUrl = "/Home/Index")
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuthentication");

            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password,string ReturnUrl) //TODO: pass user credentials
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            var user = _context.PageUser.FirstOrDefault(user => user.EmailAddress == email);
            if (user == null)
            {
                ViewData["error"] = "Podana nazwa u¿ytkownika nie istnieje";
                return View();
            }
            else
            {
                user.Credentials = _context.Credentials.FirstOrDefault(u => u.Userid == user.Userid);
                user.Type = _context.Usertype.FirstOrDefault(u => u.Typeid == user.TypeId);
                if (ValideteUser(user, password))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.EmailAddress),
                        new Claim(ClaimTypes.Role,user.Type.Name),
                       
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");

                    await HttpContext.SignInAsync("CookieAuthentication", new
                    ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                    });
                    return Redirect(ReturnUrl);
                }
            }
            ViewData["error"] = "Podano z³e has³o";
            return View();
        }
        private bool ValideteUser(PageUser user, string password)
        {

            var hasher = new PasswordHasher<string>();
            if (hasher.VerifyHashedPassword(user.EmailAddress, user.Credentials.Password, password) == PasswordVerificationResult.Success)
            {
                return true;
            }
            else return false;
        }
        [HttpPost]
        public async Task<IActionResult> Register(string name, string surname, int phoneNumber, string email, string password , string passwordConfirmed) //TODO: pass user credentials
        {
            if (ModelState.IsValid)
            {
                PageUser pageUser = new PageUser() // TODO SPRAWDZIC CZY NIE MA USERA W BAZIE
                {
                    FirstName = name,
                    Surname = surname,
                    EmailAddress = email,                                    
                    Phonenumber = phoneNumber,
                    TypeId = 1,
                };
                var passwordHasher = new PasswordHasher<string>();
                Credentials credentials = new Credentials()
                {
                    Password = passwordHasher.HashPassword(email, password),                    
                };
                var passwordHasherConfirmation = new PasswordHasher<string>();
                if (passwordHasherConfirmation.VerifyHashedPassword(null,credentials.Password,passwordConfirmed) == PasswordVerificationResult.Success) // strawdzic czy nie ma takiego usera
                {
                    pageUser.Credentials = credentials;
                    _context.Add(pageUser);
                    _context.Add(credentials);
                    await _context.SaveChangesAsync();
                    ViewBag.message = "ok";
                }
                else
                {
                    ViewData["Error"] = "Hasla nie sa taki same";
                    return View();
                }
            }
            
            //TODO: check user credentials
            return RedirectToAction("Index","Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}