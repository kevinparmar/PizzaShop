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
        public async Task<IActionResult> OrderConfirmation()
        {
            if (!TempData.ContainsKey("OrderId"))
            {
                // Handle the case where TempData["OrderId"] is missing
                // You can redirect the user to an error page or perform some other action.
                return RedirectToAction("Index", "Home"); // Example: Redirect to the home page
            }

            // Retrieve the Order Id from TempData
            if (TempData.Peek("OrderId") is int orderId)
            {
                // Use the 'orderId' to fetch order details
                var orderDetails = await _orderRepository.GetOrderDetails(orderId);

                if (orderDetails == null)
                {
                    // Handle the case where order details are not found
                    // You can redirect the user to an error page or perform some other action.
                    return RedirectToAction("Index", "Home"); // Example: Redirect to the home page
                }

                // Pass the order details to the view
                return View(orderDetails);
            }

            // Handle invalid Order Id in TempData
            // You can redirect the user to an error page or perform some other action.
            return RedirectToAction("Index", "Home"); // Example: Redirect to the home page
        }
    }
}
