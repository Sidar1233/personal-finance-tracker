using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payzo.Models;
using Payzo.Services;

namespace Payzo.Controllers;

public class AccountController : Controller
{
    private readonly UserService _users;
    public AccountController(UserService users) => _users = users;

    private string? CurrentUserId =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Dashboard");
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        var user = _users.ValidateLogin(vm.Email, vm.Password);
        if (user == null)
        { vm.Error = "Невалиден имейл или парола."; return View(vm); }
        if (!user.Active)
        { vm.Error = "Акаунтът е деактивиран. Свържете се с администратор."; return View(vm); }

        _users.UpdateLastLogin(user.Id);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name,           user.Name),
            new(ClaimTypes.Email,          user.Email),
            new(ClaimTypes.Role,           user.Role.ToString()),
            new("Avatar",                  user.Avatar),
            new("AvatarColor",             user.AvatarColor),
        };

        await HttpContext.SignInAsync("PayzoCookie",
            new ClaimsPrincipal(new ClaimsIdentity(claims, "PayzoCookie")));

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public IActionResult Register(RegisterViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Name) || string.IsNullOrWhiteSpace(vm.Email) || string.IsNullOrWhiteSpace(vm.Password))
        { vm.Error = "Попълнете всички полета."; return View(vm); }
        if (vm.Password != vm.ConfirmPassword)
        { vm.Error = "Паролите не съвпадат."; return View(vm); }
        if (vm.Password.Length < 6)
        { vm.Error = "Паролата трябва да е поне 6 символа."; return View(vm); }
        if (_users.EmailExists(vm.Email))
        { vm.Error = "Имейлът вече е регистриран."; return View(vm); }

        _users.Create(vm.Name, vm.Email, vm.Password);
        TempData["Success"] = "Регистрацията е успешна! Влезте в акаунта си.";
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("PayzoCookie");
        return RedirectToAction("Login");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        var user = _users.GetById(CurrentUserId!);
        if (user == null) return RedirectToAction("Login");
        ViewData["Title"]     = "Профил";
        ViewData["Subtitle"]  = "Управление на вашия акаунт";
        ViewData["ActiveNav"] = "profile";
        return View(user);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Profile(string name, string email, string? password)
    {
        var user = _users.GetById(CurrentUserId!);
        if (user == null) return RedirectToAction("Login");
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        { TempData["Error"] = "Попълнете всички полета."; return RedirectToAction("Profile"); }
        if (_users.EmailExists(email, user.Id))
        { TempData["Error"] = "Имейлът вече е зает."; return RedirectToAction("Profile"); }
        _users.Update(user, name, email, user.Role, user.Active, password);
        TempData["Success"] = "Профилът е обновен!";
        return RedirectToAction("Profile");
    }
}
