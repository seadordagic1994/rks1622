using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CondorExtreme3.ModelsLocalDB;
using CondorExtreme3.DAL;
using CondorExtreme3.Helper;
using Newtonsoft.Json;

namespace CondorExtreme3.ModelsLocalDB
{
    public class ProjectionVM
    {
        
        public List<Projection> Projections { get; set; } = new List<Projection>();
        public Dictionary<int, List<string[]>> d { get; set; } = new Dictionary<int, List<string[]>>();

        public string MyDictionaryToJson(Dictionary<int, List<string[]>> dict)
        {           
            return JsonConvert.SerializeObject(dict);
        }
       public  bool didUMean { get; set; } = false;

        public ProjectionVM()
        {
            
        }
    }
}