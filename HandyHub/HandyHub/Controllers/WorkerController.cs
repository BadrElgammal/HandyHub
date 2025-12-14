using HandyHub.Data;
using HandyHub.Helper;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using HandyHub.Models.ViewModels.WorkerVM;
using HandyHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace HandyHub.Controllers
{
    [Authorize(Roles = "Worker")]
    public class WorkerController : Controller
    {
        IWorkerService workerService;
        GenericService<Category> catigoryService;
        GenericService<Review> ReviewService;
        GenericService<WorkerPortfolio> Workerprotfilio;
        IClientService clientService;


        private readonly HandyHubDbContext db;


        public WorkerController ( IWorkerService workerService, GenericService<Category> catigoryService, HandyHubDbContext context, GenericService<Review> reviewService, GenericService<WorkerPortfolio> workerprotfilio, IClientService clientService )
        {
            db = context;
            this.workerService = workerService;
            this.catigoryService = catigoryService;
            ReviewService = reviewService;
            Workerprotfilio = workerprotfilio;
            this.clientService = clientService;
        }
        [HttpGet]
        [HttpGet]
        public IActionResult Search ( int? categoryId, string? city, double? rating, bool? available )
        {
            var workers = workerService.GetAllWorkersWithPortfolioWithUserWithReviews();
            if (categoryId.HasValue)
                workers = workers.Where(w => w.CategoryId == categoryId.Value).ToList();

            if (!string.IsNullOrEmpty(city))
                workers = workers.Where(w => w.User.City.Contains(city)).ToList();

            if (rating.HasValue)
                workers = workers.Where(w => w.Reviews.Any() && w.Reviews.Average(r => r.Rating) >= rating.Value).ToList();

            if (available.HasValue && available.Value)
                workers = workers.Where(w => w.IsAvailable).ToList();

            ViewBag.caregories = catigoryService.GetAll();
            return View(workers);
        }
        // GET: WorkerPortfolio/Create
        //[HttpGet]
        //public IActionResult Create(int id)
        //{
        //    ViewBag["id"] = (int)id;
        //    // 1. استخراج الـ WorkerId بالطريقة المتبعة في Profile()
        //    var userIdClaim = User.FindFirst("UserId");
        //    if (userIdClaim == null)
        //    {
        //        // إذا لم يكن هناك مستخدم مسجل دخوله
        //        return RedirectToAction("Login", "Account"); // أو أي صفحة مناسبة
        //    }
        //    var userId = int.Parse(userIdClaim.Value);

        //    // 🚨 يجب التأكد من أن db موجود في هذا الـ Controller (وهو كذلك في السياق السابق)
        //    var workerId = db.Workers.Where(c => c.UserId == userId).Select(c => c.Id).FirstOrDefault();

        //    if (workerId == 0)
        //    {
        //        // إذا لم يتم العثور على Worker مرتبط
        //        return NotFound();
        //    }

        //    // 2. إرسال الكائن مع تعيين WorkerId مسبقاً
        //    var newPortfolio = new WorkerPortfolio { WorkerId = workerId };

        //    return View(newPortfolio);
        //}
        //// POST: WorkerPortfolio/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(WorkerPortfolio portfolio)
        //{
        //    portfolio.WorkerId = ViewBag["id"];
        //    // 1. استخراج الـ WorkerId من هوية المستخدم المسجل دخوله
        //    try
        //    {
        //        var userIdClaim = User.FindFirst("UserId");
        //        if (userIdClaim == null)
        //        {
        //            ModelState.AddModelError("", "خطأ في الجلسة: لا يوجد عامل مسجل دخوله.");
        //            return View(portfolio);
        //        }

        //        var userId = int.Parse(userIdClaim.Value);

        //        // 🚨 البحث عن الـ WorkerId في قاعدة البيانات
        //        var workerId = db.Workers.Where(c => c.UserId == userId).Select(c => c.Id).FirstOrDefault();

        //        if (workerId == 0)
        //        {
        //            ModelState.AddModelError("", "خطأ: لم يتم العثور على سجل العامل المرتبط.");
        //            return View(portfolio);
        //        }

        //        // 🎯 تعيين الـ WorkerId الصحيح لكائن الـ Portfolio
        //        portfolio.WorkerId = workerId;
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("", "حدث خطأ أثناء تحديد هوية العامل.");
        //        return View(portfolio);
        //    }

        //    // 2. تعيين قيمة افتراضية للـ ImageUrl (إذا كان فارغاً، لتجنب خطأ [Required])
        //    if (string.IsNullOrEmpty(portfolio.ImageUrl))
        //    {
        //        portfolio.ImageUrl = "/images/default_portfolio.png"; // أو أي رابط صورة افتراضية
        //    }

        //    // الآن ModelState.IsValid من المرجح أن تكون صحيحة لأن WorkerId تم تعيينه
        //    if (ModelState.IsValid)
        //    {
        //        // 3. حفظ الكائن في قاعدة البيانات
        //        Workerprotfilio.Insert(portfolio);
        //        TempData["success"] = "تم إضافة العمل بنجاح.";

        //        // 4. التوجيه
        //        return RedirectToAction("Profile", "Worker", new { id = portfolio.WorkerId });
        //    }

        //    // إذا فشل التحقق (لأي سبب آخر غير WorkerId)، أعد الـ View
        //    return View(portfolio);
        //}
        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    var worker = workerService.GetById(id);
        //    if (worker != null)
        //    {
        //        workerService.Delete(worker);
        //        Response.Cookies.Delete("jwt");
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
        [HttpPost]
        public IActionResult DeleteWorker ( int id )
        {
            workerService.DeleteWorkerWithUser(id);
            Response.Cookies.Delete("jwt");
            TempData["msg"] = "تم حذف العامل بنجاح.";
            return RedirectToAction("index", "Home");
        }
        [HttpPost]
        public IActionResult logout ()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");

        }
        //[HttpGet]
        //public IActionResult Edit(WorkerEditViewModel WorkerVM)
        //{
        //    var User = clientService.GetById(WorkerVM.User.Id);
        //    if (User.User.Role=="Worker")
        //    {
        //    var workerEditViewModel = new WorkerEditViewModel
        //    {
        //        Worker = User,
        //        User = User.User,
        //        Categories = WorkerVM.Categories
        //    };

        //    }
        //    if (User == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(workerEditViewModel);
        //}
        //[HttpPost]

        //public IActionResult Edit(WorkerEditViewModel WorkerVM, IFormFile? imageFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var worker = workerService.GetById(WorkerVM.Worker.Id);
        //        if (worker == null)
        //        {
        //            return NotFound();
        //        }
        //        // Update worker properties
        //        worker.Area = WorkerVM.Worker.Area;
        //        worker.Bio = WorkerVM.Worker.Bio;
        //        worker.IsAvailable = WorkerVM.Worker.IsAvailable;
        //        worker.CategoryId = WorkerVM.Worker.CategoryId;
        //        // Update user properties
        //        worker.User.Name = WorkerVM.User.Name;
        //        worker.User.Email = WorkerVM.User.Email;
        //        worker.User.Phone = WorkerVM.User.Phone;
        //        worker.User.City = WorkerVM.User.City;
        //        // Handle image upload if a new file is provided
        //        if (imageFile != null && imageFile.Length > 0)
        //        {
        //            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        //            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        //            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                imageFile.CopyTo(fileStream);
        //            }
        //            // Update the user's ImageUrl
        //            worker.User.ImageUrl = "/images/" + uniqueFileName;
        //        }
        //        workerService.Update(worker);
        //        return RedirectToAction("Profile", "Worker", new { id = worker.Id });
        //    }
        //    // If we got this far, something failed; redisplay form
        //    WorkerVM.Categories = workerService.GetAllWithUser().Select(w => w.Category).Distinct().ToList();
        //    return View(WorkerVM);
        //}
        //[HttpGet]
        //public IActionResult EditWorker(int? id)
        //{
        //    if (id == null)
        //        return BadRequest();

        //    // تأكد من جلب بيانات العامل والمستخدم
        //    var worker = workerService.GetWorkerWithUserById(id.Value);
        //    if (worker == null) return NotFound();

        //    // ✅ التعديل هنا: نستخدم WorkerEditViewModel
        //    //var vm = new WorkerEditViewModel
        //    //{
        //    //    Worker = worker,
        //    //    User = worker.User, // تأكد من تعبئة خاصية الـ User أيضًا
        //    //    Categories = catigoryService.GetAll()
        //    //};

        //    return View(vm);
        //}

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
                Id=worker.Id,
                UserId=(int)worker.UserId,
                Name=worker.User.Name,
                Email = worker.User.Email,
                Phone = worker.User.Phone,
                City = worker.User.City,
                Bio = worker.Bio,
                Area= worker.Area,
                CategoryId =worker.CategoryId,
                Categorys = catigoryService.GetAll(),
                ExistingProfileImagePath = worker.User.ImageUrl
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult EditWorker ( EditWorkerVM model )
        {
            var exist = workerService.IsEmailExist(model.Email, model.UserId);
            if (exist)
            {
                ModelState.AddModelError("", "email already exists");
            }

            if (!ModelState.IsValid)
            {
                model.Categorys = catigoryService.GetAll();
                return View(model);
            }

            var worker = workerService.GetWorkerWithUserById(model.Id);
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

            worker.Area = model.Area;
            worker.Bio = model.Bio;
            worker.CategoryId = model.CategoryId;

            worker.User.Name = model.Name;
            worker.User.Email = model.Email;
            worker.User.Phone = model.Phone;
            worker.User.City = model.City;



            if (model.ProfileImage != null)
            {
                if (!string.IsNullOrEmpty(worker.User.ImageUrl))
                {
                    if (worker.User.ImageUrl != "default-avatar-admin.png")
                    {
                        Upload.RemoveProfileImage("ProfileImages", worker.User.ImageUrl);
                    }
                }

                var fileName = Upload.UploadProfileImage("ProfileImages", model.ProfileImage);
                worker.User.ImageUrl = fileName;
            }

            workerService.UpdateWorkerWithUser(worker);

            TempData["msg"] = "تم تحديث بيانات العامل بنجاح.";
            return RedirectToAction("Profile");
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

        public IActionResult Profile ()
        {



            var userId = int.Parse(User.FindFirst("UserId").Value);
            int id = db.Workers.Where(c => c.UserId == userId).Select(c => c.Id).First();
            var worker = workerService.GetWorkerWithUserById(id);

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
        public IActionResult WorkerProfile ( int id )
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
        [HttpGet]
        public IActionResult AddService(int id)
        {
            var worker = workerService.GetById(id);
            if (worker == null)
            {
                return NotFound();
            }

            var model = new WorkerPortfolio { WorkerId = id };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddService(WorkerPortfolio model, IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                    throw new Exception("يجب اختيار صورة للعمل.");

                if (model.WorkerId == 0)
                    throw new Exception("حدث خطأ في تحديد هوية العامل.");

                string fileName = Upload.UploadProfileImage("worker-portfolio", image);

                if (string.IsNullOrEmpty(fileName))
                    throw new Exception("فشل رفع الصورة.. يرجى التأكد من صلاحيات المجلد.");

                model.ImageUrl = fileName;

                model.Id = 0;

                Workerprotfilio.Insert(model);

                TempData["msg"] = "تم إضافة العمل الجديد بنجاح!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                ModelState.AddModelError("", $"عفواً حدث خطأ: {realError}");

                return View(model);
            }
        }
    }

}


