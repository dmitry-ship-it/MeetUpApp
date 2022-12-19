using AutoMapper;
using MeetUpApp.Data.Models;

namespace MeetUpApp.Api.ViewModels.Mapping
{
    public class MeetupProfile : Profile
    {
        public MeetupProfile()
        {
            CreateMap<MeetupViewModel, Meetup>();
            CreateMap<AddressViewModel, Meetup>();
            CreateMap<int, Meetup>();
        }
    }
}
