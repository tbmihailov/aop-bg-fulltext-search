using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using AOPSearch.Crawler.Crawlers;
using AOPSearch.Crawler.Helpers;
using Common.Utils.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AOPSearch.Crawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            int pagesWithListToCrawl = 10;
            string outputFileName = "Cases.json";
            string cookie = @"PHPSESSID=5599c3113c8154746a5a59a9ab0af07e";

            if (args.Length > 0)
            {
                pagesWithListToCrawl = Int32.Parse(args[0]);
            }

            if (args.Length > 1)
            {
                outputFileName = args[1];
            }
            Logger.Log(Logger.LogLevel.DEBUG, string.Format("OutputFile={0}", outputFileName));

            if (args.Length > 2)
            {
                cookie = args[2];
            }
            Logger.Log(Logger.LogLevel.DEBUG, string.Format("Cookie={0}", cookie));

            Logger.Log(Logger.LogLevel.INFO, "Starting");

            Logger.Log(Logger.LogLevel.INFO, string.Format("Crawling {0} pages", pagesWithListToCrawl));
            
            var cases = CaseCrawler.CrawlCases(pagesWithListToCrawl, cookie, true);
            Logger.Log(Logger.LogLevel.INFO, string.Format("Downloaded {0} cases", cases.Count));
            CaseCrawler.SaveCasesToFile(cases, outputFileName);
            Logger.Log(Logger.LogLevel.INFO, string.Format("{0} cases saved to {0}", cases.Count, outputFileName));
            //var loadedCases = CaseCrawler.LoadCasesFromFile("Cases.json");
        }

    }
}
