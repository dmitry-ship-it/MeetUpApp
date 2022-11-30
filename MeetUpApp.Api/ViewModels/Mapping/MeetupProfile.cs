using AutoMapper;
using MeetUpApp.Api.Data.Models;

namespace MeetUpApp.Api.ViewModels.Mapping
{
    public class MeetupProfile : Profile
    {
        public MeetupProfile()
        {
            CreateMap<MeetupViewModel, Meetup>();
        }
    }
}
