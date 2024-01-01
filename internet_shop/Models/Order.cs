using System.ComponentModel.DataAnnotations;

namespace internet_shop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
    }
}
