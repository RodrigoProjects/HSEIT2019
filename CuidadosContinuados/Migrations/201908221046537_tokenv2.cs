namespace CuidadosContinuados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tokenv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tokens", "Token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tokens", "Token");
        }
    }
}
