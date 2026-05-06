using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payzo.Models;
using Payzo.Services;

namespace Payzo.Controllers;

[Authorize]
public class BudgetController : Controller
{
    private readonly UserService _users;
    private readonly FinanceService _finance;
    public BudgetController(UserService u, FinanceService f) { _users=u; _finance=f; }
    private string Uid => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public IActionResult Index()
    {
        var now = DateTime.Today;
        ViewBag.Spend = _finance.GetSpendByCategory(Uid, now.Month, now.Year);
        ViewBag.User  = _users.GetById(Uid);
        ViewData["Title"]     = "Бюджет";
        ViewData["ActiveNav"] = "budget";
        return View(_finance.GetBudgets(Uid, now.Month, now.Year));
    }

    [HttpPost] public IActionResult Add(string category, decimal limit)
    {
        if (limit <= 0) { TempData["Error"] = "Въведете валиден лимит."; return RedirectToAction("Index"); }
        var now = DateTime.Today;
        _finance.AddBudget(Uid, category, limit, now.Month, now.Year);
        TempData["Success"] = "Бюджетът е добавен!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Edit(string id, decimal limit)
    {
        _finance.UpdateBudget(Uid, id, limit);
        TempData["Success"] = "Бюджетът е обновен!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Delete(string id)
    {
        _finance.DeleteBudget(Uid, id);
        TempData["Success"] = "Бюджетът е изтрит!";
        return RedirectToAction("Index");
    }
}

[Authorize]
public class GoalsController : Controller
{
    private readonly UserService _users;
    private readonly FinanceService _finance;
    public GoalsController(UserService u, FinanceService f) { _users=u; _finance=f; }
    private string Uid => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public IActionResult Index()
    {
        ViewBag.User = _users.GetById(Uid);
        ViewData["Title"]     = "Спестовни цели";
        ViewData["ActiveNav"] = "goals";
        return View(_finance.GetGoals(Uid));
    }

    [HttpPost] public IActionResult Add(string name, string icon, decimal target, decimal saved, DateTime deadline)
    {
        if (string.IsNullOrWhiteSpace(name) || target <= 0)
        { TempData["Error"] = "Попълнете всички полета."; return RedirectToAction("Index"); }
        _finance.AddGoal(Uid, name, icon, target, saved, deadline);
        TempData["Success"] = "Целта е добавена!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Edit(string id, string name, string icon, decimal target, decimal saved, DateTime deadline)
    {
        var g = _finance.GetGoal(Uid, id);
        if (g != null) _finance.UpdateGoal(g, name, icon, target, saved, deadline);
        TempData["Success"] = "Целта е обновена!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Deposit(string id, decimal amount)
    {
        _finance.Deposit(Uid, id, amount);
        TempData["Success"] = $"+{amount:F2}€ добавени!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Delete(string id)
    {
        _finance.DeleteGoal(Uid, id);
        TempData["Success"] = "Целта е изтрита!";
        return RedirectToAction("Index");
    }
}
