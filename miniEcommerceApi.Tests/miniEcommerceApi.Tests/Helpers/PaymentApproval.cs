using miniEcommerceApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniEcommerceApi.Tests.Helpers
{
    public class PaymentApproval
    {
        public class AlwaysApproved : IRandomPaymentApproval
        {
            public bool IsApproved() => true;
        }

        public class AlwaysRefused : IRandomPaymentApproval
        {
            public bool IsApproved() => false;
        }
    }
}
