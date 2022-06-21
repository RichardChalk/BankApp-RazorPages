using AutoMapper;

namespace Bank.Infrastructure.Profiles
{
    public class CustomerProfile : Profile
    {

        public CustomerProfile()
        {
            // Customer <=> CustomerProfileViewModel
            CreateMap<Models.Customer, BankAPI.Controllers.CustomerProfileController.CustomerProfileViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.Birthday.Year))
            .ReverseMap();

          
        }
    }
}
