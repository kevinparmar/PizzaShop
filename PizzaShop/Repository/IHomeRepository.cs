using PizzaShop.Models;

namespace PizzaShop.Repository
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Pizza>> GetPizzas();
        Task<IEnumerable<Pizza>> GetPizzasBySearch(string searchTerm);
    }
}