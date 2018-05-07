namespace MedOffice.Migrations.AppointmentDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Service_time : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "service_time", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "service_time");
        }
    }
}
