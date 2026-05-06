namespace Payzo.Models;

public enum UserRole { User, Admin, SuperAdmin }

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.User;
    public string Avatar { get; set; } = "";
    public string AvatarColor { get; set; } = "#1a4fb5";
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
}

public enum TransactionType { Income, Expense }

public class Transaction
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string UserId { get; set; } = "";
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "💳";
    public DateTime Date { get; set; } = DateTime.Today;
}

public class Budget
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string UserId { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal LimitAmount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class SavingsGoal
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public string UserId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "🎯";
    public decimal TargetAmount { get; set; }
    public decimal SavedAmount { get; set; }
    public DateTime Deadline { get; set; }
}

// ── View Models ──────────────────────────────────────────────────

public class LoginViewModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string? Error { get; set; }
}

public class RegisterViewModel
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
    public string? Error { get; set; }
}

public class DashboardViewModel
{
    public User CurrentUser { get; set; } = new();
    public decimal Balance { get; set; }
    public decimal MonthIncome { get; set; }
    public decimal MonthExpenses { get; set; }
    public int TransactionCount { get; set; }
    public List<Transaction> RecentTransactions { get; set; } = new();
    public List<Budget> Budgets { get; set; } = new();
    public Dictionary<string, decimal> SpendByCategory { get; set; } = new();
    public List<SavingsGoal> Goals { get; set; } = new();
}

public class TransactionsViewModel
{
    public User CurrentUser { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public string FilterType { get; set; } = "";
    public string FilterCategory { get; set; } = "";
    public string FilterSearch { get; set; } = "";
}

public class UsersViewModel
{
    public User CurrentUser { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int AdminCount { get; set; }
    public int TotalTransactions { get; set; }
}

public class UserFormViewModel
{
    public string? Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.User;
    public bool Active { get; set; } = true;
    public string? Error { get; set; }
}
