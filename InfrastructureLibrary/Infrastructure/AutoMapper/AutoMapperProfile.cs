using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ModelLibrary.DTO;
using ModelLibrary.Models;
using ModelLibrary.ViewModels;

namespace InfrastructureLibrary.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Disposition, AccountViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.TransactionCount, opt => opt.MapFrom(src => src.Account.Transactions.Count))
                .ForMember(dest => dest.LoanCount, opt => opt.MapFrom(src => src.Account.Loans.Count))
                .ForMember(dest => dest.PermOrderCount, opt => opt.MapFrom(src => src.Account.PermenentOrders.Count))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Customer.Givenname + " " + src.Customer.Surname))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Account.Frequency))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Account.Balance))
                .ReverseMap();

            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TransactionId))
                .ReverseMap();

            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.MapFrom(src => src.NationalId))
                .ReverseMap();

            CreateMap<Account, AccountViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId))
                .ReverseMap();

            CreateMap<Customer, CustomerListView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Streetaddress))
                .ReverseMap();

            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Streetaddress))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ReverseMap();

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ReverseMap();
        }
    }
}
