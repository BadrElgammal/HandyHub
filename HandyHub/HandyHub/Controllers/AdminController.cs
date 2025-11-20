using HandyHub.Data;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using HandyHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandyHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HandyHubDbContext _context;
        private readonly ClientService clientService;
        private readonly WorkerService workerService;
        private readonly GenericService<Category> catigoryService;
        private readonly GenericService<Review> ReviewService;

        public AdminController ( HandyHubDbContext context, ClientService _clientService, WorkerService _workerService, GenericService<Category> _catigoryService, GenericService<Review> _ReviewService )
        {
            _context = context;
            clientService = _clientService;
            workerService = _workerService;
            catigoryService = _catigoryService;
            ReviewService = _ReviewService;
        }

        // =============================== Dashboard ===============================
        [Route("/Admin")]
        public IActionResult Dashboard ()
        {
            var model = new AdminDashboardViewModel
            {
                TotalClients = clientService.GetAll().Count(),
                TotalWorkers = workerService.GetAll().Count(),
                TotalCategories = catigoryService.GetAll().Count(),
                TotalReviews = ReviewService.GetAll().Count(),
                AverageRating = ReviewService.GetAll().Any() ? Math.Round(ReviewService.GetAll().Average(r => r.Rating), 2) : 0,
                RecentWorkers = workerService.GetAll(),
                RecentClients = clientService.GetAll()
            };

            return View(model);
        }

        // =============================== Manage Users ===============================
        public IActionResult ManageClients ()
        {
            var clients = clientService.GetAll();
            return View(clients);
        }

        [HttpGet]
        public IActionResult CreateClient ()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateClient ( Client model )
        {
            var exist = clientService.IsEmailExist(model.Email);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            model.CreatedAt = DateTime.Now;
            clientService.Insert(model);
            TempData["msg"] = "created successfully";
            return RedirectToAction("ManageClients");

        }



        public IActionResult ClientDetails ( int id )
        {
            var client = clientService.GetById(id);
            if (client == null)
                return NotFound();
            return View(client);
        }



        [HttpGet]
        public IActionResult EditClient ( int? id )
        {
            if (id == null)
                return BadRequest();
            var client = clientService.GetById(id);
            if (client == null)
                return NotFound();
            return View(client);
        }

        [HttpPost]
        public IActionResult EditClient ( Client model )
        {
            var exist = clientService.IsEmailExist(model.Email, model.Id);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            clientService.Update(model);
            TempData["msg"] = "Update successfully";
            return RedirectToAction("ManageClients");
        }





        [HttpPost]
        public IActionResult DeleteClient ( int id )
        {
            var user = clientService.GetById(id);
            if (user != null)
            {
                clientService.Delete(id);
                TempData["msg"] = "تم حذف المستخدم بنجاح.";
            }
            return RedirectToAction("ManageClients");
        }

        // =============================== Manage Workers ===============================
        public IActionResult ManageWorkers ()
        {
            var workers = _context.Workers.Include(w => w.Category).ToList();
            return View(workers);
        }

        [HttpPost]
        public IActionResult SuspendWorker ( int id )
        {
            var worker = workerService.GetById(id);
            if (worker == null)
                return NotFound();

            if (workerService.SuspendWorker(worker))
                TempData["msg"] = $"تم اتاحة العامل {worker.Name}.";
            else
                TempData["msg"] = $"تم إيقاف العامل {worker.Name} مؤقتاً.";
            return RedirectToAction("ManageWorkers");
        }

        [HttpPost]
        public IActionResult DeleteWorker ( int id )
        {
            var worker = workerService.GetById(id);
            if (worker != null)
            {
                workerService.Delete(id);
                TempData["msg"] = "تم حذف العامل بنجاح.";
            }
            return RedirectToAction("ManageWorkers");
        }

        [HttpGet]
        public IActionResult CreateWorker ()
        {
            return View(GetWorkerCreateViewModel());
        }
        [HttpPost]
        public IActionResult CreateWorker ( WorkerWithCatigoryViewModel vm )
        {
            var exists = workerService.IsEmailExist(vm.Worker.Email);
            if (exists)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (ModelState.IsValid == false)
            {
                return View(GetWorkerCreateViewModel(vm.Worker));
            }

            workerService.Insert(vm.Worker);
            TempData["msg"] = "created successfully";
            return RedirectToAction("ManageWorkers");
        }

        public IActionResult WorkerDetails ( int? id )
        {
            if (id == null)
                return BadRequest();
            var worker = GetByIdWithCatigory(id.Value);
            if (worker == null)
                return NotFound();
            return View(worker);
        }


        [HttpGet]
        public IActionResult EditWorker ( int? id )
        {
            if (id == null)
                return BadRequest();
            var worker = workerService.GetById(id);
            var vm = GetWorkerCreateViewModel(worker);
            if (worker == null)
                return NotFound();
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditWorker ( WorkerWithCatigoryViewModel model )
        {
            var exist = workerService.IsEmailExist(model.Worker.Email, model.Worker.Id);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            workerService.Update(model.Worker);
            TempData["msg"] = "Update successfully";
            return RedirectToAction("ManageWorkers");
        }
        [HttpPost]
        public IActionResult DeleteWorkers ( int id )
        {
            var worker = workerService.GetById(id);
            if (worker != null)
            {
                workerService.Delete(id);
                TempData["msg"] = "تم حذف العامل بنجاح.";
            }
            return RedirectToAction("ManageWorkers");
        }

        // =============================== Manage Categories ===============================
        public IActionResult ManageCategories ()
        {
            var categories = catigoryService.GetAll();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategory ( Category category )
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                TempData["Error"] = "الاسم مطلوب.";
                return RedirectToAction("ManageCategories");
            }

            catigoryService.Insert(category);
            TempData["msg"] = "تمت الإضافة بنجاح.";
            return RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public IActionResult DeleteCategory ( int id )
        {
            var cat = catigoryService.GetById(id);
            if (cat != null)
            {
                catigoryService.Delete(id);
                TempData["msg"] = "تم حذف الفئة.";
            }
            return RedirectToAction("ManageCategories");
        }

        // =============================== Manage Reviews ===============================
        public IActionResult ManageReviews ()
        {
            var reviews = _context.Reviews.Include(r => r.Client).Include(r => r.Worker).ToList();
            return View(reviews);
        }

        [HttpPost]
        public IActionResult DeleteReview ( int id )
        {
            var review = ReviewService.GetById(id);
            if (review != null)
            {
                ReviewService.Delete(id);
                TempData["msg"] = "تم حذف التقييم.";
            }
            return RedirectToAction("ManageReviews");
        }

        // =============================== Reports ===============================
        public IActionResult Reports ()
        {
            var model = new AdminReportViewModel
            {
                TotalClients = clientService.GetAll().Count(),
                TotalWorkers = workerService.GetAll().Count(),
                TotalReviews = ReviewService.GetAll().Count(),
                AverageRating = ReviewService.GetAll().Any() ? Math.Round(ReviewService.GetAll().Average(r => r.Rating), 2) : 0,
                TopWorkers = _context.Workers
                    .OrderByDescending(w => w.Reviews.Average(r => r.Rating))
                    .Take(5)
                    .ToList(),
                Categories = catigoryService.GetAll()
            };

            return View(model);
        }



        public WorkerWithCatigoryViewModel GetWorkerCreateViewModel ( Worker? worker = null )
        {
            return new WorkerWithCatigoryViewModel
            {
                Worker = worker ?? new Worker(),
                Categorys = catigoryService.GetAll()
            };
        }
        public Worker? GetByIdWithCatigory ( int id )
        {
            return _context.Workers.Include(w => w.Category).FirstOrDefault(c => c.Id == id);
        }

    }
}




