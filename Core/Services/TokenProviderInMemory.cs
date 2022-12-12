using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TokenProviderInMemory : ITokenProvider
    {
        private static string token = "";
        public Task<string> GetTokenAsync()
        {
            return Task.FromResult(token);
        }

        public Task SaveToken(string token)
        {
            TokenProviderInMemory.token = token;
            return Task.CompletedTask;
        }
    }
}
