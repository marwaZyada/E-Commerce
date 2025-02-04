using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {

        public OrderSpecification(string email)
        { 
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.OrderItems);
            AddOrderByDescending(o => o.OrderDate);
        }
        public OrderSpecification(string email,int orderid):base(o=>o.BuyerEmail==email && o.Id==orderid)
        {
            Includes.Add(o=>o.DeliveryMethod);
            Includes.Add(o=>o.OrderItems);
            AddOrderByDescending(o=>o.OrderDate);
        }
       
    }
}
