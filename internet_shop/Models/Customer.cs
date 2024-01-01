using System.ComponentModel.DataAnnotations;
using System.Data;

namespace internet_shop.Models
{
    public class Customer
    {
        public Customer()
        {

        }

        public int ID { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
