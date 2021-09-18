using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager logger;

        private readonly IMapper mapper;

        private readonly UserManager<User> userManager;

        private readonly IAuthenticationManager authManager;

        public AuthenticationController(ILoggerManager logger,
            IMapper mapper, UserManager<User> userManager, IAuthenticationManager authManager)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.authManager = authManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            User user = this.mapper.Map<User>(userForRegistration);

            IdentityResult result = await this.userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.TryAddModelError(error.Code, error.Description);
                }

                return this.BadRequest(this.ModelState);
            }

            await this.userManager.AddToRolesAsync(user, userForRegistration.Roles);

            return this.StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await this.authManager.ValidateUser(user))
            {
                this.logger.LogWarn($"{nameof(this.Authenticate)}: Authentication Failed. Wrong user name or password");
                return this.Unauthorized();
            }

            return this.Ok(new { Token = await this.authManager.CreateToken() });
        }
    }
}
