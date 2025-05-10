using Project.BusinessLogic.DBModel;
using Project.Domain;
using Project.Domain.Entities;
using Project.Domain.Entities.User;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Core
{
    public class UserService
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        public async Task<User> LoginUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            string hashedPassword = HashPassword(password);
            UserTable user = await _db.Users.FirstOrDefaultAsync(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(hashedPassword)
            );

            if (user != null)
            {
                return new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                };
            } else
            {
                return null;
            }
        }

        public async Task<User> RegisterUser(string email, string name, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    bool emailExists = await _db.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase), cancellationToken);

                    if (emailExists)
                    {
                        return null;
                    }

                    string hashedPassword = HashPassword(password);

                    var user = new UserTable
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
                    return new User
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Role = user.Role,
                        Email = user.Email,
                        CreatedAt = user.CreatedAt
                    };
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
    }
}