namespace UET_BTL.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add2 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ControllerActions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ControllerActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
