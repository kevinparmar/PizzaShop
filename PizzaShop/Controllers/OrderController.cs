using Microsoft.AspNetCore.Mvc;

namespace PizzaShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.Orders();
            return View(orders);
        }  
    }
}
