using HandyHub.Data;
using HandyHub.Helper; 
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels.ClientVM;
using HandyHub.Models.ViewModels.WorkerVM;
using HandyHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HandyHub.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly HandyHubDbContext db;
        private readonly IClientService clientService;
        private readonly IWorkerService workerService;
        private readonly IService<Review> reviewService;
        private readonly IService<Favorite> favoriteService;
        private readonly GenericService<WorkerPortfolio> workerPortfolioService;
        private readonly IService<Category> categoryService;

        public ClientController (
            HandyHubDbContext context,
            ClientService _clientService,
            WorkerService _workerService,
            GenericService<Review> _reviewService,
            GenericService<Favorite> _favoriteService,
            GenericService<WorkerPortfolio> workerPortfolioService,
            GenericService<Category> _categoryService )
        {
            db = context;
            clientService = _clientService;
            workerService = _workerService;
            reviewService = _reviewService;
            favoriteService = _favoriteService;
            this.workerPortfolioService = workerPortfolioService;
            categoryService = _categoryService;
        }

        public IActionResult Profile ()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            int id = db.Clients
                       .Where(c => c.UserId == userId)
                       .Select(c => c.Id)
                       .First();

            var client = clientService.GetClientWithUserById(id);
            var reviews = reviewService.GetAll().Where(r => r.ClientId == id).ToList();
            var favorites = favoriteService.GetAll().Where(f => f.ClientId == id).ToList();
            var workers = workerService.GetAllWithUser();
            var category = categoryService.GetAll();

            var vm = new Models.ViewModels.ClientDashboardVM
            {
                Client = client,
                Reviews = reviews,
                Favorites = favorites,
                Workers = workers,
                Categories = category
            };

            return View(vm);
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
                ExistingProfileImagePath = client.User.ImageUrl
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
                    if (client.User.ImageUrl != "default-avatar-admin.png")
                    {
                        Upload.RemoveProfileImage("ProfileImages", client.User.ImageUrl);
                    }
                }

                var fileName = Upload.UploadProfileImage("ProfileImages", model.ProfileImage);
                client.User.ImageUrl = fileName;
            }

            // معالجة الباسورد
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

            TempData["Success"] = "تم تحديث بيانات العميل بنجاح.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult DeleteClient ( int id )
        {
            clientService.DeleteClientWithUser(id);
            TempData["Success"] = "تم حذف المستخدم بنجاح.";
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }

        // Search
        [HttpGet]
        public IActionResult Search ( int? categoryId, string? city, double? rating, bool? available )
        {
            var workers = workerService.GetAllWorkersWithPortfolioWithUserWithReviews();

            if (categoryId.HasValue)
                workers = workers.Where(w => w.CategoryId == categoryId.Value).ToList();

            if (!string.IsNullOrEmpty(city))
                workers = workers.Where(w => w.User.City.Contains(city)).ToList();

            if (rating.HasValue)
                workers = workers.Where(w => w.Reviews.Any() &&
                                             w.Reviews.Average(r => r.Rating) >= rating.Value)
                                 .ToList();

            if (available.HasValue && available.Value)
                workers = workers.Where(w => w.IsAvailable).ToList();

            ViewBag.caregories = categoryService.GetAll();
            return View(workers);
        }

        // Worker Profile
        public IActionResult WorkerProfile ( int id )
        {
            var worker = workerService.GetWorkerWithUserById(id);
            if (worker == null)
            {
                TempData["Error"] = "العامل غير موجود";
                return RedirectToAction("Search");
            }

            var reviews = reviewService.GetAll().Where(r => r.WorkerId == id).ToList();
            var clients = clientService.GetAllWithUser();
            var favorites= favoriteService.GetAll();

            var vm = new WorkerEditViewModel
            {
                Worker = worker,
                Review = reviews,
                Portfolio = workerPortfolioService.GetAll().Where(p => p.WorkerId == id).ToList(),
                Categories = worker.Category,
                Clients = clients,
                Favorites = favorites
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddReview ( Review model )
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var clientId = db.Clients.FirstOrDefault(c => c.UserId == userId)?.Id ?? 0;

            model.ClientId = clientId;
            model.CreatedAt = DateTime.Now;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "الرجاء ملء جميع الحقول.";
                return RedirectToAction("WorkerProfile", "Client", new { id = model.WorkerId });
            }

            reviewService.Insert(model);
            TempData["Success"] = "✅ تم إضافة التقييم بنجاح.";
            return RedirectToAction("WorkerProfile", "Client", new { id = model.WorkerId });
        }

        [HttpPost]
        public IActionResult AddToFavorite ( int workerId )
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var clientId = db.Clients.FirstOrDefault(c => c.UserId == userId)?.Id ?? 0;

            if (clientId == 0)
            {
                TempData["Error"] = "خطأ: العميل غير موجود.";
                return RedirectToAction("Search");
            }

            var favorite = favoriteService.GetAll()
                .FirstOrDefault(f => f.ClientId == clientId && f.WorkerId == workerId);

            if (favorite != null)
            {
                favoriteService.Delete(favorite.Id);
                TempData["Success"] = "✅ تم إزالة العامل من المفضلة.";
            }
            else
            {
                var newFavorite = new Favorite
                {
                    ClientId = clientId,
                    WorkerId = workerId,
                    CreatedAt = DateTime.Now
                };
                favoriteService.Insert(newFavorite);
                TempData["Success"] = "✅ تم إضافة العامل إلى المفضلة.";
            }

            return RedirectToAction("WorkerProfile", new { id = workerId });
        }
    }
}
