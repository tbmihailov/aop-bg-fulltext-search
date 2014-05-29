using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AOPSearch.Models;
using SolrUtils.Models;

namespace AOPSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "AOP Full Text Search";

            return View();
        }

        private readonly ISolrReadOnlyOperations<Case> solr;

        public HomeController()
            : this(ServiceLocator.Current.GetInstance<ISolrOperations<Case>>())
        {

        }
        public HomeController(ISolrReadOnlyOperations<Case> solr)
        {
            this.solr = solr;
        }

        public ActionResult RebuildIndex()
        {
            string file = Server.MapPath("/StoredData/Cases.json");
            SolrData.RebuildDocumentIndex(file);
            return View();
        }

        /// <summary>
        /// Builds the Solr query from the search parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ISolrQuery BuildQuery(SearchParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.FreeSearch))
                return new SolrQuery(parameters.FreeSearch);
            return SolrQuery.All;
        }

        public ICollection<ISolrQuery> BuildFilterQueries(SearchParameters parameters)
        {
            var queriesFromFacets = from p in parameters.Facets
                                    select (ISolrQuery)Query.Field(p.Key).Is(p.Value);
            return queriesFromFacets.ToList();
        }


        /// <summary>
        /// All selectable facet fields
        /// </summary>
        private static readonly string[] AllFacetFields = new[] { "", "" };

        /// <summary>
        /// Gets the selected facet fields
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<string> SelectedFacetFields(SearchParameters parameters)
        {
            return parameters.Facets.Select(f => f.Key);
        }

        public SortOrder[] GetSelectedSort(SearchParameters parameters)
        {
            return new[] { SortOrder.Parse(parameters.Sort) }.Where(o => o != null).ToArray();
        }

        //[HttpGet]
        //public ActionResult IndexSearch()
        //{
        //    return View(new CaseView() { Search = new SearchParameters() });
        //}

        [HttpGet]
        public ActionResult IndexSearch(SearchParameters parameters)
        {
            if (parameters == null)
            {
                return View(new CaseView() { Search = new SearchParameters() });
            }

            try
            {
                var start = (parameters.PageIndex - 1) * parameters.PageSize;
                var matchingProducts = solr.Query(BuildQuery(parameters), new QueryOptions
                {
                    FilterQueries = BuildFilterQueries(parameters),
                    Rows = parameters.PageSize,
                    Start = start,
                    OrderBy = GetSelectedSort(parameters),
                    SpellCheck = new SpellCheckingParameters(),
                    Highlight = new HighlightingParameters() { UsePhraseHighlighter = true},
                    Fields = new List<string> {"*","score"}

                    //Facet = new FacetParameters
                    //{
                    //    Queries = AllFacetFields.Except(SelectedFacetFields(parameters))
                    //                                                          .Select(f => new SolrFacetFieldQuery(f) { MinCount = 1 })
                    //                                                          .Cast<ISolrFacetQuery>()
                    //                                                          .ToList(),
                    //},
                });
                var view = new CaseView
                {
                    Cases = matchingProducts,
                    Search = parameters,
                    TotalCount = matchingProducts.NumFound,
                    Facets = matchingProducts.FacetFields,
                    DidYouMean = GetSpellCheckingResult(matchingProducts),
                };
                return View(view);
            }
            catch (InvalidFieldException)
            {
                return View(new CaseView
                {
                    QueryError = true,
                });
            }
        }

        private string GetSpellCheckingResult(SolrQueryResults<Case> products)
        {
            return string.Join(" ", products.SpellChecking
                                        .Select(c => c.Suggestions.FirstOrDefault())
                                        .Where(c => !string.IsNullOrEmpty(c))
                                        .ToArray());
        }
    }
}
