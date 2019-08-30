using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class Utente
    {
        public int Id { get; set; }
        [Required]
        public string HSE { get; set; }
        public string Name { get; set; }
        public string Morada { get; set; }
        public string Freguesia { get; set; }
        public string NumeroTelefone { get; set; }
        public DateTime Nascimento { get; set; }
        public string EstadoCivil { get; set; }
        public string Escolaridade { get; set; }
        public string Beneficiario { get; set; }
    }
}