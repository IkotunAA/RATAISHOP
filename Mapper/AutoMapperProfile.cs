using AutoMapper;
using RATAISHOP.Entities;
using RATAISHOP.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Exclude sensitive data
            .ReverseMap();

        // Order Mappings
        CreateMap<Order, OrderDto>().ReverseMap();

        // Wallet Mappings
        CreateMap<Wallet, WalletDto>().ReverseMap();

        // Payment Mappings
       // CreateMap<Payment, PaymentDto>().ReverseMap();

    }
}
