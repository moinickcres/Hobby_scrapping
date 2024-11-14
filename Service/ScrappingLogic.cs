﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebScrapper.Controllers;

namespace WebScrapper.Service
{
    public class Product
    {
        //public ObjectId Id { get; set; } = ObjectId.GenerateNewId(); // Auto-generated by MongoDB
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string PageName { get; set; }
        public string Link { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}\nPrice: {Price:F2}€\nIn Stock: {InStock}\nImage URL: {ImageUrl}\nLink: {Link}\nPage: {PageName}";
        }
    }

    public class ScrappingLogic
    {
        public async Task<Product> ScrapeThomann(string searchTerm)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run in headless mode for background scraping

            // Replace spaces in the search term with "+" for the URL query
            string formattedSearchTerm = searchTerm.Replace(" ", "+");

            string url = $"https://www.thomann.de/es/search_dir.html?sw={formattedSearchTerm}";

            using (IWebDriver driver = new ChromeDriver(options))
            {
                // Navigate to the Madrid Hifi page
                driver.Navigate().GoToUrl(url);

                // Wait a few seconds for content to load if necessary
                await Task.Delay(3000);

                // Find all product cards on the page
                var productElements = driver.FindElements(By.CssSelector(".product"));

                var products = new List<Product>();

                foreach (var productElement in productElements)
                {
                    try
                    {
                        // Get product image
                        var imageUrl = productElement.FindElement(By.CssSelector(".fx-image")).GetAttribute("src");

                        // Get product name
                        var name = productElement.FindElement(By.CssSelector(".title__name")).Text.Trim();

                        // Get product stock
                        bool inStock = driver.FindElements(By.CssSelector(".fx-availability--in-stock")).Count > 0;

                        // Get product price
                        var priceText = driver.FindElement(By.CssSelector(".fx-typography-price-primary")).Text.Replace("€", "").Trim();
                        decimal.TryParse(priceText.Replace(",", "."), out decimal price);

                        // Get product link
                        var link = driver.FindElement(By.CssSelector("a.product__content.no-underline")).GetAttribute("href");

                        // Create a Product object and add it to the list
                        products.Add(new Product
                        {
                            ImageUrl = imageUrl,
                            Name = name,
                            Price = price,
                            InStock = inStock,
                            Link = link,
                            PageName = "Thomann"
                        });
                    }
                    catch (NoSuchElementException ex)
                    {
                        // Handle cases where some elements are missing on the page
                        Console.WriteLine("Error extracting data for a product: " + ex.Message);
                    }
                }

                driver.Quit();

                // Find the product with the closest name match to the user's search term
                return FindClosestMatch(products, searchTerm);
            }
        }

        public async Task<Product> ScrapeMadridHifi(string searchTerm)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run in headless mode for background scraping

            // Replace spaces in the search term with "+" for the URL query
            string formattedSearchTerm = searchTerm.Replace(" ", "+");

            string url = $"https://www.madridhifi.com/#c6df/fullscreen/m=and&q={formattedSearchTerm}";

            using (IWebDriver driver = new ChromeDriver(options))
            {
                // Navigate to the Madrid Hifi page
                driver.Navigate().GoToUrl(url);

                // Wait a few seconds for content to load if necessary
                await Task.Delay(3000);

                // Find all product cards on the page
                var productElements = driver.FindElements(By.CssSelector(".dfd-card.dfd-card-type-product"));

                var products = new List<Product>();

                foreach (var productElement in productElements)
                {
                    try
                    {
                        // Get product image
                        var imageUrl = productElement.FindElement(By.CssSelector(".dfd-card-thumbnail img")).GetAttribute("src");

                        // Get product name
                        var name = productElement.FindElement(By.CssSelector(".dfd-card-title")).Text.Trim();

                        // Get product price (handling sale price if applicable)
                        var priceText = productElement.FindElement(By.CssSelector(".dfd-card-price")).Text.Replace("€", "").Trim();
                        decimal.TryParse(priceText, out decimal price);

                        // Get stock status
                        var inStock = productElement.FindElements(By.CssSelector(".availability_flag"))
                                                   .Any(el => el.Text.Contains("En Stock"));

                        // Get product link
                        var link = productElement.FindElement(By.CssSelector(".dfd-card-link")).GetAttribute("href");

                        // Create a Product object and add it to the list
                        products.Add(new Product
                        {
                            ImageUrl = imageUrl,
                            Name = name,
                            Price = price,
                            InStock = inStock,
                            Link = link,
                            PageName = "Madrid Hifi"
                        });
                    }
                    catch (NoSuchElementException ex)
                    {
                        // Handle cases where some elements are missing on the page
                        Console.WriteLine("Error extracting data for a product: " + ex.Message);
                    }
                }

                driver.Quit();

                // Find the product with the closest name match to the user's search term
                return FindClosestMatch(products, searchTerm);
            }
        }

        public async Task<Product> ScrapeMalaga8(string searchTerm)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run in headless mode for background scraping

            // Replace spaces in the search term with "+" for the URL query
            string formattedSearchTerm = searchTerm.Replace(" ", "+");

            // Build the full URL with the formatted search term
            string url = $"https://www.malaga8.com/#1725/fullscreen/m=and&q={formattedSearchTerm}";

            using (IWebDriver driver = new ChromeDriver(options))
            {
                // Navigate to the Malaga8 search page
                driver.Navigate().GoToUrl(url);

                // Wait a few seconds for content to load if necessary
                await Task.Delay(3000);

                // Find all product cards on the page
                var productElements = driver.FindElements(By.CssSelector(".dfd-card.dfd-card-type-product"));

                var products = new List<Product>();

                foreach (var productElement in productElements)
                {
                    try
                    {
                        // Get product image
                        var imageUrl = productElement.FindElement(By.CssSelector(".dfd-card-thumbnail img")).GetAttribute("src");

                        // Get product name
                        var name = productElement.FindElement(By.CssSelector(".dfd-card-title")).Text.Trim();

                        // Get product price
                        var priceText = productElement.FindElement(By.CssSelector(".dfd-card-price")).Text.Replace("€", "").Trim();
                        decimal.TryParse(priceText.Replace(",", "."), out decimal price);

                        // Check stock status based on presence of 'Agotado' (out of stock)
                        bool inStock = !productElement.FindElements(By.CssSelector(".dfd-card-flag[data-availability='out-of-stock']")).Any();

                        // Get product link
                        var link = productElement.FindElement(By.CssSelector(".dfd-card-link")).GetAttribute("href");

                        // Create a Product object and add it to the list
                        products.Add(new Product
                        {
                            ImageUrl = imageUrl,
                            Name = name,
                            Price = price,
                            InStock = inStock,
                            Link = link,
                            PageName = "Malaga8"
                        });
                    }
                    catch (NoSuchElementException ex)
                    {
                        // Handle cases where some elements are missing on the page
                        Console.WriteLine("Error extracting data for a product: " + ex.Message);
                    }
                }

                driver.Quit();

                // Find the product with the closest name match to the user's search term
                return FindClosestMatch(products, searchTerm);
            }
        }


        // Basic name-matching function to find the most similar product name
        private Product FindClosestMatch(List<Product> products, string searchTerm)
        {
            return products
                .OrderBy(p => GetLevenshteinDistance(p.Name.ToLower(), searchTerm.ToLower()))
                .FirstOrDefault();
        }

        // Levenshtein Distance Algorithm to find the closest match
        private int GetLevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            var d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = t[j - 1] == s[i - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[s.Length, t.Length];
        }
    }
}
