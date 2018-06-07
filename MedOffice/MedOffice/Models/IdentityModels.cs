using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedOffice.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Experience = DateTime.Now;
            Seniority = DateTime.Now;
        }

        public string Specialization { get; set; }

        [Required(ErrorMessage = "Wprowadź imię pracownika.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+$", ErrorMessage = "Pole Imię może zawierać wyłącznie litery. Dopuszczalne są polskie znaki.")]
        [Display(Name = "Imię:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wprowadź nazwisko pracownika.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+(?:[\s\-][A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]+)?$", ErrorMessage = "Pole Nazwisko może zawierać wyłącznie litery. Dopuszczalne są nazwiska dwuczłonowe i zawierające polskie znaki.")]
        [Display(Name = "Nazwisko:")]
        public string Surname { get; set; }

        public DateTime Experience { get; set; }
        public DateTime Seniority { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UsersArch> UsersArch { get; set; }
    }
    
}