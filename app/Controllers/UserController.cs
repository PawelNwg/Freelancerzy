using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using app.Models;
using freelancerzy.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password) //TODO: pass user credentials
        {
            
            //TODO: check user credentials
            return RedirectToAction("Index","Home");
        }        
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password) //TODO: pass user credentials
        {
            if (ModelState.IsValid)
            {
                PageUser pageUser = new PageUser() // TODO SPRAWDZIC CZY NIE MA USERA W BAZIE
                {
                    FirstName = name,
                    EmailAddress = email,
                    TypeId = 1,
                    Surname = "kovalsky",
                    Phonenumber = 666,

                    
                };
                var passwordHasher = new PasswordHasher<string>();
                Credentials credentials = new Credentials()
                {
                    Password = passwordHasher.HashPassword(email, password),
                };
                pageUser.Credentials = credentials;
                _context.Add(pageUser);
                _context.Add(credentials);
                await _context.SaveChangesAsync();
                ViewBag.message = "ok";
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