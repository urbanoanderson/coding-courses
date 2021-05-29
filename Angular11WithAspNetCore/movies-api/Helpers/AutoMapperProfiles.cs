using System.Collections.Generic;
using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            this.CreateMap<GenreDTO, Genre>().ReverseMap();
            this.CreateMap<GenreCreationDTO, Genre>();

            this.CreateMap<ActorDTO, Actor>().ReverseMap();
            this.CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());

            this.CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(e => e.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
                .ForMember(e => e.Longitude, dto => dto.MapFrom(prop => prop.Location.X));
            this.CreateMap<MovieTheaterCreationDTO, MovieTheater>()
                .ForMember(e => e.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            this.CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors))
                .ForMember(x => x.MovieTheatersMovies, options => options.MapFrom(MapMovieTheatersMovies));
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();

            if (movieCreationDTO.GenresIds != null)
            {
                foreach (var id in movieCreationDTO.GenresIds)
                {
                    result.Add(new MoviesGenres() { GenreId = id });
                }
            }

            return result;
        }

        private List<MovieTheatersMovies> MapMovieTheatersMovies(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieTheatersMovies>();

            if (movieCreationDTO.MovieTheatersIds != null)
            {
                foreach (var id in movieCreationDTO.MovieTheatersIds)
                {
                    result.Add(new MovieTheatersMovies() { MovieTheaterId = id });
                }
            }

            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (movieCreationDTO.Actors != null)
            {
                foreach (var actor in movieCreationDTO.Actors)
                {
                    result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
                }
            }

            return result;
        }
    }
}