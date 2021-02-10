using Checkout.PaymentGateway.Acquirer.Models;
using Checkout.PaymentGateway.Acquirer.Validators;
using Shouldly;
using System.Linq;
using Xunit;

namespace Checkout.PaymentGateway.Acquirer.Tests
{
    public class AcquirerRequestValidatorTests
    {
        private readonly AcquirerRequestValidator _validator;
        private readonly AcquirerRequest _acquirerRequest;
        public AcquirerRequestValidatorTests()
        {
            _validator = new AcquirerRequestValidator();
            _acquirerRequest = new AcquirerRequest()
            {
                Amount = 1000,
                CurrencyCode = "GBP",
                CardNumber = "5555 4444 3333 1111",
                ExpiryMonth = "08",
                ExpiryYear = "2021",
                CVV = "322",
                HolderName = "Jamie Jam",
                PaymentMethod = "VISA"
            };
        }

        [Theory]
        [InlineData(10)]
        public void Validate_ShouldReturnFalse_WhenAmountIsInvalid(int amount)
        {
            _acquirerRequest.Amount = amount;
         
            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid amount specified");
        }

        [Theory]
        [InlineData(1030)]
        public void Validate_ShouldReturnIsValid_WhenAmountIsValid(int amount)
        {
            _acquirerRequest.Amount = amount;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("AED")]
        public void Validate_ShouldReturnFalse_WhenCurrencyCodeIsInvalid(string currencyCode)
        {
            _acquirerRequest.CurrencyCode = currencyCode;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid currency specified");
        }

        [Theory]
        [InlineData("GBP")]
        [InlineData("EUR")]
        [InlineData("USD")]
        public void Validate_ShouldReturnIsValid_WhenCurrencyCodeIsValid(string currencyCode)
        {
            _acquirerRequest.CurrencyCode = currencyCode;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("5555 1111")]
        [InlineData("5555 1111 2222")]
        public void Validate_ShouldReturnFalse_WhenCardNumberIsInvalid(string cardNumber)
        {
            _acquirerRequest.CardNumber = cardNumber;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid card number");
        }

        [Theory]
        [InlineData("5555 4444 3333 1111")]
        public void Validate_ShouldReturnIsValid_WhenCardNumberIsValid(string cardNumber)
        {
            _acquirerRequest.CardNumber = cardNumber;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("23")]
        [InlineData("0")]
        public void Validate_ShouldReturnFalse_WhenExpiryMonthIsInvalid(string expiryMonth)
        {
            _acquirerRequest.ExpiryMonth = expiryMonth;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(2);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid expiry month");
        }

        [Theory]
        [InlineData("12")]
        [InlineData("03")]
        public void Validate_ShouldReturnIsValid_WhenExpiryMonthIsValid(string expiryMonth)
        {
            _acquirerRequest.ExpiryMonth = expiryMonth;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("2020")]
        public void Validate_ShouldReturnFalse_WhenExpiryYearIsInvalid(string expiryYear)
        {
            _acquirerRequest.ExpiryYear = expiryYear;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid expiry year");
        }

        [Theory]
        [InlineData("2120")]
        [InlineData("2023")]
        public void Validate_ShouldReturnIsValid_WhenExpiryYearIsValid(string expiryYear)
        {
            _acquirerRequest.ExpiryYear = expiryYear;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("11","2021")]
        [InlineData("10","2025")]
        public void Validate_ShouldReturnFalse_WhenExpiryDateIsValid(string expiryMonth, string expiryYear)
        {
            _acquirerRequest.ExpiryMonth = expiryMonth;
            _acquirerRequest.ExpiryYear = expiryYear;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("2222")]
        [InlineData("10")]
        public void Validate_ShouldReturnFalse_WhenCVVIsInvalid(string cvv)
        {
            _acquirerRequest.CVV = cvv;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid CVV");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("321")]
        public void Validate_ShouldReturnIsValid_WhenCVVIsValid(string cvv)
        {
            _acquirerRequest.CVV = cvv;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("Jose Mou")]
        [InlineData("Jo Mourinho")]
        public void Validate_ShouldReturnFalse_WhenHolderNameIsInvalid(string holderName)
        {
            _acquirerRequest.HolderName = holderName;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors.First().ErrorMessage.ShouldBe("Invalid cardholder name");
        }

        [Theory]
        [InlineData("Jamie Tee")]
        [InlineData("Jaime Jamie")]
        public void Validate_ShouldReturnIsValid_WhenHolderNameIsValid(string holderName)
        {
            _acquirerRequest.HolderName = holderName;

            var result = _validator.Validate(_acquirerRequest);

            result.IsValid.ShouldBe(true);
            result.Errors.Count.ShouldBe(0);
        }
    }
}
