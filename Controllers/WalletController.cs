using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Authorize(Roles = "Seller")]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var sellerId = int.Parse(User.FindFirst("Id").Value);
            var response = await _walletService.GetWalletBySellerIdAsync(sellerId);
            if (!response.Status == false)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferToBank(decimal amount, string bankAccount, string accountName)
        {
            var sellerId = int.Parse(User.FindFirst("Id").Value);
            var response = await _walletService.WithdrawFromWalletAsync(sellerId, amount, bankAccount, accountName);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }
    }

}
