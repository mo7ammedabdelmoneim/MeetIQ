using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MeetIQ.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MeetIQ.Infrastructure.Services
{
    public class JitsiTokenService : IJitsiTokenService
    {
        private readonly IConfiguration config;

        public JitsiTokenService(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(
            string roomId,
            string userId,
            string userName,
            string userEmail,
            bool isModerator)
        {
            var appId = config["Jitsi:AppId"]!;
            var secret = config["Jitsi:AppSecret"]!;
            var domain = config["Jitsi:Domain"]!;          // e.g. meet.jit.si

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var context = new
            {
                user = new
                {
                    id = userId,
                    name = userName,
                    email = userEmail,
                    moderator = isModerator
                },
                features = new
                {
                    recording = isModerator,
                    livestreaming = false,
                    outbound_call = false
                }
            };

            var claims = new[]
            {
                new Claim("sub", domain),
                new Claim("iss", appId),
                new Claim("aud", appId),
                new Claim("room", roomId),
                new Claim("context", System.Text.Json.JsonSerializer.Serialize(context))
            };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
