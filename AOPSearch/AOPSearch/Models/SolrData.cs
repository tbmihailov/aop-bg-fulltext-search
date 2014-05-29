using AOPSearch.Crawler.Crawlers;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AOPSearch.Models
{
    public class SolrData
    {
        public static void RebuildDocumentIndex(string file)
        {
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<CaseUpdate>>();
            solr.Delete(SolrQuery.All);
            var connection = ServiceLocator.Current.GetInstance<ISolrConnection>();


            var sampleCasesItems = CaseCrawler.LoadCasesFromFile(file);

            var cases = sampleCasesItems.Select(ci => new CaseUpdate()
            {
                Id = ci.CaseId,
                Assigner = ci.Assigner,
                Url = ci.Url,
                Name = ci.Name,
                Number = ci.CaseNumber,
                Status = ci.CaseStatus,
                Recieved = ci.Recieved,
                Description = ci.CaseDescr, //new List<string>{ ci.CaseDescr},
            }).ToList();

            foreach (var caseItem in cases)
            {
                solr.Add(caseItem);
            }
            solr.Commit();
            solr.BuildSpellCheckDictionary();
        }

    }
}