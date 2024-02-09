using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories.CompletedOrderT;
using ETicaretAPI.Application.Repositories.OrderT;
using ETicaretAPI.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
        }
        private string getOrderCode()
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf(",") + 1, orderCode.Length - orderCode.IndexOf(",") - 1);
            if (_orderWriteRepository.Table.Any(c => c.OrderCode == orderCode))
            {
                    getOrderCode();
            }
                return orderCode ;
        }
        public async Task CreateOrderAsync(CreateOrder model)
        {
            
                var orderCode=getOrderCode();
                await _orderWriteRepository.AddAsync(new()
                {
                    Description = model.Description,
                    Address = model.Address,
                    Id = Guid.Parse(model.BasketId),
                    OrderCode =orderCode,
                });
                await _orderWriteRepository.SaveAsync();
            
        }

        public async Task<ResponseOrder> GetAllOrdersAsync(int size, int page)
        {
            var query = _orderReadRepository.Table.Include(c => c.Basket).ThenInclude(a => a.User)
                                                                           .Include(c => c.Basket).ThenInclude(a => a.BasketItems)          
                                                                                                                 .ThenInclude(a=>a.Product);
            var data=query.Skip(page * size).Take(size);
            var data2 = from order in data
                        join CompletedOrder in _completedOrderWriteRepository.Table
                        on order.Id equals CompletedOrder.OrderId into co
                        from _co in co.DefaultIfEmpty()
                        select new
                        {
                            Id=order.Id,
                            OrderCode=order.OrderCode,
                            CreatedDate=order.CreatedDate,
                            Basket=order.Basket,
                            Completed=_co!=null?true:false,
                        };




            return new ResponseOrder()
            {
                TotalOrdersCount =await query.CountAsync(),
                Orders = data2.Select(c=>new ListOrder()
                {
                    Id=c.Id.ToString(),
                    OrderCode =c.OrderCode,
                    Username=c.Basket.User.ToString(),
                    TotalPrice=c.Basket.BasketItems.Sum(c=>c.Quantity*c.Product.Price),
                    CreatedDate=c.CreatedDate,
                    IsCompleted=c.Completed
                }).ToList()
            };
        }

        public async Task<SingleOrder> GetOrderByIdAsync(string orderId)
        {
            var data =  _orderReadRepository.Table
                                                    .Include(c => c.Basket)
                                                        .ThenInclude(a => a.BasketItems)
                                                            .ThenInclude(bi => bi.Product);
                                                            
            var data2 = await ( from order in data
                        join completeOrder in _completedOrderWriteRepository.Table
                        on order.Id equals completeOrder.OrderId into co
                        from _co in co.DefaultIfEmpty()
                        select new SingleOrder()
                        {
                            Address = order.Address,
                            BasketItems = order.Basket.BasketItems.Select(c => new
                            {
                                c.Product.Name,
                                c.Product.Price,
                                c.Quantity,
                            }),
                            CreatedDate = order.CreatedDate,
                            Description = order.Description,
                            Id = order.Id.ToString(),
                            OrderCode = order.OrderCode,
                            IsCompleted = _co!=null?true:false,
                        }).FirstOrDefaultAsync(c=>c.Id==orderId);
            if (data2 != null)
            {
                 return data2;
            }
            throw new Exception("hata");

        }

        public async Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id)
        {
            Order? order = await _orderReadRepository.Table
            .Include(o => o.Basket)
            .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

            if (order != null)
            {
                await _completedOrderWriteRepository.AddAsync(new() { OrderId = Guid.Parse(id) });
                return (await _completedOrderWriteRepository.SaveAsync() > 0, new()
                {
                    OrderCode = order.OrderCode,
                    OrderDate = order.CreatedDate,
                    Username = order.Basket.User.UserName,
                    EMail = order.Basket.User.Email
                });
            }
            return (false, null);
        }
    }
}
