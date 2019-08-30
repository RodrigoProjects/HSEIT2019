using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuidadosContinuados.Models
{
    public class Referenciacao
    {
        [Key]
        public int Id { get; set; }
        public int UtenteId { get; set; }
        [DefaultValue("null")]
        [DisplayName("Cuidador:")]
        public Cuidador Cuidador { get; set; }

        [DisplayName("Entidade Referenciadora:")]
        public string EntidadeReferenciadora { get; set; }

        [DisplayName("Diagnóstico Clínico:")]
        public string DiagnosticoClinico { get; set; }

        [Display(Name="Previsão de Alta:"), DataType(DataType.Date)]
        public DateTime DataDeAlta { get; set; }

        [DisplayName("Critérios de Triagem (Para cuidados continuados):")]
        public string CriteriosDeTriagem { get; set; }
        [DisplayName("Dependência nas AVD:")]
        public bool DependenciaAVD { get; set; }
        [DisplayName("Desnutrição:")]
        public bool Desnutricao { get; set; }
        [DisplayName("Deterioração Cognitiva:")]
        public bool Deteorioracao { get; set; }
        [DisplayName("Problemas Sensoriais:")]
        public bool ProblemasSensoriais { get; set; }
        [DisplayName("Doenças Crónicas com episódios de reagudização:")]
        public DCER DCronicas { get; set; }
        [DisplayName("Necessidade de Continuidade de Cuidados no Domicílio:")]
        public NCCD NCCD { get; set; }
        [DisplayName("Necessidade de Tratamentos Complexos:")]
        public NTC NTC { get; set; }
        [DisplayName("Cuidados Paliativos:")]
        public CP CP { get; set; }
        [DisplayName("Necessidades de ensino:")]
        public NE NE { get; set; }
        [DisplayName("Avaliação do nível de Dependência:")]
        public int AND { get; set; }
        [DisplayName("Estado de Consciência e de Expressão:")]
        public ECE ECE { get; set; }
        public CuidadorDetalhes CuidadorDetalhes { get; set; }
        [DisplayName("Indicadores de Risco Social:")]
        public IRS IRS { get; set; }

        [Display(Name = "Data de Criação:"), DataType(DataType.Date)]
        public DateTime Criacao { get; set; }

        [DefaultValue(null)]
        public int Enfermeiro { get; set; }
        public int Medico { get; set; }

        public bool MedicoOk { get; set; }
        public bool EnfermeiroOk { get; set; }
        public bool AssistOk { get; set; }

    }
}