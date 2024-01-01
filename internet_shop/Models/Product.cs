using System.ComponentModel.DataAnnotations;

namespace internet_shop.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 100000)]
        public float Price { get; set; }

        [Required]
        [Range(0, 100)]
        public int Count { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
