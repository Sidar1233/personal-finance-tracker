using Microsoft.EntityFrameworkCore;
using Payzo.Data;
using Payzo.Models;

namespace Payzo.Services;

public class UserService
{
    private readonly PayzoDb _db;
    public UserService(PayzoDb db) => _db = db;

    public User? ValidateLogin(string email, string password)
    {
        var user = _db.Users.FirstOrDefault(u =>
            u.Email.ToLower() == email.ToLower() && u.Active);
        if (user == null) return null;
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }

    public User? GetById(string id) => _db.Users.Find(id);

    public List<User> GetAll() => _db.Users.OrderBy(u => u.Name).ToList();

    public bool EmailExists(string email, string? excludeId = null) =>
        _db.Users.Any(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeId);

    public User Create(string name, string email, string password, UserRole role = UserRole.User)
    {
        var parts  = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var avatar = ((parts.ElementAtOrDefault(0)?[0].ToString() ?? "") +
                      (parts.ElementAtOrDefault(1)?[0].ToString() ?? "")).ToUpperInvariant();
        var colors = new[]{"#1a4fb5","#f59e0b","#ec4899","#16a34a","#7c3aed","#0d9488"};
        var user = new User
        {
            Id           = Guid.NewGuid().ToString("N")[..8],
            Name         = name,
            Email        = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role         = role,
            Avatar       = avatar,
            AvatarColor  = colors[Random.Shared.Next(colors.Length)],
        };
        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }

    public void Update(User user, string name, string email, UserRole role, bool active, string? newPassword)
    {
        user.Name   = name;
        user.Email  = email;
        user.Role   = role;
        user.Active = active;
        var parts   = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        user.Avatar = ((parts.ElementAtOrDefault(0)?[0].ToString() ?? "") +
                       (parts.ElementAtOrDefault(1)?[0].ToString() ?? "")).ToUpperInvariant();
        if (!string.IsNullOrWhiteSpace(newPassword))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _db.SaveChanges();
    }

    public void Delete(string id)
    {
        var user = _db.Users.Find(id);
        if (user != null) { _db.Users.Remove(user); _db.SaveChanges(); }
    }

    public void UpdateLastLogin(string id)
    {
        var u = GetById(id);
        if (u != null) { u.LastLogin = DateTime.UtcNow; _db.SaveChanges(); }
    }

    public void ToggleActive(string id)
    {
        var u = GetById(id);
        if (u != null) { u.Active = !u.Active; _db.SaveChanges(); }
    }
}
