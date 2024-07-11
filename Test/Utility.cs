using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    public static class Utility {

        public static bool FindText(IWebElement element, string text) {
            foreach (IWebElement e in element.FindElements(By.XPath(".//*"))) {
                if (e.Text.Contains(text)) {
                    return true;
                }
            }
            return false;
        }

    }
}
