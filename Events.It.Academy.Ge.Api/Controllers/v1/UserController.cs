using Application.User.Models.Request;
using Application.User;
using Events.It.Academy.Ge.Api.Infrastructure.Auth.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Events.It.Academy.Ge.Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfiguration> _options;

        public UserController(IUserService userService, IOptions<JWTConfiguration> options)
        {
            _userService = userService;
            _options = options;
        }
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="cancellationToken"></param>
        ///<param name="model"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserRegisterModel), StatusCodes.Status200OK)]
        [HttpPost("Register")]
        public async Task Register(UserRegisterModel model, CancellationToken cancellationToken) => await _userService.RegisterAsync(model, cancellationToken);

        /// <summary>
        /// Log In user
        /// </summary>
        /// <param name="cancellationToken"></param>
        ///<param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<string> Login(UserLoginModel model, CancellationToken cancellationToken)
        {
            string id = await _userService.LoginAsync(model, cancellationToken);
            return JWTHelper.GenerateSecurityToken(id, _options);
        }
    }
}
