using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class MedicosViewPage
    {
        public IList<Utente> utentes { get; set; }
        public IList<Referenciacao> refeDone { get; set; }
        public IList<Referenciacao> refeNot { get; set; }
        public IList<Medicos> meds { get; set; }
        public IList<Enfermeiros> enfe { get; set; }
    }
}