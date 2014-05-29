using AOPSearch.Crawler.Helpers;
using Common.Utils.Logging;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AOPSearch.Crawler.Crawlers
{
    public class CaseCrawler
    {
        public CaseCrawler()
        {

        }

        public static List<CaseExtractItem> CrawlCases(int maxPages, string cookie, bool continueOnException)
        {
            List<CaseExtractItem> caseExtractItems = new List<CaseExtractItem>();
            try
            {

                for (int i = 0; i < maxPages; i++)
                {
                    int pageId = i + 1;
                    string address = string.Format("http://www.aop.bg/esearch.php?ss_type=1&mode=search&_page={0}", pageId);

                    CookieContainer cookieJar = new CookieContainer();
                    HttpWebRequest req = GenerateRequestToAOP(address, cookie, cookieJar);

                    Logger.Log(Logger.LogLevel.INFO, string.Format("Page {0}/{1}", pageId, maxPages));


                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();

                     TextReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1251));
                    //TextReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8, false);
                    string tmp = reader.ReadToEnd();
                    string utf8String = TextHelpers.ToUtf8(tmp);
                    byte[] utf8Bytes = TextHelpers.ToUtf8Binary(tmp);
                    Stream stream = new MemoryStream(utf8Bytes);
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(stream, Encoding.UTF8);

                    string docStr = doc.DocumentNode.InnerHtml;

                    //var table = doc.DocumentNode.SelectSingleNode("//table[@id='rop_table']");
                    var table = doc.DocumentNode.SelectNodes("//table[@id='resultaTable']")
                                .Descendants("tr")
                                .Where(tr => tr.Elements("td").Count() > 1)
                                .Select(tr => tr.Elements("td").Select(td => td).ToList()).ToList();


                    CaseExtractItem currentItem = null;
                    foreach (var row in table)
                    {
                        var labelElem = row[0];
                        var valueElem = row[1];
                        if (ExtractionHelpers.IsVyzlojitelLabel(labelElem.InnerText))
                        {
                            if (currentItem != null)
                            {
                                caseExtractItems.Add(currentItem);
                            }
                            currentItem = new CaseExtractItem();
                            currentItem.Assigner = valueElem.InnerText;
                        }
                        else if (ExtractionHelpers.IsPoluchenNaLabel(labelElem.InnerText))
                        {
                            if (currentItem != null)
                            {
                                DateTime? recievedDate = ExtractionHelpers.ExtractDateFromPoluchenNa(valueElem.InnerText);
                                currentItem.Recieved = recievedDate;
                            }
                        }
                        else if (ExtractionHelpers.IsPrepiskaLabel(labelElem.InnerText))
                        {
                            if (currentItem != null)
                            {
                                string number = ExtractionHelpers.ExtractNumberFromPrepiska(valueElem.InnerText);
                                currentItem.CaseNumber = number;
                                string status = ExtractionHelpers.ExtractStatusFromPrepiska(valueElem.InnerText);
                                currentItem.CaseStatus = status;
                                string url = ExtractionHelpers.ExtractUrlFromValueElem(valueElem);
                                currentItem.Url = string.Format("http://aop.bg/{0}",url);
                                string caseId = ExtractionHelpers.ExtractCaseIdFromUrl(url);
                                currentItem.CaseId = caseId;
                            }
                        }
                        else if (ExtractionHelpers.IsImeLabel(labelElem.InnerText))
                        {
                            if (currentItem != null)
                            {
                                string name = valueElem.InnerText;
                                currentItem.Name = name;
                            }
                        }
                        else if (ExtractionHelpers.IsOpisanieLabel(labelElem.InnerText))
                        {
                            if (currentItem != null)
                            {
                                string description = valueElem.InnerText;
                                currentItem.CaseDescr = description;
                            }
                        }
                    }

                    if (currentItem != null)
                    {
                        caseExtractItems.Add(currentItem);
                    }
                }

            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogLevel.ERROR, e.ToString());
                if (!continueOnException)
                {
                    throw e;
                }
                Logger.Log(Logger.LogLevel.INFO, "Continue anyway");
            }

            return caseExtractItems;
        }

        private static HttpWebRequest GenerateRequestToAOP(string address, string cookie, CookieContainer cookieJar)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0";
            req.Method = "GET";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.KeepAlive = true;
            req.Headers.Add("Keep-Alive: 300");
            req.AllowAutoRedirect = false;
            //req.ContentType = "application/x-www-form-urlencoded";

            req.CookieContainer = cookieJar;

            cookieJar.SetCookies(new Uri("http://www.aop.bg"), cookie);
            return req;
        }

        public static List<CaseExtractItem> LoadCasesFromFile(string jsonFileName)
        {
            List<CaseExtractItem> casesList = null;
            using (JsonReader reader = new JsonTextReader(new StreamReader(jsonFileName)))
            {
                JsonSerializer deserializer = new JsonSerializer();
                casesList = deserializer.Deserialize<List<CaseExtractItem>>(reader);
            }
            return casesList;
        }

        public static void SaveCasesToFile(List<CaseExtractItem> allCases, string jsonFileName)
        {
            using (var streamWriter = new StreamWriter(jsonFileName, false, Encoding.UTF8))
            {
                JsonWriter writer = new JsonTextWriter(streamWriter);
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, allCases);
            }
        }
    }
}
