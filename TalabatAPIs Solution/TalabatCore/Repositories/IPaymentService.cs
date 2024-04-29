using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Entities.Order_Aggregation;

namespace TalabatCore.Repositories
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CrreateOrUpdatePaymentIntent(string BaasketId);

        Task<Order> UpdaePaymentIntentWithSucceedorFailed(string paymentIntentId,bool IsSucceed);
    }
}
