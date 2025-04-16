using Project.Domain;
using System.Linq;

namespace Project.BusinessLogic.Core
{
    public class UserService
    {
        private readonly AppDbContext _db = new AppDbContext();

        public bool EmailExists(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public void RegisterUser(string email, string password)
        {
            var user = new User
            {
                Email = email,
                Password = password 
            };

            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}