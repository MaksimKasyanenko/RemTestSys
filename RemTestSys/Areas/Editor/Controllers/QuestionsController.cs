using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class QuestionsController : Controller
    {
        public QuestionsController(AppDbContext context)
        {
            dbContext = context;
        }
        private readonly AppDbContext dbContext;
        // GET: QuestionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuestionsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuestionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuestionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuestionsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
