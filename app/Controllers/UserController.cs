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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Edit()
        {
            //TODO: error handling
            String email = this.User.Identity.Name;
            if (email == null) return NotFound();
            var user = _context.PageUser.Include(u => u.Credentials).Include(t => t.Type).Include(a => a.Useraddress).FirstOrDefault(u => u.EmailAddress == email);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
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
        public async Task<IActionResult> Login(string email, string password, string ReturnUrl) //TODO: pass user credentials
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            var user = _context.PageUser.FirstOrDefault(user => user.EmailAddress == email);
            if (user == null)
            {
                ViewData["error"] = "Podana nazwa u�ytkownika nie istnieje";
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
            ViewData["error"] = "Podano złe hasło";
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
        public async Task<IActionResult> Register(PageUser pageuser) //TODO: pass user credentials
        {
            if (ModelState.IsValid)
            {
                PageUser pageUser = new PageUser() // TODO SPRAWDZIC CZY NIE MA USERA W BAZIE
                {
                    FirstName = pageuser.FirstName,
                    Surname = pageuser.Surname,
                    EmailAddress = pageuser.EmailAddress,
                    Phonenumber = pageuser.Phonenumber,
                    TypeId = 1,
                };
                var passwordHasher = new PasswordHasher<string>();
                Credentials credentials = new Credentials()
                {
                    Password = passwordHasher.HashPassword(pageuser.EmailAddress, pageuser.Credentials.Password),
                };
                var passwordHasherConfirmation = new PasswordHasher<string>();
                if (passwordHasherConfirmation.VerifyHashedPassword(null, credentials.Password, pageuser.Credentials.PasswordConfirmed) == PasswordVerificationResult.Success) // strawdzic czy nie ma takiego usera
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
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditGeneral(PageUser user) //TODO: dodać userId
        {
            //if (user.EmailAddress == null) user.EmailAddress = this.User.Identity.Name;
            // if (!ModelState.IsValid){
            //     return View("Edit",user);
            // }
            if (user.EmailAddress == null) return NotFound();
            //TODO: dodać walidacje po stronie serwera
            _context.PageUser.Attach(user);
            _context.Entry(user).Property(u => u.FirstName).IsModified = true;
            _context.Entry(user).Property(u => u.Surname).IsModified = true;
            _context.Entry(user).Property(u => u.Phonenumber).IsModified = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public async Task<IActionResult> EditAddress(PageUser user)
        {
            //TODO: dodać polskie znaki do nazw w adresie
            if (user.Userid == null) return NotFound();
            var adress = _context.Useraddress.FirstOrDefault(a => a.Userid == user.Userid);
            Useraddress address = user.Useraddress;
            address.Userid = user.Userid;

            if (adress == null)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            //TODO: dodać walidacje po stronie serwera
            _context.Entry(adress).State = EntityState.Detached;
            _context.Useraddress.Attach(address);

            _context.Entry(address).Property(u => u.Street).IsModified = true;
            _context.Entry(address).Property(u => u.Number).IsModified = true;
            _context.Entry(address).Property(u => u.ApartmentNumber).IsModified = true;
            _context.Entry(address).Property(u => u.ZipCode).IsModified = true;
            _context.Entry(address).Property(u => u.City).IsModified = true;
            await _context.SaveChangesAsync();
            
            // if (ModelState.IsValid)
            // {
            //     return View();
            // }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditCredentials(PageUser user)
        {
            if (user.Userid == null) return NotFound();
            //TODO: sprawdzenie czy stare hasło jest zgodne z bazą
            _context.Credentials.Attach(user.Credentials);

            Credentials credentials = user.Credentials;
            var passwordHasher = new PasswordHasher<string>();
            credentials.Password = passwordHasher.HashPassword(user.EmailAddress, credentials.Password);
            _context.Entry(user.Credentials).Property(u => u.Password).IsModified = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}