namespace CuidadosContinuados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IRS : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Referenciacaos", "IRS_Outras", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Referenciacaos", "IRS_Outras", c => c.Boolean(nullable: false));
        }
    }
}
