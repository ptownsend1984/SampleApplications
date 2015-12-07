namespace GMailLabelCleanup.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilterProperties",
                c => new
                    {
                        FilterPropertyId = c.Int(nullable: false, identity: true),
                        FilterId = c.Int(nullable: false),
                        IsIncluded = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Value = c.String(maxLength: 1000),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FilterPropertyId)
                .ForeignKey("dbo.Filters", t => t.FilterId, cascadeDelete: true)
                .Index(t => t.FilterId);
            
            CreateTable(
                "dbo.Filters",
                c => new
                    {
                        FilterId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ImportId = c.String(maxLength: 25),
                        Description = c.String(nullable: false, maxLength: 500),
                        DateCreatedUtc = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.FilterId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GoogleAuthData",
                c => new
                    {
                        GoogleAuthDataId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Key = c.String(nullable: false, maxLength: 500),
                        Value = c.String(nullable: false, maxLength: 500),
                        Type = c.String(nullable: false, maxLength: 500),
                        DateCreatedUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GoogleAuthDataId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoogleAuthData", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Filters", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FilterProperties", "FilterId", "dbo.Filters");
            DropIndex("dbo.GoogleAuthData", new[] { "UserId" });
            DropIndex("dbo.Filters", new[] { "UserId" });
            DropIndex("dbo.FilterProperties", new[] { "FilterId" });
            DropTable("dbo.GoogleAuthData");
            DropTable("dbo.Filters");
            DropTable("dbo.FilterProperties");
        }
    }
}
