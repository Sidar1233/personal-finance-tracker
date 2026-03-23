using Payzo.Models;

namespace Payzo.Data;

/// <summary>
/// In-memory data store. Replace with EF Core + SQL Server for production.
/// </summary>
public class PayzoDb
{
    // ── Seed data ────────────────────────────────────────────────
    public List<User> Users { get; } = new()
    {
        new User { Id="u001", Name="Супер Администратор", Email="superadmin@payzo.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("superadmin123"),
            Role=UserRole.SuperAdmin, Avatar="СА", AvatarColor="#7c3aed",
            Active=true, CreatedAt=new DateTime(2025,1,1), LastLogin=DateTime.Today },

        new User { Id="u002", Name="Мария Димитрова", Email="admin@payzo.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role=UserRole.Admin, Avatar="МД", AvatarColor="#1a4fb5",
            Active=true, CreatedAt=new DateTime(2025,2,15), LastLogin=DateTime.Today.AddDays(-1) },

        new User { Id="u003", Name="Георги Иванов", Email="g.ivanov@payzo.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("admin456"),
            Role=UserRole.Admin, Avatar="ГИ", AvatarColor="#0d9488",
            Active=true, CreatedAt=new DateTime(2025,3,10), LastLogin=DateTime.Today.AddDays(-2) },

        new User { Id="u004", Name="Иван Петров", Email="ivan@example.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("ivan123"),
            Role=UserRole.User, Avatar="ИП", AvatarColor="#f59e0b",
            Active=true, CreatedAt=new DateTime(2025,4,1), LastLogin=DateTime.Today },

        new User { Id="u005", Name="Елена Николова", Email="elena@example.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("elena123"),
            Role=UserRole.User, Avatar="ЕН", AvatarColor="#ec4899",
            Active=true, CreatedAt=new DateTime(2025,5,12), LastLogin=DateTime.Today.AddDays(-3) },

        new User { Id="u006", Name="Петър Стоянов", Email="petar@example.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("petar123"),
            Role=UserRole.User, Avatar="ПС", AvatarColor="#16a34a",
            Active=true, CreatedAt=new DateTime(2025,6,20), LastLogin=DateTime.Today.AddDays(-4) },

        new User { Id="u007", Name="Анна Тодорова", Email="anna@example.bg",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("anna123"),
            Role=UserRole.User, Avatar="АТ", AvatarColor="#dc2626",
            Active=false, CreatedAt=new DateTime(2025,7,3), LastLogin=new DateTime(2026,1,15) },
    };

    public List<Transaction> Transactions { get; } = new()
    {
        new Transaction { Id="t001", UserId="u004", Type=TransactionType.Expense, Amount=45.30m,  Category="Храна",       Icon="🛒", Description="Lidl",           Date=new DateTime(2026,3,9) },
        new Transaction { Id="t002", UserId="u004", Type=TransactionType.Income,  Amount=2400.00m, Category="Заплата",    Icon="💰", Description="Заплата март",   Date=new DateTime(2026,3,5) },
        new Transaction { Id="t003", UserId="u004", Type=TransactionType.Expense, Amount=80.00m,  Category="Транспорт",   Icon="⛽", Description="Гориво Shell",    Date=new DateTime(2026,3,4) },
        new Transaction { Id="t004", UserId="u004", Type=TransactionType.Expense, Amount=17.99m,  Category="Забавление",  Icon="🎬", Description="Netflix",        Date=new DateTime(2026,3,3) },
        new Transaction { Id="t005", UserId="u004", Type=TransactionType.Expense, Amount=32.50m,  Category="Храна",       Icon="🍕", Description="Domino's Pizza",  Date=new DateTime(2026,3,2) },
        new Transaction { Id="t006", UserId="u004", Type=TransactionType.Expense, Amount=28.00m,  Category="Здраве",      Icon="💊", Description="Аптека",          Date=new DateTime(2026,3,1) },
        new Transaction { Id="t007", UserId="u004", Type=TransactionType.Expense, Amount=650.00m, Category="Наем",        Icon="🏠", Description="Наем март",       Date=new DateTime(2026,3,1) },
        new Transaction { Id="t008", UserId="u004", Type=TransactionType.Income,  Amount=350.00m, Category="Заплата",     Icon="💵", Description="Фрийланс",        Date=new DateTime(2026,2,28) },
        new Transaction { Id="t009", UserId="u004", Type=TransactionType.Expense, Amount=62.00m,  Category="Храна",       Icon="🛍️", Description="Kaufland",        Date=new DateTime(2026,2,26) },
        new Transaction { Id="t010", UserId="u004", Type=TransactionType.Expense, Amount=45.00m,  Category="Транспорт",   Icon="🚌", Description="Месечна карта",   Date=new DateTime(2026,2,25) },
        new Transaction { Id="t011", UserId="u005", Type=TransactionType.Income,  Amount=1900.00m,Category="Заплата",     Icon="💰", Description="Заплата март",    Date=new DateTime(2026,3,5) },
        new Transaction { Id="t012", UserId="u005", Type=TransactionType.Expense, Amount=340.00m, Category="Наем",        Icon="🏠", Description="Наем",            Date=new DateTime(2026,3,1) },
    };

    public List<Budget> Budgets { get; } = new()
    {
        new Budget { Id="b001", UserId="u004", Category="Храна",      LimitAmount=500m,  Month=3, Year=2026 },
        new Budget { Id="b002", UserId="u004", Category="Транспорт",  LimitAmount=300m,  Month=3, Year=2026 },
        new Budget { Id="b003", UserId="u004", Category="Забавление", LimitAmount=200m,  Month=3, Year=2026 },
        new Budget { Id="b004", UserId="u004", Category="Здраве",     LimitAmount=250m,  Month=3, Year=2026 },
        new Budget { Id="b005", UserId="u004", Category="Наем",       LimitAmount=700m,  Month=3, Year=2026 },
    };

    public List<SavingsGoal> Goals { get; } = new()
    {
        new SavingsGoal { Id="g001", UserId="u004", Name="Ваканция в Гърция", Icon="✈️", TargetAmount=2000m,  SavedAmount=1400m, Deadline=new DateTime(2026,7,1) },
        new SavingsGoal { Id="g002", UserId="u004", Name="Нова кола",         Icon="🚗", TargetAmount=15000m, SavedAmount=5200m, Deadline=new DateTime(2027,12,1) },
        new SavingsGoal { Id="g003", UserId="u004", Name="Извънреден фонд",   Icon="🛡️", TargetAmount=6000m,  SavedAmount=3800m, Deadline=new DateTime(2026,12,31) },
    };
}
