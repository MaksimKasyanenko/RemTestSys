using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class SessionController : Controller
    {
        public SessionController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private readonly AppDbContext dbContext;
        public async Task<IActionResult> Index()
        {
            List<Session> sessions = await dbContext.Sessions.Include(s => s.Student).Include(s => s.Test).ToListAsync();
            return View(sessions);
        }
        public async Task<IActionResult> Close(int id)
        {
            Session session = await dbContext.Sessions.SingleOrDefaultAsync(s=>s.Id==id);
            if (session != null)
            {
                dbContext.Sessions.Remove(session);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ClearAll()
        {
            dbContext.Sessions.RemoveRange(dbContext.Sessions);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
