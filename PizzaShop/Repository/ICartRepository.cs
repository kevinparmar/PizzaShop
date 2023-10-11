namespace PizzaShop.Repository
{
    public interface ICartRepository
    {
        Task<int> AddPizzaToCart(int pizzaId, int quantity=1);
        Task<int> RemovePizzaFromCart(int cartItemId);
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<ShoppingCart> GetUserCart();
        Task<int> DoCheckout();
        string GetUserId();
    }
}