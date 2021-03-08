using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using freelancerzy.Models;
using Microsoft.AspNetCore.Mvc;
using TokenGenerator.Managers.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using app.Models;
using System.Net.Mail;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace app.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _config;

        private readonly cb2020freedbContext _context;

        private readonly ITokenManager _tokenManager;
        // GET: User List
        public async Task<IActionResult> List()
        {
            return View(await _context.PageUser
            .Include(u => u.Type)
            .ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.PageUser
                .Include(u => u.Useraddress)
                .Include(u => u.Type)
                .FirstOrDefaultAsync(u => u.Userid == id);

            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Confirmation = user.emailConfirmation ? "Potwierdzono" : "Nie potwierdzono";
            return View(user);
        }

        // GET: PageUser/Delete/5
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.PageUser.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            List<Offer> offers = await _context.Offer.Where(o => o.UserId == user.Userid).ToListAsync();


            foreach (Offer o in offers)
            {
                List<OfferReport> reports = await _context.OfferReport.Where(m => m.OfferId == o.Offerid).ToListAsync();

                foreach (OfferReport r in reports) _context.OfferReport.Remove(r);
                _context.Offer.Remove(o);
            }

            _context.PageUser.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        //private readonly ILogger<UserController> _logger;
        public UserController(cb2020freedbContext context, IConfiguration config, ITokenManager tokenManager)
        {
            _context = context;
            _config = config;
            _tokenManager = tokenManager;
        }

        //public UserController(ILogger<UserController> logger)
        //{
        //    _logger = logger;
        //}
        public IActionResult ConfirmUserRegistration()
        {
            return View();
        }
        public IActionResult Login(string ReturnUrl = "/Home/Index")
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
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
                ViewData["error"] = "Podana nazwa użytkownika nie istnieje";
                return View();
            }
            else
            {
                user.Credentials = _context.Credentials.FirstOrDefault(u => u.Userid == user.Userid);
                user.Type = _context.Usertype.FirstOrDefault(u => u.Typeid == user.TypeId);

                if (user.emailConfirmation != true)
                {
                    ViewData["error"] = "Email nie został potwierdzony";
                    return View();
                }

                if (ValidateUser(user, password))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.EmailAddress),
                        new Claim(ClaimTypes.Role,user.Type.Name),
                        new Claim("nameandsurname",user.FirstName + " " + user.Surname),
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
        private bool ValidateUser(PageUser user, string password)
        {
            var hasher = new PasswordHasher<string>();
            if (hasher.VerifyHashedPassword(user.EmailAddress, user.Credentials.Password, password) == PasswordVerificationResult.Success)
            {
                return true;
            }
            else return false;
        }

        [HttpPost]
        public async Task<IActionResult> Register(PageUser pageuser)
        {
            if (ModelState.IsValid)
            {
                if (_context.PageUser.FirstOrDefault(u => u.EmailAddress == pageuser.EmailAddress) == null)
                {
                    PageUser pageUser = new PageUser()
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
                        EmailAsync(pageuser);
                    }
                    else
                    {
                        ViewData["Error"] = "Hasła nie sa taki same";
                        return View();
                    }
                }
                else
                {
                    ViewData["ErrorEmail"] = "Podany adres email jest juz zajety, proszę podac inny";
                    return View();
                }
            }
            else
            {
                return View();
            }

            //TODO: check user credentials
            return View("ConfirmUserRegistration");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> EditGeneral(PageUser user)
        {
            if (user.EmailAddress == null) return NotFound();

            _context.PageUser.Attach(user);
            _context.Entry(user).Property(u => u.FirstName).IsModified = true;
            _context.Entry(user).Property(u => u.Surname).IsModified = true;
            if (user.Phonenumber == null)
            {
                ViewBag.Message = "Podano nieprawidłowy numer telefonu";
                var dbUser = _context.PageUser.Include(u => u.Credentials).Include(t => t.Type).Include(a => a.Useraddress).FirstOrDefault(u => u.EmailAddress == user.EmailAddress);
                return View("Edit", dbUser);
            }
            _context.Entry(user).Property(u => u.Phonenumber).IsModified = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> EditAddress(PageUser user)
        {
            if (user.Userid == null) return NotFound();
            //to check if adress exists in DB
            var adress = _context.Useraddress.FirstOrDefault(a => a.Userid == user.Userid);
            //only address entity
            Useraddress address = user.Useraddress;
            address.Userid = user.Userid;

            if (adress == null)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            _context.Entry(adress).State = EntityState.Detached;
            _context.Useraddress.Attach(address);

            _context.Entry(address).Property(u => u.Street).IsModified = true;
            _context.Entry(address).Property(u => u.Number).IsModified = true;
            _context.Entry(address).Property(u => u.ApartmentNumber).IsModified = true;
            _context.Entry(address).Property(u => u.ZipCode).IsModified = true;
            _context.Entry(address).Property(u => u.City).IsModified = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> EditCredentials(PageUser user)
        {
            if (user.EmailAddress == null) return NotFound();

            Credentials credentials = user.Credentials;
            PageUser dbUser = _context.PageUser.Include(u => u.Credentials).Include(t => t.Type).Include(a => a.Useraddress).FirstOrDefault(u => u.EmailAddress == user.EmailAddress);

            if (!ValidateUser(dbUser, credentials.OldPassword))
            {
                ViewBag.Message = "Podano nieprawidłowe hasło";
                return View("Edit", dbUser);
            }
            _context.Entry(dbUser).State = EntityState.Detached;
            _context.Credentials.Attach(user.Credentials);
            var passwordHasher = new PasswordHasher<string>();

            if (passwordHasher.VerifyHashedPassword(dbUser.EmailAddress, dbUser.Credentials.Password, user.Credentials.Password) == PasswordVerificationResult.Success)
            {
                ViewBag.Message = "Hasło jest identyczne jak stare hasło";
                return View("Edit", dbUser);
            }
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

        private void EmailAsync(PageUser pageuser)
        {

            Mail mailInfo = new Mail();
            MailMessage mail = new MailMessage();
            mail.To.Add(pageuser.EmailAddress);
            mail.From = new MailAddress(mailInfo.SmtpEmailAdress);
            mail.Subject = mailInfo.EmailSubject;
            var token = generateToken(pageuser);
            string Body = mailInfo.EmailBody;
            string url = HttpContext.Request.Host.Value;
            string ConfirmationLink = "https://" + url + "/User/ConfirmEmail?" + "token=" + token;
            //int x = Body.IndexOf("qq");
            Body = Body.Insert(1736, ConfirmationLink);
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailInfo.SmtpHost;
            smtp.Port = mailInfo.SmtpPort;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_config.GetValue<String>("SmtpServers:login"), _config.GetValue<String>("SmtpServers:password"));
            smtp.EnableSsl = true;
            smtp.Send(mail);

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            if (token == null)
            {
                ViewData["Data"] = "B��D";
                return View(); // TODO widok errora
            }

            var claims = _tokenManager.GetClaims(token);

            if (claims != null)
            {
                var userId = claims["userId"];
                var email = claims["userEmail"];
                DateTime date = DateTime.Parse((claims["nameAndSurname"] as string));


                var user = _context.PageUser.FirstOrDefault(u => u.EmailAddress == email);
                if (user == null)
                {
                    ViewData["Data"] = "BŁĄD";
                    return View(); // TODO widok errora
                }
                else if (DateTime.Compare(DateTime.Now, date.AddMinutes(15)) > 0)
                {
                    ViewData["Data"] = "token wygasl";
                    return View();
                }
                user.emailConfirmation = true;
                _context.Update(user);
                await _context.SaveChangesAsync();
                ViewData["Data"] = "Zarejestrowano pomyślnie :)";
                ViewData["Wynik"] = true;
                return View();
            }
            ViewData["Data"] = "token wygasl";
            return View();
        }

        public string generateToken(PageUser pageuser)
        {
            var token = _tokenManager.GenerateToken(new Dictionary<string, object>
            {
                {
                    "userId", pageuser.Userid
                },
                {
                    "userEmail", pageuser.EmailAddress
                },
                {
                    "nameAndSurname", DateTime.Now.ToString()
                }
            });
            return token;
        }

    }
}