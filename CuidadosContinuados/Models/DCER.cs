using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class DCER
    {
        public bool DPOC { get; set; }
        public bool ICC { get; set; }
        public bool DCV { get; set; }
        
        [DisplayName("Outras:")]
        public string Outras { get; set; }
    }
}