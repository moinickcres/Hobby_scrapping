using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using ScrapySharp.Html;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebScrapper.Service;
using System.Linq;

namespace WebScrapper.Controllers
{
    // Model to handle incoming JSON request
    public class ScrapeRequest
    {
        public string SearchTerm { get; set; }
    }

    [Route("api/scrape")]
    [ApiController]
    public class ScraperController : ControllerBase
    {
        private readonly ScrappingLogic _logic;
        public ScraperController(ScrappingLogic _logic) 
        { 
            this._logic = _logic;
        }

        [HttpPost]
        public async Task<IActionResult> Scrape([FromBody] ScrapeRequest request)
        {
            // Define all scraping tasks
            List<Task<Product>> scrapingTasks = new List<Task<Product>>
            {
                _logic.ScrapeMadridHifi(request.SearchTerm),
                _logic.ScrapeMalaga8(request.SearchTerm),
                _logic.ScrapeThomann(request.SearchTerm)
            };

            /*var madridHifiProduct = await _logic.ScrapeMadridHifi(request.SearchTerm);
            var malaga8Product = await _logic.ScrapeMalaga8(request.SearchTerm);
            var thomannProduct = await _logic.ScrapeThomann(request.SearchTerm);*/

            // Combine all products into a list
            //var products = new List<Product> { madridHifiProduct, malaga8Product, thomannProduct };
            var products = new List<Product>();
            products = (await Task.WhenAll(scrapingTasks)).ToList();

            // Filter out any null values in case a product is not found
            var nonNullProducts = products.Where(p => p != null).ToList();

            if (nonNullProducts.Any())
            {
                return Ok(nonNullProducts);
            }
            else
            {
                return NotFound("No matching product found.");
            }
        }

        /*private async Task<List<Product>> ScrapeThomann(string searchTerm)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run in headless mode for background scraping

            using (IWebDriver driver = new ChromeDriver(options))
            {
                string url = $"https://www.thomann.de/gb/search_dir.html?sw={searchTerm}";
                driver.Navigate().GoToUrl(url);

                List<Product> products = new List<Product>();
                var productElements = driver.FindElements(By.CssSelector(".product-list-item")); // Adjust selector based on site structure

                foreach (var element in productElements)
                {
                    // Retrieve specific product details
                    string imageUrl = element.FindElement(By.CssSelector("img")).GetAttribute("src");
                    string articleName = element.FindElement(By.CssSelector(".product-title")).Text;
                    bool inStock = element.FindElement(By.CssSelector(".availability")).Text.Contains("In stock");
                    string priceText = element.FindElement(By.CssSelector(".price")).Text;
                    string productLink = element.FindElement(By.CssSelector("a")).GetAttribute("href");

                    // Convert price text to decimal if necessary
                    decimal price = decimal.TryParse(priceText.Replace("$", ""), out var parsedPrice) ? parsedPrice : 0;

                    products.Add(new Product
                    {
                        ImageUrl = imageUrl,
                        ArticleName = articleName,
                        InStock = inStock,
                        Price = price,
                        Link = productLink,
                        PageName = "Thomann"
                    });
                }

                driver.Quit();
                return products;
            }
        }*/
    }
}