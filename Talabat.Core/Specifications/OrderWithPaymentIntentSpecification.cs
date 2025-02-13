using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications
{
    public class OrderWithPaymentIntentSpecification:BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string IntentId):base(o=>o.PaymentIntetId==IntentId) 
        {
            
        }
    }
}
