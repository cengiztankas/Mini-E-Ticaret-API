using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributies;
using ETicaretAPI.Application.Enum;
using ETicaretAPI.Application.Features.Commands.OrderSQRS.CreateOrder;
using ETicaretAPI.Application.Features.Queries.OrderSQRS.CompleteOrder;
using ETicaretAPI.Application.Features.Queries.OrderSQRS.GetOrder;
using ETicaretAPI.Application.Features.Queries.OrderSQRS.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get All Orders")]
        public async Task<IActionResult> GetOrderAsync([FromQuery] GetOrderQueryRequest getOrderQueryRequest)
        {
            GetOrderQueryResponse response = await _mediator.Send(getOrderQueryRequest);
            return Ok(response);
        }
        [HttpGet("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Order by Id")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
        {
            GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
            return Ok(response);    
        }
        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Writing, Definition = "Create Order")]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            CreateOrderCommandResponse response=await _mediator.Send(createOrderCommandRequest);
            return Ok(response);
        }
        [HttpGet("complete-order/{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating, Definition = "Complete Order")]
        public async Task<IActionResult> CompleteOrder([FromRoute]GetCompleteOrderQueryRequest completeOrderQueryRequest)
        {
            GetCompleteOrderQueryResponse response = await _mediator.Send(completeOrderQueryRequest);
            return Ok(response);
        }
    }
}
