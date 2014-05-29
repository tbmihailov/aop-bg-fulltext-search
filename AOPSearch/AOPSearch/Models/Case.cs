using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPSearch.Models
{
    public class Case
    {
        //<field name="id" type="string" indexed="true" stored="true" required="true" multiValued="false" /> 
        [SolrUniqueKey("id")]
        public string Id { get; set; }
        //<field name="case_number" type="text_en_splitting_tight" indexed="true" stored="true" omitNorms="true"/>
        [SolrUniqueKey("url")]
        public string Url { get; set; }
        [SolrField("case_number")]
        public string Number { get; set; }
        //<field name="name" type="text_general" indexed="true" stored="true"/>
        [SolrField("name")]
        public string Name { get; set; }
        //<field name="assigner" type="text_general" indexed="true" stored="true" omitNorms="true"/>
        [SolrField("assigner")]
        public string Assigner { get; set; }
        //<field name="case_type" type="string" indexed="true" stored="true" multiValued="true"/>
        [SolrField("case_type")]
        public ICollection<string> CaseType { get; set; }
        [SolrField("case_status")]
        public string CaseStatus { get; set; }
        //<field name="case_descr" type="text_general" indexed="true" stored="true" multiValued="true" termVectors="true" termPositions="true" termOffsets="true"/>
        [SolrField("case_descr")]
        //public ICollection<string> Description { get; set; }
        public string Description { get; set; }
        //<field name="max_amount"  type="float" indexed="true" stored="true"/>
        [SolrField("max_amount")]
        public decimal MaxAmount { get; set; }
        //<field name="max_duration"  type="int" indexed="true" stored="true"/>
        [SolrField("max_duration")]
        public int MaxDuration { get; set; }
        //<field name="status" type="string" indexed="true" stored="true" />
        [SolrField("status")]
        public string Status { get; set; }
        //<field name="recieved" type="date" indexed="true" stored="true"/>
        [SolrField("recieved")]
        public DateTime? Recieved { get; set; }
        //<field name="deadline" type="date" indexed="true" stored="true"/>
        [SolrField("deadline")]
        public DateTime? Deadline { get; set; }

        [SolrField("score")]
        public double Score { get; set; }
        //<field name="title" type="text_general" indexed="true" stored="true" multiValued="true"/>
        //<field name="subject" type="text_general" indexed="true" stored="true"/>
        //<field name="description" type="text_general" indexed="true" stored="true"/>
        //<field name="comments" type="text_general" indexed="true" stored="true"/>
        //<field name="author" type="text_general" indexed="true" stored="true"/>
        //<field name="keywords" type="text_general" indexed="true" stored="true"/>
        //<field name="category" type="text_general" indexed="true" stored="true"/>
        //<field name="resourcename" type="text_general" indexed="true" stored="true"/>
        //<field name="url" type="text_general" indexed="true" stored="true"/>
        //<field name="content_type" type="string" indexed="true" stored="true" multiValued="true"/>
        //<field name="last_modified" type="date" indexed="true" stored="true"/>
        //<field name="links" type="string" indexed="true" stored="true" multiValued="true"/>
        //<field name="content" type="text_general" indexed="false" stored="true" multiValued="true"/>

        //<field name="text" type="text_general" indexed="true" stored="false" multiValued="true"/>
    }
}
