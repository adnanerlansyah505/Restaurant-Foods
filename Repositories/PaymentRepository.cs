using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment> CreatePaymentAsync(Guid orderId)
    {
        var payment = new Payment
        {
            Guid = Guid.NewGuid(),
            OrderId = orderId,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = "Pending",
            PaymentStatus = PaymentStatus.Pending,
            AmountPaid = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Payments.AddAsync(payment);
        return payment;
    }

    public async Task<Order?> GetOrderWithItemsAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Food)
            .FirstOrDefaultAsync(o => o.Guid == orderId);
    }

    public Task UpdateOrderStatusAsync(Order order)
    {
        _context.Orders.Update(order);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}