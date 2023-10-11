using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Models
{
    [Table("Pizza")]
    public class Pizza
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
        public bool IsVegetarian { get; set; }
        public List<Topping> Toppings { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<CartDetail> CartDetails { get; set; }
    }
}
