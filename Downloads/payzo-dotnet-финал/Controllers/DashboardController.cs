using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payzo.Models;
using Payzo.Services;

namespace Payzo.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly UserService _users;
    private readonly FinanceService _finance;

    public DashboardController(UserService users, FinanceService finance)
    { _users = users; _finance = finance; }

    private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

    public IActionResult Index()
    {
        var user = _users.GetById(UserId);
        if (user == null) return RedirectToAction("Logout", "Account");
        var (balance, income, expenses) = _finance.GetStats(UserId);
        var now = DateTime.Today;
        var spend = _finance.GetSpendByCategory(UserId, now.Month, now.Year);

        var vm = new DashboardViewModel
        {
            CurrentUser        = user,
            Balance            = balance,
            MonthIncome        = income,
            MonthExpenses      = expenses,
            TransactionCount   = _finance.GetTransactions(UserId).Count,
            RecentTransactions = _finance.GetTransactions(UserId).Take(6).ToList(),
            Budgets            = _finance.GetBudgets(UserId, now.Month, now.Year),
            SpendByCategory    = spend,
            Goals              = _finance.GetGoals(UserId),
        };
        return View(vm);
    }
}
