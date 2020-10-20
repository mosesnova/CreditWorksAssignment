using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditWorksAssignment.Models;

namespace CreditWorksAssignment.Controllers
{
    public class CreditWorksController : Controller
    {
        private readonly CreditWorksContext _context;

        public CreditWorksController(CreditWorksContext context)
        {
            _context = context;
        }

        // GET: CreditWorks
        public async Task<IActionResult> Index(string sortOrder,string currentFilter, string searchString, int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewData["ManufacturerSortParm"] = String.IsNullOrEmpty(sortOrder) ? "manufacturer_desc" : "";

            ViewData["YearSortParm"] = String.IsNullOrEmpty(sortOrder) ? "year_desc" : "";

            ViewData["WeightSortParm"] = String.IsNullOrEmpty(sortOrder) ? "weight_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var creditworks = from c in _context.CreditWorks select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                creditworks = creditworks.Where(s => s.Name.Contains(searchString)
                                       || s.Manufacturer.Contains(searchString) || s.Year.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    creditworks = creditworks.OrderByDescending(s => s.Name);
                    break;
                case "manufacturer_desc":
                    creditworks = creditworks.OrderByDescending(s => s.Manufacturer);
                    break;
                case "year_desc":
                    creditworks = creditworks.OrderByDescending(s => s.Year);
                    break;
                case "weight_desc":
                    creditworks = creditworks.OrderByDescending(s => s.Weight);
                    break;
                default:
                    creditworks = creditworks.OrderBy(s => s.Name);
                    break;
            }


            int pageSize = 3;
            return View(await PaginatedList<CreditWorks>.CreateAsync(creditworks.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: CreditWorks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditWorks = await _context.CreditWorks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditWorks == null)
            {
                return NotFound();
            }

            return View(creditWorks);
        }

        // GET: CreditWorks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreditWorks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Manufacturer,Year,Weight")] CreditWorks creditWorks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(creditWorks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(creditWorks);
        }

        // GET: CreditWorks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditWorks = await _context.CreditWorks.FindAsync(id);
            if (creditWorks == null)
            {
                return NotFound();
            }
            return View(creditWorks);
        }

        // POST: CreditWorks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Manufacturer,Year,Weight")] CreditWorks creditWorks)
        {
            if (id != creditWorks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditWorks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditWorksExists(creditWorks.Id))
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
            return View(creditWorks);
        }

        // GET: CreditWorks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditWorks = await _context.CreditWorks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (creditWorks == null)
            {
                return NotFound();
            }

            return View(creditWorks);
        }

        // POST: CreditWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var creditWorks = await _context.CreditWorks.FindAsync(id);
            _context.CreditWorks.Remove(creditWorks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditWorksExists(int id)
        {
            return _context.CreditWorks.Any(e => e.Id == id);
        }
    }
}
