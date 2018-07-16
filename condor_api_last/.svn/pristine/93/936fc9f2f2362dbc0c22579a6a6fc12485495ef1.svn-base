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
    public class SeatsVM
    {

        public Projection Projections { get; set; } = new Projection();
        public List<ProjectionSeat> ProjectionSeat { get; set; } = new List<ProjectionSeat>();
        public List<SeatRow> SeatRows { get; set; } = new List<SeatRow>();
        public Dictionary<int, List<string[]>> d { get; set; } = new Dictionary<int, List<string[]>>();

        public string MyDictionaryToJson(Dictionary<int, List<string[]>> dict)
        {

            return JsonConvert.SerializeObject(dict);
        }
        public char GetLetter(int broj)
        {
            return (char)broj;
        }
        private CondorDBContextChild _myVar;


        public SeatsVM()
        {
            using (CondorDBContextChild principal = new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString()))
            {
                SeatRows = principal.SeatRows.ToList();
            }
        }
    }
}