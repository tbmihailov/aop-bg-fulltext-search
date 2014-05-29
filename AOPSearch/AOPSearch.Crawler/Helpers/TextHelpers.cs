using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPSearch.Crawler.Helpers
{
    public class TextHelpers
    {
        public static string ToUtf8(string tmp)
        {
            string unicodeString = tmp;
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(unicodeString);
            string utf8String = Encoding.UTF8.GetString(encodedBytes);
            return utf8String;
        }

        public static byte[] ToUtf8Binary(string tmp)
        {
            string unicodeString = tmp;
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(unicodeString);
            return encodedBytes;
        }
    }
}
