namespace Checkout.PaymentGateway.Payment.Application.Extensions
{
    public static class MaskingExtension
    {
        public static string Mask(this string valueToMask, int maskLength)
        {
            if (string.IsNullOrEmpty(valueToMask)) 
                return valueToMask;

            if (valueToMask.Length < maskLength)
                return Mask(valueToMask);

            return valueToMask.Substring(maskLength).PadLeft(valueToMask.Length, '*');
        }

        public static string Mask(this string valueToMask)
        {
            return valueToMask.Substring(valueToMask.Length).PadLeft(valueToMask.Length, '*');
        }
    }
}
