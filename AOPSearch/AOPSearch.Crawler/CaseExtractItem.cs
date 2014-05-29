using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOPSearch.Crawler
{
    public class CaseExtractItem
    {
        public string Assigner { get; set; }

        public DateTime? Recieved { get; set; }

        public string CaseNumber { get; set; }

        public string Name { get; set; }

        public string CaseDescr { get; set; }

        public string Url { get; set; }

        public string CaseId { get; set; }

        public string CaseStatus { get; set; }
    }
}
