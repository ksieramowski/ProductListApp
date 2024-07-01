using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Test.Chrome {
    public class Tests {

        private string Root;
        private ChromeDriver Driver;


        [SetUp]
        public void Setup() {
            Root = @"https://localhost:7051/";
            Driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown() {
            Driver.Close();
            Driver.Dispose();
        }

        [Test]
        public void Test1() {
            Assert.Pass();
        }

        [Test]
        public void AddProduct() {
            string productsUrl = Root + "Products";
            Driver.Navigate().GoToUrl(productsUrl);
            Assert.That(Driver.Url, Is.EqualTo(productsUrl));

            var tbody = Driver.FindElement(By.Id("tbody-product-list"));
            Assert.That(tbody, Is.Not.Null);
            var productsInList = tbody.FindElements(By.XPath(".//*"));
            int countBefore = productsInList.Count();



            var nameInput = Driver.FindElement(By.Id("input-product-name"));
            Assert.That(nameInput, Is.Not.Null);
            nameInput.SendKeys("produkt testowy");

            var quantityInput = Driver.FindElement(By.Id("input-product-quantity"));
            Assert.That(quantityInput, Is.Not.Null);
            quantityInput.SendKeys("2");

            var priceInput = Driver.FindElement(By.Id("input-product-price"));
            Assert.That(priceInput, Is.Not.Null);
            priceInput.SendKeys("12");

            var button = Driver.FindElement(By.Id("button-add-product"));
            Assert.That(button, Is.Not.Null);
            button.Click();

            Assert.That(Driver.Url, Is.EqualTo(productsUrl));

            tbody = Driver.FindElement(By.Id("tbody-product-list"));
            Assert.That(tbody, Is.Not.Null);
            productsInList = tbody.FindElements(By.XPath(".//*"));
            Assert.That(countBefore, Is.LessThan(productsInList.Count()));

        }
    }
}