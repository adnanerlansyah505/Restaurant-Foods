using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Utilities.Enum;
using RestaurantFoods.Dtos.Orders;
using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IFoodRepository _foodRepository;
    private readonly IPaymentService _paymentService;

    public OrderService(
        IOrderRepository orderRepository,
        IFoodRepository foodRepository,
        IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _foodRepository = foodRepository;
        _paymentService = paymentService;
    }

    public async Task<object> CreateOrderAsync(Guid userId, CreateOrderDto dto)
    {
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = TypeStatusOrder.Pending,
            TotalAmount = 0
        };

        int totalAmount = 0;

        foreach (var item in dto.Items)
        {
            var food = await _foodRepository.GetByIdAsync(item.FoodId);

            if (food == null)
                throw new Exception("Food not found");

            var subtotal = food.Price * item.Quantity;
            totalAmount += subtotal;

            order.OrderItems.Add(new OrderItem
            {
                FoodId = food.Guid,
                Quantity = item.Quantity,
                UnitPrice = food.Price,
                Subtotal = subtotal
            });
        }

        order.TotalAmount = totalAmount;

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        // 🔥 Create Midtrans Transaction
        var snapToken = await _paymentService
            .CreateTransactionAsync(order.Guid, userId);

        return new
        {
            orderId = order.Guid,
            totalAmount = totalAmount,
            snapToken = snapToken
        };
    }
}