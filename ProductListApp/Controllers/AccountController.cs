using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductListApp.Models;

namespace ProductListApp.Controllers {
    /// <summary>
    /// Controller class for managing user accounts.
    /// </summary>
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Returns registration view with empty fields.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        /// <summary>
        /// If registration data is correct, then create new user, log them in and redirect to main page.
        /// Otherwise displays errors for each invalid field in registration form.
        /// </summary>
        /// <param name="registerData">Data entered by user into the form</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(Register registerData) {
            if (ModelState.IsValid) {
                var user = new User { Nickname = registerData.Name, UserName = registerData.Email, Email = registerData.Email };
                var result = await _userManager.CreateAsync(user, registerData.Password);
                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerData);
        }

        /// <summary>
        /// Returns login view with empty fields.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        /// <summary>
        /// If login data is correct, then log the user in and redirect to main page.
        /// Otherwise displays errors for each invalid field in login form.
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(Login loginData) {
            if (ModelState.IsValid) {
                var result = await _signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, loginData.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginData);
        }

        /// <summary>
        /// Logs out current user and redirects to main page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
