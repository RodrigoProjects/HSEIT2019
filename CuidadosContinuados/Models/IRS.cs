using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class IRS
    {
        [DisplayName("Isolamento Social/Geográfico:")]
        public bool Isolamento { get; set; }
        [DisplayName("Ausência de Suporte Familiar:")]
        public bool ASF { get; set; }
        [DisplayName("História de Conflituidade Familiar/Rutura Familiar:")]
        public bool Conflitualidade { get; set; }
        [DisplayName("Antecedentes Pessoais/Familiares de Violência:")]
        public bool AntecedentesViolencia { get; set; }
        [DisplayName("Suspeita de Maus Tratos::")]
        public bool MausTratos { get; set; }
        [DisplayName("Negligência na Prestação de Cuidados:")]
        public bool Negligencia { get; set; }
        [DisplayName("Dependência do idoso na sua gestão económica/financeira e de bens:")]
        public bool DependenciaEconomica { get; set; }
        [DisplayName("Baixos Rendimentos:")]
        public bool BaixosRendimentos { get; set; }
        [DisplayName("Habitação Degradada:")]
        public bool HabitacaoDegradada { get; set; }
        [DisplayName("Condições de Salubridade Precárias:")]
        public bool SalubridadePrecaria { get; set; }
        [DisplayName("Coabitação/Sobrelotação Habitacional:")]
        public bool CoabitacaoHabitacional { get; set; }
        [DisplayName("Barreiras Arquitectónicas:")]
        public bool BarreirasArquitetonicas { get; set; }
        [DisplayName("Outras:")]
        public string Outras { get; set; }
    }
}