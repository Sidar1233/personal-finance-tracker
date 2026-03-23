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
    private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public IActionResult Index()
    {
        var now = DateTime.Today;
        var budgets = _finance.GetBudgets(UserId, now.Month, now.Year);
        var spend   = _finance.GetSpendByCategory(UserId, now.Month, now.Year);
        ViewBag.Spend = spend;
        ViewBag.User  = _users.GetById(UserId);
        return View(budgets);
    }

    [HttpPost] public IActionResult Add(string category, decimal limit)
    {
        if (limit <= 0) { TempData["Error"] = "Въведете валиден лимит."; return RedirectToAction("Index"); }
        var now = DateTime.Today;
        _finance.AddBudget(UserId, category, limit, now.Month, now.Year);
        TempData["Success"] = "Бюджетът е добавен!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Edit(string id, decimal limit)
    {
        var b = _finance.GetBudget(id);
        if (b != null && b.UserId == UserId) b.LimitAmount = limit;
        TempData["Success"] = "Бюджетът е обновен!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Delete(string id)
    {
        var b = _finance.GetBudget(id);
        if (b != null && b.UserId == UserId) _finance.DeleteBudget(id);
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
    private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public IActionResult Index()
    {
        ViewBag.User = _users.GetById(UserId);
        return View(_finance.GetGoals(UserId));
    }

    [HttpPost] public IActionResult Add(string name, string icon, decimal target, decimal saved, DateTime deadline)
    {
        if (string.IsNullOrWhiteSpace(name) || target <= 0)
        { TempData["Error"] = "Попълнете всички полета."; return RedirectToAction("Index"); }
        _finance.AddGoal(UserId, name, icon, target, saved, deadline);
        TempData["Success"] = "Целта е добавена!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Edit(string id, string name, string icon, decimal target, decimal saved, DateTime deadline)
    {
        var g = _finance.GetGoal(id);
        if (g != null && g.UserId == UserId)
        { g.Name=name; g.Icon=icon; g.TargetAmount=target; g.SavedAmount=saved; g.Deadline=deadline; }
        TempData["Success"] = "Целта е обновена!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Deposit(string id, decimal amount)
    {
        var g = _finance.GetGoal(id);
        if (g != null && g.UserId == UserId) g.SavedAmount += amount;
        TempData["Success"] = $"+{amount:F2}€ добавени!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Delete(string id)
    {
        var g = _finance.GetGoal(id);
        if (g != null && g.UserId == UserId) _finance.DeleteGoal(id);
        TempData["Success"] = "Целта е изтрита!";
        return RedirectToAction("Index");
    }
}
