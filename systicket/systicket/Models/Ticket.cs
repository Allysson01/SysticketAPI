using System.Collections.Generic;

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
        public string Tipo { get; set; }
        public string Cor { get; set; }
        public int PrioridadeId { get; set; }
        public string Prioridade { get; set; }
        public string DataSolicitacao { get; set; }
        public bool Atendido { get; set; }
        public string Validation { get; set; }
        public IList<string> FileBase64 { get; set; }
    }
}
