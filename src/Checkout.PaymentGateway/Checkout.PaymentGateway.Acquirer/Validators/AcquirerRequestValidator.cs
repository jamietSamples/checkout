using Checkout.PaymentGateway.Acquirer.Models;
using FluentValidation;
using System;
using System.Linq;

namespace Checkout.PaymentGateway.Acquirer.Validators
{
    public class AcquirerRequestValidator : AbstractValidator<AcquirerRequest>
	{
		public AcquirerRequestValidator()
		{
			RuleFor(r => r.Amount).GreaterThan(100).WithMessage("Invalid amount specified");
			RuleFor(r => r.CurrencyCode).Must(BeAValidCurrencyCode).WithMessage("Invalid currency specified");
			RuleFor(r => r.PaymentMethod).Must(BeAValidPaymentMethod).WithMessage("Invalid payment method");
			RuleFor(r => r.CardNumber).Must(BeAValidCardNumber).WithMessage("Invalid card number");
			RuleFor(r => r.ExpiryMonth).Must(BeAValidExpiryMonth).WithMessage("Invalid expiry month");
			RuleFor(r => r.ExpiryYear).Must(BeAValidExpiryYear).WithMessage("Invalid expiry year");
			RuleFor(r => new { r.ExpiryMonth, r.ExpiryYear }).Must(x => BeAValidExpiryDate(x.ExpiryMonth, x.ExpiryYear)).WithMessage("Invalid expiry date");
			RuleFor(r => r.CVV).Must(BeAValidCVV).WithMessage("Invalid CVV");
			RuleFor(r => r.HolderName).Must(BeAValidHolderName).WithMessage("Invalid cardholder name");
		}

		private bool BeAValidCurrencyCode(string currencyCode)
		{
			var currencyCodes = new[] { "GBP", "EUR", "USD"};

			return currencyCodes.Contains(currencyCode.ToUpper());
		}

		private bool BeAValidPaymentMethod(string paymentMethod)
		{
			var paymentMethods = new[] { "VISA", "MASTERCARD", "AMEX" };

			return paymentMethods.Contains(paymentMethod.ToUpper());
		}

		private bool BeAValidCardNumber(string cardNumber)
		{
			cardNumber = cardNumber.Replace(" ","");

			if (cardNumber.Length != 16)
				return false;
			if (!long.TryParse(cardNumber, out _))
				return false;

			return true;
		}

		private bool BeAValidExpiryMonth(string expiryMonth)
		{
			if (expiryMonth.Length != 2)
				return false;

			if (!int.TryParse(expiryMonth, out var intExpiryMonth))
				return false;

			if (intExpiryMonth < 1 || intExpiryMonth > 12)
				return false;

			return true;
		}

		private bool BeAValidExpiryYear(string expiryYear)
		{
			if (expiryYear.Length != 4)
				return false;

			if (!int.TryParse(expiryYear, out var intExpiryYear))
				return false;

			if (intExpiryYear < 2021)
				return false;

			return true;
		}

		private bool BeAValidExpiryDate(string expiryMonth, string expiryYear)
		{
			try
			{
				var expiryDate = DateTime.ParseExact($"{expiryMonth}{expiryYear}", "MMyyyy", System.Globalization.CultureInfo.InvariantCulture);

				if (expiryDate.Month < DateTime.Now.Month && expiryDate.Year < DateTime.Now.Year)
					return false;

				return true;
			}
			catch(Exception)
			{
				return false;
			}
			
		}

		private bool BeAValidCVV(string cvv)
		{
			if (cvv.Length != 3)
				return false;
			if (!int.TryParse(cvv, out _))
				return false;

			return true;
		}

		private bool BeAValidHolderName(string holderName)
		{
			var forbiddenWords = new[] { "JOSE", "MOURINHO" };

			return !forbiddenWords.Any(s => holderName.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0);
		}



	}
}
