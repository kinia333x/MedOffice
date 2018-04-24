namespace MedOffice.Migrations.PatientDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataUrodzenia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "BirthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Patients", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patients", "Age", c => c.String(nullable: false));
            DropColumn("dbo.Patients", "BirthDate");
        }
    }
}
