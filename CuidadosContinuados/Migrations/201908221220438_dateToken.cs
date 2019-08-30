namespace CuidadosContinuados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tokens", "crt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tokens", "crt");
        }
    }
}
