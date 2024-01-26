using OpenQA.Selenium.Chrome;
using System;

namespace Webcrawler
{
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
            webCrawler.SaveLog();
        }
    }
}
