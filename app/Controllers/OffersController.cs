﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using freelancerzy.Models;

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
        public async Task<IActionResult> Index()
        {
            var cb2020freedbContext = _context.Offer.Include(o => o.Category).Include(o => o.User);
            return View(await cb2020freedbContext.ToListAsync());
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

        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName");
            ViewData["UserId"] = new SelectList(_context.PageUser, "Userid", "EmailAddress");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Offerid,UserId,CategoryId,Title,Description,CreationDate,LastModificationDate,ExpirationDate,ViewCounter,Wage")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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