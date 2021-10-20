using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class ResultsController : Controller
    {
        public ResultsController(AppDbContext dbContext)
        {
            this.dbContext=dbContext??throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext dbContext;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await GetResults(r=>true));
        }
        [HttpGet]
        public async Task<IActionResult> Student(int id)
        {
            Student student = await dbContext.Students.SingleAsync(s=>s.Id==id);
            ViewData["StudentFullName"]=student.FullName;
            ViewData["StudentId"]=student.Id;
            return View(await GetResults(r=>r.Student.Id == id));
        }
        [HttpGet]
        public async Task<IActionResult> Group(int id)
        {
            Group group  = await dbContext.Groups.SingleAsync(g=>g.Id==id);
            ViewData["GroupName"] = group.Name;
            ViewData["GroupId"] = group.Id;
            return View(await GetResults(r=>r.Student.Group.Id == id));
        }
        [HttpGet]
        public async Task<IActionResult> Test(int id)
        {
            Test test = await dbContext.SingleAsync(t=>t.Id==id);
            ViewData["TestName"]=test.Name;
            ViewData["TestId"]=test.Id;
            return View(await GetResults(r=>r.Test.Id == id));
        }

        [HttpGet]
        public async Task<IActionResult> ClearAll() => await Remove(r => true);
        [HttpGet]
        public async Task<IActionResult> ClearForGroup(int id) => await Remove(r => r.Student.Group.Id == id);
        [HttpGet]
        public async Task<IActionResult> ClearForStudent(int id) => await Remove(r => r.Student.Id == id);
        [HttpGet]
        public async Task<IActionResult> ClearForTest(int id) => await Remove(r => r.Test.Id == id);



        private async Task<List<ResultOfTesting>> GetResults(Expression<Func<ResultOfTesting, bool>> filter)
        {
            List<ResultOfTesting> resultList = await dbContext.ResultsOfTesting
                                                              .Where(filter)
                                                              .Include(r => r.Student)
                                                              .ThenInclude(r => r.Group)
                                                              .Include(r => r.Test)
                                                              .OrderByDescending(r => r.PassedAt)
                                                              .ToListAsync();
            return resultList;
        }
        private async Task<IActionResult> Remove(Expression<Func<ResultOfTesting, bool>> filter)
        {
            dbContext.ResultsOfTesting.RemoveRange(dbContext.ResultsOfTesting.Where(filter));
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
