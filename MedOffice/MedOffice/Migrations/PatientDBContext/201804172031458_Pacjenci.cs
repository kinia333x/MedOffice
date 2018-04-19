namespace MedOffice.Migrations.PatientDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pacjenci : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Surname", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Age", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Pesel", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "Address", c => c.String());
            AlterColumn("dbo.Patients", "Pesel", c => c.String());
            AlterColumn("dbo.Patients", "Age", c => c.String());
            AlterColumn("dbo.Patients", "Surname", c => c.String());
            AlterColumn("dbo.Patients", "Name", c => c.String());
        }
    }
}
