using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using freelancerzy.Models;
using Microsoft.AspNetCore.Authorization;
using NinjaNye.SearchExtensions;
using System.Collections;

namespace freelancerzy.Controllers
{
    public class OffersController : Controller
    {
        private readonly cb2020freedbContext _context;

        public OffersController(cb2020freedbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Search));
        }
        // GET: Offers
        public IActionResult Search()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName");
            return View();

        }

        [Authorize]
        public async Task<PartialViewResult> MyOffersListPartial(int? pageNumber, string order)
        {
            if (User.Identity.Name == null) return null;
            var Offers = SortedList(order);
            string email = User.Identity.Name;
            PageUser user = _context.PageUser.FirstOrDefault(u => u.EmailAddress == email);
            if (user == null) return null;
            Offers = Offers.Where(o => o.UserId == user.Userid);
            int pageSize = 15;
            return PartialView("_MyOfferList", await PaginatedList<Offer>.CreateAsync(Offers, pageNumber ?? 1, pageSize));
        }

        public async Task<PartialViewResult> OfferListPartial(int? pageNumber, string order, string searchString, int categoryId, Filter Filter)
        {
            var Offers = SortedList(order);
            Offers = Offers.Where(o => o.ExpirationDate >= DateTime.Now);
            if (categoryId != 0)
            {
                Offers = Offers.Where(o => o.CategoryId == categoryId);
            }
            Offers = Filters(Offers, Filter);
            if (searchString != null)
            {
                searchString = searchString.TrimEnd(); // uciecie nadmiaru spacji na koncu

                var wordList = searchString.ToLower().Split(' ').ToList();
                wordList.RemoveAll(o => o == ""); //usumoecie z listy pustych strignow
                for (int i = 0; i < wordList.Count; i++) // usuwanie końcówek
                {

                    if (wordList[i].Length > 4) wordList[i] = wordList[i].Substring(0, wordList[i].Length - 2);
                }
                var words = wordList.ToArray();
                if (words.Count() >= 2) // jesli wiecej niz dwa slowa musza pasowac conajmniej  2
                {
                    Offers = Offers.Search(o => o.Title.ToLower(), o => o.Description.ToLower()).Containing(words).ToRanked().Where(o => o.Hits >= 2).Select(o => o.Item);
                }
                Offers = Offers.Search(o => o.Title, o => o.Description).Containing(words); //https://ninjanye.github.io/SearchExtensions/
            }
            int pageSize = 15;
            return PartialView("_OfferList", await PaginatedList<Offer>.CreateAsync(Offers, pageNumber ?? 1, pageSize));

        }
        private IQueryable<Offer> Filters(IQueryable<Offer> offers, Filter filter)
        {
            var offerList = offers;
            if (filter.WageLow != null || filter.WageUp != null)
            {
                offerList = offerList.Where(o => o.Wage >= (filter.WageLow == null ? 0 : filter.WageLow) && o.Wage <= (filter.WageUp == null ? Int32.MaxValue : filter.WageUp));
            }
            if (filter.DateLow != null || filter.DateUp != null)
            {
                offerList = offerList.Where(o => o.CreationDate >= (filter.DateLow == null ? DateTime.MinValue : filter.DateLow) && o.CreationDate <= (filter.DateUp == null ? DateTime.MaxValue : filter.DateUp));
            }
            return offerList;
        }
        private IQueryable<Offer> SortedList(string order)
        {
            switch (order)
            {
                case "nameAsc":
                    return _context.Offer.Include(o => o.Category).OrderBy(o => o.Title);
                case "nameDesc":
                    return _context.Offer.Include(o => o.Category).OrderByDescending(o => o.Title);
                case "wageAsc":
                    return _context.Offer.Include(o => o.Category).OrderBy(o => o.Wage);
                case "wageDesc":
                    return _context.Offer.Include(o => o.Category).OrderByDescending(o => o.Wage);
                case "dateAsc":
                    return _context.Offer.Include(o => o.Category).OrderBy(o => o.CreationDate);
                case "dateDesc":
                    return _context.Offer.Include(o => o.Category).OrderByDescending(o => o.CreationDate);
                default:
                    return _context.Offer.Include(o => o.Category).OrderBy(o => o.Title);
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult MyOffers()
        {
            return View();
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
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public IActionResult NewOffer()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName");

            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOffer(Offer offer)
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> Edit(int? id)
        {
            //TODO: sprawdzać czy to jest oferta tego usera i czy może w nią wejść, żeby nie przepuszczało adresu z palca jeśli nie ma dostępu
            if (id == null)
            {
                return NotFound();
            }

            //TODO: ustawiać wartość last modification date
            var offer = await _context.Offer.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            /* User authorization */
            String email = this.User.Identity.Name;
            if (email == null) return NotFound();
            int userId = Convert.ToInt32(_context.PageUser.FirstOrDefault(u => u.EmailAddress == email).Userid);
            if (offer.UserId != userId) return RedirectToAction(nameof(Search));
            //TODO: dodać komunikat informujący, że użytkownik nie ma uprawnień do edycji oferty
            ViewData["CategoryId"] = new SelectList(_context.Category, "Categoryid", "CategoryName", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.PageUser, "Userid", "EmailAddress", offer.UserId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public async Task<IActionResult> Edit(int id, [Bind("Offerid,UserId,CategoryId,Title,Description,CreationDate,LastModificationDate,ExpirationDate,ViewCounter,Wage,WageValue")] Offer offer)
        {
            if (id != offer.Offerid)
            {
                return NotFound();
            }
            /* User authorization */
            String email = this.User.Identity.Name;
            if (email == null) return NotFound();
            int userId = Convert.ToInt32(_context.PageUser.FirstOrDefault(u => u.EmailAddress == email).Userid);
            if (offer.UserId != userId) return RedirectToAction(nameof(Search));
            //TODO: dodać komunikat informujący, że użytkownik nie ma uprawnień do edycji oferty
            offer.WageValue = offer.WageValue.Replace(".",",");
            decimal wage;
            if (decimal.TryParse(offer.WageValue, out wage))
            {
                offer.Wage = wage;
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
                    return RedirectToAction(nameof(Search)); //TODO: ustalić na co przekierowywać
                }
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
        public class Filter
        {
            public int? WageLow { get; set; }
            public int? WageUp { get; set; }
            public DateTime? DateLow { get; set; }
            public DateTime? DateUp { get; set; }

        }
    }
}
