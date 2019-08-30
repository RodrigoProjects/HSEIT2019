using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class CuidadorDetalhes
    {
        [DisplayName("Sobrecarga física/emocional do Cuidador:")]
        public bool Sobrecarga { get; set; }
        [DisplayName("Doença do Cuidador Principal:")]
        public bool DCP { get; set; }
        [DisplayName("Ausência de Suporte Familiar:")]
        public bool ASF { get; set; }
        [DisplayName("Cuidador com Idade Avançada:")]
        public bool CuidadorIdadeAvancada { get; set; }
    }
}