using Microsoft.AspNetCore.Mvc;
using PizzaShop.Models;
using PizzaShop.Repository;
using System.Diagnostics;

namespace PizzaShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var pizzas = string.IsNullOrWhiteSpace(searchTerm)
                        ? await _homeRepository.GetPizzas()
                        : await _homeRepository.GetPizzasBySearch(searchTerm);

            var toppings = await _homeRepository.GetAvailableToppings();

            var pizzaDisplayModel = new PizzaDisplayModel
            {
                Pizzas = pizzas,
                Toppings = toppings
            };

            return View(pizzaDisplayModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}