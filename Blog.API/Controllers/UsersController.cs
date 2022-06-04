using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Blog.API.Email;
using Blog.API.Helpers;
using Blog.EntityFrameworkCore.Identity;
using Blog.Shared.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppSettings _appSettings;
        private IPasswordHasher<AppUser> _passwordHasher;
        private ISendMailService _sendMailservice;

        public UsersController(UserManager<AppUser> userManager,
           IOptions<AppSettings> appSettings,
           IPasswordHasher<AppUser> passwordHash,
           ISendMailService sendMailservice
           )
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _passwordHasher = passwordHash;
            _sendMailservice = sendMailservice;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState,"Đăng ký không thành công"));
            }

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FullName= request.FullName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                // Sending Confirmation Email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Users", new { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);

                //await _emailsender.SendEmailAsync(user.Email, "Confirm Your Email", "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>");
                MailContent content = new MailContent
                {
                    To = user.Email,
                    Subject = "Confirm Your Email",
                    Body = "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>"
                };

                await _sendMailservice.SendMail(content);
                return Ok(new { username = user.UserName, FullName=user.FullName,email = user.Email, status = 1, message = "Đăng ký tài khoản thành công, vui lòng xác nhận Email" });

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(new ApiBadRequestResponse(ModelState, "Đăng ký không thành công"));

        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
        {
            // Get the User from Database
            var user = await _userManager.FindByNameAsync(request.Username);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {

                // THen Check If Email Is confirmed
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản chưa xác thực");
                    return Unauthorized(new ApiUnauthorizedResponse(ModelState, "Đăng nhập không thành công"));
                }

                // get user Role
                var roles = await _userManager.GetRolesAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim(ClaimTypes.Name,user.FullName),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                     }),

                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Site,
                    Audience = _appSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
                };

                // Generate Token
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new {
                    token = tokenHandler.WriteToken(token),
                    UserId=user.Id,
                    Expiration = token.ValidTo,
                    Username = user.UserName,
                    Email=user.Email,
                    FullName=user.FullName,
                    UserRole = roles.FirstOrDefault() 
                });

            }

            // return error
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
            return Unauthorized(new ApiUnauthorizedResponse(ModelState, "Đăng nhập không thành công"));

        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id và Code yêu cầu");
                return BadRequest(new ApiBadRequestResponse(ModelState, "Xác nhận không thành công"));

            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError("", $"Không tìm thấy tài khoản Id: {userId}");
                return BadRequest(new ApiBadRequestResponse(ModelState, "Xác nhận không thành công"));
            }

            if (user.EmailConfirmed)
            {
                return Redirect(_appSettings.ReturnUrl);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Redirect(_appSettings.ReturnUrl);
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.ToString());
                }
                return new JsonResult(errors);
            }
        }

        [HttpGet("paging")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllPaging(string keyword, int page, int pageSize = 10)
        {
            var users = await _userManager.Users.ToListAsync();
            var responseData = users.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                Items = responseData,
                Page = page,
                TotalItems = users.Count(),
                PageSize = pageSize
            });

        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetById(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var detail = new UserDto();
            detail.UserName = user.UserName;
            detail.Email = user.Email;
            detail.FullName = user.FullName;
            detail.PhoneNumber = user.PhoneNumber;
            detail.Role = roles.FirstOrDefault();
            return Ok(detail);
        }

        [HttpPost("create")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Đăng ký không thành công"));
            }

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FullName = request.FullName,
                EmailConfirmed=true,
                PhoneNumber=request.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, request.Role);
                return Ok(new { username = user.UserName, FullName = user.FullName, email = user.Email, status = 1, message = "Thêm mới người dùng thành công"});

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(new ApiBadRequestResponse(ModelState, "Thêm mới người dùng không thành công"));

        }

        [HttpPut("{userId}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Update([FromRoute] string userId, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Cập nhật không thành công"));
            }

            AppUser user =await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy người dùng Id: {userId}"));

            user.Email = request.Email;
            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            }
            var result=await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await _userManager.AddToRoleAsync(user, request.Role);
                return Ok(new { username = user.UserName, FullName = user.FullName, email = user.Email, status = 1, message = "Cập nhật người dùng thành công" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(new ApiBadRequestResponse(ModelState, "Cập nhật người dùng không thành công"));

        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    return Ok();
                }
                else
                    return BadRequest(new ApiBadRequestResponse(ModelState, "Xóa người dùng không thành công"));
            }
            else
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy người dùng Id: {userId}"));
            }              
        }

    }
}
