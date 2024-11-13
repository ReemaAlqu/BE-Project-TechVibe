using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO;
using src.Entity;
using src.Services;
using src.Utils;

namespace src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        protected readonly IOrderService _orderService;
        protected readonly IAuthorizationService _authorization;

        public OrderController(IOrderService orderService, IAuthorizationService authorization)
        {
            _orderService = orderService;
            _authorization = authorization;
        }

        [HttpGet("users/{userId}")]
        // [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId([FromRoute] Guid userId)
        {
            return Ok(await _orderService.GetOrdersByUserId(userId));
        }


        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrderById(Guid orderId)
        {
            return Ok(await _orderService.GetOrderByIdAsync(orderId));
        }

        [HttpGet]
        // [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            return Ok(await _orderService.GetAllOrdersAsync());
        }

        //************************************************************************
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDTO.Get>> CreateOneAsync(
            [FromBody] OrderDTO.Create orderCreateDto
        )
        {
            var authenticatedClaims = HttpContext.User;
            var userId = authenticatedClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);
            return await _orderService.CreateOneOrderAsync(userGuid, orderCreateDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO.Update order)
        {
            if (await _orderService.UpdateOrderAsync(id, order))
            {
                return NoContent();
            }
            throw CustomException.NotFound($"An error occurred while updating the order.");
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _orderService.DeleteByIdAsync(id))
            {
                return NoContent();
            }
            throw CustomException.NotFound();
        }
    }
}
