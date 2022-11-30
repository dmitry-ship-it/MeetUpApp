using AutoMapper;
using MeetUpApp.Api.Data.Models;

namespace MeetUpApp.Api.ViewModels.Mapping
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressViewModel, Address>();
        }
    }
}
