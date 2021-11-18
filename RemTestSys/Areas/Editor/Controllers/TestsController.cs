using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RemTestSys;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class TestsController : Controller
    {
        private readonly AppDbContext _context;

        public TestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Editor/Tests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tests.Include(t=>t.MapParts).ToListAsync());
        }

        // GET: Editor/Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t=>t.Questions)
                .ThenInclude(q=>q.Answer)
                .Include(t=>t.MapParts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Test {
                    Name = testViewModel.Name,
                    Description = testViewModel.Description,
                    Duration = testViewModel.Duration,
                    MapParts = testViewModel.MapParts
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var test = await _context.Tests.Where(t => t.Id == id).Include(t => t.MapParts).SingleOrDefaultAsync();
            if (test == null)
            {
                return NotFound();
            }
            return View(new TestViewModel {
                Id=test.Id,
                Name=test.Name,
                Description=test.Description,
                Duration=test.Duration,
                MapParts=test.MapParts.ToArray()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestViewModel testViewModel)
        {
            if (ModelState.IsValid)
            {
                Test test = await _context.Tests.Where(t=>t.Id == testViewModel.Id).Include(t=>t.MapParts).SingleOrDefaultAsync();
                if (test == null) return NotFound();
                _context.MapParts.RemoveRange(test.MapParts.ToArray());
                await _context.SaveChangesAsync();
                test.Name = testViewModel.Name;
                test.Description = testViewModel.Description;
                test.Duration = testViewModel.Duration;
                test.MapParts = testViewModel.MapParts;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testViewModel);
        }

        // GET: Editor/Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Editor/Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}
