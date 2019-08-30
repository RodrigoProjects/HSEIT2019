using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class Medicos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

}
