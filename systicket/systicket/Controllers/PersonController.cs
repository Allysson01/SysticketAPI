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
    public class PersonController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PersonController(IConfiguration config)
        {
            this.configuration = config;
        }

        #region  Chamados Post        
        [Route("api/[controller]/personcad")]
        [EnableCors("postSysticket")]
        [HttpPost]
        public ValidaTicket PersonRegistration(Person people)
        {
            ValidationPassWord oValidation = new ValidationPassWord();

            ValidaTicket oValidaTicket = new ValidaTicket();

            string Qry = string.Format(@"SELECT [AccessKey], [dtAcess] FROM [SysticketDb].[dbo].[Users] WHERE [personId]= {0}", people.PersonId);
            
            Dictionary<object, object> dtnParamns = new Dictionary<object, object>();
            DataTable dt = new DataTable();
            Conexao oConex = new Conexao(configuration);
            try
            {
                dt = oConex.Get(Qry, dtnParamns, CommandType.Text);
                DateTime dataKey = Convert.ToDateTime(dt.Rows[0][1]);

                TimeSpan date = dataKey - DateTime.Now;

                if (dt.Rows.Count == 0 || date.Days > 0)
                {
                    oValidaTicket.isValidDate = false;
                    oValidaTicket.Message = date.Days > 0 ? "Token expirado! Redirecionando ao login;" : dt.Rows.Count == 0 ? "Token não cadastrado, Redirecionando ao login" : "";
                    return oValidaTicket;
                }

                string pass = dt.Rows[0][0].ToString();

                if (date.Days == 0)
                {
                    if (oValidation.Validation(people.Validation, pass))
                    {                        
                        dt.Clear();
                        string proc = "[dbo].[PersonResgistration]";

                        dtnParamns.Add("NAME", people.Name);
                        dtnParamns.Add("CPF", people.CPF);
                        dtnParamns.Add("EMAIL", people.Email);
                        dtnParamns.Add("ISMANAGER", people.isManager);

                        dt = oConex.Post(proc, dtnParamns, CommandType.StoredProcedure);

                        int idPeople = Convert.ToInt32(dt.Rows[0][0]);

                        dt.Clear();

                        oValidaTicket.isValidDate = idPeople > 0;

                        pass = oValidation.Validation(people.Password);

                        dtnParamns.Clear();
                        if (oValidaTicket.isValidDate)
                        {
                            proc = "[dbo].[LoginResgistration]";
                            dtnParamns.Add("PERSONID", idPeople);
                            dtnParamns.Add("PASSWORD", pass);

                            dt = oConex.Post(proc, dtnParamns, CommandType.StoredProcedure);
                        }
                    }
                }               
            }
            catch (Exception)
            { }

            return oValidaTicket;
        }
        #endregion

        #region retorno de pessoas
        [Route("api/[controller]/getpeopleall")]
        [EnableCors("getSysticket")]
        [HttpGet]
        public List<Person> GetPeopleAll()
        {
            List<Person> lstPeople = new List<Person>();

            Conexao oConex = new Conexao(configuration);

            string Qry = "SELECT [Id],[FullName],[CPF],[Email],[isManager] FROM [dbo].[Person]";

            Dictionary<object, object> dtnParamns = new Dictionary<object, object>();

            DataTable dt = oConex.Get(Qry, dtnParamns, CommandType.Text);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var inRow = dt.Rows[i];

                lstPeople.Add(new Person
                {
                    Id = Convert.ToInt32(inRow["Id"]),

                    Name = inRow["FullName"].ToString(),

                    CPF = inRow["CPF"].ToString(),

                    Email = inRow["Email"].ToString(),                    

                    isManager = Convert.ToBoolean(inRow["isManager"]),
                   
                });
            }

            return lstPeople;
        }
        #endregion

        #region retorno UserPage
        [Route("api/[controller]/getuserpage")]
        [EnableCors("postSysticket")]
        [HttpPost]
        public Person GetUserPage(Person person)
        {
            Person oPeople = new Person();

            Conexao oConex = new Conexao(configuration);

            string Qry = String.Format(@"SELECT [Id],[FullName],[CPF],[Email],[isManager] FROM [dbo].[Person] WHERE [Id] = {0}", person.Id);

            Dictionary<object, object> dtnParamns = new Dictionary<object, object>();

            DataTable dt = oConex.Get(Qry, dtnParamns, CommandType.Text);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var inRow = dt.Rows[i];

                oPeople.Id = Convert.ToInt32(inRow["Id"]);

                oPeople.Name = inRow["FullName"].ToString();

                oPeople.CPF = inRow["CPF"].ToString();

                oPeople.Email = inRow["Email"].ToString();

                oPeople.isManager = Convert.ToBoolean(inRow["isManager"]);

            }

            return oPeople;
        }
            #endregion
        }
}
