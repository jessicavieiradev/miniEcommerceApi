using miniEcommerceApi.Interfaces;

namespace miniEcommerceApi.Helpers
{
    public class RandomPaymentApproval : IRandomPaymentApproval
    {
        public bool IsApproved() => new Random().Next(0, 10) > 1;
    }
}
