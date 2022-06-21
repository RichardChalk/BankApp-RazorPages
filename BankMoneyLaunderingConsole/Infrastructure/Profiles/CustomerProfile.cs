using AutoMapper;


namespace Bank.Infrastructure.Profiles
{
    public class CustomerProfile : Profile
    {

        public CustomerProfile()
        {
            // Customer <=> CustomerProfileViewModel
            CreateMap<Models.Customer, BankMoneyLaunderingConsole.Services.LaunderingService.CustomerConsoleViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
            .ReverseMap();

          
        }
    }
}
