using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;
using PizzaShop.Models;

namespace PizzaShop.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Pizza>> GetPizzas()
        {
            return await _db.Pizzas.ToListAsync();
        }
        public async Task<IEnumerable<Pizza>> GetPizzasBySearch(string searchTerm)
        {
            // Fetch pizzas that match the search term (adjust the query as needed)
            return await _db.Pizzas
                .Where(pizza => pizza.Name.Contains(searchTerm) || pizza.Description.Contains(searchTerm))
                .ToListAsync();
        }
        public async Task<IEnumerable<Topping>> GetAvailableToppings()
        {
            return await _db.Toppings.ToListAsync();
        }

    }
}
