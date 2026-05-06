using Microsoft.EntityFrameworkCore;
using Payzo.Models;

namespace Payzo.Data;

public class PayzoDb : DbContext
{
    public PayzoDb(DbContextOptions<PayzoDb> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<SavingsGoal> Goals => Set<SavingsGoal>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>().HasKey(u => u.Id);
        mb.Entity<Transaction>().HasKey(t => t.Id);
        mb.Entity<Budget>().HasKey(b => b.Id);
        mb.Entity<SavingsGoal>().HasKey(g => g.Id);

        // Store enum as string
        mb.Entity<User>().Property(u => u.Role).HasConversion<string>();
        mb.Entity<Transaction>().Property(t => t.Type).HasConversion<string>();
    }
}
