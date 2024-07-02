using Microsoft.AspNetCore.Identity;

namespace ProductListApp.Models {
    public class User : IdentityUser {
        public string? Nickname { get; set; }
    }
}
