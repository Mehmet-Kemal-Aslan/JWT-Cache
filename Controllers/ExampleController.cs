using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_Caching.Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace JWT_Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly AppDbContext _db;
        private readonly IDistributedCache _distributedCache;

        public ExampleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMemoryCache memoryCache, IDistributedCache distributedCache, AppDbContext database)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _db = database;
            _distributedCache = distributedCache;
        }

        [HttpGet("GetMessagesCache")]
        public IEnumerable<Messages> GetMessages(int Id)
        {
            Messages[] messages = _db.Messages.Where(x => x.Id == Id).ToArray();

            if (_memoryCache.TryGetValue("usermessages", out messages))
            {
                return messages;
            }

            var messagesByts = _distributedCache.Get("usermessages");
            var messagesJson = Encoding.UTF8.GetString(messagesByts);
            var messagesArr = JsonSerializer.Deserialize<Messages[]>(messagesJson);

            MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
            memoryCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(45);
            memoryCacheEntryOptions.Priority = CacheItemPriority.Normal;


            _memoryCache.Set("usermessages", messages, memoryCacheEntryOptions);

            var jsonMessagesArr = JsonSerializer.Serialize(messages);
            _distributedCache.Set("userposts", Encoding.UTF8.GetBytes(jsonMessagesArr));

            return messages;
        }


            [HttpPost]
        public async Task<IActionResult> Login([FromBody] AppUser userLogin)
        {
            List<Claim> claims = new List<Claim>();
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null) throw new Exception("Wrong mail!");

            var result = await _userManager.CheckPasswordAsync(user, userLogin.PasswordHash);
            if (result)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                claims.Add(new Claim(ClaimTypes.Name, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                var token = GetToken(claims);

                var handler = new JwtSecurityTokenHandler();
                string jwt = handler.WriteToken(token);

                return Ok(new
                {
                    token = jwt,
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                 signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                 issuer: _configuration["JWT:Issuer"],
                 audience: _configuration["JWT:Audience"],
                 expires: DateTime.Now.AddDays(1),
                 claims: claims
                );

            return token;
        }
    }
}
