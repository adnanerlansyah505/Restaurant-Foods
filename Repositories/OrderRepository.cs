using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Guid == orderId);
    }

    public async Task<Order?> GetByIdAndUserAsync(Guid orderId, Guid userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Guid == orderId && o.UserId == userId);
    }

    public async Task UpdateStatusAsync(Guid orderId, TypeStatusOrder status)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Guid == orderId);

        if (order == null)
            throw new Exception("Order not found");

        order.Status = status;

        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}