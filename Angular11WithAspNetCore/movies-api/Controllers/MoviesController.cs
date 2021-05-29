using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private const string STORAGE_CONTAINER_NAME = "movies";

        private ApplicationDbContext dbContext;

        private IMapper mapper;

        private IFileStorageService fileStorageService;

        public MoviesController(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movieTheaters = await this.dbContext.MovieTheaters.OrderBy(x => x.Name).ToListAsync();
            var genres = await this.dbContext.Genres.OrderBy(x => x.Name).ToListAsync();

            var movieTheaterDTOs = this.mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
            var genreDTOs = this.mapper.Map<List<GenreDTO>>(genres);

            return new MoviePostGetDTO() { Genres = genreDTOs, MovieTheaters = movieTheaterDTOs };
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var entity = this.mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster != null)
            {
                entity.Poster = await this.fileStorageService.SaveFile(STORAGE_CONTAINER_NAME, movieCreationDTO.Poster);
            }

            this.AnnotateActorsOrder(entity);

            this.dbContext.Add(entity);
            await this.dbContext.SaveChangesAsync();
            return this.NoContent();
        }

        private void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}