using AutoMapper;

namespace Bank.Infrastructure.Profiles
{
    public class AccountProfile : Profile
    {

        public AccountProfile()
        {
            CreateMap<Models.Account, Bank.Pages.Accounts.AccountModel.AccountProfileViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId))
            .ReverseMap();

            CreateMap<Models.Account, Bank.Pages.Accounts.AccountsModel.AccountViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AccountId))
            .ReverseMap();

            CreateMap<Models.Transaction, Bank.Pages.Accounts.AccountModel.TransactionViewModel>()
           .ReverseMap();
        }
    }
}
