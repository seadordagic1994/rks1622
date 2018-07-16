using CondorExtreme3_API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/CinemaStatistics")]
    public class CinemaStatisticsController : ApiController
    {
        public CondorDBXEntities principal = new CondorDBXEntities();
        

        
        [HttpPost]
        [Route("TotalProfit")]
        public IHttpActionResult TotalProfit(StatisticsParameters statisticsParameters)
        {
            
            var fltMovies = new List<int>();
           var fltTechTypes = new List<int>();
            var CinemaID = statisticsParameters.CinemaID;
            var MovieID = statisticsParameters.MovieID;
            var TechTypeID = statisticsParameters.TechTypeID;
            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;

            fltMovies.AddRange((MovieID == 0) ? principal.Movies.Select(x => x.MovieID).ToList() : principal.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
           fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var profit = (from T in principal.Tickets
                         join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                         join M in principal.Movies on P.MovieID equals M.MovieID
                         join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                         join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                         join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                         where P.DateTimeStart >= DateTimeFrom
                                && P.DateTimeStart <= DateTimeTo
                                && fltMovies.Contains(M.MovieID)
                                && fltTechTypes.Contains(TT.TechnologyTypeID)
                                && CS.CinemaID == CinemaID
                         group T by new
                         { 
                             T.Projections.DateTimeStart.Day,
                             T.Projections.DateTimeStart.Month,
                             T.Projections.DateTimeStart.Year

                         } into Tgroup
                         orderby Tgroup.Key.Day
                         select new
                         {
                             DayMonth = Tgroup.Key.Day.ToString()+"."+Tgroup.Key.Month.ToString(),
                             SumOfTicketPrice = Tgroup.Sum(x => x.TicketPrice)
                         }).ToDictionary(t=>t.DayMonth, t=>t.SumOfTicketPrice);

            if (profit.Count == 0)
                return NotFound();
            return Ok(profit);
        }

        [HttpPost]
        [Route("TotalProfitByPaymentMethod")]
        public IHttpActionResult TotalProfitByPaymentMethod(StatisticsParameters statisticsParameters)
        {
            var fltMovies = new List<int>();
            var fltTechTypes = new List<int>();
            var CinemaID = statisticsParameters.CinemaID;
            var MovieID = statisticsParameters.MovieID;
            var TechTypeID = statisticsParameters.TechTypeID;
            var PaymentMethodID = statisticsParameters.PaymentMethodID;

            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;

            fltMovies.AddRange((MovieID == 0) ? principal.Movies.Select(x => x.MovieID).ToList() : principal.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var profitSpecific = (from T in principal.Tickets
                          join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                          join M in principal.Movies on P.MovieID equals M.MovieID
                          join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                          join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                          join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                          join R in principal.Reservations on T.ReservationID equals R.ReservationID
                          join PM in principal.PaymentMethods on R.PaymentMethodID equals PM.PaymentMethodID
                          where P.DateTimeStart >= DateTimeFrom
                                 && P.DateTimeStart <= DateTimeTo
                                 && fltMovies.Contains(M.MovieID)
                                 && fltTechTypes.Contains(TT.TechnologyTypeID)
                                 && CS.CinemaID == CinemaID
                                 && PM.PaymentMethodID==PaymentMethodID
                          group T by new
                          {
                              T.Projections.DateTimeStart.Day,
                              T.Projections.DateTimeStart.Month,
                              T.Projections.DateTimeStart.Year

                          } into Tgroup
                          orderby Tgroup.Key.Day
                          select new
                          {
                              DayMonth = Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString(),
                              SumOfTicketPrice = Tgroup.Sum(x => x.TicketPrice)
                          }).ToDictionary(t => t.DayMonth, t => t.SumOfTicketPrice);

            if (profitSpecific.Count == 0)
                return NotFound();
            return Ok(profitSpecific);
        }

        [HttpPost]
        [Route("TotalProfitByMovie")]
        public IHttpActionResult TotalProfitByMovie(StatisticsParameters statisticsParameters)
        {
            var fltTechTypes = new List<int>();
            var CinemaID = statisticsParameters.CinemaID;     
            var TechTypeID = statisticsParameters.TechTypeID;
            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;
          
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var profitByMovie = (from T in principal.Tickets
                          join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                          join M in principal.Movies on P.MovieID equals M.MovieID
                          join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                          join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                          join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                          where P.DateTimeStart >= DateTimeFrom
                                 && P.DateTimeStart <= DateTimeTo  
                                 && fltTechTypes.Contains(TT.TechnologyTypeID)
                                 && CS.CinemaID == CinemaID
                          group T by T.Projections.Movies into Tgroup
                          orderby Tgroup.Sum(x => x.TicketPrice) descending
                          select new
                          {
                              MovieName = Tgroup.Key.Name,
                              SumOfTicketPrice = Tgroup.Sum(x => x.TicketPrice)
                          }).ToDictionary(t => t.MovieName, t => t.SumOfTicketPrice);

            if (profitByMovie.Count == 0)
                return NotFound();
            return Ok(profitByMovie);
        }


        [HttpPost]
        [Route("TotalProfitByTechnologyType")]
        public IHttpActionResult TotalProfitByTechnologyType(StatisticsParameters statisticsParameters)
        {
            var fltMovies = new List<int>();           
            var CinemaID = statisticsParameters.CinemaID;
            var MovieID = statisticsParameters.MovieID;
            var TechTypeID = statisticsParameters.TechTypeID;
            

            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;

            fltMovies.AddRange((MovieID == 0) ? principal.Movies.Select(x => x.MovieID).ToList() : principal.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());          

            var profitTechType = (from T in principal.Tickets
                                  join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                                  join M in principal.Movies on P.MovieID equals M.MovieID
                                  join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                                  join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                                  join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID                                            
                                  where P.DateTimeStart >= DateTimeFrom
                                         && P.DateTimeStart <= DateTimeTo
                                         && fltMovies.Contains(M.MovieID)     
                                         && CS.CinemaID == CinemaID
                                         && TT.TechnologyTypeID==TechTypeID
                                  group T by new
                                  {
                                      T.Projections.DateTimeStart.Day,
                                      T.Projections.DateTimeStart.Month,
                                      T.Projections.DateTimeStart.Year

                                  } into Tgroup
                                  orderby Tgroup.Key.Day
                                  select new
                                  {
                                      DayMonth = Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString() + "." + Tgroup.Key.Year.ToString(),
                                      SumOfTicketPrice = Tgroup.Sum(x => x.TicketPrice)
                                  }).ToDictionary(t => t.DayMonth, t => t.SumOfTicketPrice);


            


            Dictionary<DateTime, decimal> profitFinalConvert = new Dictionary<DateTime, decimal>();
            foreach (var item in profitTechType)
            {              
                    string [] dayMonthYear = item.Key.Split('.');
                    string temp = String.Empty;
                    foreach (var s in dayMonthYear)
                    {
                        if (s.Length == 1)
                            temp += "0" + s+".";
                        else
                            temp += s + ".";
                    }
                    string Final = temp.Substring(0, 10);
                    profitFinalConvert.Add(DateTime.ParseExact(Final, "dd.MM.yyyy", CultureInfo.InvariantCulture), item.Value);                             
            }

            if (profitFinalConvert.Count == 0)
                return NotFound();
            return Ok(profitFinalConvert);
        }

        [HttpPost]
        [Route("TotalProfitByMovieGenre")]
        public IHttpActionResult TotalProfitByMovieGenre(StatisticsParameters statisticsParameters)
        {          
            var fltTechTypes = new List<int>();
            var CinemaID = statisticsParameters.CinemaID;    
            var TechTypeID = statisticsParameters.TechTypeID;
            var GenreID = statisticsParameters.GenreID;

            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;

         
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var profitGenres = (from T in principal.Tickets
                                  join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                                  join M in principal.Movies on P.MovieID equals M.MovieID
                                  join G in principal.Genres on M.GenreID equals G.GenreID
                                  join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                                  join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                                  join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID                                           
                                  where P.DateTimeStart >= DateTimeFrom
                                         && P.DateTimeStart <= DateTimeTo
                                         && fltTechTypes.Contains(TT.TechnologyTypeID)
                                         && CS.CinemaID == CinemaID
                                         && G.GenreID==GenreID
                                  group T by new
                                  {
                                      T.Projections.DateTimeStart.Day,
                                      T.Projections.DateTimeStart.Month,
                                      T.Projections.DateTimeStart.Year


                                  } into Tgroup
                                  orderby Tgroup.Key.Day
                                  select new
                                  {
                                      DayMonth =Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString()+"."+ Tgroup.Key.Year.ToString(),
                                      SumOfTicketPrice = Tgroup.Sum(x => x.TicketPrice)
                                  }).ToDictionary(t => t.DayMonth, t =>t.SumOfTicketPrice);

            Dictionary<DateTime, decimal> profitFinalConvert = new Dictionary<DateTime, decimal>();
            foreach (var item in profitGenres)
            {
                string[] dayMonthYear = item.Key.Split('.');
                string temp = String.Empty;
                foreach (var s in dayMonthYear)
                {
                    if (s.Length == 1)
                        temp += "0" + s + ".";
                    else
                        temp += s + ".";
                }
                string Final = temp.Substring(0, 10);
                profitFinalConvert.Add(DateTime.ParseExact(Final,"dd.MM.yyyy",CultureInfo.InvariantCulture), item.Value);

            }

            if (profitFinalConvert.Count == 0)
                return NotFound();
            return Ok(profitFinalConvert);
        }
        [HttpPost]
        [Route("Top10SeatReservationFrequency")]
        public IHttpActionResult Top10SeatReservationFrequency(StatisticsParameters statisticsParameters)
        {
            
            var CinemaID = statisticsParameters.CinemaID;
            var TechTypeID = statisticsParameters.TechTypeID;
            var MovieID = statisticsParameters.MovieID;

            var fltMovies = new List<int>();
            var fltTechTypes = new List<int>();

          


            fltMovies.AddRange((MovieID == 0) ? principal.Movies.Select(x => x.MovieID).ToList() : principal.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());
   
            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;


           

            var Frequency = (from T in principal.Tickets
                                  join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                                  join M in principal.Movies on P.MovieID equals M.MovieID
                                  join G in principal.Genres on M.GenreID equals G.GenreID
                                  join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                                  join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                                  join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                               where P.DateTimeStart >= DateTimeFrom
                                && P.DateTimeStart <= DateTimeTo
                                && fltMovies.Contains(M.MovieID)
                                && fltTechTypes.Contains(TT.TechnologyTypeID)
                                && CS.CinemaID == CinemaID
                              
                             group T by T.Seats into SeatGroup
                             orderby SeatGroup.Count() descending
                             select new
                             {
                                 SeatLbl = SeatGroup.Key.CinemaHalls.Name + " - seat " + SeatGroup.Key.SeatRows.SeatRowLbl + SeatGroup.Key.SeatColumns.SeatColumnLbl.ToString(),
                                 SeatFreq = SeatGroup.Count()
                             }).Take(10).ToDictionary(t=>t.SeatLbl, t=>t.SeatFreq);

            if (Frequency.Count == 0)
                return NotFound();
            return Ok(Frequency);
        }


        [HttpPost]
        [Route("TotalTicketsSold")]
        public IHttpActionResult TotalTicketsSold(StatisticsParameters statisticsParameters)
        {

            var CinemaID = statisticsParameters.CinemaID;
            var TechTypeID = statisticsParameters.TechTypeID;
            var MovieID = statisticsParameters.MovieID;

            var fltMovies = new List<int>();
            var fltTechTypes = new List<int>();




            fltMovies.AddRange((MovieID == 0) ? principal.Movies.Select(x => x.MovieID).ToList() : principal.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            DateTime DateTimeFrom = statisticsParameters.DateTimeFrom;
            DateTime DateTimeTo = statisticsParameters.DateTimeTo;




            var countTickets = (from T in principal.Tickets
                          join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                          join M in principal.Movies on P.MovieID equals M.MovieID
                          join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                          join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                          join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                          where P.DateTimeStart >= DateTimeFrom
                                 && P.DateTimeStart <= DateTimeTo
                                 && fltMovies.Contains(M.MovieID)
                                 && fltTechTypes.Contains(TT.TechnologyTypeID)
                                 && CS.CinemaID == CinemaID
                          group T by new
                          {
                              T.Projections.DateTimeStart.Day,
                              T.Projections.DateTimeStart.Month,
                              T.Projections.DateTimeStart.Year

                          } into Tgroup
                          orderby Tgroup.Key.Day
                          select new
                          {
                              DayMonth = Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString(),
                              SumOfTicketPrice = Tgroup.Count()
                          }).ToDictionary(t => t.DayMonth, t => t.SumOfTicketPrice);

            if (countTickets.Count == 0)
                return NotFound();
            return Ok(countTickets);
        }

        [HttpPost]
        [Route("EmployeeSalary")]
        public IHttpActionResult EmployeeSalary(ReportingParameters reportingParameters)
        {
            int CinemaID = reportingParameters.CinemaID;
            bool Gender = reportingParameters.Gender;

            var Employments = principal.Employments.ToList()
                .Where(x => !x.IsDeleted &&
                           x.Employees.CinemaID == CinemaID &&
                           !x.Employees.IsDeleted &&
                           x.Employees.Gender==Gender
                           ).ToList();

            if (Employments.Count == 0)
                return BadRequest();

            var allEmps= new List<dynamic>();

            foreach (var item in Employments)
            {
                allEmps.Add(new
                {
                    item.EmployeeID,
                    FullName=$"{item.Employees.FirstName} {item.Employees.LastName}",
                    Gender = (!item.Employees.Gender) ? "Male" : "Female",
                    item.EmploymentID,
                    EmploymentType= item.EmploymentTypes.Name,
                    CurrentSalary=item.CurrentSalary
                });
            }
            return Ok(allEmps);
        }

        [HttpPost]
        [Route("ProfitByMovies")]
        public IHttpActionResult ProfitByMovies(ReportingParameters reportingParameters)
        {
            var fltTechTypes = new List<int>();
            var CinemaID = reportingParameters.CinemaID;
            var TechTypeID = reportingParameters.TechTypeID;
            DateTime DateTimeFrom = reportingParameters.DateTimeFrom;
            DateTime DateTimeTo = reportingParameters.DateTimeTo;

            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var profitByMovie = (from T in principal.Tickets
                                 join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                                 join M in principal.Movies on P.MovieID equals M.MovieID
                                 join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                                 join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                                 join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                                 where P.DateTimeStart >= DateTimeFrom
                                        && P.DateTimeStart <= DateTimeTo
                                        && fltTechTypes.Contains(TT.TechnologyTypeID)
                                        && CS.CinemaID == CinemaID
                                 group T by T.Projections.Movies into Tgroup
                                 orderby Tgroup.Sum(x => x.TicketPrice) descending
                                 select new
                                 {
                                     MovieID = Tgroup.Key.MovieID,
                                     Name=Tgroup.Key.Name,
                                     Genre=Tgroup.Key.Genres.Name,
                                     Director=Tgroup.Key.MovieDirectors.FirstName+" "+ Tgroup.Key.MovieDirectors.LastName,
                                     Total = Tgroup.Sum(x => x.TicketPrice)

                                 }).Take(10).
                                 ToDictionary(t => t.MovieID, t => new {
                                     t.Name, t.Genre, t.Director, t.Total
                                 });

            if (profitByMovie.Count == 0)
                return NotFound();
            return Ok(profitByMovie);
        }

        [HttpPost]
        [Route("ProfitByPaymentMethods")]
        public IHttpActionResult ProfitByPaymentMethods(ReportingParameters reportingParameters)
        {
          
            var CinemaID = reportingParameters.CinemaID; 
            DateTime DateTimeFrom = reportingParameters.DateTimeFrom;
            DateTime DateTimeTo = reportingParameters.DateTimeTo;
            int TechTypeID = reportingParameters.TechTypeID;

            var payMethods = principal.PaymentMethods.ToList();

            var dctprofitByPaymentMethods = new Dictionary<string, Dictionary<string, decimal>>();

            var fltTechTypes = new List<int>();
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() 
                                         : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            foreach (var item in payMethods)
            {

                var DICTprofitByPaymentMethods = (from T in principal.Tickets
                                              join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                                              join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                                              join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                                              join R in principal.Reservations on T.ReservationID equals R.ReservationID
                                              join PM in principal.PaymentMethods on R.PaymentMethodID equals PM.PaymentMethodID
                                              join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                                              where P.DateTimeStart >= DateTimeFrom
                                                     && P.DateTimeStart <= DateTimeTo
                                                     && CS.CinemaID == CinemaID
                                                     && PM.PaymentMethodID == item.PaymentMethodID
                                                     && fltTechTypes.Contains(TT.TechnologyTypeID)
                                              group T by new
                                              {
                                                  T.Projections.DateTimeStart.Day,
                                                  T.Projections.DateTimeStart.Month,
                                                  T.Projections.DateTimeStart.Year

                                              } into Tgroup
                                              orderby Tgroup.Key.Day
                                              select new
                                              {

                                                  DateDayMonth = Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString(),
                                                  Total = Tgroup.Sum(x => x.TicketPrice)
                                              }).ToDictionary(t => t.DateDayMonth, t => t.Total);
                 
                    dctprofitByPaymentMethods.Add(
 
                                            item.Name,
                                        
                                        DICTprofitByPaymentMethods);
            }

            if (dctprofitByPaymentMethods.Count == 0)
                return NotFound();


            return Ok(dctprofitByPaymentMethods);
        }
        [HttpPost]
        [Route("SeatReservationFrequencyReporting")]
        public IHttpActionResult SeatReservationFrequencyReporting(ReportingParameters reportingParameters)
        {

            var CinemaID = reportingParameters.CinemaID;
            var TechTypeID = reportingParameters.TechTypeID;
            DateTime DateTimeFrom = reportingParameters.DateTimeFrom;
            DateTime DateTimeTo = reportingParameters.DateTimeTo;
   
            var fltTechTypes = new List<int>();  
            fltTechTypes.AddRange((TechTypeID == 0) ? principal.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : principal.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var dctSeatReservationFrequency = (from T in principal.Tickets
                             join P in principal.Projections on T.ProjectionID equals P.ProjectionID
                             join M in principal.Movies on P.MovieID equals M.MovieID
                             join G in principal.Genres on M.GenreID equals G.GenreID
                             join TT in principal.TechnologyTypes on P.TechnologyTypeID equals TT.TechnologyTypeID
                             join CHS in principal.CinemaHalls on P.CinemaHallID equals CHS.CinemaHallID
                             join CS in principal.Cinemas on CHS.CinemaID equals CS.CinemaID
                             where P.DateTimeStart >= DateTimeFrom
                              && P.DateTimeStart <= DateTimeTo
                              && fltTechTypes.Contains(TT.TechnologyTypeID)
                              && CS.CinemaID == CinemaID

                             group T by T.Seats into SeatGroup
                             orderby SeatGroup.Count() descending
                             select new
                             {
                                 SeatID=SeatGroup.Key.SeatID,
                                 SeatLabel=SeatGroup.Key.SeatRows.SeatRowLbl+SeatGroup.Key.SeatColumns.SeatColumnLbl,
                                 CinemaHall=SeatGroup.Key.CinemaHalls.Name,
                                 TotalFrequency = SeatGroup.Count()
                             }).Take(10)             
                             .ToDictionary(t => t.SeatID, t=>new {
                                SeatLabel=t.SeatLabel,
                                CinemaHall = t.CinemaHall,
                                TotalFrequency=t.TotalFrequency
                             });

            if (dctSeatReservationFrequency.Count == 0)
                return NotFound();
            return Ok(dctSeatReservationFrequency);
        }

        public class StatisticsParameters
        {
            public int CinemaID { get; set; }
            public int MovieID { get; set; }
            public int TechTypeID { get; set; }
            public DateTime DateTimeFrom { get; set; }
            public DateTime DateTimeTo { get; set; }

            public int PaymentMethodID { get; set; }
            public int GenreID { get; set; }
        }

        public class ReportingParameters
        {
            public int CinemaID { get; set; }
            public bool Gender { get; set; }           
            public int TechTypeID { get; set; }
            public DateTime DateTimeFrom { get; set; }
            public DateTime DateTimeTo { get; set; }

        }




        [Route("GetMovies")]
        public IHttpActionResult GetMovies()
        {
            var listMovies = principal.Movies.ToList().Select(x => new
            {
                MovieID = x.MovieID,
                Name = x.Name
            }).ToList();

            if (listMovies.Count == 0)
                return NotFound();

            var finalList = new List<dynamic>();
            finalList.Add(new { MovieID = 0, Name = "(All Movies)" });
            finalList.AddRange(listMovies);

            return Ok(finalList);

        }

        [Route("GetTechnologyTypes")]
        public IHttpActionResult GetTechnologyTypes()
        {
            var listTechTypes = principal.TechnologyTypes.ToList().Select(x => new
            {
                TechnologyTypeID = x.TechnologyTypeID,
                Name = x.Name
            }).ToList();

            if (listTechTypes.Count == 0)
                return NotFound();

            var finalList = new List<dynamic>();
            finalList.Add(new { TechnologyTypeID = 0, Name = "(All Technology Types)" });
            finalList.AddRange(listTechTypes);

            return Ok(finalList);

        }

        [HttpGet]
        [Route("GetPaymentMethods")]
        public IHttpActionResult GetPaymentMethods()
        {
            var PaymentMethods = principal.PaymentMethods.ToList();
            if (PaymentMethods.Count == 0)
                return NotFound();


            return Ok(PaymentMethods.Select(x=> new {
                PaymentMethodID=x.PaymentMethodID,
                Name=x.Name
            }));

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (principal != null)
                {
                    principal.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    }
}
