namespace MedOffice.Migrations.PatientDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientsArch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientsArch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Pesel = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Address = c.String(nullable: false),
                        DateOfChange = c.DateTime(nullable: false),
                        DBUser = c.String(nullable: false),
                        TypeOfChange = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PatientsArch");
        }
    }
}
