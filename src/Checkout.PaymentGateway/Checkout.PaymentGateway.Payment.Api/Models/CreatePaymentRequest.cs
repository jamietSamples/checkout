using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentGateway.Payment.Api.Models
{
    public class CreatePaymentRequest
    {
        [Required]
        
        public int Amount { get; set; }

        [Required]
        public string CurrencyCode { get; set; }

        [Required]
        public string MerchantAccount { get; set; }

        [Required]
        public string MerchantReference { get; set; }

        [Required]
        public PaymentDetailsRequest PaymentDetails { get; set; }

    }

    public class PaymentDetailsRequest
    { 
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string ExpiryMonth { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string ExpiryYear { get; set; }

        [Required]
        [StringLength(3, MinimumLength =3)]
        public string CVV { get; set; }

        [Required]
        public string HolderName { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
    }
}
