using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Web.Data;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users.Select(user=> new UserViewModel 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName=user.Lastname,
                UserName=user.UserName,
                Email=user.Email
            }).ToList();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName=model.FirstName,
                    Lastname=model.LastName,
                    UserName=model.UserName,
                    Email=model.Email
                };
               var result=await _userManager.CreateAsync(user,model.Password);
                if(!result.Succeeded)
                    return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user =await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

                var viewModel = new UserViewModel
                {
                    Id=user.Id,
                    FirstName=user.FirstName,
                    LastName=user.Lastname,
                    UserName=user.UserName,
                    Email=user.Email
                };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.Lastname= model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        { 
            var user=await _userManager.FindByIdAsync(id);
            if(user==null)
                return NotFound();

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName=user.Lastname,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var viewModel = new UserViewModel
            {
                Id=user.Id,
                FirstName=user.FirstName,
                LastName=user.Lastname,
                UserName=user.UserName,
                Email=user.Email
            };
            return View(viewModel);
        }
    }
}
