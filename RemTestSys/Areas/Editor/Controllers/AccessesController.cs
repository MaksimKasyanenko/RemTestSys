using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class AccessesController : Controller
    {
        public AccessesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private readonly AppDbContext dbContext;
        public async Task<IActionResult> Index()
        {
            AccessesViewModel accesses = new AccessesViewModel();
            accesses.AccessesForAll = await dbContext.AccessesToTestForAll.Include(a=>a.Test).ToListAsync();
            accesses.AccessesForGroups = await dbContext.AccessesToTestForGroup.Include(a => a.Test).Include(a=>a.Group).ToListAsync();
            accesses.AccessesForStudents = await dbContext.AccessesToTestForStudent.Include(a=>a.Test).Include(a=>a.Student).ToListAsync();
            return View(accesses);
        }
        public async Task<IActionResult> ClearAll()
        {
            dbContext.AccessesToTestForAll.RemoveRange(dbContext.AccessesToTestForAll);
            dbContext.AccessesToTestForGroup.RemoveRange(dbContext.AccessesToTestForGroup);
            dbContext.AccessesToTestForStudent.RemoveRange(dbContext.AccessesToTestForStudent);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteForAll(int id)
        {
            dbContext.AccessesToTestForAll.Remove(dbContext.AccessesToTestForAll.Find(id));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteForGroups(int id)
        {
            dbContext.AccessesToTestForGroup.Remove(dbContext.AccessesToTestForGroup.Find(id));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteForStudent(int id)
        {
            dbContext.AccessesToTestForStudent.Remove(dbContext.AccessesToTestForStudent.Find(id));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
