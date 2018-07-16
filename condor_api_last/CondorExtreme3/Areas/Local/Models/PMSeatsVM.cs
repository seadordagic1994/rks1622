using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CondorExtreme3.ModelsLocalDB;
using System.Web.Mvc;
using CondorExtreme3.DAL;
using Newtonsoft.Json;

namespace CondorExtreme3.Areas.Local.Models
{
    public class PMSeatsVM
    {
        public CinemaHall CinemaHall { get; set; }
        public List<SeatRow> SeatRow { get; set; } = new List<SeatRow>();
         public List<SeatRow> SeatRowCount { get; set; } = new List<SeatRow>();
        public Dictionary<string, List<string>> Seats { get; set; } = new Dictionary<string, List<string>>();

        public string MyDictionaryToJson(Dictionary<string, List<string>> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        public PMSeatsVM()
        {
            using (CondorDBContextChild  principal= new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString()))
            {
                SeatRow = principal.SeatRows.ToList(); 
            }
        }
    }
}