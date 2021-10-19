using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Areas.Editor.ViewModel;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<ResultOfTesting> resultList=await dbContext.ResultsOfTesting
                                                            .Include(r=>r.Student)
                                                            .ThenInclude(s=>s.Group)
                                                            .Include(r=>r.Test)
                                                            .OrderByDescending(r=>r.PassedAt)
                                                            .ToListAsync();
            return View(resultList);
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentResults(int id)
        {
            List<ResultOfTesting> resultList = await dbContext.ResultsOfTesting
                                                              .Where(r => r.Student.Id == id)
                                                              .Include(r => r.Student)
                                                              .ThenInclude(s => s.Group)
                                                              .Include(r => r.Test)
                                                              .OrderByDescending(r => r.PassedAt)
                                                              .ToListAsync();
            !return View(resultList);
        }
        [HttpGet]
        public async Task<IActionResult> GetGroupResults(int id)
        {
            List<ResultOfTesting> resultList = await dbContext.ResultsOfTesting
                                                              .Where(r => r.Student.Group.Id == id)
                                                              .Include(r => r.Student)
                                                              .Include(r => r.Test)
                                                              .OrderByDescending(r => r.PassedAt)
                                                              .ToListAsync();
            !return View(resultList);
        }
        [HttpGet]
        public async Task<IActionResult> ClearAll()
        {
            dbContext.ResultsOfTesting.RemoveRange(dbContext.ResultsOfTesting);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
