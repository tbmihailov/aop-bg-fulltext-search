using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOPSearch.Crawler.Helpers
{
    public static class ExtractionHelpers
    {

        internal static bool IsVyzlojitelLabel(string p)
        {
            string input = p.Trim().ToLower();
            string valueFilter = "Възложител:";
            return input.Contains(valueFilter.ToLower());
        }

        internal static bool IsPoluchenNaLabel(string p)
        {
            string input = p.Trim().ToLower();
            string valueFilter = "Получен на:";
            return input.Contains(valueFilter.ToLower());
        }

        internal static DateTime? ExtractDateFromPoluchenNa(string p)
        {
            DateTime? parsedDate = null;
            try
            {

                /// <summary>
                ///  Regular expression built for C# on: Сб, Февруари 15, 2014, 04:18:37 
                ///  Using Expresso Version: 3.0.4750, http://www.ultrapico.com
                ///  
                ///  A description of the regular expression:
                ///  
                ///  [1]: A numbered capture group. [\d{2}\.\d{2}\.\d{4}]
                ///      \d{2}\.\d{2}\.\d{4}
                ///          Any digit, exactly 2 repetitions
                ///          Literal .
                ///          Any digit, exactly 2 repetitions
                ///          Literal .
                ///          Any digit, exactly 4 repetitions
                ///  Any character
                ///  Any character in this class: [г|Г]
                ///  Literal .
                ///  
                ///
                /// </summary>
                Regex dateBgRegex = new Regex(
                      "(\\d{2}\\.\\d{2}\\.\\d{4})",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );

                if (dateBgRegex.IsMatch(p))
                {
                    string date = dateBgRegex.Matches(p)[0].Groups[0].Value;
                    parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }

            }
            catch (Exception e)
            {
            }
            return parsedDate;

        }

        internal static bool IsProceduraLabel(string p)
        {
            string input = p.Trim().ToLower();
            string valueFilter = "Процедура:";
            return input.Contains(valueFilter.ToLower());
        }

        internal static bool IsPrepiskaLabel(string p)
        {
            string input = p.Trim().ToLower();
            string valueFilter = "Преписка:";
            return input.Contains(valueFilter.ToLower());
        }

        internal static string ExtractNumberFromPrepiska(string p)
        {
            string result = string.Empty;
            try
            {
 Regex dateBgRegex = new Regex(
                      "\\d{5}\\-\\d{4}\\-\\d{4}",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );

                if (dateBgRegex.IsMatch(p))
                {
                    string number = dateBgRegex.Matches(p)[0].Value;
                    result = number;
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        internal static bool IsImeLabel(string p)
        {
            string input = p.Trim().ToLower();
            string valueFilter = "Име:";
            return input.Contains(valueFilter.ToLower());
        }

        internal static bool IsOpisanieLabel(string p)
        {
            string input = p.Trim().ToLower();
            string valueFilter = "Описание:";
            return input.Contains(valueFilter.ToLower());
        }

        internal static string ExtractUrlFromValueElem(HtmlAgilityPack.HtmlNode valueElem)
        {
            string html = valueElem.InnerHtml;

            Regex urlRegex = new Regex(
                     "href=(?<!\\\\)\"(.*?)(?<!\\\\)\"",
                   RegexOptions.IgnoreCase
                   | RegexOptions.Multiline
                   | RegexOptions.IgnorePatternWhitespace
                   | RegexOptions.Compiled
                   );

            string url = string.Empty;
            if (urlRegex.IsMatch(html))
            {
                url = urlRegex.Matches(html)[0].Groups[1].Value;
                url = url.Replace("&amp;", "&");
            }

            return url;

            //string url = string.Empty;
            //var node = valueElem.SelectNodes("//a").FirstOrDefault();
            //if (node != null)
            //{
            //    url = node.Attributes["href"].Value;
            //}
            //return url;
        }

        /// <summary>
        /// Url ot vida case2.php?mode=show_case&case_id=308167
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string ExtractCaseIdFromUrl(string url)
        {
            Regex caseIdRegex = new Regex(
                     "case_id=(\\d+)",
                   RegexOptions.IgnoreCase
                   | RegexOptions.Multiline
                   | RegexOptions.IgnorePatternWhitespace
                   | RegexOptions.Compiled
                   );

            string caseId= string.Empty;
            if (caseIdRegex.IsMatch(url))
            {
                caseId = caseIdRegex.Matches(url)[0].Groups[1].Value;
            }

            return caseId;
        }

        internal static string ExtractStatusFromPrepiska(string p)
        {
            Regex statusRegex = new Regex(
                    @"\((.+)\)",
                  RegexOptions.IgnoreCase
                  | RegexOptions.Multiline
                  | RegexOptions.IgnorePatternWhitespace
                  | RegexOptions.Compiled
                  );

            string status = string.Empty;
            if (statusRegex.IsMatch(p))
            {
                status = statusRegex.Matches(p)[0].Groups[1].Value;
            }

            return status;
        }
    }
}
