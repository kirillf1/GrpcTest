using Core.Repositories;
using GrpcService.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GrpcService
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,IPersonRepository personRepository)
        {
            var authHeader = context.Request.Headers["Authorization"]
                .FirstOrDefault();

            if (authHeader != null)
            {
                var secret = "secret secret secret secret secret";
                var key = Encoding.UTF8.GetBytes(secret);

                var token = authHeader.Split(" ").Last();

                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };

                tokenHandler.ValidateToken(token,
                    validationParameters,
                    out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var personName = jwtToken.Claims
                    .First(x => x.Type == "Name")
                    .Value;
                var person = await personRepository.GetPersonByName(personName);
                context.Items["Person"] = new PersonContext(person);

            }

            await _next(context);
        }
    }
}
