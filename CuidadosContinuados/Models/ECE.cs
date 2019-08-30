using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class ECE
    {
        [DisplayName("Confusão:")]
        public bool Confusao { get; set; }
        [DisplayName("Desorientação:")]
        public bool Desorientacao { get; set; }
        [DisplayName("Demência:")]
        public bool Demencia { get; set; }
        [DisplayName("Afasia:")]
        public bool Afasia { get; set; }
        [DisplayName("Disartria:")]
        public bool Disartria { get; set; }
        [DisplayName("Inconsciente:")]
        public bool Inconsciente { get; set; }

    }
}