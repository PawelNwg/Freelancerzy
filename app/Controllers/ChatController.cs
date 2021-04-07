using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using freelancerzy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace freelancerzy.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly cb2020freedbContext _context;

        public ChatController(cb2020freedbContext context)
        {
            _context = context;
        }

        public async Task  <IActionResult> Index()
        {
            var currUser = await _context.PageUser.FirstOrDefaultAsync(u => u.EmailAddress == HttpContext.User.Identity.Name);
            var messages = await _context.Message.Include(m => m.UserTo).Include(m => m.UserFrom).
                Where(m => m.UserFromId == currUser.Userid || m.UserToId == currUser.Userid).OrderBy(m => m.Date).ToListAsync();
          
            var users = messages.Select(message => message.UserFrom).Distinct();
            var users2 = messages.Select(message => message.UserTo).Distinct();
            var together = users.Concat(users2).Distinct().ToList();
            together.Remove(currUser);
            return View(together);
        }

        // GET: ChatController/Create
        public async Task<IActionResult> Message(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await _context.PageUser.FirstOrDefaultAsync(u => u.Userid == id);
            if(user == null)
            {
                return NotFound();
            }
            Message message = new Message();
            message.UserTo = user;
            message.UserToId = user.Userid;
            return View(message);
        }

        // POST: ChatController1cs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Message(Message message)
        {
            if (ModelState.IsValid)
            {
                message.Date = DateTime.Now;
                message.UserFrom = await _context.PageUser.FirstOrDefaultAsync(u => u.EmailAddress == HttpContext.User.Identity.Name);
                message.Status = "Wysłana";
                _context.Message.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Chat", new { userId = message.UserToId });
            }
            else return View();
        }


        public async Task<IActionResult> Chat(int userId)
        {
            var currentUser = await _context.PageUser.FirstOrDefaultAsync(u => u.EmailAddress
            == HttpContext.User.Identity.Name);
            var userTo = await _context.PageUser.FirstOrDefaultAsync(u => u.Userid == userId);
            if (userTo == null) return NotFound();
            var messages = await _context.Message.Include(m => m.UserFrom).
                Include(m => userTo).Where(m => (m.UserFromId == currentUser.Userid && m.UserToId == userId) 
                || (m.UserFromId == userId && m.UserToId == currentUser.Userid))
                .ToListAsync();
            if (messages.Count == 0) return RedirectToAction("Message", new { id = userId });
            foreach(var message in messages)
            {
                message.Seen = true;
                message.Status = "Wyświetlona";
                _context.Update(message);
            }
            await _context.SaveChangesAsync();
            ViewBag.CurrentUserName = currentUser.EmailAddress;
            ViewBag.UserToId = userId;
            ViewBag.UserToName = userTo.EmailAddress;
            return View(messages);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Message message)
        {
            
                message.UserFrom = await _context.PageUser.FirstOrDefaultAsync
                    (u => u.EmailAddress == HttpContext.User.Identity.Name);
                message.Date = DateTime.Now;
                message.Seen = false;
                message.Status = "Wysłana";
                await _context.AddAsync(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Chat", new { userId = message.UserToId });
            
        }

       
    }
}
