using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetMetal_Admin.DB;
using BudgetMetal_Admin.Models;

namespace BudgetMetal_Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.bm_user.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bm_user = await _context.bm_user
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bm_user == null)
            {
                return NotFound();
            }

            return View(bm_user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,IsActive,Version,SiteAdmin")] bm_user bm_user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bm_user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bm_user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bm_user = await _context.bm_user.SingleOrDefaultAsync(m => m.Id == id);
            if (bm_user == null)
            {
                return NotFound();
            }
            return View(bm_user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy,IsActive,Version,SiteAdmin")] bm_user bm_user)
        {
            if (id != bm_user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bm_user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bm_userExists(bm_user.Id))
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
            return View(bm_user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bm_user = await _context.bm_user
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bm_user == null)
            {
                return NotFound();
            }

            return View(bm_user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bm_user = await _context.bm_user.SingleOrDefaultAsync(m => m.Id == id);
            _context.bm_user.Remove(bm_user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool bm_userExists(int id)
        {
            return _context.bm_user.Any(e => e.Id == id);
        }
    }
}
