using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlMyGames.Data;
using ControlMyGames.Models;
using Microsoft.AspNetCore.Authorization;

namespace ControlMyGames.Controllers
{
    [Authorize()]
    public class RentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index(string sortOrder, string currentFilter,
                                               string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["GameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PersonSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            var rents = _context.Rents
            .Include(g => g.Game)
            .Include(p => p.Person)
            .AsNoTracking();

            if (!String.IsNullOrEmpty(searchString))
                rents = rents.Where(s => s.Person.Name.Contains(searchString));

            switch (sortOrder)
            {
                case "title_desc":
                    rents = rents.OrderByDescending(s => s.Game.Title);
                    break;
                case "Name":
                    rents = rents.OrderBy(s => s.Person.Name);
                    break;
                case "name_desc":
                    rents = rents.OrderByDescending(s => s.Person.Name);
                    break;
                default:
                    rents = rents.OrderBy(s => s.Game.Title);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Rent>.CreateAsync(rents.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(g => g.Game)
                .Include(p => p.Person)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // GET: Rents/Create
        public IActionResult Create()
        {
            PopulateFriendsDropDownList();
            PopulateGamesDropDownList();
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameID,PersonID,RentDate,ReturnedDate,Returned")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateFriendsDropDownList(rent.PersonID);
            PopulateGamesDropDownList(rent.GameID);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (rent == null)
            {
                return NotFound();
            }
            PopulateFriendsDropDownList(rent.PersonID);
            PopulateGamesDropDownList(rent.GameID);
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GameID,PersonID,RentDate,ReturnedDate,Returned")] Rent rent)
        {
            if (id != rent.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.ID))
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
            PopulateFriendsDropDownList(rent.PersonID);
            PopulateGamesDropDownList(rent.GameID);
            return View(rent);
        }

        private void PopulateFriendsDropDownList(object selectedFriend = null)
        {
            var friendsQuery = from f in _context.Friends
                                   orderby f.Name
                                   select f;

            ViewBag.PersonID = new SelectList(friendsQuery.AsNoTracking(), "ID", "Name", selectedFriend);
        }

        private void PopulateGamesDropDownList(object selectedGame = null)
        {
            var gamesQuery = from g in _context.Games
                               orderby g.Title
                               select g;

            ViewBag.GameID = new SelectList(gamesQuery.AsNoTracking(), "ID", "Title", selectedGame);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rents.SingleOrDefaultAsync(m => m.ID == id);
            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.ID == id);
        }
    }
}
