using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain
{
    [Table("UserDbTables")]
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}