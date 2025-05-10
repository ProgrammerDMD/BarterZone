using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities.Database
{
    [Table("Products")]
    public class ProductTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        public virtual ICollection<CategoryTable> Categories { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual UserTable User { get; set; }
    }
}
