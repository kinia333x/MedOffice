namespace MedOffice.Migrations.AppointmentDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointArch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentsArch",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Idd = c.Int(nullable: false),
                        patients_pesel = c.String(),
                        estim_disease = c.String(),
                        real_disease = c.String(),
                        dis_descript = c.String(),
                        appoint_date = c.DateTime(nullable: false),
                        specialization = c.String(),
                        docs_pesel = c.String(),
                        service_type = c.String(),
                        service_time = c.Int(nullable: false),
                        service_name = c.String(),
                        service_price = c.Single(nullable: false),
                        is_paid = c.Boolean(nullable: false),
                        supplies_price = c.Single(nullable: false),
                        DateOfChange = c.DateTime(nullable: false),
                        DBUser = c.String(),
                        TypeOfChange = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppointmentsArch");
        }
    }
}
