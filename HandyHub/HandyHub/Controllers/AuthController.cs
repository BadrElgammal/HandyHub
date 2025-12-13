using HandyHub.Data;
using HandyHub.Models.Entities;
using HandyHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = _jwtService.GenerateToken(user);

        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            Expires = DateTime.UtcNow.AddHours(2)
        });

        ViewBag.Token = token;
        TempData["Success"] = "تم تسجيل الدخول بنجاح";
        if (user.Role == "Admin")
        {
            return RedirectToAction("Index", "Home");
        }
        else if (user.Role == "Worker")
        {
            return RedirectToAction("Index", "Home");
        }
        else if (user.Role == "Client")
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
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
        switch (model.Role.ToLower())
        {
            case "admin":
                var admin = new Admin { UserId = user.Id };
                _context.Admins.Add(admin);
                break;

            case "client":
                var client = new Client { UserId = user.Id };
                _context.Clients.Add(client);
                break;

            case "worker":
                var worker = new Worker { UserId = user.Id };
                _context.Workers.Add(worker);
                break;
        }

        _context.SaveChanges();
        TempData["Success"] = "تم إنشاء الحساب بنجاح. يمكنك الآن تسجيل الدخول.";
        return RedirectToAction("Login");
    }
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        TempData["Success"] = "تم تسجيل الخروج بنجاح";
        return RedirectToAction("Index", "Home");
    }
}
