using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeetIQ.Infrastructure.Services
{
    public class JitsiTokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JitsiTokenService> _logger;

        public JitsiTokenService(IConfiguration config, ILogger<JitsiTokenService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public string GenerateToken(
            string roomName,
            string userId,
            string displayName,
            bool isModerator)
        {
            var secret = _config["Jitsi:AppSecret"] ?? throw new InvalidOperationException("Jitsi:AppSecret not configured");
            var appId = _config["Jitsi:AppId"] ?? throw new InvalidOperationException("Jitsi:AppId not configured");
            var domain = _config["Jitsi:Domain"] ?? "meet.jit.si";
            var expMins = _config.GetValue<int>("Jitsi:TokenExpiryMinutes", 120);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;

            // Jitsi expects a specific context claim structure
            var context = new
            {
                user = new
                {
                    id = userId,
                    name = displayName,
                    moderator = isModerator.ToString().ToLower(),
                },
                features = new
                {
                    livestreaming = false,
                    outbound_call = false,
                    transcription = false,
                    recording = false,
                }
            };

            var claims = new[]
            {
                new Claim("iss",     appId),
                new Claim("sub",     domain),
                new Claim("aud",     "jitsi"),
                new Claim("room",    roomName),
                new Claim("context", System.Text.Json.JsonSerializer.Serialize(context)),
            };

            var token = new JwtSecurityToken(
                issuer: appId,
                audience: "jitsi",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(expMins),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogDebug("Generated Jitsi token for room {Room}, user {User}", roomName, userId);
            return tokenString;
        }
    }
}
