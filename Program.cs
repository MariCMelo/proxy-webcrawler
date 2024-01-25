using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Webcrawler
{
    public class WebCrawler
    {
        private readonly ChromeOptions options;
        private readonly List<Dictionary<string, object>> rowsList = new List<Dictionary<string, object>>();
        private readonly List<string> htmlList = new List<string>();

        public WebCrawler(ChromeOptions options)
        {
            this.options = options;
        }

        public void GetPagesSource(string url)
        {
            string originalUrl = url;
            using (ChromeDriver driver = new ChromeDriver(options))
            {
                var htmlDocument = new HtmlDocument();

                driver.Navigate().GoToUrl(url);
                string html = driver.PageSource;
                htmlList.Add(html);
                htmlDocument.LoadHtml(html);

                var pagesNumbers = htmlDocument.DocumentNode.SelectNodes("//li[@class='page-item']");
                var lastPageNumber = pagesNumbers[pagesNumbers.Count - 1];
                int lastNumber = int.Parse(lastPageNumber.InnerText.Trim());

                for (int i = 2; i <= lastNumber; i++)
                {
                    url = originalUrl + "/page/" + i;
                    driver.Navigate().GoToUrl(url);
                    html = driver.PageSource;
                    htmlList.Add(html);
                }
            }
        }

        public void ExportHtml()
        {
            string directoryPath = "htmls";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            for (int i = 0; i < htmlList.Count; i++)
            {
                string html = htmlList[i];
                string fileName = $"page_{i + 1}.html";
                string filePath = Path.Combine(directoryPath, fileName);

                // Salva o HTML em um arquivo .html
                File.WriteAllText(filePath, html);
                Console.WriteLine($"HTML {i + 1} salvo em: {filePath}");
            }
        }

        public void ExtractProxyInfo()
        {
            var htmlDocument = new HtmlDocument();
            string html = "";

            for (int i = 0; i < htmlList.Count; i++)
            {
                html = htmlList[i];
                htmlDocument.LoadHtml(html);

                var ipAddressElements = htmlDocument.DocumentNode.SelectNodes("//a[@class='ajax1 action-dialog-ajax-inact action-modal-ajax-inact']");
                var portElements = htmlDocument.DocumentNode.SelectNodes("//span[@class='port']");
                var countryElements = htmlDocument.DocumentNode.SelectNodes("//img[@class='icon-flag']");
                var protocolElements = htmlDocument.DocumentNode.SelectNodes("//tr/td[last()-1]");

                PrintProxyInfo(ipAddressElements, portElements, countryElements, protocolElements);
            }
            Console.WriteLine(rowsList.Count);

            string jsonResult = JsonSerializer.Serialize(rowsList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("All_Proxies.json", jsonResult);
            // Console.WriteLine(jsonResult);
        }


        private void PrintProxyInfo(HtmlNodeCollection ipAddressElements, HtmlNodeCollection portElements, HtmlNodeCollection countryElements, HtmlNodeCollection protocolElements)
        {
            if (ipAddressElements != null && portElements != null && countryElements != null && protocolElements != null)
            {

                for (int i = 0; i < ipAddressElements.Count; i++)
                {
                    var ipAddress = ipAddressElements[i].InnerText.Trim();
                    var port = portElements[i].InnerText.Trim();
                    var country = countryElements[i].NextSibling.InnerText.Trim();
                    var protocol = protocolElements[i].InnerText.Trim();

                    Dictionary<string, object> row = new Dictionary<string, object>
                    {
                        { "ip_address", ipAddress },
                        { "port", port },
                        { "country", country },
                        { "protocol", protocol }
                    };
                    rowsList.Add(row);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");

            var webCrawler = new WebCrawler(options);
            string url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";
            webCrawler.GetPagesSource(url);
            webCrawler.ExportHtml();
            webCrawler.ExtractProxyInfo();
        }
    }
}
