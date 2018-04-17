namespace MedOffice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rejestracja : DbMigration
    {
        public override void Up()
        { 
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: true, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String(nullable: true, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Experience", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Seniority", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Seniority");
            DropColumn("dbo.AspNetUsers", "Experience");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
