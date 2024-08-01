using AutoMapper;
using PatikaMovieStore.DtoLayer.ActorDtos;
using PatikaMovieStore.DtoLayer.ActorMovieDtos;
using PatikaMovieStore.DtoLayer.AuthDtos;
using PatikaMovieStore.DtoLayer.CustomerDtos;
using PatikaMovieStore.DtoLayer.DirectorDtos;
using PatikaMovieStore.DtoLayer.GenreDtos;
using PatikaMovieStore.DtoLayer.MovieDtos;
using PatikaMovieStore.DtoLayer.OrderDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Actor, ActorDto>()
                .ForMember(dest => dest.Movies, opt => opt.MapFrom(src => src.ActorMovies.Select(am => am.Movie)))
                .ReverseMap();

            CreateMap<Actor, ActorWithMovieNameDto>()
                .ForMember(dest => dest.MovieNames, opt => opt.MapFrom(src => src.ActorMovies.Select(am => am.Movie.Title).ToList()));

            CreateMap<Actor, CreateActorDto>().ReverseMap();
            CreateMap<Actor, UpdateActorDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerAuthDto>().ReverseMap();

            CreateMap<Director, DirectorDto>().ReverseMap();
            CreateMap<Director, CreateDirectorDto>().ReverseMap();
            CreateMap<Director, UpdateDirectorDto>().ReverseMap();

            //CreateMap<Movie, MovieDto>().ReverseMap();

            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genres))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director));
          
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
            CreateMap<Movie, UpdateMovieDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();
            
            CreateMap<Genre, GenreDto>().ReverseMap();

            CreateMap<ActorMovie, ActorMovieDto>()
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Title))
                .ReverseMap();
        }
    }
}
