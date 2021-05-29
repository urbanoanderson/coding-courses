using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieTheatersController : ControllerBase
    {
        private ApplicationDbContext dbContext;

        private IMapper mapper;

        public MovieTheatersController(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDTO>>> Get()
        {
            var entities = await this.dbContext.MovieTheaters.OrderBy(x => x.Name).ToListAsync();
            return this.mapper.Map<List<MovieTheaterDTO>>(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheaterDTO>> Get(int id)
        {
            var entity = await this.dbContext.MovieTheaters.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return this.NotFound();
            }

            return this.mapper.Map<MovieTheaterDTO>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            var entity = this.mapper.Map<MovieTheater>(movieTheaterCreationDTO);
            this.dbContext.Add(entity);
            await this.dbContext.SaveChangesAsync();
            return this.NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] MovieTheaterCreationDTO movieTheaterCreationDTO)
        {
            var entity = await this.dbContext.MovieTheaters.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return this.NotFound();
            }

            entity = this.mapper.Map(movieTheaterCreationDTO, entity);
            await this.dbContext.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await this.dbContext.MovieTheaters.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return this.NotFound();
            }

            this.dbContext.Remove(entity);
            await this.dbContext.SaveChangesAsync();

            return this.NoContent();
        }
    }
}