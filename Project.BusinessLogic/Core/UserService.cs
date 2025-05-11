using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project.BusinessLogic.DBModel;
using Project.Domain.Entities;

namespace Project.BusinessLogic.Core
{
    public class UserService
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);

                var sb = new StringBuilder();
                foreach (var b in hash) sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        public async Task<User> LoginUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return null;

            var hashedPassword = HashPassword(password);
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(hashedPassword)
            );

            return user;
        }

        public async Task<User> RegisterUser(string email, string name, string password,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return null;

            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var emailExists =
                        await _db.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase),
                            cancellationToken);

                    if (emailExists) return null;

                    var hashedPassword = HashPassword(password);

                    var user = new User
                    {
                        Email = email,
                        Name = name,
                        Password = hashedPassword,
                        Role = "User",
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.Users.Add(user);
                    await _db.SaveChangesAsync(cancellationToken);

                    dbContextTransaction.Commit();
                    return user;
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("User registration was canceled.");
                    return null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during transactional user registration: {ex.Message}");
                    return null;
                }
            }
        }

        public async Task<ProfileViewModel> GetProfileById(int userId)
        {
            var profile = await _db.Users.Where(u => u.Id == userId)
                .Select(u => new ProfileViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    Products = u.Products
                        .OrderBy(p => p.Id)
                        .Take(10)
                        .Select(p => new ProductSummaryViewModel
                        {
                            Id = p.Id,
                            CreatorId = p.CreatorId,
                            Title = p.Title,
                            Price = p.Price,
                            Description = p.Description,
                            Categories = p.Categories.OrderBy(c => c.Id)
                                .Select(c => c.Name),
                            CreatedAt = p.CreatedAt
                        })
                }).FirstOrDefaultAsync();

            return profile;
        }
    }
}