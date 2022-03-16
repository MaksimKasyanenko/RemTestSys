using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain.Interfaces;
using RemTestSys.Domain;
using RemTestSys.Domain.ViewModels;

namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class GroupsController : Controller
    {
        public GroupsController(IGroupService groupService)
        {
            this.groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }
        private readonly IGroupService groupService;
        public async Task<IActionResult> Index()
        {
            return View(await groupService.GetGroupListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)return View("Error");
            var group = await groupService.FindAsync((int)id);
            if (group == null)return NotFound();
            return View(group);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] GroupViewModel group)
        {
            if (ModelState.IsValid)
            {
                await groupService.CreateAsync(group);
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)return View("Error");
            var group = await groupService.FindAsync((int)id);
            if (group == null)return NotFound();
            return View(group);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] GroupViewModel group)
        {
            if (group == null)return View("Error");
            if (ModelState.IsValid)
            {
                if(!groupService.Exists(group.Id))return View("Error");
                await groupService.UpdateAsync(group);
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)return View("Error");
            var group = await groupService.FindAsync((int)id);
            if (group == null)return NotFound();
            return View(group);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(!groupService.Exists(id))return View("Error");
            await groupService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
