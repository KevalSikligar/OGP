using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OGP_Portal.Data.DbContext;
using OGP_Portal.Models;
using OGP_Portal.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGP_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userMananger;

        public HomeController(UserManager<ApplicationUser> userMananger, SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger)
        {
            _userMananger = userMananger;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Redirect("/Identity/Account/Login");
        }



        public async Task<bool> CheckEmail(string ContactEmail = "", long Id = 0)
        {
            bool isExist;
            var result = await _userMananger.FindByEmailAsync(ContactEmail);

            if (result != null)
            {

                isExist = result.Email.ToLower().Trim().Equals(ContactEmail.ToLower().Trim()) ? true : false;
                if (isExist && Id != 0)
                {
                    var resultExist = _userMananger.FindByIdAsync(Id.ToString());
                    return resultExist == null ? false : true;
                }
                else
                {
                    return result == null ? true : false;
                }
            }
            else
            {
                return result == null ? true : false;
            }
        }


        public async Task<IActionResult> ResetPassword(string userId, string code)
        {
            var user = await _userMananger.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userMananger.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View(user);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Resetpassword(long id, string password)
        {
            var user = await _userMananger.FindByEmailAsync(id.ToString());
            var code = await _userMananger.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            if (user == null)
            {
                var result = await _userMananger.ResetPasswordAsync(user, code, password);
                if (result.Succeeded)
                {
                    return View();
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }


        }
    }
}
