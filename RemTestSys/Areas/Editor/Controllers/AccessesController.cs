using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles = "Editor")]
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
            accesses.AccessesForAll = await dbContext.AccessesToTestForAll.Include(a => a.Test).ToListAsync();
            accesses.AccessesForGroups = await dbContext.AccessesToTestForGroup.Include(a => a.Test).Include(a => a.Group).ToListAsync();
            accesses.AccessesForStudents = await dbContext.AccessesToTestForStudent.Include(a => a.Test).Include(a => a.Student).ToListAsync();
            return View(accesses);
        }
        public async Task<IActionResult> OpenAccessToTestForAll(int id)
        {
            Test test = await dbContext.Tests.FindAsync(id);
            if (test != null && await dbContext.AccessesToTestForAll.AllAsync(a => a.TestId != test.Id))
            {
                dbContext.AccessesToTestForAll.Add(new AccessToTestForAll { Test = test });
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> OpenAccessToTestForGroup(int id)
        {
            ViewData["TestId"] = id;
            var groups = await dbContext.Groups.ToListAsync();
            return View(groups);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenAccessToTestForGroup(int id, int testId)
        {
            var group = await dbContext.Groups.FindAsync(id);
            var test = await dbContext.Tests.FindAsync(testId);
            if(test != null && group != null && await dbContext.AccessesToTestForGroup.AllAsync(a=>a.TestId != testId || a.GroupId != id))
            {
                AccessToTestForGroup access = new AccessToTestForGroup
                {
                    GroupId = id,
                    TestId = testId
                };
                dbContext.AccessesToTestForGroup.Add(access);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> OpenAccessToTestForStudent(int id)
        {
            ViewData["TestId"] = id;
            var students = await dbContext.Students.ToListAsync();
            return View(students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenAccessToTestForStudent(int id, int testId)
        {
            var student = await dbContext.Students.FindAsync(id);
            var test = await dbContext.Tests.FindAsync(testId);
            if (test != null && student != null && await dbContext.AccessesToTestForStudent.AllAsync(a => a.TestId != testId || a.StudentId != id))
            {
                AccessToTestForStudent access = new AccessToTestForStudent
                {
                    StudentId = id,
                    TestId = testId
                };
                dbContext.AccessesToTestForStudent.Add(access);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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
            dbContext.AccessesToTestForAll.Remove(await dbContext.AccessesToTestForAll.FindAsync(id));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteForGroups(int id)
        {
            dbContext.AccessesToTestForGroup.Remove(await dbContext.AccessesToTestForGroup.FindAsync(id));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteForStudent(int id)
        {
            dbContext.AccessesToTestForStudent.Remove(await dbContext.AccessesToTestForStudent.FindAsync(id));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
