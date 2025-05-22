using Razorpay.Api;
using System;
using System.Threading.Tasks;



public interface IRazorpayService
{
    Task<string> CreateOrderAsync(decimal? totalAmount);
    Task<string> VerifyPaymentAsync(string paymentId);
}
public class RazorpayService : IRazorpayService
{
    private readonly string razorPayKey;
    private readonly string razorPaySecret;

    public RazorpayService(IConfiguration configuration)
    {
        razorPayKey = configuration["RazorPay:NetGuard.Dev.Key"];
        razorPaySecret = configuration["RazorPay:NetGuard.Dev.Secret"];
    }
     

    public async Task<string> CreateOrderAsync(decimal? totalAmount)
    {
        try
        {
            RazorpayClient razorpayClient = new RazorpayClient(razorPayKey, razorPaySecret);
            var orderId = "1";
            //var amount = 1233;
            var currency = "INR";
            Dictionary<string, object> options = new Dictionary<string, object>();
            /*options.Add("amount", amount * 100);*/
            options.Add("amount", totalAmount * 100);// Amount is in paise, so multiply by 100
            options.Add("currency", currency);
            options.Add("receipt", orderId); // Unique order ID
            options.Add("payment_capture", 1); // Auto capture payment

            Order order = await Task.Run(() => razorpayClient.Order.Create(options));
            return order["id"];
        }
        catch (Razorpay.Api.Errors.GatewayError ex)
        {
            // Handle Razorpay API errors
            Console.WriteLine($"Razorpay API Error: {ex.Message}");
            return "Some thing went wrong.";
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
            return "Some thing went wrong.";
        }

    }

	public async Task<string> VerifyPaymentAsync(string paymentId)
	{
		try
		{
			RazorpayClient razorpayClient = new RazorpayClient(razorPayKey, razorPaySecret);

			// Fetch payment details using paymentId
			Payment payment = await Task.Run(() => razorpayClient.Payment.Fetch(paymentId));

			// Verify payment status
			if (payment != null)
			{
				var paymentStatus = payment["status"].ToString(); 
				return paymentStatus;
			}
			else
			{
				return "Payment not found.";
			}
		}
		catch (Razorpay.Api.Errors.GatewayError ex)
		{
			// Handle Razorpay API errors
			Console.WriteLine($"Razorpay API Error: {ex.Message}");
			return "Payment verification failed.";
		}
		catch (Exception ex)
		{
			// Handle other exceptions
			Console.WriteLine($"An error occurred: {ex.Message}");
			return "Payment verification failed.";
		}
	}
}


public class RazorpayWebhookData
{
    public string Id { get; set; } // Unique identifier for the webhook event
    public string EntityType { get; set; } // Type of entity associated with the event (e.g., payment)
    public string Event { get; set; } // Type of event (e.g., payment.captured)
    public RazorpayEventData Data { get; set; } // Data associated with the event
}

public class RazorpayEventData
{
    public string Id { get; set; } // ID of the payment associated with the event
    public decimal Amount { get; set; } // Amount of the payment
    public string Currency { get; set; } // Currency of the payment
    // Add other properties as needed
}