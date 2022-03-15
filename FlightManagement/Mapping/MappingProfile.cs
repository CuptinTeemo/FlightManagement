using AutoMapper;
using FlightManagement.Models;
namespace FlightManagement.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FlightViewModel, Flight>();
            CreateMap<Flight, FlightViewModel>()
                .ForMember(x => x.DestinationId, opt => opt.MapFrom(src=>src.Destination.ID))
                .ForMember(x => x.DepartureId, opt => opt.MapFrom(src => src.Departure.ID));
        }
    }
}
