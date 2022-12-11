using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAuthService
    {
        public Task<PersonDto?> Login(LoginModel loginModel);
        public Task<PersonDto?> Register(RegisterModel registerModel); 
    }
}
