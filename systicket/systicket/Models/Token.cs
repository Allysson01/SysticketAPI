using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace systicket.Models
{
    public class Token :IDisposable
    {
        private const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        private const string cli = "datastack-cli";
        private const string api = "datastack-api";

        public Token(){}

        public void Dispose()
        {
           
        }

        public string GenerationToken(string user)
        {
            #region Teste inicialização com JWT 

            string token = string.Empty;
            string cookie = string.Empty;

            token = new JwtBuilder()
                 .WithAlgorithm(new HMACSHA256Algorithm()) // simétrico
                 .WithSecret(secret)
                 .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds())
                 .AddClaim("subject", user)
                 .AddClaim("issuer", cli)
                 .AddClaim("audience", api)
                 .Encode();

            return token;
            #endregion

        }
    }
}
