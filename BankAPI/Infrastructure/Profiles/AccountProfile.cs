using AutoMapper;

namespace Bank.Infrastructure.Profiles
{
    public class AccountProfile : Profile
    {

        public AccountProfile()
        {
            CreateMap<Models.Account, BankAPI.Controllers.TransactionsController.AccountProfileViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId))
            .ReverseMap();

        }
    }
}
