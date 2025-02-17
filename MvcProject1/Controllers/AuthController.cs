using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MvcProject1.DAL.Models;
using MvcProject1.PL.Helpers;
using MvcProject1.PL.ViewModels.AuthViewModel;
using Twilio.TwiML.Voice;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace MvcProject1.PL.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISendEmail sendEmail;
        private readonly ISendSms sendSms;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ISendEmail sendEmail,
            ISendSms sendSms )
        {
            _userManager = userManager;
            this._signInManager = signInManager;
            this.sendEmail = sendEmail;
            this.sendSms = sendSms;
        }
        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password.Equals(model.ConfirmPassword))
                {
                    
                    if (model.IsAgree)
                    {
                        var user = await _userManager.FindByEmailAsync(model.Email);
                        if (user == null)
                        {
                            AppUser appUser = new AppUser()
                            {
                                Email = model.Email,
                                IsAgree = model.IsAgree,
                                UserName = model.UserName,
                                PhoneNumber = model.Phone
                            };
                            var result = await _userManager.CreateAsync(appUser,model.Password);
                            if (result.Succeeded)
                            {
                                return Redirect("SignIn");
                            }
                            foreach (var e in result.Errors)
                            {
                                ModelState.AddModelError("", e.Description);
                                
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "The current email is used before");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "You must agree");
                    }
                    

                }
                else
                { 
                  ModelState.AddModelError("", "You must write the same password");
                }
            }


            return View(model);
        }
        #endregion

        #region SignIn
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Check password using ASP.NET Core Identity's hashing mechanism
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flag)
                    {
                        // Use lockoutOnFailure: false to avoid immediate lockout
                        var result = await _signInManager.PasswordSignInAsync(user,
                                                                             model.Password,
                                                                             model.RememberMe,
                                                                             true);

                        if (result.Succeeded)
                        {
                            // Login successful, redirect to Home
                            return RedirectToAction("Index", "Home");
                        }

                    }
     
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not correct"); // Or a more specific message
                }
            }

            return View(model);
        }
        #endregion

        #region SignOut

        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }
        #endregion

        #region ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid) { 
                var user =await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var email = new Email();
                    email.To = model.Email;
                    email.Subject = "Reset your password";
                    var url = Url.Action("ResetPassword", "Auth", new { Email = user.Email,Token = token},Request.Scheme);
                    
                    email.Body = $@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Reset Your Password</title>
                    </head>
                    <body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
                        <table cellpadding='0' cellspacing='0' border='0' width='100%' style='max-width: 600px; margin: 20px auto; background: #ffffff; border: 1px solid #dddddd; border-radius: 10px;'>
                            <tr>
                                <td style='padding: 20px; text-align: center; background-color: #F32013; color: #ffffff; border-top-left-radius: 10px; border-top-right-radius: 10px;'>
                                    <h1 style='margin: 0; font-size: 24px;'>Reset Your Password</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 20px; font-size: 16px; color: #555555;'>
                                    <p>Hi,</p>
                                    <p>We received a request to reset your password. Click the button below to proceed.</p>
                                    <p>If you didn't request this, you can safely ignore this email.</p>
                                    <p style='text-align: center; margin: 20px 0;'>
                                        <a href='{url}' style='display: inline-block; padding: 10px 20px; background-color: #F32013; color: #ffffff; text-decoration: none; border-radius: 5px; font-size: 16px;'>Reset Password</a>
                                    </p>
                                    <p>Thank you,<br>The Support Team</p>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 10px; text-align: center; font-size: 12px; color: #aaaaaa;'>
                                    <p>If you have any questions, please contact our support team.</p>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";

                    sendEmail.Send(email);
                    return RedirectToAction("CheckInbox");

                }
                else
                {
                    ModelState.AddModelError("", "The email is not found");
                }


            }
            return View(model);
        }

        public IActionResult CheckInbox()
        {
            return View();
        }
        #endregion

        #region ForgetPasswordUsingPhoneNumber

        public IActionResult ForgetPasswordUsingPhoneNumber()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordUsingPhoneNumber(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var sms = new Sms();
                    sms.To = user.PhoneNumber;
                    var url = Url.Action("ResetPassword", "Auth", new { Email = user.Email, Token = token }, Request.Scheme);
                    sms.Message = $@"Reset Your Password
                                        Hi,

                                        We received a request to reset your password. Click the link below to proceed:
                                        [{url}]

                                        If you didn't request this, you can safely ignore this message.

                                        Thank you,
                                        The Support Team";

                    var result = sendSms.Send(sms);

                    return RedirectToAction("CheckYourPhoneSms");

                }
                else
                {
                    ModelState.AddModelError("", "The email is not found");
                }


            }
            return View(model);
        }

        public IActionResult CheckYourPhoneSms()
        {
            return View();
        }
        #endregion

        #region ResetPassword
        public IActionResult ResetPassword(string Email, string Token)
        {
            TempData["Email"] = Email;
            TempData["Token"] = Token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string Email = TempData["Email"] as string;
                string Token = TempData["Token"] as string;
                if (!Email.IsNullOrEmpty() && !Token.IsNullOrEmpty())
                {
                    var user = await _userManager.FindByEmailAsync(Email);
                    if (user is not null)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, Token, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");

                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }

                }
            }

            return View(model);
        }

        #endregion

        #region ExternalLogin


        #region Google
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse");
            var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Value,
                c.Type

            });
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new AppUser()
                {
                    Email = email,
                    UserName = email
                };
                await _userManager.CreateAsync(user);
            }
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }

        #endregion



        #endregion

    }
}
