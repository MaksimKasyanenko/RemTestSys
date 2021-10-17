using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(int? page)
        {
            if (page == null || page < 1) page = 1;
            int listLength=30;
            int countOfResults = await dbContext.ResultsOfTesting.CountAsync();
            int countOfPages = countOfResults / listLength;
            if (countOfPages == 0 && countOfResults > 0) countOfPages = 1;
            ViewBag.CountOfPages = countOfPages;
            ViewBag.CurrentPageNum = page;
            List<ResultOfTesting> resultList=await dbContext.ResultsOfTesting
                                                            .Skip(((int)page-1)*listLength)
                                                            .Take(listLength)
                                                            .Include(r=>r.Student)
                                                            .Include(r=>r.Test)
                                                            .OrderByDescending(r=>r.PassedAt)
                                                            .ToListAsync();
            return View(resultList);
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
