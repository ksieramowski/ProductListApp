using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductListApp.Models;

namespace ProductListApp.Controllers {
    public class BaseController : Controller {
        protected readonly UserManager<User> _userManager;

        public BaseController(UserManager<User> userManager) {
            _userManager = userManager;
        }

        /// <summary>
        /// Returns username of current user or null.
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetUserNameAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) {
                return user.UserName;
            }
            return null;
        }
    }

}
