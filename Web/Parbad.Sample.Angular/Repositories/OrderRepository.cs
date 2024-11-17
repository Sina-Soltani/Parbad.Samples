using System.Collections.Generic;
using System.Linq;

namespace Parbad.Sample.Angular.Repositories;

public interface IOrderRepository
{
    void AddOrder(Order order);

    Order GetOrderById(int id);

    Order GetOrderByPaymentTrackingNumber(long paymentTrackingNumber);

    void OrderFailed(Order order, string message);

    void UpdateOrder(Order order, bool isSucceed, string message, string transactionCode);
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
        order.Id = GenerateNewId();

        _orders.Add(order);
    }

    public Order GetOrderById(int id)
    {
        return _orders.Single(model => model.Id == id);
    }

    public Order GetOrderByPaymentTrackingNumber(long paymentTrackingNumber)
    {
        return _orders.Single(model => model.PaymentTrackingNumber == paymentTrackingNumber);
    }

    public void OrderFailed(Order order, string message)
    {
        order.IsPaid = false;
        order.Message = message;
    }

    public void UpdateOrder(Order order, bool isSucceed, string message, string transactionCode)
    {
        order.IsPaid = isSucceed;
        order.TransactionCode = transactionCode;
        order.Message = message;
    }

    private int GenerateNewId()
    {
        if (_orders.Any())
        {
            return _orders.Max(model => model.Id) + 1;
        }

        return 1;
    }
}

/// <summary>
/// Order is a simple domain object that keeps the information of order and payment in it.
/// </summary>
public class Order
{
    public int Id { get; set; }

    public long PaymentTrackingNumber { get; set; }

    public decimal Amount { get; set; }

    public bool IsPaid { get; set; }

    public string TransactionCode { get; set; }

    public string GatewayName { get; set; }

    public string GatewayAccountName { get; set; }

    public string Message { get; set; }
}