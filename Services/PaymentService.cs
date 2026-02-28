using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IConfiguration configuration,
        HttpClient httpClient
    )
    {
        _paymentRepository = paymentRepository;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> CreateTransactionAsync(Guid orderId)
    {
        var order = await _paymentRepository.GetOrderWithItemsAsync(orderId);

        if (order == null)
            throw new Exception("Order not found");

        var serverKey = _configuration["Midtrans:ServerKey"];
        var baseUrl = _configuration["Midtrans:BaseUrl"];

        var auth = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(serverKey + ":"));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", auth);

        var payload = new
        {
            transaction_details = new
            {
                order_id = order.Guid.ToString(),
                gross_amount = order.TotalAmount
            },
            item_details = order.OrderItems.Select(i => new
            {
                id = i.FoodId.ToString(),
                price = i.UnitPrice,
                quantity = i.Quantity,
                name = i.Food.Name
            })
        };

        var response = await _httpClient.PostAsync(
            $"{baseUrl}/snap/v1/transactions",
            new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"));

        var result = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(result);

        return doc.RootElement
            .GetProperty("token")
            .GetString()!;
    }

    public async Task HandleNotificationAsync(string json)
    {
        using var doc = JsonDocument.Parse(json);

        var orderId = doc.RootElement
            .GetProperty("order_id").GetString();

        var transactionStatus = doc.RootElement
            .GetProperty("transaction_status").GetString();

        if (!Guid.TryParse(orderId, out var guid))
            return;

        var order = await _paymentRepository
            .GetOrderWithItemsAsync(guid);

        if (order == null)
            return;

        switch (transactionStatus)
        {
            case "settlement":
                order.Status = TypeStatusOrder.Confirmed;
                break;

            case "expire":
            case "cancel":
                order.Status = TypeStatusOrder.Cancelled;
                break;
        }

        await _paymentRepository.UpdateOrderStatusAsync(order);
        await _paymentRepository.SaveChangesAsync();
    }
}