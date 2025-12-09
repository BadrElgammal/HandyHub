using HandyHub.Data;
using HandyHub.Helper;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using HandyHub.Models.ViewModels.ClientVM;
using HandyHub.Models.ViewModels.WorkerVM;
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
                RecentClients = _context.Clients
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(5)
                    .ToList(),
                RecentWorkers = _context.Workers
                    .Include(w => w.User)
                    .OrderByDescending(w => w.CreatedAt)
                    .Take(5)
                    .ToList(),
            };

            return View(model);
        }

        // =============================== Manage Users ===============================
        public IActionResult ManageClients ()
        {
            var clients = clientService.GetAllWithUser();
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
            var exist = clientService.IsEmailExist(model.User.Email);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.User.PasswordHash);
            clientService.CreateClientWithUser(model);
            TempData["msg"] = "created successfully";
            return RedirectToAction("ManageClients");
        }


        public IActionResult ClientDetails ( int id )
        {
            var client = clientService.GetClientWithUserById(id);
            if (client == null)
                return NotFound();
            return View(client);
        }



        [HttpGet]
        public IActionResult EditClient ( int id )
        {
            var client = clientService.GetClientWithUserById(id);
            if (client == null)
                return NotFound();

            var vm = new EditClientVM
            {
                Id = client.Id,
                UserId = (int)client.UserId,
                Name = client.User.Name,
                Email = client.User.Email,
                Phone = client.User.Phone,
                City = client.User.City,
                ExistingProfileImagePath = client.User.ImageUrl   // جديد
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult EditClient ( EditClientVM model )
        {
            var exist = clientService.IsEmailExist(model.Email, model.UserId);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = clientService.GetClientWithUserById(model.Id);
            if (client == null)
                return NotFound();

            if (model.ProfileImage != null)
            {
                if (!string.IsNullOrEmpty(client.User.ImageUrl))
                {
                    Upload.RemoveProfileImage("ProfileImages", client.User.ImageUrl);
                }

                var fileName = Upload.UploadProfileImage("ProfileImages", model.ProfileImage);
                client.User.ImageUrl = fileName;
            }

            if (!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                if (string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "يجب إدخال كلمة المرور وتأكيدها معًا.");
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "كلمتا المرور غير متطابقتين.");
                    return View(model);
                }

                client.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            client.User.Name = model.Name;
            client.User.Email = model.Email;
            client.User.Phone = model.Phone;
            client.User.City = model.City;

            clientService.UpdateClientWithUser(client);

            TempData["msg"] = "تم تحديث بيانات العميل بنجاح.";
            return RedirectToAction("ManageClients");
        }





        [HttpPost]
        public IActionResult DeleteClient ( int id )
        {
            clientService.DeleteClientWithUser(id);
            TempData["msg"] = "تم حذف المستخدم بنجاح.";
            return RedirectToAction("ManageClients");
        }

        // =============================== Manage Workers ===============================
        public IActionResult ManageWorkers ()
        {
            var workers = _context.Workers.Include(w => w.User).Include(w => w.Category).ToList();
            return View(workers);
        }

        [HttpPost]
        public IActionResult SuspendWorker ( int id )
        {
            var worker = workerService.GetWorkerWithUserById(id);
            if (worker == null)
                return NotFound();

            if (workerService.SuspendWorker(worker))
                TempData["msg"] = $"تم اتاحة العامل {worker.User.Name}.";
            else
                TempData["msg"] = $"تم إيقاف العامل {worker.User.Name} مؤقتاً.";
            return RedirectToAction("ManageWorkers");
        }

        [HttpPost]
        public IActionResult DeleteWorker ( int id )
        {
            workerService.DeleteWorkerWithUser(id);
            TempData["msg"] = "تم حذف العامل بنجاح.";
            return RedirectToAction("ManageWorkers");
        }

        [HttpGet]
        public IActionResult CreateWorker ()
        {
            var vm = new WorkerWithCatigoryViewModel
            {
                Worker = new Worker(),
                Categorys = catigoryService.GetAll()
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult CreateWorker ( WorkerWithCatigoryViewModel vm )
        {
            var exists = workerService.IsEmailExist(vm.Worker.User.Email);
            if (exists)
            {
                ModelState.AddModelError("", "email already exists");
            }
            if (!ModelState.IsValid)
            {
                vm.Categorys = catigoryService.GetAll();
                return View(vm);
            }


            if (vm.ProfileImage != null)
            {
                var fileName = Upload.UploadProfileImage("ProfileImages", vm.ProfileImage);
                vm.Worker.User.ImageUrl = fileName;
            }

            vm.Worker.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Worker.User.PasswordHash);
            workerService.CreateWorkerWithUser(vm.Worker);
            TempData["msg"] = "created successfully";
            return RedirectToAction("ManageWorkers");
        }


        public IActionResult WorkerDetails ( int? id )
        {
            if (id == null)
                return BadRequest();
            var worker = workerService.GetWorkerWithUserById(id.Value);
            if (worker == null)
                return NotFound();
            return View(worker);
        }


        [HttpGet]
        public IActionResult EditWorker ( int? id )
        {
            if (id == null)
                return BadRequest();

            var worker = workerService.GetWorkerWithUserById(id.Value);
            if (worker == null)
                return NotFound();

            var vm = new EditWorkerVM
            {
                Worker = worker,
                Categorys = catigoryService.GetAll()
            };
            return View(vm);
        }


        [HttpPost]
        public IActionResult EditWorker ( EditWorkerVM model )
        {
            var exist = workerService.IsEmailExist(model.Worker.User.Email, model.Worker.UserId);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }

            if (!ModelState.IsValid)
            {
                model.Categorys = catigoryService.GetAll();
                return View(model);
            }

            var worker = workerService.GetWorkerWithUserById(model.Worker.Id);
            if (worker == null)
                return NotFound();
            if (!string.IsNullOrWhiteSpace(model.Password) || !string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                if (string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "يجب إدخال كلمة المرور وتأكيدها معًا.");
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "كلمتا المرور غير متطابقتين.");
                    return View(model);
                }

                worker.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            //worker.Area = model.Worker.Area;
            //worker.Bio = model.Worker.Bio;
            //worker.IsAvailable = model.Worker.IsAvailable;
            //worker.CategoryId = model.Worker.CategoryId;

            //worker.User.Name = model.Worker.User.Name;
            //worker.User.Email = model.Worker.User.Email;
            //worker.User.Phone = model.Worker.User.Phone;
            //worker.User.City = model.Worker.User.City;



            //if (model.ProfileImage != null)
            //{
            //    if (!string.IsNullOrEmpty(worker.ProfileImagePath))
            //    {
            //        Upload.RemoveProfileImage("ProfileImages", worker.User.ImageUrl);
            //    }

            //    var fileName = Upload.UploadProfileImage("ProfileImages", model.ProfileImage);
            //    worker.User.ImageUrl = fileName;
            //}

            workerService.UpdateWorkerWithUser(worker);

            TempData["msg"] = "تم تحديث بيانات العامل بنجاح.";
            return RedirectToAction("ManageWorkers");
        }

        [HttpPost]
        public IActionResult DeleteWorkers ( int id )
        {
            workerService.DeleteWorkerWithUser(id);
            TempData["msg"] = "تم حذف العامل بنجاح.";
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
            var reviews = _context.Reviews.Include(r => r.Client).ThenInclude(c => c.User).Include(r => r.Worker).ThenInclude(w => w.User).ToList();
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
                TotalClients = _context.Clients.Count(),
                TotalWorkers = _context.Workers.Count(),
                TotalReviews = _context.Reviews.Count(),
                AverageRating = _context.Reviews.Any()
                    ? Math.Round(_context.Reviews.Average(r => r.Rating), 2)
                    : 0,
                TopWorkers = _context.Workers
                    .Include(w => w.User)
                    .Include(w => w.Reviews)
                    .OrderByDescending(w => w.Reviews.Average(r => (double?)r.Rating) ?? 0)
                    .Take(5)
                    .ToList(),
                Categories = _context.Categories.ToList()
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


        public IActionResult logout ()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }
    }
}




