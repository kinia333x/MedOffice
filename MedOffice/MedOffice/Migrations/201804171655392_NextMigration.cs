namespace MedOffice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NextMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Experience", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Seniority", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Seniority", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Experience", c => c.String());
        }
    }
}
