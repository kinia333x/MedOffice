using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MedOffice.Models;

[assembly: OwinStartupAttribute(typeof(MedOffice.Startup))]
namespace MedOffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        // metoda do tworzenia roli i Admina  
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // tworzymy role admina i samego Admina    
            if (!roleManager.RoleExists("Administrator"))
            {
                // tworzymy role admina   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);

                // tworzymy usera                 
                var user = new ApplicationUser();       /* Trzeba uzgodnić login i hasło dla admina */
                user.UserName = "admin";
                user.Email = "admin@hotmail.com";

                string userPWD = "!Admin123";

                var chkUser = UserManager.Create(user, userPWD);

                //przypisujemy uzytkownikowi role admina  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Administrator");
                }
            }

            // Pozostałe role:
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Doctor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Doctor";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Accountant"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Accountant";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Registrant"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Registrant";
                roleManager.Create(role);
            }
        }
    }
}
