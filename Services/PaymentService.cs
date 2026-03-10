using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using RestaurantFoods.Models.Data;
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

    public async Task<string> CreateTransactionAsync(Guid orderId, Guid userId)
    {
        var order = await _paymentRepository.GetOrderWithItemsAsync(orderId);

        if (order == null)
            throw new Exception("Order not found");
            
        if(order.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");

        if(order.Status != TypeStatusOrder.Pending)
            throw new InvalidOperationException("Order already paid");

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

        var root = doc.RootElement;

        var orderId = root.GetProperty("order_id").GetString();
        var statusCode = root.GetProperty("status_code").GetString();
        var grossAmount = root.GetProperty("gross_amount").GetString();
        var signatureKey = root.GetProperty("signature_key").GetString();
        var transactionStatus = root.GetProperty("transaction_status").GetString();
        var paymentType = root.GetProperty("payment_type").GetString();

        if (!Guid.TryParse(orderId, out var guid))
            return;

        var serverKey = _configuration["Midtrans:ServerKey"];

        // ================================
        // SIGNATURE VALIDATION
        // ================================
        var raw = orderId + statusCode + grossAmount + serverKey;

        using var sha = SHA512.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        var computedSignature = BitConverter.ToString(hash)
            .Replace("-", "")
            .ToLower();

        if (computedSignature != signatureKey)
            return; // Invalid signature

        // ================================
        // GET ORDER & PAYMENT
        // ================================
        var order = await _paymentRepository
            .GetOrderWithItemsAsync(guid);

        if (order == null)
            return;

        var payment = await _paymentRepository
            .GetPaymentByOrderIdAsync(guid);

        if (payment == null)
        {
            payment = await _paymentRepository
                .CreatePaymentAsync(order.Guid);
        }

        var grossAmountStr = root.GetProperty("gross_amount").GetString();
        var grossTotal = Convert.ToDecimal(grossAmountStr);

        payment.AmountPaid = grossTotal;
        // ================================
        // EXTRACT BANK / VA NUMBER
        // ================================
        string? bank = null;
        string? vaNumber = null;

        if (paymentType == "bank_transfer" &&
            root.TryGetProperty("va_numbers", out var vaNumbers) &&
            vaNumbers.GetArrayLength() > 0)
        {
            bank = vaNumbers[0].GetProperty("bank").GetString();
            vaNumber = vaNumbers[0].GetProperty("va_number").GetString();
        }

        // ================================
        // UPDATE PAYMENT DATA
        // ================================
        payment.PaymentMethod = paymentType;
        payment.VaNumber = vaNumber;
        PaymentStatus status = transactionStatus switch
        {
            "settlement" => PaymentStatus.Paid,
            "capture"    => PaymentStatus.Paid,
            "pending"    => PaymentStatus.Pending,
            "expire"     => PaymentStatus.Expired,
            "cancel"     => PaymentStatus.Failed,
            "deny"       => PaymentStatus.Failed,
            _            => PaymentStatus.Pending
        };

        payment.PaymentStatus = status;

        if (transactionStatus == "settlement" ||
            transactionStatus == "capture")
        {
            payment.PaymentDate = DateTime.UtcNow;
        }

        // ================================
        // UPDATE ORDER STATUS
        // ================================
        switch (transactionStatus)
        {
            case "capture":
            case "settlement":
                order.Status = TypeStatusOrder.Confirmed;
                break;

            case "pending":
                order.Status = TypeStatusOrder.Pending;
                break;

            case "expire":
            case "cancel":
            case "deny":
                order.Status = TypeStatusOrder.Cancelled;
                break;
        }

        await _paymentRepository.SaveChangesAsync();
    }
}