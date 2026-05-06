using Payzo.Data;
using Payzo.Models;

namespace Payzo.Services;

public class FinanceService
{
    private readonly PayzoDb _db;
    public FinanceService(PayzoDb db) => _db = db;

    // ── Transactions ──────────────────────────────────────────────
    public List<Transaction> GetTransactions(string userId) =>
        _db.Transactions.Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date).ToList();

    public Transaction? GetTransaction(string userId, string id) =>
        _db.Transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);

    public void AddTransaction(string userId, TransactionType type, decimal amount,
        string category, string description, string icon, DateTime date)
    {
        _db.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            UserId = userId, Type = type, Amount = amount,
            Category = category, Description = description, Icon = icon, Date = date
        });
        _db.SaveChanges();
    }

    public void UpdateTransaction(Transaction t, TransactionType type, decimal amount,
        string category, string description, string icon, DateTime date)
    {
        t.Type = type; t.Amount = amount; t.Category = category;
        t.Description = description; t.Icon = icon; t.Date = date;
        _db.SaveChanges();
    }

    public void DeleteTransaction(string userId, string id)
    {
        var t = GetTransaction(userId, id);
        if (t != null) { _db.Transactions.Remove(t); _db.SaveChanges(); }
    }

    // ── Stats ──────────────────────────────────────────────────────
    public (decimal balance, decimal income, decimal expenses) GetStats(string userId)
    {
        var all      = GetTransactions(userId);
        var now      = DateTime.Today;
        var month    = all.Where(t => t.Date.Month == now.Month && t.Date.Year == now.Year);
        var income   = month.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var expenses = month.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        var balance  = all.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
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

    public Budget? GetBudget(string userId, string id) =>
        _db.Budgets.FirstOrDefault(b => b.Id == id && b.UserId == userId);

    public void AddBudget(string userId, string category, decimal limit, int month, int year)
    {
        _db.Budgets.Add(new Budget
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            UserId = userId, Category = category,
            LimitAmount = limit, Month = month, Year = year
        });
        _db.SaveChanges();
    }

    public void UpdateBudget(string userId, string id, decimal limit)
    {
        var b = GetBudget(userId, id);
        if (b != null) { b.LimitAmount = limit; _db.SaveChanges(); }
    }

    public void DeleteBudget(string userId, string id)
    {
        var b = GetBudget(userId, id);
        if (b != null) { _db.Budgets.Remove(b); _db.SaveChanges(); }
    }

    // ── Goals ─────────────────────────────────────────────────────
    public List<SavingsGoal> GetGoals(string userId) =>
        _db.Goals.Where(g => g.UserId == userId).ToList();

    public SavingsGoal? GetGoal(string userId, string id) =>
        _db.Goals.FirstOrDefault(g => g.Id == id && g.UserId == userId);

    public void AddGoal(string userId, string name, string icon,
        decimal target, decimal saved, DateTime deadline)
    {
        _db.Goals.Add(new SavingsGoal
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            UserId = userId, Name = name, Icon = icon,
            TargetAmount = target, SavedAmount = saved, Deadline = deadline
        });
        _db.SaveChanges();
    }

    public void UpdateGoal(SavingsGoal g, string name, string icon,
        decimal target, decimal saved, DateTime deadline)
    {
        g.Name = name; g.Icon = icon;
        g.TargetAmount = target; g.SavedAmount = saved; g.Deadline = deadline;
        _db.SaveChanges();
    }

    public void Deposit(string userId, string id, decimal amount)
    {
        var g = GetGoal(userId, id);
        if (g != null) { g.SavedAmount += amount; _db.SaveChanges(); }
    }

    public void DeleteGoal(string userId, string id)
    {
        var g = GetGoal(userId, id);
        if (g != null) { _db.Goals.Remove(g); _db.SaveChanges(); }
    }
}
