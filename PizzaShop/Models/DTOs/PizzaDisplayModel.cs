namespace PizzaShop.Models.DTOs
{
    public class PizzaDisplayModel
    {
        public IEnumerable<Pizza> Pizzas { get; set; }
        public IEnumerable<Topping> Toppings { get; set; }
    }
}
