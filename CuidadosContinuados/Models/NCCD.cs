using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class NCCD
    {
        [DisplayName("Tratamento de Feridas:")]
        public bool TratamentoDeFeridas { get; set; }
        [DisplayName("Reabilitação:")]
        public bool Reabilitacao { get; set; }
        [DisplayName("Manutenção de Dispositivos:")]
        public bool ManutencaoDeDispositivos { get; set; }
        [DisplayName("Gestão de Regime Terapêutico:")]
        public bool GestaoDeRegimeTerapeutico { get; set; }
        [DisplayName("Outras:")]
        public string Outras { get; set; }

    }
}