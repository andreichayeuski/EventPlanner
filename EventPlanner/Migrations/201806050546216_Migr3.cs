namespace EventPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Events", new[] { "UserCreator_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            RenameColumn(table: "dbo.Events", name: "ApplicationUser_Id", newName: "UserCreator_Id");
            CreateTable(
                "dbo.EventUserRelationships",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Events", "UserId", c => c.String());
            AlterColumn("dbo.Events", "UserCreator_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Events", "UserCreator_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Events", "UserCreator_Id");
            DropColumn("dbo.Events", "ApplicationUser_Id1");
            DropColumn("dbo.AspNetUsers", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Event_Id", c => c.Int());
            AddColumn("dbo.Events", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            DropForeignKey("dbo.EventUserRelationships", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventUserRelationships", "EventId", "dbo.Events");
            DropIndex("dbo.EventUserRelationships", new[] { "UserId" });
            DropIndex("dbo.EventUserRelationships", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "UserCreator_Id" });
            AlterColumn("dbo.Events", "UserCreator_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Events", "UserCreator_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Events", "UserId");
            DropTable("dbo.EventUserRelationships");
            RenameColumn(table: "dbo.Events", name: "UserCreator_Id", newName: "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Event_Id");
            CreateIndex("dbo.Events", "UserCreator_Id");
            CreateIndex("dbo.Events", "ApplicationUser_Id1");
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events", "Id");
            AddForeignKey("dbo.Events", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
        }
    }
}
