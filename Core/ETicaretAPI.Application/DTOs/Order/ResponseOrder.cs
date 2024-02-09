using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ETicaretAPI.Application.DTOs.Order
{
    public class ResponseOrder
    {
        public int TotalOrdersCount { get; set; }
        public List<ListOrder> Orders { get; set; }
    }
}
