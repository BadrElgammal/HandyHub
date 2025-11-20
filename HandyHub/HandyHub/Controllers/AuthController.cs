using HandyHub.Data;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

//[Route("[controller]")]
public class AuthController : Controller
{
    private readonly HandyHubDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController ( HandyHubDbContext context, JwtService jwtService )
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpGet]
    public IActionResult Login ()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login ( LoginModel model )
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users
            .FirstOrDefault(u => u.Email == model.Email && u.Role == model.Role);

        if (user == null)
        {
            ModelState.AddModelError("", "لا يوجد حساب بهذا البريد الإلكتروني أو نوع الحساب غير صحيح.");
            return View(model);
        }

        bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

        if (!validPassword)
        {
            ModelState.AddModelError("Password", "كلمة المرور غير صحيحة، حاول مرة أخرى.");
            return View(model);
        }

        var token = _jwtService.GenerateToken(user);

        TempData["SuccessMessage"] = "Login Successfully";
        ViewBag.Token = token;
        ViewBag.Message = "تم تسجيل الدخول بنجاح.";
        return View(model);
    }
    [HttpGet]
    public IActionResult Register ()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register ( RegisterModel model )
    {
        if (!ModelState.IsValid)
            return View(model);

        if (_context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم بالفعل. يرجى استخدام بريد آخر.");
            return View(model);
        }
        if (model.Password != model.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "كلمة المرور وتأكيد كلمة المرور غير متطابقتين.");
            return View(model);
        }

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            City = model.City,
            Role = model.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        ViewBag.Message = "تم إنشاء الحساب بنجاح. يمكنك الآن تسجيل الدخول.";
        return View(model);
    }
}
