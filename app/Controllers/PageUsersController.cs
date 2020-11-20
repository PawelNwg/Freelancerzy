using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using freelancerzy.Models;

namespace freelancerzy.Controllers
{
    public class PageUsersController : Controller
    {
        private readonly cb2020freedbContext _context;

        public PageUsersController(cb2020freedbContext context)
        {
            _context = context;
        }

        // GET: PageUsers
        public async Task<IActionResult> Index()
        {
            var cb2020freedbContext = _context.PageUser.Include(p => p.Type);
            return View(await cb2020freedbContext.ToListAsync());
        }

        // GET: PageUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageUser = await _context.PageUser
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (pageUser == null)
            {
                return NotFound();
            }

            return View(pageUser);
        }

        // GET: PageUsers/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Usertype, "Typeid", "Name");
            return View();
        }

        // POST: PageUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,TypeId,FirstName,Surname,EmailAddress,Phonenumber")] PageUser pageUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pageUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Usertype, "Typeid", "Name", pageUser.TypeId);
            return View(pageUser);
        }

        // GET: PageUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageUser = await _context.PageUser.FindAsync(id);
            if (pageUser == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Usertype, "Typeid", "Name", pageUser.TypeId);
            return View(pageUser);
        }

        // POST: PageUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Userid,TypeId,FirstName,Surname,EmailAddress,Phonenumber")] PageUser pageUser)
        {
            if (id != pageUser.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pageUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageUserExists(pageUser.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Usertype, "Typeid", "Name", pageUser.TypeId);
            return View(pageUser);
        }

        // GET: PageUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageUser = await _context.PageUser
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (pageUser == null)
            {
                return NotFound();
            }

            return View(pageUser);
        }

        // POST: PageUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageUser = await _context.PageUser.FindAsync(id);
            _context.PageUser.Remove(pageUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageUserExists(int id)
        {
            return _context.PageUser.Any(e => e.Userid == id);
        }
    }
}
