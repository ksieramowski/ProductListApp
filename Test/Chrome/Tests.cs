using OpenQA.Selenium.Chrome;
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
        /// Root URL of web application.
        /// </summary>
        private static string Root;
        /// <summary>
        /// Selenium web driver.
        /// </summary>
        private static ChromeDriver Driver;

        /// <summary>
        /// Sets up website's root path and initializes web driver.
        /// </summary>
        [SetUp]
        public void Setup() {
            Root = @"https://localhost:7051/"; // local test

            Driver = new ChromeDriver();
        }

        /// <summary>
        /// Cleans after web driver.
        /// </summary>
        [TearDown]
        public void TearDown() {
            Driver.Close();
            Driver.Dispose();
        }

    }
}
