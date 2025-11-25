using HandyHub.Data;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using HandyHub.Services;
using Microsoft.AspNetCore.Mvc;
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

        public ClientController(HandyHubDbContext context, ClientService _clientService, WorkerService _workerService, GenericService<Review> _ReviewService,GenericService<Favorite> _favoritService)
        {
            db = context;
            clientService = _clientService;
            workerService = _workerService;
            ReviewService = _ReviewService;
            favoriteService = _favoritService;
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

	}
}
