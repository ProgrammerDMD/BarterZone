using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Entities.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> CategoryNames { get; set; }
    }
}
