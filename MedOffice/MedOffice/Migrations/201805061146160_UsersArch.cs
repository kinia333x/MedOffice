namespace MedOffice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersArch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersArch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UId = c.String(),
                        RId = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(nullable: true),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                        Specialization = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Experience = c.DateTime(nullable: false),
                        Seniority = c.DateTime(nullable: false),
                        DateOfChange = c.DateTime(nullable: false),
                        DBUser = c.String(nullable: true),
                        TypeOfChange = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UsersArch");
        }
    }
}
