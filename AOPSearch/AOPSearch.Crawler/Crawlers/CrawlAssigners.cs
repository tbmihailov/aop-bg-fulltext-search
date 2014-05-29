using Common.Utils.Logging;
using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AOPSearch.Crawler.Crawlers
{
    public class CrawlAssigners
    {
        public static void LoadAssignersAndSaveToFile()
        {
            List<List<string>> allAssignersList = new List<List<string>>();

            int maxPages = 267;
            for (int i = 0; i < maxPages; i++)
            {
                CookieContainer cookieJar = new CookieContainer();

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://rop3-app1.aop.bg:7778/portal/page?_pageid=93,158255&_dad=portal&_schema=PORTAL");
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0";
                req.Method = "POST";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.KeepAlive = true;
                req.Headers.Add("Keep-Alive: 300");
                req.AllowAutoRedirect = false;
                req.ContentType = "application/x-www-form-urlencoded";

                req.CookieContainer = cookieJar;
                string cookie = @"__utma=106444544.975927958.1386279125.1392228798.1392241472.10; __utmz=106444544.1392241472.10.10.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided); portal=9.0.3+en-us+us+AMERICA+F23998F8CAC23520E040A8C00C0A4E13+34DC1F683A8A0E27A84F63B81A816DD8DBC712F27972F14F9FC96011AEB5E0729D40EF57D7FE71D04683BD322D882CB86EABAE57CAB0F5B3EB9E56A7BB38E44DF49755C0558DC3FC10AF5E27EB9665048416B90D13070A4C; ORACLE_SMP_CHRONOS_GL=79:1392228785:673082; SSO_ID=v1.2~1~8FFB0C4B363A17285E4056B1D7A55A88963745FA0F6E862C18FA0B5FE5D69A0581D78C79363258EE7CD5B638A97ADB3352570F3F6CBD9C0477C6AB6459EE1215582D172C2669D9072172328EE0F3F27FE238073F80D4559673C2938B3F0ACF2B57287CE291A9542EB4D0690E0D18FEF847B275428DC7EF2EE5FD66F33F2FA75E4946093E8BC09C53776B660532D52AA1A368DC60F86F62ACDDCAF801F8135C21CC051BCE9F62C69C1502BD98569F95733AFC2B3E1AA5E66920065F727ADCD79EDFE8790E651868E11B19391C2259CCB338B2ACD185795F6FA529913898AB26C1EF59F3032D5A3307; __utmb=106444544.15.10.1392241472; __utmc=106444544";
                cookieJar.SetCookies(new Uri("rop3-app1.aop.bg:7778"), cookie);


                int szGoPage = i;

                Logger.Log(Logger.LogLevel.INFO,string.Format("Page {0}/{1}", i, maxPages));
                StreamWriter sw = new StreamWriter(req.GetRequestStream());
                sw.Write(string.Format("zSGoPg={0}&zSNameOpt=&zSPNumOpt=0&zSTypeOpt=3", szGoPage));
                sw.Close();

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                TextReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                //string tmp = reader.ReadToEnd();

                HtmlDocument doc = new HtmlDocument();
                doc.Load(reader);

                //var table = doc.DocumentNode.SelectSingleNode("//table[@id='rop_table']");
                List<List<string>> table = doc.DocumentNode.SelectNodes("//table[@id='rop_table']")
                            .Descendants("tr")
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .ToList();

                allAssignersList.AddRange(table);
            }

            string outputfile = "vazlozhiteli.csv";
            using (var textWriter = new StreamWriter(outputfile, false, Encoding.UTF8))
            {
                var csv = new CsvWriter(textWriter);
                foreach (var row in allAssignersList)
                {
                    csv.WriteRecord<AssignerItem>(new AssignerItem()
                    {
                        Field0 = row[0],
                        Field1 = row[1],
                        Field2 = row[2],
                        Field3 = row[3],
                    });

                }
            }

        }

        class AssignerItem
        {
            public string Field0 { get; set; }
            public string Field1 { get; set; }
            public string Field2 { get; set; }
            public string Field3 { get; set; }
        }

    }
}
