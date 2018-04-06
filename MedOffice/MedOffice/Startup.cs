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
            if (!roleManager.RoleExists("Admin"))
            {

                // tworzymy role admina   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // tworzymy usera                 
                var user = new ApplicationUser();
                user.UserName = "naszAdmin";
                user.Email = "naszAdmin@gmail.com";

                string userPWD = "A@Z200711";

                var chkUser = UserManager.Create(user, userPWD);

                //przypisujemy uzytkownikowi role admina  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // pozostale role
        }
    }
}
