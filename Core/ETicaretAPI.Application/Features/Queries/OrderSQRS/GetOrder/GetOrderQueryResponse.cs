using ETicaretAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.OrderSQRS.GetOrder
{
    public class GetOrderQueryResponse
    {
        public int TotalOrdersCount { get; set; }
        public List<ListOrder> Orders { get; set; }
    }
}
