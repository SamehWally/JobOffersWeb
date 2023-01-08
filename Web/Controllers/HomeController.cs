using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Web.Data;
using Web.Models;
using Web.ViewModels;
using System.Linq;
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext Context,UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = Context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var Categories = _context.Categories?.Include(c => c.Jobs).ToList();
            return View(Categories);
        }
        public IActionResult Details(int jobId) 
        {
            var job = _context.Jobs.Include(x => x.Category).FirstOrDefault(x => x.Id == jobId);
            if(job==null)
                return NotFound();

            HttpContext.Session.SetInt32("jobId", jobId);
            return View(job);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Apply() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(string Message)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;

            //if (HttpContext.Session.GetInt32("jobId") > 0)
            //{
                var JobId = HttpContext.Session.GetInt32("jobId");
            //}
            var check = _context.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == userId).ToList();
            if (check.Count < 1)
            {
                var job = new ApplyForJob();

                job.UserId = userId;
                job.JobId = Convert.ToInt32(JobId);
                job.ApplyDate = DateTime.Now;
                job.Message = Message;

                _context.ApplyForJobs.Add(job);
                _context.SaveChanges();
                ViewBag.Result = "تمت الاضافة بنجاح";
            }
            else 
            {
                ViewBag.Result = "لقد تقدمت الي هذه الوظيفة من قبل";
            }
            return View();
        }

        public IActionResult GetJobsByUser()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var userId = currentUser.Id;
            var jobs = _context.ApplyForJobs.Include(x=>x.Job).Include(x=>x.User)
                .Where(a => a.UserId == userId).ToList();
            return View(jobs);
        }
        public IActionResult DetailsForJob(int id) 
        {
            var job = _context.ApplyForJobs.Find(id);
            return View(job);
        }
        public IActionResult EditForJob(int id)
        {
            var job = _context.ApplyForJobs.Find(id);
            if (job == null)
                return NotFound();

            var viewModel = new ApplyForJob
            {
                Id=job.Id,
                ApplyDate=job.ApplyDate,
                Message=job.Message,
                JobId=job.JobId,
                UserId=job.UserId,
                Job = job.Job,
                User = job.User
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditForJob(ApplyForJob model)
        {
            if (ModelState.IsValid)
            {
                var job = _context.ApplyForJobs.Find(model.Id);
                job.JobId = model.JobId;
                job.Message = model.Message;

                _context.Update(job);
                _context.SaveChanges();
                return RedirectToAction(nameof(GetJobsByUser));
            }
            return View(model);
        }
        public IActionResult DeleteForJob(int id)
        {
            var job = _context.ApplyForJobs.Find(id);
            if (job == null)
                return NotFound();
            return View(job);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteForJob(ApplyForJob model)
        {
            _context.ApplyForJobs.Remove(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(GetJobsByUser));  
        }
        public IActionResult GetJobsByPublisher()
        {
            var currentUser=_userManager.GetUserAsync(HttpContext.User).Result;
            var userId = currentUser.Id;

            var jobs = _context.ApplyForJobs.Include(x => x.User).Include(x => x.Job)
                .Where(x => x.UserId == userId).ToList();

            return View(jobs);
        }
        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string SearchName)
        {
            var result = _context.Jobs.Where(j => j.JobTitle.Contains(SearchName) || j.Category.CategoryName.Contains(SearchName)
            || j.Category.CategoryDescription.Contains(SearchName)).ToList();
            return View(result);
        }
    }
}