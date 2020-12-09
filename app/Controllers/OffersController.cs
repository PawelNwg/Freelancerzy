using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using freelancerzy.Models;
using Microsoft.AspNetCore.Authorization;

namespace freelancerzy.Controllers
{
    public class OffersController : Controller
    {
        private readonly cb2020freedbContext _context;

        public OffersController(cb2020freedbContext context)
        {
            _context = context;
        }

        // GET: Offers
        public  IActionResult Search()
        {
            
            return View();
            
        }
        
        public async Task<PartialViewResult> OfferListPartial(int? pageNumber)
        {
            var cb2020freedbContext = _context.Offer.Include(o => o.Category).Include(o => o.User).OrderBy(o => o.Title);
            int pageSize = 15;
            return PartialView("_OfferList", await PaginatedList<Offer>.CreateAsync(cb2020freedbContext, pageNumber ?? 1, pageSize));
            
        }
        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .Include(o => o.Category)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Offerid == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offers/NewOffer
        [Authorize]
        public IActionResult NewOffer()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName");
           
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOffer( Offer offer)
        {
            if (ModelState.IsValid)
            {
                PageUser user = await _context.PageUser.FirstOrDefaultAsync(user => user.EmailAddress == User.Identity.Name);
                offer.User = user;
                offer.CreationDate = DateTime.Now;
                offer.ExpirationDate = DateTime.Now.AddDays(14);
                offer.ViewCounter = 0;
                if (offer.WageValue != null)
                offer.Wage = Decimal.Parse(offer.WageValue);
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Search));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.PageUser, "Userid", "EmailAddress", offer.UserId);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.PageUser, "Userid", "EmailAddress", offer.UserId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Offerid,UserId,CategoryId,Title,Description,CreationDate,LastModificationDate,ExpirationDate,ViewCounter,Wage")] Offer offer)
        {
            if (id != offer.Offerid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Offerid))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.PageUser, "Userid", "EmailAddress", offer.UserId);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offer
                .Include(o => o.Category)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Offerid == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offer.FindAsync(id);
            _context.Offer.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
            return _context.Offer.Any(e => e.Offerid == id);
        }
    }
}
