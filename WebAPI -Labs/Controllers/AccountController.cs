using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Labs.DTOs;
using WebAPI_Labs.Model;

namespace WebAPI_Labs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager
                                     , IConfiguration config, IMapper mapper)
        {
            this.userManager = userManager;
            this.config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto userFromRequest)
        {
            if (ModelState.IsValid)
            {
                var userModel = _mapper.Map<ApplicationUser>(userFromRequest);

                IdentityResult result = await userManager.CreateAsync(userModel, userFromRequest.Password);

                if (result.Succeeded)
                {
                    return Ok("Account Created Success");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userFromRequest)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser userModel =
                            await userManager.FindByNameAsync(userFromRequest.UserName);
                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, userFromRequest.Password);
                    if (found == true)
                    {

                        List<Claim> claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.NameIdentifier, userModel.Id));
                        claims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var Roles = await userManager.GetRolesAsync(userModel);
                        if (Roles != null)
                        {
                            foreach (var RoleName in Roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, RoleName));
                            }
                        }

                        string key = config["JWT:Key"];
                        var Secritkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                        SigningCredentials signingCredentials =
                            new SigningCredentials(Secritkey, SecurityAlgorithms.HmacSha256);


                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:Issuer"],
                            audience: config["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            claims: claims,
                            signingCredentials: signingCredentials);

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expires = DateTime.Now.AddHours(1)
                        });

                    }
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }
    }
}