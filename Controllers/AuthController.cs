using JWT_AUTH.Contexts;
using JWT_AUTH.Helper;
using JWT_AUTH.Models;
using JWT_AUTH.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT_AUTH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly UserManager<Register_DTO> userManager_;
        //private readonly SignInManager<Register_DTO> signInManager_;
        //private readonly TokenService tokenService_;
        //private readonly JWTHelper jwtHelper_;
        //private readonly TEST_JMContext context_;

        private readonly AuthService authService_;

        public AuthController(AuthService authService)
        {
            authService_ = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register_request)
        {
            try
            {
                var user = await authService_.Register(register_request);
                return Ok(user);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Register login_request)
        {
            var token = await authService_.Login(login_request);
            if(token == null)
                return Unauthorized();

            return Ok(token);
        }


        //public AuthController(UserManager<Register_DTO> userManager, SignInManager<Register_DTO> signInManager, TokenService tokenservice, JWTHelper jwtHelper, TEST_JMContext contexts)
        //{
        //     userManager_ = userManager;
        //     signInManager_ = signInManager;
        //     jwtHelper_ = jwtHelper;
        //     context_ = contexts;
        //    tokenService_ = tokenservice;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register([FromBody] Register register)
        //{
        //    var user = new Register_DTO { UserName = register.Name, Email = register.UserName };
        //    var result = await userManager_.CreateAsync(user, register.Password);

        //    if (result.Succeeded)
        //        return Ok(new { message = "User registered successfully" });

        //    return BadRequest(result.Errors);
        //}


        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] Login login)
        //{
        //    var user = await userManager_.FindByEmailAsync(login.UserName);
        //    if (user == null) return Unauthorized();

        //    var result = await signInManager_.CheckPasswordSignInAsync(user, login.Password, false);
        //    if (!result.Succeeded) return Unauthorized();

        //    var token = tokenService_.GenerateToken(user);
        //    return Ok(new { token });
        //}
    }
}
