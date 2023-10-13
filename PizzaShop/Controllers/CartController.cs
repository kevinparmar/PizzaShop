using Microsoft.AspNetCore.Mvc;

namespace PizzaShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> AddItem(int pizzaId, int quantity=1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddPizzaToCart(pizzaId, quantity);
            Console.Out.WriteLine("Cart Count in Add method: ",cartCount);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var count = await _cartRepository.RemovePizzaFromCart(cartItemId);
            Console.Out.WriteLine("Cart Count in Remove method: ", count);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            var count = await _cartRepository.RemoveAllItemsFromCart();
            Console.Out.WriteLine("Cart Count in RemoveAll method: ", count);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemsInCart()
        {
            var totalItems = await _cartRepository.GetCartItemCount();
            Console.Out.WriteLine("Cart Count in GetTotalItemsInCart method: ", totalItems);
            return Ok(totalItems);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            bool confirmOrder = await _cartRepository.ConfirmOrder();
            if (!confirmOrder)
            {
                Console.Out.WriteLine("Order Not Confirmed");
            }
            return View();
        }
    }
}
