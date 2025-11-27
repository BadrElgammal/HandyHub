using HandyHub.Data;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using HandyHub.Models.ViewModels.WorkerVM;
using HandyHub.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace HandyHub.Controllers
{
    public class ClientController : Controller
    {
        private readonly HandyHubDbContext db;
        private readonly IClientService clientService;
        private readonly IWorkerService workerService;
        private readonly IService<Review> ReviewService;
        private readonly IService<Favorite> favoriteService;
        GenericService<WorkerPortfolio> Workerprotfilio;


        public ClientController(HandyHubDbContext context, ClientService _clientService, WorkerService _workerService, GenericService<Review> _ReviewService,GenericService<Favorite> _favoritService, GenericService<WorkerPortfolio> workerprotfilio)
        {
            db = context;
            clientService = _clientService;
            workerService = _workerService;
            ReviewService = _ReviewService;
            favoriteService = _favoritService;
            Workerprotfilio = workerprotfilio;
        }

        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            int id = db.Clients.Where(c => c.UserId == userId).Select(c => c.Id).First();
            var client = clientService.GetClientWithUserById(id);
            var reviews = ReviewService.GetAll().Where(r => r.ClientId == id).ToList();
            var favorits = favoriteService.GetAll().Where(f => f.ClientId == id).ToList();
            var workers = workerService.GetAllWithUser();

            var vm = new ClientDashboardVM
            {
                Client = client,
                Reviews = reviews,
                Favorites = favorits,
                Workers = workers
            };
            return View(vm);
        }
		[HttpGet]
		public IActionResult EditClient(int id)
		{
			if (id == null)
				return BadRequest();
			var client = clientService.GetClientWithUserById(id);
			if (client == null)
				return NotFound();
			return View(client);
		}

		[HttpPost]
		public IActionResult EditClient(Client model)
		{
			var exist = clientService.IsEmailExist(model.User.Email, model.UserId);
			if (exist)
			{
				ModelState.AddModelError("", "email already exists");
			}
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			model.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.User.PasswordHash);
			clientService.UpdateClientWithUser(model);
			TempData["msg"] = "Update successfully";
			return RedirectToAction("Profile");
		}
		[HttpPost]
		public IActionResult DeleteClient(int id)
		{
			clientService.DeleteClientWithUser(id);
			TempData["msg"] = "تم حذف المستخدم بنجاح.";
			Response.Cookies.Delete("jwt");
			return RedirectToAction("Index", "Home");
		}

		// Seach
		[HttpGet]
		public IActionResult Search()
		{
			var workers = workerService.GetAllWorkersWithPortfolioWithUserWithReviews();

			return View(workers);
		}

        // Worker Profile
      
        public IActionResult WorkerProfile(int id)
        {
            var worker = workerService.GetWorkerWithUserById(id);
            if (worker == null)
            {
                TempData["msg"] = "العامل غير موجود.";
                return RedirectToAction("Search");
            }

            var reviews = ReviewService.GetAll().Where(r => r.WorkerId == id).ToList();
            var client = clientService.GetAllWithUser();
            var vm = new WorkerEditViewModel
            {
                Worker = worker,
                Review = reviews,
                Portfolio = Workerprotfilio.GetAll().Where(p => p.WorkerId == id).ToList(),
                Categories = worker.Category,
                Clients = client
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult AddReview(Review model)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var clientId = db.Clients.FirstOrDefault(c => c.UserId == userId)?.Id ?? 0;

            model.ClientId = clientId;
            model.CreatedAt = DateTime.Now;


            if (!ModelState.IsValid)
            {
                TempData["msg"] = "الرجاء ملء جميع الحقول.";
                return RedirectToAction("WorkerProfile", "Client", new { id = model.WorkerId });
            }

            ReviewService.Insert(model);
            TempData["msg"] = "✅ تم إضافة التقييم بنجاح.";
            return RedirectToAction("WorkerProfile", "Client", new { id = model.WorkerId });
        }

      



    }
}
