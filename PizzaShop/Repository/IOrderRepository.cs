namespace PizzaShop.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> Orders();
        Task<List<OrderDetail>> GetOrderDetails(int orderId);
        string GetUserId();
    }
}