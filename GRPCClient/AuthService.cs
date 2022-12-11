using Core.Contracts;
using Core.Services;
using Grpc.Core.Interceptors;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcService.Protos;

namespace GRPCClient
{
    public class AuthService : IAuthService
    {
        private readonly Auth.AuthClient auth;
        private readonly ITokenProvider tokenProvider;

        public AuthService(Auth.AuthClient auth,ITokenProvider tokenProvider)
        {
            this.auth = auth;
            this.tokenProvider = tokenProvider;
        }
        public async Task<PersonDto?> Login(LoginModel loginModel)
        {
            var call = auth.LoginAsync(new LoginRequest
            {
                Name = loginModel.Name,
                Password = loginModel.Password
            });
            if(!await SaveJWTToken(call) )
            {
                return default;
            }
            var person = await call.ResponseAsync;
            return new PersonDto 
            {
                Name = person.Name,
                RoleName = person.RoleName
            };

        }

        public async Task<PersonDto?> Register(RegisterModel registerModel)
        {
            var call = auth.RegisterAsync(new RegisterRequest
            {
                Name = registerModel.Name,
                Password = registerModel.Password,
                RoleName = registerModel.Role
            });
            if (!await SaveJWTToken(call))
            {
                return default;
            }
            var person = await call.ResponseAsync;
            return new PersonDto
            {
                Name = person.Name,
                RoleName = person.RoleName
            };
        }
        private async Task<bool> SaveJWTToken(AsyncUnaryCall<Person> call)
        {
            Metadata headers = await call.ResponseHeadersAsync;
            var authHeader = headers.GetValue("Authorization");
            if (authHeader is null)
                return false;
            // структура хедера такая: Bearer {(тип токена).(информация о пользователе).(шифрование для проверки подлинности)}
            var token = authHeader.Split(" ").Last();
            await tokenProvider.SaveToken(token);
            return true;
        }
    }
}
