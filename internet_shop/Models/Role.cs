using System.ComponentModel.DataAnnotations;

namespace internet_shop.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<Customer> Customers { get; set; }
        public Role()
        {
            Customers = new List<Customer>();
        }
    }
}
