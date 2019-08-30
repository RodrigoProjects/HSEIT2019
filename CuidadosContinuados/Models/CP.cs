using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class CP
    {
        [DisplayName("Controlo de Sintomas:")]
        public bool ControloDeSintomas { get; set; }
        [DisplayName("Processo de Luto:")]
        public bool ProcessoDeLuto { get; set; }
        [DisplayName("Outras:")]
        public string Outras { get; set; }
    }
}