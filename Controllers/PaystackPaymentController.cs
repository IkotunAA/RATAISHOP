﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Enum;
using RATAISHOP.Models;
using RATAISHOP.PaymentModel;
using RATAISHOP.PaymentServices.Interfaces;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaystackPaymentController : ControllerBase
    {
        private readonly IPaystackPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IWalletService _walletService;
        private readonly IOrderRepository _orderRepository;

        public PaystackPaymentController(IPaystackPaymentService paymentService, IOrderRepository orderRepository, IOrderService orderService, IWalletService walletService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _orderRepository = orderRepository;
            _walletService = walletService;
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializePayment([FromBody] PaystackPaymentModel paymentRequest)
        {
            var response = await _paymentService.InitializePaymentAsync(paymentRequest);
            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("verify/{reference}")]
        public async Task<OrderDto?> VerifyPaymentAsync(string reference)
        {
            var response = await _paymentService.VerifyPaymentAsync(reference);
            if (response.Status == true)
            {
                // Find the order associated with the payment reference
                var order = await _orderRepository.GetOrderByReferenceAsync(reference);

                if (order != null)
                {
                    return new OrderDto
                    {
                        Id = order.Id,
                        Buyer = order.Buyer,
                        BuyerEmail = order.BuyerEmail,
                        SellerId = order.SellerId,
                        TotalAmount = order.TotalAmount,
                        PaymentStatus = order.PaymentStatus
                    };
                }
            }

            return null;
        }
        [HttpPost("paystack/callback")]
        public async Task<IActionResult> PaystackCallback(PaystackPaymentResponse model)
        {
            var order = await _paymentService.VerifyPaymentAsync(model.Reference);
            //var or = await _orderRepository.GetOrderByIdAsync(model.);
            //    var p = PaymentStatus.Paid;
            //    if (order == null) return NotFound(new BaseResponse<string> { Status = false, Message = "Order not found." });

            //    if (model.Status == "success")
            //    {

            //        if (order.Status is true)
            //        {
            //            p = PaymentStatus.Paid;
            //        }
            //        await _orderRepository.UpdateOrderAsync(order);

            //        // Credit seller's wallet
            //        var wallet = await _walletService.GetWalletBySellerIdAsync(order.SellerId);
            //        wallet.Balance += order.TotalAmount;
            //        await _walletService.UpdateWalletAsync(wallet);

            //        return Ok(new BaseResponse<string> { Status = true, Message = "Payment successful.", Data = null });
            //    }

            //    return BadRequest(new BaseResponse<string> { Status = false, Message = "Payment failed." });
            if (order == null)
                return NotFound(new BaseResponse<OrderDto> { Status = false, Message = "Order not found." });

            if (model.Status == "success")
            {
                var orderId = order.Data.Id;
                var orderEntity = await _orderRepository.GetOrderByIdAsync(orderId);
                orderEntity.PaymentStatus = PaymentStatus.Paid;
                await _orderRepository.UpdateOrderAsync(orderEntity);

                // Credit seller's wallet
                var wallet = await _walletService.GetWalletBySellerIdAsync(orderEntity.SellerId);
                var newWalletBalance = wallet.Data.Balance += orderEntity.TotalAmount;
                var wall = new WalletUpdateDto
                {
                    SellerId = orderEntity.SellerId,
                    Balance = newWalletBalance
                };
                await _walletService.UpdateWalletAsync(wall);

                return Ok(new BaseResponse<string> { Status = true, Message = "Payment successful.", Data = null });
            }

            return BadRequest(new BaseResponse<string>{ Status = false, Message = "Payment failed.", Data = null });
        }
    
    }
}