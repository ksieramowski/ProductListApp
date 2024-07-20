using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Chrome {
    /// <summary>
    /// Main test class using NUnit and Selenium.
    /// </summary>
    public partial class Tests {

        /// <summary>
        /// Reister test with auto-generated seed
        /// </summary>
        [Test]
        public void Register() {
            TestSeed seed = new();
            Register(seed);
        }

        /// <summary>
        /// Register test with given seed.
        /// Registers new user with valid field values.
        /// </summary>
        /// <param name="seed">Used to define values sent to application.</param>
        public void Register(TestSeed seed) {
            Logout();

            Driver.Navigate().GoToUrl(Root);
            var registerLink = Driver.FindElement(By.Id("pla-nav-register"));
            Assert.That(registerLink, Is.Not.Null);

            registerLink.Click();
            Assert.That(Driver.Url, Is.EqualTo($"{Root}Account/Register"));

            var usernameInput = Driver.FindElement(By.Id("pla-register-username"));
            var emailInput = Driver.FindElement(By.Id("pla-register-email"));
            var passwordInput = Driver.FindElement(By.Id("pla-register-password"));
            var confirmPasswordInput = Driver.FindElement(By.Id("pla-register-confirm-password"));

            Assert.That(usernameInput, Is.Not.Null);
            Assert.That(emailInput, Is.Not.Null);
            Assert.That(passwordInput, Is.Not.Null);
            Assert.That(confirmPasswordInput, Is.Not.Null);

            string username = $"user{seed.Value}";
            string email = $"user{seed.Value}@gmail.com";
            string password = $"{seed.Value}";
            string confirmPassword = $"{seed.Value}";

            usernameInput.SendKeys(username);
            emailInput.SendKeys(email);
            passwordInput.SendKeys(password);
            confirmPasswordInput.SendKeys(confirmPassword);

            var submitButton = Driver.FindElement(By.Id("pla-register-submit"));
            Assert.That(submitButton, Is.Not.Null);
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo(Root));

        }

        /// <summary>
        /// Login test with auto-generated seed.
        /// </summary>
        [Test]
        public void Login() {
            TestSeed seed = new();
            Login(seed);
        }

        /// <summary>
        /// Login test with given seed.
        /// Logs in new user with valid field values.
        /// </summary>
        /// <param name="seed">Used to define values sent to application.</param>
        public void Login(TestSeed seed) {
            Register(seed);
            Logout();

            Driver.Navigate().GoToUrl(Root);
            var loginLink = Driver.FindElement(By.Id("pla-nav-login"));
            Assert.That(loginLink, Is.Not.Null);
            loginLink.Click();
            Assert.That(Driver.Url, Is.EqualTo($"{Root}Account/Login"));

            var emailInput = Driver.FindElement(By.Id("pla-login-email"));
            var passwordInput = Driver.FindElement(By.Id("pla-login-password"));

            Assert.That(emailInput, Is.Not.Null);
            Assert.That(passwordInput, Is.Not.Null);

            string email = $"user{seed.Value}@gmail.com";
            string password = $"{seed.Value}";

            emailInput.SendKeys(email);
            passwordInput.SendKeys(password);

            var submitButton = Driver.FindElement(By.Id("pla-login-submit"));
            Assert.That(submitButton, Is.Not.Null);

            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo(Root));
        }

        /// <summary>
        /// Logout test.
        /// Checks if login and register buttons are present if user is not logged in.
        /// </summary>
        [Test]
        public void Logout() {
            Driver.Navigate().GoToUrl(Root);

            IWebElement? logoutLink = null;
            try { logoutLink = Driver.FindElement(By.Id("pla-nav-logout")); }
            catch {}
            if (logoutLink == null) {
                var registerLink = Driver.FindElement(By.Id("pla-nav-register"));
                var loginLink = Driver.FindElement(By.Id("pla-nav-login"));
                Assert.That(registerLink != null && loginLink != null, Is.EqualTo(true));
                return;
            }
            Assert.That(logoutLink, Is.Not.Null);
            logoutLink.Click();
            Assert.That(Driver.Url, Is.EqualTo(Root));

        }
        
    }

}
