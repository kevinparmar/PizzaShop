using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Models
{
    [Table("Topping")]
    public class Topping
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public double Price { get; set; }

        public List<Pizza> Pizzas { get; set; }
    }
}
