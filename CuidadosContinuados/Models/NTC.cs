using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class NTC
    {
        [DisplayName("úlceras Por Pressão Múltiplas:")]
        public bool UlcerasPorPressaoMultiplas { get; set; }
        [DisplayName("Portadores de SNG/PEG:")]
        public bool PortadoresDeSNG_PEG { get; set; }
        [DisplayName("Ventilação Assistida:")]
        public bool VentilacaoAssistida { get; set; }
        [DisplayName("Outras:")]
        public string Outras { get; set; }
    }
}