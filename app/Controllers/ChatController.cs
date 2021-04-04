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

        public ActionResult Index()
        {
            return View();
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

        // GET: ChatController1cs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChatController1cs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChatController1cs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChatController1cs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
