using Payzo.Data;
using Payzo.Models;

namespace Payzo.Services;

public class FinanceService
{
    private readonly PayzoDb _db;
    public FinanceService(PayzoDb db) => _db = db;

    // ── Transactions ──────────────────────────────────────────────
    public List<Transaction> GetTransactions(string userId) =>
        _db.Transactions.Where(t => t.UserId == userId).OrderByDescending(t => t.Date).ToList();

    public Transaction? GetTransaction(string id) => _db.Transactions.FirstOrDefault(t => t.Id == id);

    public Transaction AddTransaction(string userId, TransactionType type, decimal amount,
        string category, string description, string icon, DateTime date)
    {
        var t = new Transaction { UserId=userId, Type=type, Amount=amount,
            Category=category, Description=description, Icon=icon, Date=date };
        _db.Transactions.Add(t);
        return t;
    }

    public void UpdateTransaction(Transaction t, TransactionType type, decimal amount,
        string category, string description, string icon, DateTime date)
    {
        t.Type=type; t.Amount=amount; t.Category=category;
        t.Description=description; t.Icon=icon; t.Date=date;
    }

    public void DeleteTransaction(string id)
    {
        var t = _db.Transactions.FirstOrDefault(x => x.Id == id);
        if (t != null) _db.Transactions.Remove(t);
    }

    // ── Stats ──────────────────────────────────────────────────────
    public (decimal balance, decimal income, decimal expenses) GetStats(string userId)
    {
        var txns = GetTransactions(userId);
        var now = DateTime.Today;
        var thisMonth = txns.Where(t => t.Date.Month == now.Month && t.Date.Year == now.Year);
        var income   = thisMonth.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var expenses = thisMonth.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        var balance  = txns.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        return (balance, income, expenses);
    }

    public Dictionary<string, decimal> GetSpendByCategory(string userId, int month, int year) =>
        _db.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense
                     && t.Date.Month == month && t.Date.Year == year)
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

    // ── Budgets ───────────────────────────────────────────────────
    public List<Budget> GetBudgets(string userId, int month, int year) =>
        _db.Budgets.Where(b => b.UserId == userId && b.Month == month && b.Year == year).ToList();

    public Budget? GetBudget(string id) => _db.Budgets.FirstOrDefault(b => b.Id == id);

    public Budget AddBudget(string userId, string category, decimal limit, int month, int year)
    {
        var b = new Budget { UserId=userId, Category=category, LimitAmount=limit, Month=month, Year=year };
        _db.Budgets.Add(b);
        return b;
    }

    public void DeleteBudget(string id)
    {
        var b = _db.Budgets.FirstOrDefault(x => x.Id == id);
        if (b != null) _db.Budgets.Remove(b);
    }

    // ── Goals ─────────────────────────────────────────────────────
    public List<SavingsGoal> GetGoals(string userId) =>
        _db.Goals.Where(g => g.UserId == userId).ToList();

    public SavingsGoal? GetGoal(string id) => _db.Goals.FirstOrDefault(g => g.Id == id);

    public SavingsGoal AddGoal(string userId, string name, string icon, decimal target, decimal saved, DateTime deadline)
    {
        var g = new SavingsGoal { UserId=userId, Name=name, Icon=icon, TargetAmount=target, SavedAmount=saved, Deadline=deadline };
        _db.Goals.Add(g);
        return g;
    }

    public void DeleteGoal(string id)
    {
        var g = _db.Goals.FirstOrDefault(x => x.Id == id);
        if (g != null) _db.Goals.Remove(g);
    }
}
