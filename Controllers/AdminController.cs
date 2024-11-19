using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public AdminController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("confirm-bank-transfer/{orderId}")]
        public async Task<IActionResult> ConfirmBankTransfer(int orderId, string transactionReference)
        {
            var response = await _paymentService.ConfirmBankTransfer(orderId, transactionReference);
            if (response.Status == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }
    }

}
