using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        // GET: RolesController
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public ActionResult Index()
        {
           var roles= _roleManager.Roles.Select(role => new RoleViewModel { Id=role.Id,Name=role.Name }).ToList();
            return View(roles);
        }

        // GET: RolesController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // GET: RolesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roleManager.CreateAsync(role);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: RolesController/Edit/5
        public async Task<ActionResult> Edit(string Id)
        {
            if (Id == null || await _roleManager.FindByIdAsync(Id) == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string Id, [Bind("Id,Name")] IdentityRole role)
        {
            if (Id != role.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var _role = await _roleManager.FindByIdAsync(Id);
                    _role.Name = role.Name;
                    await _roleManager.UpdateAsync(_role);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: RolesController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var role =await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: RolesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string Id,IdentityRole role)
        {
            if (Id != role.Id)
            {
                return NotFound();
            }
            var _role = await _roleManager.FindByIdAsync(Id);
            if (_role != null)
            {
                try
                {
                    await _roleManager.DeleteAsync(_role);
                }
                catch
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }
        private bool RoleExists(string id)
        {
            return (_roleManager.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
