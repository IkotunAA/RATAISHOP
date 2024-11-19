using AutoMapper;
using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WalletService(IWalletRepository walletRepository, IPaymentService paymentService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _paymentService = paymentService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<WalletDto>> CreateWalletAsync(WalletCreateDto walletDto)
        {
            var wallet = _mapper.Map<Wallet>(walletDto);
            await _walletRepository.CreateWalletAsync(wallet);
            _unitOfWork.SaveChanges();
            var createdWalletDto = _mapper.Map<WalletDto>(wallet);

            return new BaseResponse<WalletDto>
            {
                Status = true,
                Message = "Wallet created successfully.",
                Data = createdWalletDto
            };
        }

        public async Task<BaseResponse<WalletDto>> GetWalletBySellerIdAsync(int sellerId)
        {
            var wallet = await _walletRepository.GetWalletBySellerIdAsync(sellerId);
            if (wallet == null)
            {
                return new BaseResponse<WalletDto>
                {
                    Status = false,
                    Message = "Wallet not found.",
                };
            }

            var walletDto = _mapper.Map<WalletDto>(wallet);
            return new BaseResponse<WalletDto>
            {
                Status = true,
                Message = "Wallet retrieved successfully.",
                Data = walletDto
            };
        }

        public async Task<BaseResponse<WalletDto>> UpdateWalletAsync(WalletUpdateDto walletDto)
        {
            var wallet = await _walletRepository.GetWalletBySellerIdAsync(walletDto.SellerId);
            if (wallet == null)
            {
                return new BaseResponse<WalletDto>
                {
                    Status = false,
                    Message = "Wallet not found."
                };
            }

            _mapper.Map(walletDto, wallet);
            await _walletRepository.UpdateWalletAsync(wallet);
            _unitOfWork.SaveChanges();
            return new BaseResponse<WalletDto>
            {
                Status = true,
                Message = "Wallet updated successfully.",
            };
        }

        public async Task<BaseResponse<WalletDto>> WithdrawFromWalletAsync(int sellerId, decimal amount,string accountNumber, string bankName)
        {
            var wallet = await _walletRepository.GetWalletBySellerIdAsync(sellerId);
            if (wallet == null)
            {
                return new BaseResponse<WalletDto>
                {
                   Status = false,
                   Message = "Wallet not found.",
                };
            }

            if (wallet.Balance < amount)
            {
                return new BaseResponse<WalletDto>
                {
                   Status = false,
                   Message = "Insufficient balance."
                };
            }

            // Assuming seller's bank details are stored and retrieved accordingly
            accountNumber = accountNumber; // Replace with actual seller's account number
            bankName = bankName;
            string bankCode = "044"; // Replace with actual bank code
            string narration = "Withdrawal from wallet";

            var transferResponse = await _paymentService.TransferToBankAccountAsync(accountNumber, bankCode, amount, narration, bankName);
            if (!transferResponse.Status)
            {
                return new BaseResponse<WalletDto>
                {
                    Status = false,
                    Message = transferResponse.Message
                };
            }

            wallet.Balance -= amount;
            await _walletRepository.UpdateWalletAsync(wallet);
            _unitOfWork.SaveChanges();
            var walletDto = _mapper.Map<WalletDto>(wallet);
            return new BaseResponse<WalletDto>
            {
                Status = true,
                Message = "Withdrawal successful and transfer initiated.",
                Data = walletDto
            };
        }
    }
}
