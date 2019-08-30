namespace CuidadosContinuados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class medico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Referenciacaos", "Medico", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Referenciacaos", "Medico");
        }
    }
}
