using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MedOffice.Models;
using System.Collections.Generic;
using System.Net;

namespace MedOffice.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private string CurrentUser = System.Web.HttpContext.Current.User.Identity.Name;

        ApplicationDbContext context;

        public AccountController()
        {
            context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult Register()
        {
            if (User.IsInRole("Administrator"))
            {
                ViewBag.UserRoles = new SelectList(context.Roles.Where(u => !u.Name.Contains("Administrator"))
                                            .ToList(), "Name", "Name");
            }
            else if (User.IsInRole("Kierownik"))
            {
                ViewBag.UserRoles = new SelectList(context.Roles.Where(u => !u.Name.Contains("Administrator") && !u.Name.Contains("Kierownik"))
                               .ToList(), "Name", "Name");
            }

            List<SelectListItem> Specializations = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Alergologia" },
                new SelectListItem { Text = "Anestezjologia i intensywna terapia" },
                new SelectListItem { Text = "Angiologia" },
                new SelectListItem { Text = "Audiologia i foniatria" },
                new SelectListItem { Text = "Balneologia i medycyna fizykalna" },
                new SelectListItem { Text = "Chirurgia dziecięca" },
                new SelectListItem { Text = "Chirurgia klatki piersiowej" },
                new SelectListItem { Text = "Chirurgia naczyniowa" },
                new SelectListItem { Text = "Chirurgia ogólna" },
                new SelectListItem { Text = "Chirurgia onkologiczna" },
                new SelectListItem { Text = "Chirurgia plastyczna" },
                new SelectListItem { Text = "Chirurgia stomatologiczna" },
                new SelectListItem { Text = "Chirurgia szczękowo-twarzowa" },
                new SelectListItem { Text = "Chirurgia płuc" },
                new SelectListItem { Text = "Chirurgia płuc dzieci" },
                new SelectListItem { Text = "Choroby płuc" },
                new SelectListItem { Text = "Choroby płuc dzieci" },
                new SelectListItem { Text = "Choroby wewnętrzne" },
                new SelectListItem { Text = "Choroby zakaźne" },
                new SelectListItem { Text = "Dermatologia i wenerologia" },
                new SelectListItem { Text = "Diabetologia" },
                new SelectListItem { Text = "Endokrynologia" },
                new SelectListItem { Text = "Endokrynologia ginekologiczna i rozrodczość" },
                new SelectListItem { Text = "Endokrynologia i diabetologia dziecięca" },
                new SelectListItem { Text = "Epidemiologia" },
                new SelectListItem { Text = "Farmakologia kliniczna" },
                new SelectListItem { Text = "Gastroenterologia" },
                new SelectListItem { Text = "Gastroenterologia dziecięca" },
                new SelectListItem { Text = "Genetyka kliniczna" },
                new SelectListItem { Text = "Geriatria" },
                new SelectListItem { Text = "Ginekologia onkologiczna" },
                new SelectListItem { Text = "Hematologia" },
                new SelectListItem { Text = "Hipertensjologia" },
                new SelectListItem { Text = "Immunologia kliniczna" },
                new SelectListItem { Text = "Intensywna terapia" },
                new SelectListItem { Text = "Kardiochirurgia" },
                new SelectListItem { Text = "Kardiologia" },
                new SelectListItem { Text = "Kardiologia dziecięca" },
                new SelectListItem { Text = "Medycyna lotnicza" },
                new SelectListItem { Text = "Medycyna morska i tropikalna" },
                new SelectListItem { Text = "Medycyna nuklearna" },
                new SelectListItem { Text = "Medycyna paliatywna" },
                new SelectListItem { Text = "Medycyna pracy" },
                new SelectListItem { Text = "Medycyna ratunkowa" },
                new SelectListItem { Text = "Medycyna rodzinna" },
                new SelectListItem { Text = "Medycyna sądowa" },
                new SelectListItem { Text = "Medycyna sportowa" },
                new SelectListItem { Text = "Mikrobiologia lekarska" },
                new SelectListItem { Text = "Nefrologia" },
                new SelectListItem { Text = "Nefrologia dziecięca" },
                new SelectListItem { Text = "Neonatologia" },
                new SelectListItem { Text = "Neurochirurgia" },
                new SelectListItem { Text = "Neurologia" },
                new SelectListItem { Text = "Neurologia dziecięca" },
                new SelectListItem { Text = "Neuropatologia" },
                new SelectListItem { Text = "Okulistyka" },
                new SelectListItem { Text = "Onkologia i hematologia dziecięca" },
                new SelectListItem { Text = "Onkologia kliniczna " },
                new SelectListItem { Text = "Ortodoncja" },
                new SelectListItem { Text = "Ortopedia i traumatologia narządu ruchu" },
                new SelectListItem { Text = "Otorynolaryngologia" },
                new SelectListItem { Text = "Otorynolaryngologia dziecięca" },
                new SelectListItem { Text = "Patomorfologia" },
                new SelectListItem { Text = "Pediatria" },
                new SelectListItem { Text = "Pediatria metaboliczna" },
                new SelectListItem { Text = "Perinatologia" },
                new SelectListItem { Text = "Periodontologia" },
                new SelectListItem { Text = "Położnictwo i ginekologia" },
                new SelectListItem { Text = "Protetyka stomatologiczna" },
                new SelectListItem { Text = "Psychiatria" },
                new SelectListItem { Text = "Psychiatria dzieci i młodzieży" },
                new SelectListItem { Text = "Radiologia i diagnostyka obrazowa" },
                new SelectListItem { Text = "Radioterapia onkologiczna" },
                new SelectListItem { Text = "Rehabilitacja medyczna" },
                new SelectListItem { Text = "Reumatologia" },
                new SelectListItem { Text = "Seksuologia" },
                new SelectListItem { Text = "Stomatologia dziecięca" },
                new SelectListItem { Text = "Stomatologia zachowawcza z endodoncją" },
                new SelectListItem { Text = "Toksykologia kliniczna" },
                new SelectListItem { Text = "Transfuzjologia kliniczna" },
                new SelectListItem { Text = "Transplantologia kliniczna" },
                new SelectListItem { Text = "Urologia" },
                new SelectListItem { Text = "Urologia dziecięca" },
                new SelectListItem { Text = "Zdrowie publiczne " }
            };

            ViewBag.Spec = Specializations;

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Administrator, Kierownik")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Specialization != "Lekarz")
                {
                    model.Specialization = null;
                }

                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Seniority = model.Seniority,
                    Experience = model.Experience,
                    UserName = model.UserName,
                    Email = model.Email,
                    Specialization = model.Specialization
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Wyłączenie automatycznego logowania po rejestracji:
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // Dodanie roli dla nowego użytkownika.
                    // Rola dodawania jest później, stąd dwa wpisy w archiwum przy tworzeniu nowego konta.
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);

                    // Dodanie do archiwum ID osoby, która stworzyła nowe konto użytkownika, a także dodanie roli dodanego użytkownika:
                    string query = "UPDATE [dbo].[UsersArch] SET RId = '" + model.UserRoles + "', DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'INSERTED' AND UserName = " + model.UserName;
                    context.Database.ExecuteSqlCommand(query);

                    // Dodanie roli rowna sie dwom wpisom z kategorii UPDATE, wiec je usuwamy:
                    query = "DELETE FROM [dbo].[UsersArch] WHERE TypeOfChange = 'UPDATED-DELETED' AND UserName = " + model.UserName;
                    context.Database.ExecuteSqlCommand(query);
                    query = "DELETE FROM [dbo].[UsersArch] WHERE TypeOfChange = 'UPDATED-INSERTED' AND UserName = " + model.UserName;
                    context.Database.ExecuteSqlCommand(query);

                    // Dodanie do tabeli Resources danych nowo zarejestrowanego pracownika:
                    AppointmentDBContext Appdb = new AppointmentDBContext();

                    query = "INSERT INTO [dbo].[Resources] (name, fsname) VALUES ('" + model.UserName + "', '" + model.Name + " " + model.Surname + "')";
                    Appdb.Database.ExecuteSqlCommand(query);

                    return RedirectToAction("ConfirmRegistration", "Account");
                }

                AddErrors(result);
            }

            if (User.IsInRole("Administrator"))
            {
                ViewBag.UserRoles = new SelectList(context.Roles.Where(u => !u.Name.Contains("Administrator"))
                                            .ToList(), "Name", "Name");
            }
            else if (User.IsInRole("Manager"))
            {
                ViewBag.UserRoles = new SelectList(context.Roles.Where(u => !u.Name.Contains("Administrator") && !u.Name.Contains("Manager"))
                               .ToList(), "Name", "Name");
            }

            List<SelectListItem> Specializations = new List<SelectListItem>()
                {
                    new SelectListItem { Text = "Alergologia" },
                    new SelectListItem { Text = "Alergologia" },
                    new SelectListItem { Text = "Anestezjologia i intensywna terapia" },
                    new SelectListItem { Text = "Angiologia" },
                    new SelectListItem { Text = "Audiologia i foniatria" },
                    new SelectListItem { Text = "Balneologia i medycyna fizykalna" },
                    new SelectListItem { Text = "Chirurgia dziecięca" },
                    new SelectListItem { Text = "Chirurgia klatki piersiowej" },
                    new SelectListItem { Text = "Chirurgia naczyniowa" },
                    new SelectListItem { Text = "Chirurgia ogólna" },
                    new SelectListItem { Text = "Chirurgia onkologiczna" },
                    new SelectListItem { Text = "Chirurgia plastyczna" },
                    new SelectListItem { Text = "Chirurgia stomatologiczna" },
                    new SelectListItem { Text = "Chirurgia szczękowo-twarzowa" },
                    new SelectListItem { Text = "Chirurgia płuc" },
                    new SelectListItem { Text = "Chirurgia płuc dzieci" },
                    new SelectListItem { Text = "Choroby płuc" },
                    new SelectListItem { Text = "Choroby płuc dzieci" },
                    new SelectListItem { Text = "Choroby wewnętrzne" },
                    new SelectListItem { Text = "Choroby zakaźne" },
                    new SelectListItem { Text = "Dermatologia i wenerologia" },
                    new SelectListItem { Text = "Diabetologia" },
                    new SelectListItem { Text = "Endokrynologia" },
                    new SelectListItem { Text = "Endokrynologia ginekologiczna i rozrodczość" },
                    new SelectListItem { Text = "Endokrynologia i diabetologia dziecięca" },
                    new SelectListItem { Text = "Epidemiologia" },
                    new SelectListItem { Text = "Farmakologia kliniczna" },
                    new SelectListItem { Text = "Gastroenterologia" },
                    new SelectListItem { Text = "Gastroenterologia dziecięca" },
                    new SelectListItem { Text = "Genetyka kliniczna" },
                    new SelectListItem { Text = "Geriatria" },
                    new SelectListItem { Text = "Ginekologia onkologiczna" },
                    new SelectListItem { Text = "Hematologia" },
                    new SelectListItem { Text = "Hipertensjologia" },
                    new SelectListItem { Text = "Immunologia kliniczna" },
                    new SelectListItem { Text = "Intensywna terapia" },
                    new SelectListItem { Text = "Kardiochirurgia" },
                    new SelectListItem { Text = "Kardiologia" },
                    new SelectListItem { Text = "Kardiologia dziecięca" },
                    new SelectListItem { Text = "Medycyna lotnicza" },
                    new SelectListItem { Text = "Medycyna morska i tropikalna" },
                    new SelectListItem { Text = "Medycyna nuklearna" },
                    new SelectListItem { Text = "Medycyna paliatywna" },
                    new SelectListItem { Text = "Medycyna pracy" },
                    new SelectListItem { Text = "Medycyna ratunkowa" },
                    new SelectListItem { Text = "Medycyna rodzinna" },
                    new SelectListItem { Text = "Medycyna sądowa" },
                    new SelectListItem { Text = "Medycyna sportowa" },
                    new SelectListItem { Text = "Mikrobiologia lekarska" },
                    new SelectListItem { Text = "Nefrologia" },
                    new SelectListItem { Text = "Nefrologia dziecięca" },
                    new SelectListItem { Text = "Neonatologia" },
                    new SelectListItem { Text = "Neurochirurgia" },
                    new SelectListItem { Text = "Neurologia" },
                    new SelectListItem { Text = "Neurologia dziecięca" },
                    new SelectListItem { Text = "Neuropatologia" },
                    new SelectListItem { Text = "Okulistyka" },
                    new SelectListItem { Text = "Onkologia i hematologia dziecięca" },
                    new SelectListItem { Text = "Onkologia kliniczna " },
                    new SelectListItem { Text = "Ortodoncja" },
                    new SelectListItem { Text = "Ortopedia i traumatologia narządu ruchu" },
                    new SelectListItem { Text = "Otorynolaryngologia" },
                    new SelectListItem { Text = "Otorynolaryngologia dziecięca" },
                    new SelectListItem { Text = "Patomorfologia" },
                    new SelectListItem { Text = "Pediatria" },
                    new SelectListItem { Text = "Pediatria metaboliczna" },
                    new SelectListItem { Text = "Perinatologia" },
                    new SelectListItem { Text = "Periodontologia" },
                    new SelectListItem { Text = "Położnictwo i ginekologia" },
                    new SelectListItem { Text = "Protetyka stomatologiczna" },
                    new SelectListItem { Text = "Psychiatria" },
                    new SelectListItem { Text = "Psychiatria dzieci i młodzieży" },
                    new SelectListItem { Text = "Radiologia i diagnostyka obrazowa" },
                    new SelectListItem { Text = "Radioterapia onkologiczna" },
                    new SelectListItem { Text = "Rehabilitacja medyczna" },
                    new SelectListItem { Text = "Reumatologia" },
                    new SelectListItem { Text = "Seksuologia" },
                    new SelectListItem { Text = "Stomatologia dziecięca" },
                    new SelectListItem { Text = "Stomatologia zachowawcza z endodoncją" },
                    new SelectListItem { Text = "Toksykologia kliniczna" },
                    new SelectListItem { Text = "Transfuzjologia kliniczna" },
                    new SelectListItem { Text = "Transplantologia kliniczna" },
                    new SelectListItem { Text = "Urologia" },
                    new SelectListItem { Text = "Urologia dziecięca" },
                    new SelectListItem { Text = "Zdrowie publiczne " },
                };

            ViewBag.Spec = Specializations;

            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ConfirmRegistration
        [Authorize(Roles = "Administrator, Kierownik")]
        public ActionResult ConfirmRegistration()
        {
            return View();
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: Search/Delete
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = context.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Rejestrujący")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            AppointmentDBContext Appdb = new AppointmentDBContext();
            ApplicationUser user = context.Users.Find(id);

            context.Users.Remove(user);
            context.SaveChanges();

            string query = "UPDATE [dbo].[UsersArch] SET RId = (SELECT RId FROM [dbo].[UsersArch] WHERE TypeOfChange = 'INSERTED' AND UserName = '" + user.UserName + "'), DBUSer = '" + CurrentUser + "' WHERE TypeOfChange = 'DELETED' AND UserName = " + user.UserName;
            context.Database.ExecuteSqlCommand(query);

            query = "DELETE FROM [dbo].[WorkingTime] WHERE resource = (SELECT id FROM [dbo].[Resources] WHERE name = '" + user.UserName + "')";
            Appdb.Database.ExecuteSqlCommand(query);

            query = "DELETE FROM [dbo].[Resources] WHERE name = '" + user.UserName + "'";
            Appdb.Database.ExecuteSqlCommand(query);

            return RedirectToAction("WorkerSearch", "Search");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}

