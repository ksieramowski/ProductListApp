using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ProductListApp.Models;
using System.Diagnostics;

namespace ProductListApp.Controllers {
    /// <summary>
    /// Contrioller for home page
    /// </summary>
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// Returns home view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Returns privacy view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Changes language of fornt-end in entire application using cookie value.
        /// </summary>
        /// <param name="culture">Culture code. For example "en-US".</param>
        /// <param name="returnUrl">Redirects app to this url</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl) {
            Console.WriteLine(culture);
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
