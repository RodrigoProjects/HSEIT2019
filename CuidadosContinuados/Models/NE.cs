using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class NE
    {
        [DisplayName("Execução de Técnicas:")]
        public bool ExecucaoDeTecnicas { get; set; }
        [DisplayName("Regime Terapêutico:")]
        public bool RegimeTerapeutico { get; set; }
        [DisplayName("Auto-Cuidados:")]
        public bool AutoCuidados { get; set; }
        [DisplayName("Outras:")]
        public string Outras { get; set; }
    }
}