using ETicaretAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder model);
        Task<ResponseOrder> GetAllOrdersAsync(int size,int page);
        Task<SingleOrder> GetOrderByIdAsync(string orderId);
        Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
    }
}
