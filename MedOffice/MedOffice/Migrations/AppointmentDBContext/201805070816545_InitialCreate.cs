namespace MedOffice.Migrations.AppointmentDBContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        patients_pesel = c.String(nullable: false),
                        estim_disease = c.String(),
                        real_disease = c.String(),
                        dis_descript = c.String(),
                        appoint_date = c.DateTime(nullable: false),
                        specialization = c.String(nullable: false),
                        docs_pesel = c.String(nullable: false),
                        service_type = c.String(nullable: false),
                        service_name = c.String(),
                        service_price = c.Single(nullable: false),
                        is_paid = c.Boolean(nullable: false),
                        supplies_price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Appointments");
        }
    }
}
