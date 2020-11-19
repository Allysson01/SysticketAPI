using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using systicket.Models;

namespace systicket.Controllers
{

    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthenController(IConfiguration config)
        {
            this.configuration = config;
        }

        #region Login
        [EnableCors("postSysticket")]
        [Route("api/[controller]/login")]
        [HttpPost]
        public Authentication Login(Login login)
        {
            Authentication oAuth = new Authentication();

            ValidationPassWord oValidation = new ValidationPassWord(login);

            Conexao oConex = new Conexao(configuration);

            string proc = "[dbo].[ReturnLogin]";
            Dictionary<object, object> dtnParamns = new Dictionary<object, object>
            {
                { "Email", login.Email }
            };

            try
            {
                DataTable dt = oConex.Get(proc, dtnParamns, CommandType.StoredProcedure);

                if (dt.Rows.Count > 0 && oValidation.ValidationPassword(login, dt.Rows[0][3].ToString()))
                {
                    #region Geração de key
                    Random rnd = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var stringChars = new char[12];
                    var random = new Random();

                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    dtnParamns = new Dictionary<object, object>();
                    string key = new String(stringChars);
                    string keyDb = oValidation.ValidationKey(key);
                    string Qry = string.Format(@"SELECT [AccessKey] FROM [SysticketDb].[dbo].[Users] WHERE personId = {0}", Convert.ToInt32(dt.Rows[0][0]));
                    var Quant = oConex.Get(Qry, dtnParamns, CommandType.Text);

                    if (Quant.Rows.Count == 1)
                        Qry = string.Format(@"UPDATE dbo.Users SET [AccessKey] = '{0}',[dtAcess] = '{2}' WHERE [personId] = {1}", keyDb, Convert.ToInt32(dt.Rows[0][0]), DateTime.Now);
                    else
                        Qry = string.Format(@"INSERT INTO dbo.Users (AccessKey, personId, dtAcess) VALUES ('{0}', {1}, '{2}')", keyDb, Convert.ToInt32(dt.Rows[0][0]), DateTime.Now);

                    oConex.Post(Qry, dtnParamns, CommandType.Text);
                    #endregion

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow inRow = dt.Rows[i];
                        string[] partsName = inRow["FullName"].ToString().Split(" ");
                        var sName = partsName[1].Length > 3 ? partsName[1] : partsName[2];

                        oAuth.Id = Convert.ToInt32(inRow["Id"]);
                        oAuth.Name = partsName[0].ToString() + " " + sName;
                        oAuth.isManager = Convert.ToBoolean(inRow["isManager"]);
                        oAuth.Validation = oAuth.Id > 0 && oAuth.Name != null;
                        oAuth.key = key;
                    }

                }
                else
                {
                    oAuth.Validation = false;
                }
            }
            catch (Exception)
            {
                throw;
            }

            #region Teste inicialização com JWT 
            //const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            //var token = new JwtBuilder()
            //     .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
            //     .WithSecret(secret)
            //     .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            //     .AddClaim("sub", "claim1-value")
            //     .AddClaim("iss", "claim2-value")
            //     .AddClaim("aud", "claim3-value")
            //     .Encode();

            //var pp = token;
            //#endregion
            //#region parsing token
            //try
            //{
            //    var json = new JwtBuilder()
            //        .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
            //              .WithSecret(secret)
            //              .MustVerifySignature()
            //              .Decode(token);
            //    var tt = json;
            //}
            //catch (TokenExpiredException)
            //{
            //    throw;
            //}
            //catch (SignatureVerificationException)
            //{
            //    throw;
            //}

            #endregion



            return oAuth;
        }
        #endregion
    }
}
