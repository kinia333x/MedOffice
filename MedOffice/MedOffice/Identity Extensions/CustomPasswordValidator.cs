using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MedOffice.Models;
using System.Text.RegularExpressions;

namespace CPasswordValidator.IdentityExtensions
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public int RequiredLength { get; set; }
        public bool RequireNonLetterOrDigit { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }

        public CustomPasswordValidator()
        {
            RequiredLength = 5;
            RequireNonLetterOrDigit = true;
            RequireDigit = true;
            RequireLowercase = true;
            RequireUppercase = true;
        }

        public Task<IdentityResult> ValidateAsync(string password)
        {
            var ErrorMessage = "Hasło musi zawierać przynajmniej:";

            if ( password.Length < RequiredLength)
            {
                return Task.FromResult(IdentityResult.Failed(String.Format("Hasło musi zawierać przynajmniej {0} znaki(ów).", RequiredLength)));
            }

            if (RequireLowercase)
            {
                var LowerChar = new Regex(@"[a-z]+");

                if (!LowerChar.IsMatch(password))
                {
                    ErrorMessage += " jedną małą literę";
                }
            }

            if (RequireUppercase)
            {
                var UpperChar = new Regex(@"[A-Z]+");

                if (!UpperChar.IsMatch(password))
                {
                    if (object.Equals(ErrorMessage, "Hasło musi zawierać przynajmniej:"))
                    {
                        ErrorMessage += " jedną dużą literę";
                    }
                    else
                    {
                        ErrorMessage += ", jedną dużą literę";
                    }
                }
            }

            if (RequireDigit)
            {
                var Number = new Regex(@"[0-9]+");

                if (!Number.IsMatch(password))
                {
                    if (object.Equals(ErrorMessage, "Hasło musi zawierać przynajmniej:"))
                    {
                        ErrorMessage += " jedną cyfrę";
                    }
                    else
                    {
                        ErrorMessage += ", jedną cyfrę";
                    }
                }
            }

            if (RequireNonLetterOrDigit)
            {
                var Symbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!Symbols.IsMatch(password))
                {
                    if (object.Equals(ErrorMessage, "Hasło musi zawierać przynajmniej:"))
                    {
                        ErrorMessage += " jeden znak specjalny";
                    }
                    else
                    {
                        ErrorMessage += ", jeden znak specjalny";
                    }
                }
            }

            if (object.Equals(ErrorMessage, "Hasło musi zawierać przynajmniej:"))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                ErrorMessage += ".";
                return Task.FromResult(IdentityResult.Failed(ErrorMessage));
            }
        }
    }
}