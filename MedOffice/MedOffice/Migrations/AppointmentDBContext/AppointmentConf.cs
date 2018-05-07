namespace MedOffice.Migrations.AppointmentDBContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class AppointmentConf : DbMigrationsConfiguration<MedOffice.Models.AppointmentDBContext>
    {
        public AppointmentConf()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\AppointmentDBContext";
            ContextKey = "MedOffice.Models.AppointmentDBContext";
        }

        protected override void Seed(MedOffice.Models.AppointmentDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
