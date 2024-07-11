using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using NUnit.Framework.Interfaces;

namespace Test.Chrome {
    public partial class Tests {

        private static string Root;
        private static ChromeDriver Driver;


        [SetUp]
        public void Setup() {
            Root = @"https://localhost:7051/"; // local test

            Driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown() {
            Driver.Close();
            Driver.Dispose();
        }

        [Test]
        public void CreateProductList() {
            TestSeed seed = new();
            CreateProductList(seed);
        }

        public void CreateProductList(TestSeed seed) {
            Login(seed);

            Driver.Navigate().GoToUrl(Root);
            var productsButton = Driver.FindElement(By.Id("pla-nav-products"));
            Assert.That(productsButton, Is.Not.Null);

            productsButton.Click();
            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            IWebElement? tbody = null;
            try { tbody = Driver.FindElement(By.Id("pla-productlists-tbody")); }
            catch {}

            int count = 0;
            ReadOnlyCollection<IWebElement>? lists;
            if (tbody != null) {
                lists = tbody.FindElements(By.XPath(".//*"));
                count = lists.Count();
            }

            var listNameInput = Driver.FindElement(By.Id("pla-productlists-new-name"));
            Assert.That(listNameInput, Is.Not.Null);

            var submitButton = Driver.FindElement(By.Id("pla-productlists-new-submit"));
            Assert.That(submitButton, Is.Not.Null);

            string listName = $"list_{seed.Value}";

            listNameInput.SendKeys(listName);
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            try { tbody = Driver.FindElement(By.Id("pla-productlists-tbody")); }
            catch { }

            Assert.That(tbody, Is.Not.Null);

            lists = tbody.FindElements(By.XPath(".//*"));
            Assert.That(lists.Count(), Is.GreaterThan(count));
        }

        [Test]
        public void EditProductList() {
            TestSeed seed = new();
            EditProductList(seed);
        }

        public void EditProductList(TestSeed seed) {
            CreateProductList(seed);

            string editValue = "_e";

            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            bool found = false;
            foreach (IWebElement e in Driver.FindElement(By.Id("pla-productlists-tbody")).FindElements(By.XPath(".//*"))) {
                if (e.Text.Contains($"{seed}")) {
                    found = true;
                    break;
                }
            }
            Assert.That(found);

            var editButtons = Driver.FindElements(By.ClassName("pla-productlists-edit"));
            var editButton = editButtons.FirstOrDefault();
            Assert.That(editButton, Is.Not.Null);

            editButton.Click();
            Assert.That(Driver.Url, Does.Contain("ProductLists/Edit"));

            var nameInput = Driver.FindElement(By.Id("pla-edit-productlist-name"));
            Assert.That(nameInput, Is.Not.Null);
            nameInput.SendKeys(editValue);

            var submitButton = Driver.FindElement(By.Id("pla-edit-productlist-submit"));
            Assert.That(submitButton, Is.Not.Null);
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            found = false;
            foreach (IWebElement e in Driver.FindElement(By.Id("pla-productlists-tbody")).FindElements(By.XPath(".//*"))) {
                if (e.Text.Contains($"{seed}{editValue}")) {
                    found = true;
                    break;
                }
            }
            Assert.That(found);
        }

        [Test]
        public void DeleteProductList() {
            TestSeed seed = new();
            DeleteProductList(seed);
        }

        public void DeleteProductList(TestSeed seed) {
            CreateProductList(seed);

            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            Assert.That(Utility.FindText(Driver.FindElement(By.Id("pla-productlists-tbody")), $"{seed}"));

            var deleteButtons = Driver.FindElements(By.ClassName("pla-productlists-delete"));
            var deleteButton = deleteButtons.FirstOrDefault();
            Assert.That(deleteButton, Is.Not.Null);

            deleteButton.Click();
            Assert.That(Driver.Url, Does.Contain($"{Root}ProductLists/Delete"));

            var submitButton = Driver.FindElement(By.Id("pla-delete-productlist-submit"));
            Assert.That(submitButton, Is.Not.Null);
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo($"{Root}ProductLists"));

            IWebElement? tbody = null;
            try { tbody = Driver.FindElement(By.Id("pla-productlists-tbody")); } catch {}
            
            if (tbody == null) {
                Assert.Pass();
                return;
            }

            Assert.That(Utility.FindText(tbody, $"{seed}"));
        }

        [Test]
        public void AddProduct() {
            TestSeed seed = new();
            AddProduct(seed);
        }

        public void AddProduct(TestSeed seed) {
            CreateProductList(seed);

            var tbody = Driver.FindElement(By.Id("pla-productlists-tbody"));

            Assert.That(tbody, Is.Not.Null);

            var column = Driver.FindElements(By.ClassName("pla-productlists-col-1")).FirstOrDefault();
            Assert.That(column?.TagName, Is.EqualTo("td"));

            var listLink = column.FindElements(By.XPath(".//*")).FirstOrDefault();
            Assert.That(listLink?.TagName, Is.EqualTo("a"));

            listLink.Click();

            //Assert.That(Driver.Url, Does.Contain($"listName=list_{seed.Value}"));

            int count = 0;
            try {
                tbody = Driver.FindElement(By.Id("pla-products-tbody"));
                count = tbody.FindElements(By.XPath(".//*")).Count();
            }
            catch {}

            var nameInput = Driver.FindElement(By.Id("pla-products-new-name"));
            var priceInput = Driver.FindElement(By.Id("pla-products-new-price"));
            var quantityInput = Driver.FindElement(By.Id("pla-products-new-quantity"));

            Assert.That(nameInput, Is.Not.Null);
            Assert.That(priceInput, Is.Not.Null);
            Assert.That(quantityInput, Is.Not.Null);

            string name = $"product_{seed.Value}";
            string price = "\b\b\b\b21,37";
            string quantity = "23";

            nameInput.SendKeys(name);
            priceInput.SendKeys(price);
            quantityInput.SendKeys(quantity);

            var submitButton = Driver.FindElement(By.Id("pla-products-new-submit"));
            Assert.That(submitButton, Is.Not.Null);

            submitButton.Click();
            //Assert.That(Driver.Url, Does.Contain($"listName=list_{seed.Value}"));

            tbody = Driver.FindElement(By.Id("pla-products-tbody"));
            Assert.That(tbody.FindElements(By.XPath(".//*")).Count(), Is.GreaterThan(count));

        }

        [Test]
        public void EditProduct() {
            TestSeed seed = new();
            EditProduct(seed);
        }

        public void EditProduct(TestSeed seed) {
            AddProduct(seed);

            Assert.That(Driver.Url, Does.Contain($"{Root}Products"));

            var editButtons = Driver.FindElements(By.ClassName("pla-products-edit"));
            var editButton = editButtons.FirstOrDefault();
            Assert.That(editButton, Is.Not.Null);

            editButton.Click();
            Assert.That(Driver.Url, Does.Contain($"{Root}Products/Edit"));

            string name = $"edit_{seed}";
            string price = "21,37";
            string quantity = "3617";

            var nameInput = Driver.FindElement(By.Id("pla-edit-product-name"));
            var priceInput = Driver.FindElement(By.Id("pla-edit-product-price"));
            var quantityInput = Driver.FindElement(By.Id("pla-edit-product-quantity"));

            nameInput.Clear();
            nameInput.SendKeys(name);

            priceInput.Clear();
            priceInput.SendKeys(price);

            quantityInput.Clear();
            quantityInput.SendKeys(quantity);

            var submitButton = Driver.FindElement(By.Id("pla-edit-product-submit"));
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo($"{Root}Products"));

            var tbody = Driver.FindElement(By.Id("pla-products-tbody"));

            Assert.That(Utility.FindText(tbody, name));
            Assert.That(Utility.FindText(tbody, price));
            Assert.That(Utility.FindText(tbody, quantity));
        }

        [Test]
        public void DeleteProduct() {
            TestSeed seed = new();
            DeleteProduct(seed);
        }

        public void DeleteProduct(TestSeed seed) {
            AddProduct(seed);

            Assert.That(Driver.Url, Does.Contain($"{Root}Products"));

            Assert.That(Utility.FindText(Driver.FindElement(By.Id("pla-products-tbody")), $"product_{seed}"));

            var deleteButtons = Driver.FindElements(By.ClassName("pla-products-delete"));
            var deleteButton = deleteButtons.FirstOrDefault();
            Assert.That(deleteButton, Is.Not.Null);

            deleteButton.Click();
            Assert.That(Driver.Url, Does.Contain($"{Root}Products/Delete"));

            var submitButton = Driver.FindElement(By.Id("pla-delete-product-submit"));
            submitButton.Click();

            Assert.That(Driver.Url, Is.EqualTo($"{Root}Products"));

            IWebElement? tbody = null;
            try { tbody = Driver.FindElement(By.Id("pla-products-tbody")); } catch { Assert.Pass(); }

            Assert.That(Utility.FindText(tbody, $"product_{seed}"), Is.EqualTo(false));
        }
    }
}