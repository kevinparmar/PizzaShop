using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int PizzaId { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public Order Order { get; set; }

        public Pizza Pizza { get; set; }
    }
}
