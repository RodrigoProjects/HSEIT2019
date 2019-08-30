using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class Cuidador
    {
        [DisplayName("Name:")]
        public string Name { get; set; }
        [DisplayName("Morada:")]
        public string Morada { get; set; }
        [DisplayName("Telefone:")]
        public string NumeroTelefone { get; set; }

        [Display(Name = "Data de Nascimento:"), DataType(DataType.Date)]
        public DateTime Nascimento { get; set; }
        [DisplayName("Estado Civil:")]
        public string EstadoCivil { get; set; }
        [DisplayName("Grau de Parentesco:")]
        public string GrauDeParentesco { get; set; }

    }
}