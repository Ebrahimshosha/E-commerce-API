using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Order_Aggregation;

namespace TalabatCore.Specifications.OrderSpec
{
    public class OrderWithPaymentIdSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIdSpec(string paymentIntentId):base(O=>O.PaymentIntendId == paymentIntentId)
        {
            
        }
    }
}
