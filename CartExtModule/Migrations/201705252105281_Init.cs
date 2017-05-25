namespace CartExtModule.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
           
           
          
           
           
            CreateTable(
                "dbo.CartShipmentExtension",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        shipmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CartShipment", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartShipmentExtension", "Id", "dbo.CartShipment");
          
            DropIndex("dbo.CartShipmentExtension", new[] { "Id" });
           
            DropTable("dbo.CartShipmentExtension");
          
        }
    }
}
