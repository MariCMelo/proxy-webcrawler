using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

namespace Webcrawler
{
    public class WebCrawler
    {
        private readonly ChromeOptions options;
        private readonly List<Dictionary<string, object>> rowsList = new List<Dictionary<string, object>>();
        private readonly List<string> htmlList = new List<string>();
        private DateTime executionStart;
        private DateTime executionEnd;
        private int lastNumber;
        private int totalLines;
        private MongoDbConnector mongodb;

        public WebCrawler(ChromeOptions options)
        {
            this.options = options;
            this.mongodb = new MongoDbConnector("mongodb://localhost:27017", "webcrawlerLogs");
        }

        public void GetPagesSource(string url)
        {   // Record the start time of execution
            executionStart = DateTime.Now;

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
                lastNumber = int.Parse(lastPageNumber.InnerText.Trim());

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
            //Check if the "htmls" directory exists; if not, create it
            string directoryPath = "htmls";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save each HTML page string to a file
            for (int i = 0; i < htmlList.Count; i++)
            {
                string html = htmlList[i];
                string fileName = $"page_{i + 1}.html";
                string filePath = Path.Combine(directoryPath, fileName);

                // Save the HTML to an .html file.
                File.WriteAllText(filePath, html);
                // Console.WriteLine($"HTML {i + 1} salvo em: {filePath}");
            }
        }

        public void ExtractProxyInfo()
        {
            var htmlDocument = new HtmlDocument();
            string html = "";

            //Extract proxy informations from HTML and group them.
            for (int i = 0; i < htmlList.Count; i++)
            {
                html = htmlList[i];
                htmlDocument.LoadHtml(html);

                var ipAddressElements = htmlDocument.DocumentNode.SelectNodes("//a[@class='ajax1 action-dialog-ajax-inact action-modal-ajax-inact']");
                var portElements = htmlDocument.DocumentNode.SelectNodes("//span[@class='port']");
                var countryElements = htmlDocument.DocumentNode.SelectNodes("//img[@class='icon-flag']");
                var protocolElements = htmlDocument.DocumentNode.SelectNodes("//tr/td[last()-1]");

                GroupProxyInfo(ipAddressElements, portElements, countryElements, protocolElements);
            }
            // Record the total number of lines in the JSON archive
            totalLines = rowsList.Count;

            // Create a JSON file containing proxy information for all pages
            string jsonResult = JsonSerializer.Serialize(rowsList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("All_Proxies.json", jsonResult);

            // Record the end time of execution
            executionEnd = DateTime.Now;
        }

        private void GroupProxyInfo(HtmlNodeCollection ipAddressElements, HtmlNodeCollection portElements, HtmlNodeCollection countryElements, HtmlNodeCollection protocolElements)
        {
            if (ipAddressElements != null && portElements != null && countryElements != null && protocolElements != null)
            {
                //Create objects with proxy informations add them to a list
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

        // Save execution details to MongoDB log
        public void SaveLog()
        {
            Dictionary<string, object> log = new Dictionary<string, object>
            {
                {"start_time", executionStart},
                {"end_time", executionEnd},
                {"total_pages", lastNumber},
                {"total_rows", totalLines}
            };
            mongodb.AddLog(log);
            Console.WriteLine("Process completed successfully!");
        }
    }
}
