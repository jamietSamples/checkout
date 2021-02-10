using Checkout.PaymentGateway.Payment.Application.Extensions;
using Shouldly;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Application.Tests
{
    public class MaskingExtensionTests
    {
        public MaskingExtensionTests()
        {

        }

        [Theory]
        [InlineData("123456","******")]
        [InlineData("12","**")]
        public void CallingMask_MasksString(string input, string expectedOutput)
        {
            var result = input.Mask();

            result.ShouldBe(expectedOutput);
        }

        [Theory]
        [InlineData("123456",4,"****56")]
        [InlineData("1234",100,"****")]
        public void CallingMaskWithLength_MasksSpecifiedLength(string input, int maskLength, string expectedOutput)
        {
            var result = input.Mask(maskLength);

            result.ShouldBe(expectedOutput);
        }
    }
}
