namespace Lamie.Domain.Entities.Orders;

public enum OrderStatus
{
    Created = 1,
    Producing = 2,
    Shipping = 3,
    Completed = 4,
    Cancelled = 99,
}

public enum PaymentStatus
{
    Unpaid = 1,
    Deposited = 2,
    Paid = 3,
}
