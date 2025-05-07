using Project.BusinessLogic.DBModel;
using Project.Domain;
using Project.Domain.Entities;
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
        private readonly UserContext _db = new UserContext();

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

        public async Task<string> LoginUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            string hashedPassword = HashPassword(password);
            bool userExists = await _db.Users.AnyAsync(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(hashedPassword)
            );

            if (userExists)
            {
                return "JWT-TOKEN";
            } else
            {
                return null;
            }
        }

        public async Task<bool> RegisterUser(string email, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            using (var dbContextTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    bool emailExists = await _db.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase), cancellationToken);

                    if (emailExists)
                    {
                        return false;
                    }

                    string hashedPassword = HashPassword(password);

                    var user = new UserTable
                    {
                        Email = email,
                        Password = hashedPassword
                    };

                    _db.Users.Add(user);
                    await _db.SaveChangesAsync(cancellationToken);

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("User registration was canceled.");
                    return false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during transactional user registration: {ex.Message}");
                    return false;
                }
            }
        }
    }
}