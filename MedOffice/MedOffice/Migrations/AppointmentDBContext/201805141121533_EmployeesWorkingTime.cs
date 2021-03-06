namespace MedOffice.Migrations.AppointmentDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeesWorkingTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                {
                    name = c.String(maxLength: 11),
                    fsname = c.String(),
                })
                .PrimaryKey(t => t.name);
            
            CreateTable(
                "dbo.WorkingTime",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pesel = c.String(),
                        eventstart = c.DateTime(nullable: false),
                        eventend = c.DateTime(nullable: false),
                        name = c.String(),
                        resource = c.Int(nullable: false),
                        recurrence = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WorkingTime");
            DropTable("dbo.Resources");
        }
    }
}
