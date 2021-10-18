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
        public async Task<IActionResult> Index(int? page)
        {
            if (page == null || page < 1) page = 1;
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.AllElementsCount = await dbContext.ResultsOfTesting.CountAsync();
            pagination.CurrentPage = (int)page;
            pagination.ElementsPerPage = 30;
            ViewBag.PaginationData = pagination;
            
            List<ResultOfTesting> resultList=await dbContext.ResultsOfTesting
                                                            .Skip(pagination.SkippedPages)
                                                            .Take(pagination.ElementsPerPage)
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
