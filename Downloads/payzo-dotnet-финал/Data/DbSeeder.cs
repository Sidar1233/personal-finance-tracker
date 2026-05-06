using Payzo.Models;

namespace Payzo.Data;

public static class DbSeeder
{
    public static void Seed(PayzoDb db)
    {
        // Only seed if no users exist
        if (db.Users.Any()) return;

        var users = new List<User>
        {
            new(){ Id="u001", Name="Супер Администратор", Email="superadmin@payzo.bg",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("superadmin123"),
                Role=UserRole.SuperAdmin, Avatar="СА", AvatarColor="#7c3aed",
                Active=true, CreatedAt=new DateTime(2025,1,1), LastLogin=DateTime.Today },
            new(){ Id="u002", Name="Мария Димитрова", Email="admin@payzo.bg",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role=UserRole.Admin, Avatar="МД", AvatarColor="#1a4fb5",
                Active=true, CreatedAt=new DateTime(2025,2,15) },
            new(){ Id="u004", Name="Иван Петров", Email="ivan@example.bg",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("ivan123"),
                Role=UserRole.User, Avatar="ИП", AvatarColor="#f59e0b",
                Active=true, CreatedAt=new DateTime(2025,4,1) },
            new(){ Id="u005", Name="Елена Николова", Email="elena@example.bg",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword("elena123"),
                Role=UserRole.User, Avatar="ЕН", AvatarColor="#ec4899",
                Active=true, CreatedAt=new DateTime(2025,5,12) },
        };
        db.Users.AddRange(users);

        db.Transactions.AddRange(
            new(){ Id="t001",UserId="u004",Type=TransactionType.Expense,Amount=45.30m,Category="Храна",Icon="🛒",Description="Lidl",Date=new DateTime(2026,3,9)},
            new(){ Id="t002",UserId="u004",Type=TransactionType.Income,Amount=2400m,Category="Заплата",Icon="💰",Description="Заплата март",Date=new DateTime(2026,3,5)},
            new(){ Id="t003",UserId="u004",Type=TransactionType.Expense,Amount=80m,Category="Транспорт",Icon="⛽",Description="Гориво",Date=new DateTime(2026,3,4)},
            new(){ Id="t004",UserId="u004",Type=TransactionType.Expense,Amount=17.99m,Category="Забавление",Icon="🎬",Description="Netflix",Date=new DateTime(2026,3,3)},
            new(){ Id="t005",UserId="u004",Type=TransactionType.Expense,Amount=650m,Category="Наем",Icon="🏠",Description="Наем март",Date=new DateTime(2026,3,1)}
        );

        db.Budgets.AddRange(
            new(){ Id="b001",UserId="u004",Category="Храна",LimitAmount=500m,Month=3,Year=2026},
            new(){ Id="b002",UserId="u004",Category="Транспорт",LimitAmount=300m,Month=3,Year=2026},
            new(){ Id="b003",UserId="u004",Category="Забавление",LimitAmount=200m,Month=3,Year=2026},
            new(){ Id="b004",UserId="u004",Category="Наем",LimitAmount=700m,Month=3,Year=2026}
        );

        db.Goals.AddRange(
            new(){ Id="g001",UserId="u004",Name="Ваканция в Гърция",Icon="✈️",TargetAmount=2000m,SavedAmount=1400m,Deadline=new DateTime(2026,7,1)},
            new(){ Id="g002",UserId="u004",Name="Нова кола",Icon="🚗",TargetAmount=15000m,SavedAmount=5200m,Deadline=new DateTime(2027,12,1)},
            new(){ Id="g003",UserId="u004",Name="Извънреден фонд",Icon="🛡️",TargetAmount=6000m,SavedAmount=3800m,Deadline=new DateTime(2026,12,31)}
        );

        db.SaveChanges();
    }
}
