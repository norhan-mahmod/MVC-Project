using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email.Split('@')[0],
                    IsAgree = registerViewModel.IsAgree
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("Login");
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerViewModel);
        }
        public IActionResult Login()
        {
            return View(new SignInViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(signInViewModel.Email);
                if (user is null)
                    ModelState.AddModelError(string.Empty, "Email doesn't exist");
                var isCorrectPassword = await userManager.CheckPasswordAsync(user, signInViewModel.Password);
                if (isCorrectPassword)
                {
                    var result = await signInManager.PasswordSignInAsync(user, signInViewModel.Password, signInViewModel.RememberMe, false);
                    
                    if(result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(signInViewModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if(user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword", "Account", new
                    {
                        Email = forgetPasswordViewModel.Email,
                        Token = token
                    },
                    Request.Scheme);
                    var email = new Email()
                    {
                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Password",
                        Body = resetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CompleteForgetPassword");
                }
                ModelState.AddModelError("", "Invalid Email");
            }
            return View(forgetPasswordViewModel);
        }
        public IActionResult CompleteForgetPassword()
        {
            return View();
        }
        public IActionResult ResetPassword(string email , string token)
        {
            return View(new ResetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(resetPasswordViewModel.Email);
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token,
                                            resetPasswordViewModel.Password);
                    if (result.Succeeded)
                        return RedirectToAction("Login");
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(resetPasswordViewModel);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
