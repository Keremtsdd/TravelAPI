using AutoMapper;
using TravelAPI.DTOs.Auth;
using TravelAPI.DTOs.City;
using TravelAPI.DTOs.Country;
using TravelAPI.DTOs.Feedback;
using TravelAPI.DTOs.Place;
using TravelAPI.Models;

namespace TravelAPI.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // CİTY MAPPINGS
            CreateMap<City, CityListDto>();
            CreateMap<City, CitySelectDto>();
            CreateMap<CityCreateDto, City>();
            CreateMap<UpdateCityDto, City>().ReverseMap();

            // COUNTRY MAPPINGS
            CreateMap<Country, CountrySelectDto>();
            CreateMap<CountryCreateDto, Country>();
            CreateMap<CreateCountryDto, Country>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExternalCountryId));
            CreateMap<UpdateCountryDto, Country>().ReverseMap();
            CreateMap<ExternalCountryDto, Country>().ReverseMap();

            // PLACE MAPPINGS
            CreateMap<Place, PlaceCardDto>();
            CreateMap<Place, PlaceListDto>();
            CreateMap<CreatePlaceDto, Place>();
            CreateMap<UpdatePlaceDto, Place>().ReverseMap();

            // FEEDBACK MAPPINGS
            CreateMap<CreateFeedbackDTO, Feedback>();
            CreateMap<Feedback, FeedbackListDto>();

            // AUTH && ADMİN MAPPINGS
            CreateMap<AppUser, AppUserListDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<RegisterRequest, AppUser>();
        }
    }
}