using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class HospitalDb : DbContext
    {
        public DbSet<Referenciacao> Dados { get; set; }
        public DbSet<Utente> uts { get; set; }
        public DbSet<Enfermeiros> enfs { get; set; }
        public DbSet<Medicos> meds { get; set; }

        public DbSet<AssistentesSociais> assis { get; set; }

        public DbSet<Tokens> tokens { get; set; }
    }
}