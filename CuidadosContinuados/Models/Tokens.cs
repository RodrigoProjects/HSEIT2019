using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class Tokens
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public DateTime crt { get; set; }
    }
}