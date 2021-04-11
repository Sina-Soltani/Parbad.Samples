using System.Collections.Generic;
using System.Linq;

namespace Parbad.Sample.Angular.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);

        Order GetOrder();

        void OrderFailed(long trackingNumber, string message);

        void UpdateOrder(long trackingNumber, bool isSucceed, string message, string transactionCode);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;

        public OrderRepository()
        {
            _orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }

        public Order GetOrder()
        {
            return _orders.LastOrDefault();
        }

        public void OrderFailed(long trackingNumber, string message)
        {
            var order = _orders.SingleOrDefault(model => model.TrackingNumber == trackingNumber);

            if (order == null) return;

            order.IsPaid = false;
            order.Message = message;
        }

        public void UpdateOrder(long trackingNumber, bool isSucceed, string message, string transactionCode)
        {
            var order = _orders.SingleOrDefault(model => model.TrackingNumber == trackingNumber);

            if (order == null) return;

            order.IsPaid = isSucceed;
            order.TransactionCode = transactionCode;
            order.Message = message;
        }
    }

    public class Order
    {
        public long TrackingNumber { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; }

        public string TransactionCode { get; set; }

        public string GatewayName { get; set; }

        public string GatewayAccountName { get; set; }

        public string Message { get; set; }
    }
}
