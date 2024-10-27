using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SDD.Services.AuthAPI.Jobs;
using SDD.Services.AuthAPI.Models.Dto;
using SDD.Services.AuthAPI.Service.IService;

namespace SDD.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        protected ResponseDto _response;
        public AuthApiController(IAuthService authService, IBackgroundJobClient backgroundJobClient)
        {
            _authService = authService;
            _backgroundJobClient = backgroundJobClient;
            _response = new();
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {

            var responseDto = await _authService.Register(model);
            if (!responseDto.IsSuccess)
            {
                _response.IsSuccess = false;
                _response.Message = responseDto.Message;
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Message += responseDto.Message;
            _response.Result = responseDto.Result;

            _backgroundJobClient.Enqueue<EmailJob>(job => job.SendEmailJob(model.Email, "Welcome!", "Thank you for registering!"));

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);

        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            return Ok(_response);

        }


    }
}
