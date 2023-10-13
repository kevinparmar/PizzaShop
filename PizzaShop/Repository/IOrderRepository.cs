namespace PizzaShop.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> Orders();
        string GetUserId();
    }
}