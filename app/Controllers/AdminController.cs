using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using freelancerzy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NinjaNye.SearchExtensions;

namespace freelancerzy.Controllers
{
    [Authorize(Roles = "administrator", AuthenticationSchemes = "CookieAuthentication")]
    public class AdminController : Controller
    {
        private cb2020freedbContext _context;

        public AdminController(cb2020freedbContext context)
        {
            _context = context;
        }

        [HttpPut]
        public async Task<IActionResult> UnlockUser(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.PageUser.FirstOrDefaultAsync(u => u.Userid == id);
            if (user == null) return NotFound();

            user.isBlocked = false;
            user.blockType = null;
            user.dateOfBlock = null;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public IActionResult BlockedUsers()
        {
            return View();
        }

        public async Task<IActionResult> BlockedUsersPartial(int? pageNumber, string order, string searchString, DateTime? DateLow, DateTime? DateUp)
        {
            var users = BlockedUsersSortedList(order);
            users = Filter(users, DateLow, DateUp);
            if (searchString != null)
            {
                searchString = searchString.TrimEnd(); // uciecie nadmiaru spacji na koncu

                var wordList = searchString.ToLower().Split(' ').ToList();
                wordList.RemoveAll(o => o == ""); //usumoecie z listy pustych strignow

                var words = wordList.ToArray();

                users = users.Search(o => o.EmailAddress.ToLower()).Containing(words);

            }
            int pageSize = 15;
            Dictionary<int, string> blockTypes = new Dictionary<int, string>() { { 1,"tydzień"},{ 2,"miesiąc"}, { 3,"pernamentna"} };
            ViewBag.blockTypes = blockTypes;
            return PartialView("_BlockedUsers", await PaginatedList<PageUser>.CreateAsync(users, pageNumber ?? 1, pageSize));
        }

        private IQueryable<PageUser> Filter(IQueryable<PageUser> users, DateTime? dateLow, DateTime? dateUp)
        {
            var blockedUsers = users;
            if (dateLow != null || dateUp != null)
            {
                blockedUsers = blockedUsers.Where(o => o.dateOfBlock >= (dateLow == null ? DateTime.MinValue : dateLow)
                && o.dateOfBlock <= (dateUp == null ? DateTime.MaxValue : dateUp));
            }
            return blockedUsers;
        }

        private IQueryable<PageUser> BlockedUsersSortedList(string order)
        {
            switch (order)
            {
                case "nameAsc":
                    return _context.PageUser.Where(u => u.isBlocked == true).OrderBy(o => o.EmailAddress);
                case "nameDesc":             
                    return _context.PageUser.Where(u => u.isBlocked == true).OrderByDescending(o => o.EmailAddress);
                case "dateAsc":              
                    return _context.PageUser.Where(u => u.isBlocked == true).OrderBy(o => o.dateOfBlock);
                case "dateDesc":            
                    return _context.PageUser.Where(u => u.isBlocked == true).OrderByDescending(o => o.dateOfBlock);
                default:                     
                    return _context.PageUser.Where(u => u.isBlocked == true).OrderBy(o => o.EmailAddress);
            }
        }
    }
}
