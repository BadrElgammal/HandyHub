using HandyHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace HandyHub.Controllers
{
    public class WorkerController : Controller
    {
        IWorkerService workerService;
        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }
        [HttpGet]
        public IActionResult Search()
        {
            var workers = workerService.GetAllWorkersWithPortfolioWithUserWithReviews();

            return View(workers);
        }
    }
}
