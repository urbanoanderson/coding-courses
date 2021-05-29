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
    public class ActorsController : ControllerBase
    {
        private const string STORAGE_CONTAINER_NAME = "actors";

        private ApplicationDbContext dbContext;

        private IMapper mapper;

        private IFileStorageService fileStorageService;

        public ActorsController(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = this.dbContext.Actors.AsQueryable();
            await this.HttpContext.InsertParametersPaginationInHeader(queryable);
            var actors = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return this.mapper.Map<List<ActorDTO>>(actors);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            Actor actor = await this.dbContext.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return this.NotFound();
            }

            return this.mapper.Map<ActorDTO>(actor);
        }

        [HttpPost("searchByName")]
        public async Task<ActionResult<List<ActorsMovieDTO>>> SearchByName([FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<ActorsMovieDTO>();
            }

            return await this.dbContext.Actors
                .Where(x => x.Name.Contains(name))
                .OrderBy(x => x.Name)
                .Select(x => new ActorsMovieDTO() { Id = x.Id, Name = x.Name, Picture = x.Picture })
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            Actor actor = this.mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await this.fileStorageService.SaveFile(STORAGE_CONTAINER_NAME, actorCreationDTO.Picture);
            }

            this.dbContext.Add(actor);
            await this.dbContext.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            Actor actor = await this.dbContext.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return this.NotFound();
            }

            actor = this.mapper.Map(actorCreationDTO, actor);

            if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await this.fileStorageService.EditFile(STORAGE_CONTAINER_NAME, actorCreationDTO.Picture, actor.Picture);
            }

            await this.dbContext.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Actor actor = await this.dbContext.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return this.NotFound();
            }

            this.dbContext.Remove(actor);
            await this.dbContext.SaveChangesAsync();
            await this.fileStorageService.DeleteFile(actor.Picture, STORAGE_CONTAINER_NAME);

            return this.NoContent();
        }
    }
}