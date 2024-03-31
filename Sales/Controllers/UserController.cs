using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Sale.Domain.Entities;
using Sale.Service.Constant;
using Sale.Service.Dtos;
using Sale.Service.Dtos.UserDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> _logger;

		private readonly UserManager<AppUser> _user;

		private readonly RoleManager<AppRole> _roleManager;


		private readonly IConfiguration _configuration;

		public UserController(ILogger<UserController> logger, UserManager<AppUser> user, RoleManager<AppRole> roleManager, IConfiguration configuration)
		{
			_user = user;
			_logger = logger;
			_roleManager = roleManager;
			_configuration = configuration;
		}



		/// <summary>
		/// REGISTER
		/// </summary>
		/// <param name="register"></param>
		/// <returns></returns>
		[HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {

            try
            {
				var user = await _user.FindByNameAsync(register.Username);
				if(user != null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError,
					 new ResponseWithMessageDto { Status = "Error", Message = "Tài khoản đã tồn tại." });
				}
				var emailCheckExits = _user.FindByEmailAsync(register.Email);
				if(emailCheckExits != null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError,
				 new ResponseWithMessageDto { Status = "Error", Message = "Email đã tồn tại." });
				}




				AppUser appUser = new AppUser()
				{
					UserName = register.Username,
					Email = register.Email,
					FullName = register.FullName,
					PhoneNumber = register.PhoneNumber,
					SecurityStamp = Guid.NewGuid().ToString()
				};

				var success = await _user.CreateAsync(appUser,  register.Password);
				if(success.Succeeded)
				{
					return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<dynamic>
					{
						Status = StatusConstant.SUCCESS,
						Data = new
						{
							UserName = register.Username,
							Email = register.Email,
							FullName = register.FullName,
							PhoneNumber = register.PhoneNumber,
						},
						Message = "Đăng ký thành công"
					});
				}
				else
				{
					return StatusCode(StatusCodes.Status500InternalServerError,
				   new ResponseWithMessageDto { Status = "Error", Message = "Đăng ký thất bại" });
				}
				
            }
            catch (Exception ex)
            {
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
				{
					Status = StatusConstant.ERROR,
					Message = ex.Message
				});
            }


        }





		public async Task<IActionResult> Login([FromBody] LoginDto login)
		{

			try
			{

				var user = await _user.FindByNameAsync(login.UserName);
				if (user == null)
				{
					return StatusCode(StatusCodes.Status200OK, new ResponseWithMessageDto
					{
						Status = StatusConstant.ERROR,
						Message = "Tên đăng nhập không tồn tại"
					});
					
				} else
				{
					var isCheck = await _user.CheckPasswordAsync(user, login.Password);
					if(!isCheck)
					{
						return StatusCode(StatusCodes.Status200OK, new ResponseWithMessageDto
						{
							Status = StatusConstant.ERROR,
							Message = "Mật khẩu không chính xác"
						});
					}
					else
					{

						var role = await _user.GetRolesAsync(user);


						var authClaim = new List<Claim> {
							new Claim(ClaimTypes.Name, user.UserName),
							new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
						};



						foreach (var userRole in role)
						{
							var roleItem = await _roleManager.FindByIdAsync(userRole);
							if(role != null)
							{
								authClaim.Add(new Claim(ClaimTypes.Role, userRole));
								var roleName = await _roleManager.GetClaimsAsync(roleItem);
								if(roleName != null)
								{
									foreach (var roleClaim in roleName)
									{
										authClaim.Add(roleClaim);
									}
								}
							}
						}


						var authSignInkey = new SymmetricSecurityKey(
								Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
							);

						//Convert time
						TimeZone time = TimeZone.CurrentTimeZone;
						DateTime dateNow = time.ToUniversalTime(DateTime.Now);
						var getSeTime = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
						var seTime = TimeZoneInfo.ConvertTimeFromUtc(dateNow, getSeTime);

						var token = new JwtSecurityToken(
							issuer: _configuration["JWT:ValidIssuer"],
							audience: _configuration[""],
							expires: seTime.AddHours(3),
							claims: authClaim,
							signingCredentials: new SigningCredentials(authSignInkey, SecurityAlgorithms.HmacSha256 )
							);

						return StatusCode(StatusCodes.Status200OK, new ResponseWithDataDto<dynamic>
						{
							Status = StatusConstant.SUCCESS,
							Data = new
							{
								UserId = user.Id,
								UserName = user.UserName,
								Email = user.Email,
								FullName = user.FullName,
								PhoneNumber = user.PhoneNumber,
								accessTken = new JwtSecurityTokenHandler().WriteToken(token),
								expired = token.ValidTo
							},
							
							Message = "Đăng ký thành công"
						});
					}
					
				}


			
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseWithMessageDto
				{
					Status = StatusConstant.SUCCESS,
					Message = ex.Message
				});
			}


		}



	}
}
