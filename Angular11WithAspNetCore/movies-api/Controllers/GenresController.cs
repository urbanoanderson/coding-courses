using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Filters;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;

        private readonly ApplicationDbContext dbContext;

        private readonly IMapper mapper;

        public GenresController(ILogger<GenresController> logger, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            this.logger.LogInformation("Getting all genres...");

            var genres = await this.dbContext.Genres.OrderBy(x => x.Name).ToListAsync();

            return this.mapper.Map<List<GenreDTO>>(genres);
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            Genre genre = await this.dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                return this.NotFound();
            }

            return this.mapper.Map<GenreDTO>(genre);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            Genre genre = this.mapper.Map<Genre>(genreCreationDTO);
            this.dbContext.Add(genre);
            await this.dbContext.SaveChangesAsync();
            return this.NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {
            Genre genre = this.mapper.Map<Genre>(genreCreationDTO);
            genre.Id = id;
            this.dbContext.Entry(genre).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Genre genre = await this.dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                return this.NotFound();
            }

            this.dbContext.Remove(genre);
            await this.dbContext.SaveChangesAsync();

            return this.NoContent();
        }
    }
}