using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PizzaShop.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddPizzaToCart(int pizzaId, int quantity)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();

                // Add pizza to cart detail

                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.PizzaId == pizzaId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    var pizza = _db.Pizzas.Find(pizzaId);
                    cartItem = new CartDetail
                    {
                        PizzaId = pizzaId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity,
                        UnitPrice = pizza.Price // Calculate total pizza price
                    };
                    _db.CartDetails.Add(cartItem);
                }
                
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemovePizzaFromCart(int cartItemId)
        {
            string userId = GetUserId();

            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");

                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid Cart");

                var cartItem = _db.CartDetails
                    .FirstOrDefault(cd => cd.ShoppingCartId == cart.Id && cd.PizzaId == cartItemId);

                if (cartItem is null)
                    throw new Exception("Cart item not found or does not belong to the current user");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., logging or returning false
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        // Add methods for removing items, getting the cart, and checking out similar to your existing repository     

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await _db.CartDetails
                .Where(cartDetail => cartDetail.ShoppingCart.UserId == userId 
                && !cartDetail.ShoppingCart.IsDeleted)
                .CountAsync();

            return data;
        }


        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid User Id");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Pizza)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        public async Task<int> DoCheckout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                
                OrderDetail orderDetail;
                foreach (var item in cartDetail)
                {
                    orderDetail = new OrderDetail
                    {
                        PizzaId = item.PizzaId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                //return orderDetail;
                return order.Id;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }
}
