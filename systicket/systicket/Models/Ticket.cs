using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using systicket.Controllers;

namespace systicket.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public string Tela { get; set; }
        public string Cliente { get; set; }
        public string Arquivo { get; set; }
        public int TipoId { get; set; }
        public int TotalRecord { get; set; }
        public string Tipo { get; set; }
        public string Cor { get; set; }
        public int PrioridadeId { get; set; }
        public string Prioridade { get; set; }
        public string DataSolicitacao { get; set; }
        public bool Atendido { get; set; }
        public string Validation { get; set; }
        public IList<string> FileBase64 { get; set; }

        private readonly IConfiguration configuration;

        private TicketListing listing;

        public Ticket(){}
        public Ticket(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<Ticket> TicketGet(TicketListing listing, int skip, int pageSize)
        {
            this.listing = listing;
            int totalRecord = 0;
            Conexao oConex = new Conexao(configuration);

            string Filter = !string.IsNullOrWhiteSpace(listing.Search.ToString()) ? "and tp.FullName like '%" + listing.Search + "%' or tt.Topic like '%" + listing.Search + "%' or pt.priorits like '%" + listing.Search + "%' or tt.[Description] like '%" + listing.Search + "%'" : "";

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

                totalRecord = dt.Rows.Count;

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

                        Cor = Cor,

                        TotalRecord = totalRecord
                    });
                   
                }
            }
            catch (Exception)
            {
                throw;
            }

            lstTicket = lstTicket.OrderBy(o => o.Prioridade).ToList();

            if (skip > 0)
                lstTicket = lstTicket.Skip(skip).Take(skip).ToList();
            else
                lstTicket = lstTicket.Take(pageSize).ToList();

            return lstTicket;

        }

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

    }
}
