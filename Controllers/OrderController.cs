using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Entities;
using RATAISHOP.Enum;
using RATAISHOP.Models;
using RATAISHOP.Repositories.Implementations;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderRepository orderRepository, IOrderService orderService, IPaymentService paymentService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
            _paymentService = paymentService;
        }
        [HttpPost("{orderId}/pay")]
        public async Task<IActionResult> PayForOrder(int orderId, [FromBody] PaymentMethod paymentMethod)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound(new BaseResponse<string> { Status = false, Message = "Order not found." });
            BaseResponse<string> paymentResponse;

            switch (paymentMethod)
            {
                case PaymentMethod.BankTransfer:
                    paymentResponse = await _paymentService.ProcessBankTransferPayment(order);
                    break;

                case PaymentMethod.Paystack:
                    paymentResponse = await _paymentService.ProcessPaystackPayment(order);
                    break;

                default:
                    return BadRequest(new BaseResponse<string> { Status = false, Message = "Invalid payment method." });
            }

            return Ok(paymentResponse);
        }

    

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _orderRepository.GetOrderByIdAsync(id);
            if (response.OrderItems == null)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var response = await _orderRepository.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest(new BaseResponse<string> { Status = false, Message = "ID mismatch" });

            var response = await _orderRepository.UpdateOrderAsync(order);
            return Ok(response);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var response = await _orderRepository.dele(id);
        //    return Ok(response);
        //}
    }

}
