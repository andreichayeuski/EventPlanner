namespace EventPlanner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventUserRelationships", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventUserRelationships", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.EventUserRelationships", new[] { "EventId" });
            DropIndex("dbo.EventUserRelationships", new[] { "UserId" });
            AddColumn("dbo.Events", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Events", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Events", "UserCreator_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Event_Id", c => c.Int());
            CreateIndex("dbo.Events", "ApplicationUser_Id");
            CreateIndex("dbo.Events", "ApplicationUser_Id1");
            CreateIndex("dbo.Events", "UserCreator_Id");
            CreateIndex("dbo.AspNetUsers", "Event_Id");
            AddForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Events", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events", "Id");
            AddForeignKey("dbo.Events", "UserCreator_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EventUserRelationships",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId });
            
            DropForeignKey("dbo.Events", "UserCreator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Event_Id" });
            DropIndex("dbo.Events", new[] { "UserCreator_Id" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Events", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Event_Id");
            DropColumn("dbo.Events", "UserCreator_Id");
            DropColumn("dbo.Events", "ApplicationUser_Id1");
            DropColumn("dbo.Events", "ApplicationUser_Id");
            CreateIndex("dbo.EventUserRelationships", "UserId");
            CreateIndex("dbo.EventUserRelationships", "EventId");
            AddForeignKey("dbo.EventUserRelationships", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventUserRelationships", "EventId", "dbo.Events", "Id", cascadeDelete: true);
        }
    }
}
