using System;

namespace systicket.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public string key { get; set; }
        public int personId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
