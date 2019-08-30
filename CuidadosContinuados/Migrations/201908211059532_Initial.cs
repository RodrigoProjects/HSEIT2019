namespace CuidadosContinuados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssistentesSociais",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Referenciacaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UtenteId = c.Int(nullable: false),
                        Cuidador_Name = c.String(),
                        Cuidador_Morada = c.String(),
                        Cuidador_NumeroTelefone = c.String(),
                        Cuidador_Nascimento = c.DateTime(nullable: false),
                        Cuidador_EstadoCivil = c.String(),
                        Cuidador_GrauDeParentesco = c.String(),
                        EntidadeReferenciadora = c.String(),
                        DiagnosticoClinico = c.String(),
                        DataDeAlta = c.DateTime(nullable: false),
                        CriteriosDeTriagem = c.String(),
                        DependenciaAVD = c.Boolean(nullable: false),
                        Desnutricao = c.Boolean(nullable: false),
                        Deteorioracao = c.Boolean(nullable: false),
                        ProblemasSensoriais = c.Boolean(nullable: false),
                        DCronicas_DPOC = c.Boolean(nullable: false),
                        DCronicas_ICC = c.Boolean(nullable: false),
                        DCronicas_DCV = c.Boolean(nullable: false),
                        DCronicas_Outras = c.String(),
                        NCCD_TratamentoDeFeridas = c.Boolean(nullable: false),
                        NCCD_Reabilitacao = c.Boolean(nullable: false),
                        NCCD_ManutencaoDeDispositivos = c.Boolean(nullable: false),
                        NCCD_GestaoDeRegimeTerapeutico = c.Boolean(nullable: false),
                        NCCD_Outras = c.String(),
                        NTC_UlcerasPorPressaoMultiplas = c.Boolean(nullable: false),
                        NTC_PortadoresDeSNG_PEG = c.Boolean(nullable: false),
                        NTC_VentilacaoAssistida = c.Boolean(nullable: false),
                        NTC_Outras = c.String(),
                        CP_ControloDeSintomas = c.Boolean(nullable: false),
                        CP_ProcessoDeLuto = c.Boolean(nullable: false),
                        CP_Outras = c.String(),
                        NE_ExecucaoDeTecnicas = c.Boolean(nullable: false),
                        NE_RegimeTerapeutico = c.Boolean(nullable: false),
                        NE_AutoCuidados = c.Boolean(nullable: false),
                        NE_Outras = c.String(),
                        AND = c.Int(nullable: false),
                        ECE_Confusao = c.Boolean(nullable: false),
                        ECE_Desorientacao = c.Boolean(nullable: false),
                        ECE_Demencia = c.Boolean(nullable: false),
                        ECE_Afasia = c.Boolean(nullable: false),
                        ECE_Disartria = c.Boolean(nullable: false),
                        ECE_Inconsciente = c.Boolean(nullable: false),
                        CuidadorDetalhes_Sobrecarga = c.Boolean(nullable: false),
                        CuidadorDetalhes_DCP = c.Boolean(nullable: false),
                        CuidadorDetalhes_ASF = c.Boolean(nullable: false),
                        CuidadorDetalhes_CuidadorIdadeAvancada = c.Boolean(nullable: false),
                        IRS_Isolamento = c.Boolean(nullable: false),
                        IRS_ASF = c.Boolean(nullable: false),
                        IRS_Conflitualidade = c.Boolean(nullable: false),
                        IRS_AntecedentesViolencia = c.Boolean(nullable: false),
                        IRS_MausTratos = c.Boolean(nullable: false),
                        IRS_Negligencia = c.Boolean(nullable: false),
                        IRS_DependenciaEconomica = c.Boolean(nullable: false),
                        IRS_BaixosRendimentos = c.Boolean(nullable: false),
                        IRS_HabitacaoDegradada = c.Boolean(nullable: false),
                        IRS_SalubridadePrecaria = c.Boolean(nullable: false),
                        IRS_CoabitacaoHabitacional = c.Boolean(nullable: false),
                        IRS_BarreirasArquitetonicas = c.Boolean(nullable: false),
                        IRS_Outras = c.Boolean(nullable: false),
                        Criacao = c.DateTime(nullable: false),
                        Enfermeiro = c.Int(nullable: false),
                        MedicoOk = c.Boolean(nullable: false),
                        EnfermeiroOk = c.Boolean(nullable: false),
                        AssistOk = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Enfermeiros",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medicos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Utentes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HSE = c.String(nullable: false),
                        Name = c.String(),
                        Morada = c.String(),
                        Freguesia = c.String(),
                        NumeroTelefone = c.String(),
                        Nascimento = c.DateTime(nullable: false),
                        EstadoCivil = c.String(),
                        Escolaridade = c.String(),
                        Beneficiario = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Utentes");
            DropTable("dbo.Medicos");
            DropTable("dbo.Enfermeiros");
            DropTable("dbo.Referenciacaos");
            DropTable("dbo.AssistentesSociais");
        }
    }
}
