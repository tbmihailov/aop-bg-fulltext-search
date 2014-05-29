using AOPSearch.Crawler.Crawlers;
using Microsoft.Practices.ServiceLocation;
using SolrUtils;
using SolrUtils.Models.Binders;
using SolrNet;
using SolrNet.Exceptions;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AOPSearch.Models;
using SolrUtils.Models;

namespace AOPSearch
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //init solr stuff here
            var connection = new SolrConnection(solrURL);
            var loggingConnection = new LoggingConnection(connection);
            Startup.Init<Case>(loggingConnection);

            var connectionForUpdate = new SolrConnection(solrURL);
            var loggingConnectionForUpdate = new LoggingConnection(connectionForUpdate);
            Startup.Init<CaseUpdate>(loggingConnectionForUpdate);


            //RegisterAllControllers();
            //ControllerBuilder.Current.SetControllerFactory(new ServiceProviderControllerFactory(Startup.Container));
            ModelBinders.Binders[typeof(SearchParameters)] = new SearchParametersBinder();
            //AddInitialCaseDocuments();
        }

        private static readonly string solrURL = ConfigurationManager.AppSettings["solrUrl"];

        /// <summary>
        /// Add initial cases
        /// </summary>
        private void AddInitialCaseDocuments()
        {
            try
            {
                string file = Server.MapPath("/StoredData/Cases.json");
                SolrData.RebuildDocumentIndex(file);
            }
            catch (SolrConnectionException)
            {
                throw new Exception(string.Format("Couldn't connect to Solr. Please make sure that Solr is running on '{0}' or change the address in your web.config, then restart the application.", solrURL));
            }
        }

    }
}