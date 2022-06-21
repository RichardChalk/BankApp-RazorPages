using AutoMapper;

namespace Bank.Infrastructure.Profiles
{
    public class CustomerProfile : Profile
    {

        public CustomerProfile()
        {
            // Customer <=> CreateCustomerViewModel
            CreateMap<Models.Customer, Bank.Pages.Customers.CreateCustomerModel.CreateCustomerViewModel>()
            .ReverseMap();


            // Customer <=> CustomerProfileViewModel
            CreateMap<Models.Customer, Bank.Pages.Customers.CustomerModel.CustomerProfileViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.Birthday.Year))
            .ReverseMap();

            // Customer <=> CustomerViewModel
            CreateMap<Models.Customer, Bank.Pages.Customers.CustomersModel.CustomerViewModel>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ReverseMap();

            // Customer <=> EditCustomerViewModel
            CreateMap<Models.Customer, Bank.Pages.Customers.EditCustomerModel.EditCustomerViewModel>()
            .ReverseMap();







        }
    }
}
