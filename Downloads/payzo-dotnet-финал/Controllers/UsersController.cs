using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payzo.Models;
using Payzo.Data;
using Payzo.Services;

namespace Payzo.Controllers;

[Authorize(Policy = "AdminOnly")]
public class UsersController : Controller
{
    private readonly UserService _users;
    private readonly FinanceService _finance;
    private readonly PayzoDb _db;
    public UsersController(UserService u, FinanceService f) { _users=u; _finance=f; }

    private string CurrentUserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
    private bool IsSuperAdmin => User.IsInRole("SuperAdmin");

    public IActionResult Index(string? role, string? status, string? search)
    {
        var all = _users.GetAll();
        if (!string.IsNullOrEmpty(role))   all = all.Where(u => u.Role.ToString() == role).ToList();
        if (!string.IsNullOrEmpty(status)) all = all.Where(u => status=="active" ? u.Active : !u.Active).ToList();
        if (!string.IsNullOrEmpty(search)) all = all.Where(u => u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        var allUsers = _users.GetAll();
        return View(new UsersViewModel
        {
            CurrentUser      = _users.GetById(CurrentUserId)!,
            Users            = all,
            TotalUsers       = allUsers.Count,
            ActiveUsers      = allUsers.Count(u=>u.Active),
            AdminCount       = allUsers.Count(u=>u.Role!=UserRole.User),
            TotalTransactions= _db.Transactions.Count(),
        });
    }

    [HttpPost] public IActionResult Create(UserFormViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Name) || string.IsNullOrWhiteSpace(vm.Email) || string.IsNullOrWhiteSpace(vm.Password))
        { TempData["Error"] = "Попълнете всички полета."; return RedirectToAction("Index"); }
        if (_users.EmailExists(vm.Email))
        { TempData["Error"] = "Имейлът вече съществува."; return RedirectToAction("Index"); }
        // Only superadmin can create superadmin/admin
        if (vm.Role == UserRole.SuperAdmin && !IsSuperAdmin)
        { TempData["Error"] = "Нямате права за тази роля."; return RedirectToAction("Index"); }

        _users.Create(vm.Name, vm.Email, vm.Password, vm.Role);
        TempData["Success"] = "Потребителят е създаден!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Edit(string id, string name, string email,
        UserRole role, bool active, string? password)
    {
        var user = _users.GetById(id);
        if (user == null) return NotFound();
        if (!IsSuperAdmin && role != UserRole.User)
        { TempData["Error"] = "Нямате права за тази роля."; return RedirectToAction("Index"); }
        if (_users.EmailExists(email, id))
        { TempData["Error"] = "Имейлът вече съществува."; return RedirectToAction("Index"); }

        _users.Update(user, name, email, role, active, password);
        TempData["Success"] = "Потребителят е обновен!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    public IActionResult Delete(string id)
    {
        if (id == CurrentUserId)
        { TempData["Error"] = "Не може да изтриете собствения си акаунт."; return RedirectToAction("Index"); }
        _users.Delete(id);
        TempData["Success"] = "Потребителят е изтрит!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult ToggleActive(string id)
    {
        if (id == CurrentUserId)
        { TempData["Error"] = "Не може да деактивирате собствения си акаунт."; return RedirectToAction("Index"); }
        var user = _users.GetById(id);
        if (user != null) user.Active = !user.Active;
        TempData["Success"] = user?.Active == true ? "Акаунтът е активиран!" : "Акаунтът е деактивиран!";
        return RedirectToAction("Index");
    }

    // ── Reports (admin+) ──────────────────────────────────────────
    public IActionResult Reports()
    {
        ViewBag.Users = _users.GetAll();
        ViewBag.AllTransactions = _finance.GetTransactions("u004")
            .Concat(_finance.GetTransactions("u005")).ToList();
        return View();
    }

    // ── Settings (superadmin only) ────────────────────────────────
    [Authorize(Policy = "SuperAdminOnly")]
    public IActionResult Settings()
    {
        ViewBag.Stats = new { Users = _users.GetAll().Count, Active = _users.GetAll().Count(u=>u.Active) };
        return View();
    }
}
