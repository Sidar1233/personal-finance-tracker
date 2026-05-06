using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payzo.Models;
using Payzo.Services;

namespace Payzo.Controllers;

[Authorize]
public class TransactionsController : Controller
{
    private readonly UserService _users;
    private readonly FinanceService _finance;
    public TransactionsController(UserService u, FinanceService f) { _users=u; _finance=f; }
    private string Uid => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public IActionResult Index(string? type, string? category, string? search)
    {
        var all = _finance.GetTransactions(Uid);
        if (!string.IsNullOrEmpty(type))     all = all.Where(t => t.Type.ToString().ToLower() == type.ToLower()).ToList();
        if (!string.IsNullOrEmpty(category)) all = all.Where(t => t.Category == category).ToList();
        if (!string.IsNullOrEmpty(search))   all = all.Where(t => t.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        return View(new TransactionsViewModel
        {
            CurrentUser    = _users.GetById(Uid)!,
            Transactions   = all,
            TotalIncome    = all.Where(t=>t.Type==TransactionType.Income).Sum(t=>t.Amount),
            TotalExpenses  = all.Where(t=>t.Type==TransactionType.Expense).Sum(t=>t.Amount),
            FilterType     = type ?? "",
            FilterCategory = category ?? "",
            FilterSearch   = search ?? "",
        });
    }

    [HttpPost] public IActionResult Add(string type, decimal amount, string category,
        string description, string icon, DateTime date)
    {
        if (amount <= 0 || string.IsNullOrWhiteSpace(description))
        { TempData["Error"] = "Попълнете всички полета."; return RedirectToAction("Index"); }

        _finance.AddTransaction(Uid,
            Enum.Parse<TransactionType>(type, true),
            amount, category, description, icon, date);
        TempData["Success"] = "Транзакцията е добавена!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Edit(string id, string type, decimal amount,
        string category, string description, string icon, DateTime date)
    {
        var t = _finance.GetTransaction(Uid, id);
        if (t == null) return NotFound();
        _finance.UpdateTransaction(t, Enum.Parse<TransactionType>(type,true), amount, category, description, icon, date);
        TempData["Success"] = "Транзакцията е обновена!";
        return RedirectToAction("Index");
    }

    [HttpPost] public IActionResult Delete(string id)
    {
        _finance.DeleteTransaction(Uid, id);
        TempData["Success"] = "Транзакцията е изтрита!";
        return RedirectToAction("Index");
    }
}
