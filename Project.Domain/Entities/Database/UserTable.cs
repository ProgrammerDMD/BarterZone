using Project.Domain.Entities.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain.Entities
{
    [Table("Users")]
    public class UserTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        
        [Required]
        public string Role { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Invalid email")]
        public string Email { get; set; }
        
        [Required]
        [StringLength(255, ErrorMessage = "Password must be between 8 and 50 characters")]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<ProductTable> Products { get; set; }
    }
}