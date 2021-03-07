using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController : ControllerBase
    {
        private readonly ILogger<GenresController> logger;

        private readonly IRepository repository;

        public GenresController(ILogger<GenresController> logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        [HttpGet("list")]
        [ResponseCache(Duration = 60)]
        [ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            this.logger.LogInformation("Getting all genres...");

            return await this.repository.GetAllGenres();
        }

        [HttpGet("{id:int}")]
        public ActionResult<Genre> Get(int id)
        {
            Genre genre = this.repository.GetGenreById(id);

            if (genre == null)
            {
                this.logger.LogWarning($"Genre with Id '{id}' not found.");
                return this.NotFound();
            }

            return genre;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            this.repository.AddGenre(genre);

            return this.Ok();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {
            return this.NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return this.NoContent();
        }
    }
}