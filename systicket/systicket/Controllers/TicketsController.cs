using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using systicket.Models;

namespace systicket.Controllers
{
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public TicketsController(IConfiguration config)
        {
            this.configuration = config;
        }

        #region retorno de chamados
        [Route("api/[controller]/ticketget")]
        [EnableCors("postSysticket")]
        [HttpPost]
        public List<Ticket> TicketGet(TicketListing listing)
        {
            Conexao oConex = new Conexao(configuration);

            int pageSize = 10;
            int totalRecord = 0;
            if (listing.Page < 1) listing.Page = 1;
            int skip = (listing.Page * pageSize) - pageSize;
             
            string Filter = !string.IsNullOrWhiteSpace(listing.Search.ToString()) ? "and tp.FullName like '%" + listing .Search + "%' or tt.Topic like '%" + listing.Search + "%' or pt.priorits like '%" + listing.Search + "%' or tt.[Description] like '%" + listing.Search + "%'" : "";

            string Query = string.Format(@"
                                            SELECT                                        
                                            tt.Id as Ticket,
                                        	tp.FullName as Tipo,
                                        	tt.Topic as Assunto,
                                        	pt.priorits as Prioridade,
                                        	tt.RequestDate as Data_Solicitacao,
                                        	tt.answered as Atendido,
                                        	tt.[Description] as Descricao
                                        
                                            FROM dbo.Tickets tt
                                        inner join dbo.prioritys pt on tt.Priority = pt.id
                                        inner join dbo.TypeTickets tp on tp.id = tt.TypeTicket
                                        inner join dbo.Person pp on pp.Id = tt.PersonId
                                        where pp.Id = {0} {1}", listing.PersonId, Filter);

            Dictionary<object, object> dtnParamns = new Dictionary<object, object>();          

            List<Ticket> lstTicket = new List<Ticket>();

            try
            {
                DataTable dt = oConex.Get(Query, dtnParamns, CommandType.Text);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var inRow = dt.Rows[i];

                    string Cor = RespColor(inRow[1].ToString());

                    lstTicket.Add(new Ticket
                    {
                        Id = Convert.ToInt32(inRow[0]),

                        Tipo = inRow[1].ToString(),

                        Assunto = inRow[2].ToString(),

                        Prioridade = inRow[3].ToString(),

                        DataSolicitacao = Convert.ToDateTime(inRow[4]).ToString("dd/MM/yyyy"),

                        Atendido = Convert.ToBoolean(inRow[5]),

                        Descricao = inRow[6].ToString(),

                        Cor = Cor
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return lstTicket;
        }
        #endregion
        string RespColor(string tipo)
        {
            string Color = string.Empty;
            switch (tipo)
            {
                case "Correção":
                    Color = "#FF8C00";
                    break;

                case "Implantação":
                    Color = "#00FF00";
                    break;

                case "Carga":
                    Color = "#6A5ACD";
                    break;
            }

            return Color;
        }

        #region  Chamados Post        
        [Route("api/[controller]/ticketpost")]
        [EnableCors("postSysticket")]
        [HttpPost]
        public ValidaTicket TicketPost(Ticket ticket)
        {   

            ValidationPassWord oValidation = new ValidationPassWord();

            ValidaTicket oValidaTicket = new ValidaTicket();

            string Qry = string.Format(@"SELECT [AccessKey], [dtAcess] FROM [SysticketDb].[dbo].[Users] WHERE [personId]= {0}", ticket.PersonId);
            int idTicket = 0;

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
                    if (oValidation.VerificationKey(ticket.Validation, pass))
                    {
                        dt.Clear();
                        string proc = "[dbo].[GeraTicket]";

                        dtnParamns.Add("PERSONID", Convert.ToInt32(ticket.PersonId));
                        dtnParamns.Add("TYPE", ticket.TipoId);
                        dtnParamns.Add("SUBJECT", ticket.Assunto);
                        dtnParamns.Add("PRIORITY", ticket.PrioridadeId);
                        dtnParamns.Add("DESCIPTION", ticket.Descricao);

                        dt = oConex.Post(proc, dtnParamns, CommandType.StoredProcedure);
                        idTicket = Convert.ToInt32(dt.Rows[0][0]);
                        oValidaTicket.isValidDate = idTicket > 0;
                        dtnParamns.Clear();

                        var localizacaoArquivo = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Imgs";
                        bool exists = Directory.Exists(localizacaoArquivo);
                        if (!exists)
                            Directory.CreateDirectory(localizacaoArquivo);

                        int count = 0;
                        foreach (var item in ticket.FileBase64)
                        {
                            int index = item.IndexOf(',');
                            string extension = ticket.TipoId != 3 ? ".jpg" : ".xlsx";
                            string imgPath = localizacaoArquivo + @"\imgem" + "_" + count + "_" + idTicket + extension;
                            var tt = item.Remove(0, index + 1);
                            var bytes = Convert.FromBase64String(tt);
                            using (var imageFile = new FileStream(imgPath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                            }
                            count++;
                            Qry = string.Format(@"INSERT INTO [dbo].[Imagens] ([ImgPath], [dtaInsert], [Id_Ticket]) VALUES ('{0}', '{1}', '{2}')", imgPath, DateTime.Now, idTicket);
                            oConex.Post(Qry, dtnParamns, CommandType.Text);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
         

            return oValidaTicket;

        }
        #endregion

        #region retorno Imagens
        [Route("api/[controller]/ticketimg")]
        [EnableCors("postSysticket")]
        [HttpPost]
        public TicketsImg TicketsImg(TicketsImg SearchImg)
        {
            Conexao oConex = new Conexao(configuration);
            SearchImg.lst64 = new List<string>();
            string Qry = string.Format(@"SELECT [ImgPath] FROM [dbo].[Imagens] WHERE Id_Ticket = {0}", SearchImg.ticketId);

            Dictionary<object, object> dtnParamns = new Dictionary<object, object>();

            DataTable dt = oConex.Get(Qry, dtnParamns, CommandType.Text);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string imgPath = dt.Rows[i][0].ToString();
                byte[] imageArray = System.IO.File.ReadAllBytes(imgPath);
                string str64 = Convert.ToBase64String(imageArray);
                SearchImg.lst64.Add(str64);
            }
                        

            return SearchImg;
        }
        #endregion
    }
}
