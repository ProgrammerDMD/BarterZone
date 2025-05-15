using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(512)]
        public string Image { get; set; }
        
        [Required] 
        [ForeignKey("User")] 
        public int CreatorId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Title { get; set; }

        [Range(0, double.MaxValue)] 
        public double Price { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 100)]
        public string Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; } 
        
        public virtual Category Category { get; set; }

        [Required] 
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
    }
}