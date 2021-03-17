using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using systicket.Models;
using systicket.Services;

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
        [AllowAnonymous]
        public ActionResult Login(Login login)
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
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow inRow = dt.Rows[i];
                        string[] partsName = inRow["FullName"].ToString().Split(" ");
                        var sName = partsName[1].Length > 3 ? partsName[1] : partsName[2];

                        oAuth.Id = Convert.ToInt32(inRow["Id"]);
                        oAuth.Name = partsName[0].ToString() + " " + sName;
                        oAuth.Role = inRow["Role"].ToString();
                        oAuth.Validation = oAuth.Id > 0 && oAuth.Name != null;
                    } 
                }
                else
                {
                    oAuth.Validation = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            #endregion

            #region Inicialização com JWT  

            string token = TokenService.GenerateToken(oAuth);

            #endregion

            return Ok(new
            {
                user = oAuth,
                token
            });
            
        }        
    }
}
