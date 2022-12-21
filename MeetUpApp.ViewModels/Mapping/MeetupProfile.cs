using AutoMapper;
using MeetUpApp.Data.Models;

namespace MeetUpApp.ViewModels.Mapping
{
    public class MeetupProfile : Profile
    {
        public MeetupProfile()
        {
            CreateMap<MeetupViewModel, Meetup>();
            CreateMap<AddressViewModel, Meetup>();
            CreateMap<int, Meetup>()
                .ForMember(m => m.Id,
                    opt => opt.MapFrom(src => src));
        }
    }
}