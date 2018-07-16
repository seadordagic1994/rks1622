using CondorExtreme3_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Newtonsoft.Json;
using CondorExtreme3_API.Helper;
using System.Threading.Tasks;

namespace CondorExtreme3_API.Controllers
{
    [RoutePrefix("api/CashRegister")]
    public class CashRegisterController : ApiController
    {
        public CondorDBXEntities context = new CondorDBXEntities();

        [HttpGet]
        [Route("GetProjections/{cinemaId}")]
        public IHttpActionResult GetProjections(int cinemaId)
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime today = DateTime.Now;
            List<Projections> projection = context.Projections.Where(x => DbFunctions.TruncateTime(x.DateTimeStart) == DbFunctions.TruncateTime(today) && x.CinemaHalls.CinemaID == cinemaId).
                                                               GroupBy(x => x.MovieID).Select(g => g.FirstOrDefault()).ToList();

            if (projection == null || projection.Count == 0)
            {
                return NotFound();
            }

            return Ok(projection.Select(x => new
            {
                ProjectionId = x.ProjectionID,
                Name = x.Movies.Name,
                ZaglavljeCount = context.Zaglavlje.OrderByDescending(t => t.ZaglavljeId).FirstOrDefault() == null ? 1 : context.Zaglavlje.OrderByDescending(t => t.ZaglavljeId).First().ZaglavljeId
            }));
        }

        [Route("GetProjectionsForToday/{projectionId}")]
        public IHttpActionResult GetProjectionsForToday(int projectionId)
        {
            int movieId = context.Projections.Where(x => x.ProjectionID == projectionId).FirstOrDefault().MovieID;

            DateTime today = DateTime.Now;
            List<Projections> projection = context.Projections.Where(x => DbFunctions.TruncateTime(x.DateTimeStart) == DbFunctions.TruncateTime(today) && x.MovieID == movieId).ToList();
            Dictionary<int, KeyValuePair<DateTime, string>> starts = new Dictionary<int, KeyValuePair<DateTime, string>>();

            foreach (Projections p in projection)
            {
                starts.Add(p.ProjectionID, new KeyValuePair<DateTime, string>(p.DateTimeStart, p.TechnologyTypes.Name));
            }

            if (projection == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                pStarts = starts
            });
        }

        [Route("GetSeatsForProjection/{projectionId}")]
        public IHttpActionResult GetSeatsForProjection(int projectionId)
        {
            List<int> Reservations = context.Reservations.Where(x => x.ProjectionID == projectionId).Select(x => x.ReservationID).ToList();
            int CinemaHallId = context.Projections.Where(x => x.ProjectionID == projectionId).FirstOrDefault().CinemaHallID;

            Dictionary<int, Dictionary<Tuple<int, int>, bool>> Seats = new Dictionary<int, Dictionary<Tuple<int, int>, bool>>();


            foreach (Seats s in context.Seats.Where(x => x.CinemaHallID == CinemaHallId && !x.IsDeleted))
            {
                if (Seats.ContainsKey(s.SeatRowID))
                {
                    Seats[s.SeatRowID].Add(new Tuple<int, int>(s.SeatID, s.SeatColumnID), false);
                }
                else
                {
                    Seats.Add(s.SeatRowID, new Dictionary<Tuple<int, int>, bool>());
                    Seats[s.SeatRowID].Add(new Tuple<int, int>(s.SeatID, s.SeatColumnID), false);
                }
            }

            foreach (Tickets t in context.Tickets.Where(x => x.ProjectionID == projectionId).ToList())
            {
                Seats[t.Seats.SeatRowID][new Tuple<int, int>(t.SeatID, t.Seats.SeatColumnID)] = true;
            }




            if (Seats == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                pSeats = Seats,
                cinemaHall = context.Projections.Find(projectionId).CinemaHalls.Name,
            });
        }
        [HttpGet]
        [Route("PenddingSeats/{projectionId}")]
        public IHttpActionResult PenddingSeats(int projectionId)
        {
            Projections p = context.Projections.Find(projectionId);

            if (p == null)
                return NotFound();

            return Ok(new
            {
                projectionId = p.ProjectionID,
                MovieName = p.Movies.Name,
                Price = String.Format("{0:0.00}", p.TicketPrice),
            });
        }
        [HttpPost]
        [Route("MakeTickets")]
        public IHttpActionResult MakeTickets(dynamic BillItems)
        {
            List<int> selectedSeats = new List<int>();
            List<dynamic> Items = new List<dynamic>();

            int ProjectionId = BillItems.projectionId;

            selectedSeats = JsonConvert.DeserializeObject<List<int>>(BillItems.seats.ToString());
            Items = JsonConvert.DeserializeObject<List<dynamic>>(BillItems.dynamicItems.ToString());

            if (context.Zaglavlje.Find(int.Parse(Items[0].HeaderId.ToString())) == null)
            {
                Zaglavlje z = new Zaglavlje();
                z.ZaposlenikID = 26;
                z.Knjizen = false;
                z.Datum = DateTime.Now;

                context.Zaglavlje.Add(z);
                context.SaveChanges();


                Reservations r = new Reservations();
                if (selectedSeats.Count != 0)
                {                    
                    r.InitialDate = DateTime.Now;
                    r.IsCompleted = 1;
                    r.ProjectionID = ProjectionId;

                    context.Reservations.Add(r);
                    context.SaveChanges();
                }

                decimal price = context.Projections.Find(ProjectionId).TicketPrice;

                foreach (int seatId in selectedSeats)
                {
                    context.Tickets.Add(new Tickets
                    {
                        ProjectionID = ProjectionId,
                        ReservationID = r.ReservationID,
                        SeatID = seatId,
                        TicketNumber = (context.Tickets.Count() + 1),
                        TicketPrice = price,
                    });
                    context.SaveChanges();
                }

                foreach (dynamic d in Items)
                {
                    Stavke s = new Stavke();
                    s.ZaglavljeId = int.Parse(z.ZaglavljeId.ToString());

                    if (d.Identification.ToString() == "I") {
                        s.CijenovnikId = int.Parse(d.Id.ToString());
                        s.RezervacijaID = null;
                        Cjenovnik DecreaseAmount = context.Cjenovnik.Where(x => x.CjenovnikId == s.CijenovnikId).FirstOrDefault();
                        DecreaseAmount.Koliciina = DecreaseAmount.Koliciina - int.Parse(d.Amount.ToString());
                    }
                    else {
                        s.RezervacijaID = r.ReservationID;
                        s.CijenovnikId = null;
                    }

                    s.Kolicina = int.Parse(d.Amount.ToString());
                    s.PDV = Convert.ToDecimal(0.17);
                    s.Cijena = Convert.ToDecimal(d.Price.ToString());
                    s.Iznos = (s.Kolicina * s.Cijena) + (s.Kolicina * s.Cijena * s.PDV);

                    context.Stavke.Add(s);
                    context.SaveChanges();
                }

                return Ok(new
                {
                   zaglavljeId =  z.ZaglavljeId,
                });
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPost]
        [Route("AddFicscalNubmer")]
        public IHttpActionResult AddFicscalNubmer(dynamic d)
        {
            int ZaglavljeId = d.zaglavljeId;
            int fisklni = d.fiskalniBroj;

            Zaglavlje ZUpdateFiskal = context.Zaglavlje.Where(x => x.ZaglavljeId == ZaglavljeId).FirstOrDefault();
            if (ZUpdateFiskal != null)
            {
                ZUpdateFiskal.FiskalniRacun = ZaglavljeId;
                context.SaveChanges();
            }

            if (ZUpdateFiskal == null)
                return NotFound();

            return Ok(new { ZaglavljeId = ZaglavljeId });
        }

        [Route("GetBillItems")]
        public IHttpActionResult GetBillItems()
        {
            Dictionary<int, dynamic> billItems = new Dictionary<int, dynamic>();

            foreach (int c in context.Cjenovnik.Select(x => x.GrupaId).ToList().Distinct())
            {
                billItems.Add(c, context.Cjenovnik.Where(x => x.GrupaId == c).Select(t => new { Id = t.CjenovnikId, Naziv = t.Artikli.Naziv, Price = t.Cijena, Image = (t.Artikli.Image) }).ToList());
            }

            return Ok(billItems);
        }

        [HttpGet]
        [Route("FindFiscal/{ZaglavljeId}")]
        public IHttpActionResult FindFiscal(int ZaglavljeId)
        {
            Zaglavlje z = context.Zaglavlje.Where(x => x.ZaglavljeId == ZaglavljeId && x.FiskalniRacun != null && x.ReklamiraniRacun == null).FirstOrDefault();

            if (z == null)
                return NotFound();


            return Ok(new { Id = z.FiskalniRacun, Datum = z.Datum });
        }

        [HttpGet]
        [Route("FiscalReclamation/{FiscalNumber}")]
        public IHttpActionResult FiscalReclamation(int FiscalNumber)
        {
            List<dynamic> Items = new List<dynamic>();

            Zaglavlje z = context.Zaglavlje.Where(x => x.FiskalniRacun == FiscalNumber).FirstOrDefault();

            Zaglavlje novoZaglavlje = new Zaglavlje();
            novoZaglavlje.FiskalniRacun = FiscalNumber;
            novoZaglavlje.ZaposlenikID = 26;
            novoZaglavlje.Datum = DateTime.Now;
            novoZaglavlje.Knjizen = false;

            context.Zaglavlje.Add(novoZaglavlje);
            context.SaveChanges();
            int RemoveTicketsReservationId = 0;


            foreach (Stavke s in context.Stavke.Where(x => x.ZaglavljeId == z.ZaglavljeId).ToList())
            {
                Stavke novaStavka = new Stavke();
                novaStavka.ZaglavljeId = novoZaglavlje.ZaglavljeId;
                novaStavka.PDV = 0.17m;
                novaStavka.Kolicina = -s.Kolicina;
                novaStavka.Cijena = s.Cijena;
                novaStavka.Iznos = novaStavka.Kolicina * novaStavka.Cijena;
                if (s.RezervacijaID != null)
                {
                    novaStavka.RezervacijaID = s.RezervacijaID;
                    RemoveTicketsReservationId = int.Parse(s.RezervacijaID.ToString());
                    List<int> sjedista = context.Tickets.Where(x => x.ReservationID == s.RezervacijaID).Select(x => x.SeatID).ToList();
                    Items.Add(
                        new
                        {
                            Indicator = "P",
                            ProjectionId = s.Reservations.ProjectionID,
                            Naziv = context.Projections.Find(s.Reservations.ProjectionID).Movies.Name,
                            Cijena = s.Cijena,
                            Kolicina = s.Kolicina,
                            Iznos = s.Iznos,
                            seats = sjedista,

                        });
                }
                else
                {
                    novaStavka.CijenovnikId = s.CijenovnikId;
                    context.Cjenovnik.Where(x => x.CjenovnikId == s.CijenovnikId).FirstOrDefault().Koliciina += s.Kolicina;
                    context.SaveChanges();

                    Items.Add(
                        new
                        {
                            Indicator = "I",
                            StavkaId = s.CijenovnikId,
                            Naziv = s.Cjenovnik.Artikli.Naziv,
                            Cijena = s.Cijena,
                            Kolicina = s.Kolicina,
                            Iznos = s.Iznos,
                        });
                }

                context.Stavke.Add(novaStavka);
                context.SaveChanges();

            }

            if (RemoveTicketsReservationId != 0)
            {
                List<Tickets> tickets = context.Tickets.Where(x => x.ReservationID == RemoveTicketsReservationId).ToList();

                foreach (Tickets t in tickets)
                    context.Tickets.Remove(t);
                context.SaveChanges();

            }

            return Ok(Items);
        }
        [HttpGet]
        [Route("GetReservationFromCamera/{reservationId}")]
        public IHttpActionResult GetReservationFromCamera(int reservationId)
        {
            int? projectionId = context.Reservations.Where(x => x.ReservationID == reservationId).FirstOrDefault().ProjectionID;
            Projections p = context.Projections.Find(projectionId);
            List<int> sjedista = context.Tickets.Where(x => x.ReservationID == reservationId).Select(x => x.SeatID).ToList();

            dynamic d = new
            {
                Indicator = "P",
                ProjectionId = projectionId,
                Naziv = context.Projections.Find(projectionId).Movies.Name,
                Cijena = p.TicketPrice,
                Kolicina = context.Tickets.Where(x => x.ReservationID == reservationId).Count(),
                Iznos = (p.TicketPrice * context.Tickets.Where(x => x.ReservationID == reservationId).Count()),
                seats = sjedista,
            }; 

            return Ok(d);
        }


        [HttpGet]
        [Route("TotalInRegister/{empId}")]
        public IHttpActionResult TotalInRegister(int empId)
        {
            DateTime today = DateTime.Now;
            decimal s = context.Stavke.Where(x => DbFunctions.TruncateTime(x.Zaglavlje.Datum) == DbFunctions.TruncateTime(today))
                                                  .Sum(x=> x.Iznos);
            if (s == 0)
            {
                return NotFound();
            }
            return Ok(s);
        }
        [HttpPost]
        [Route("DailyReportCashRegister")]
        public IHttpActionResult DailyReportCashRegister(dynamic parameters)
        {
            int cinemaId = parameters.cinemaId;
            int empId = parameters.EmpId;
            DateTime day = parameters.date;
            List<Stavke> s = context.Stavke.Where(x => x.Zaglavlje.ZaposlenikID == empId &&
                                                  DbFunctions.TruncateTime(x.Zaglavlje.Datum) == DbFunctions.TruncateTime(day)).
                                                  OrderBy(x=> x.ZaglavljeId).                                                  
                                                  ToList();

            List<dynamic> Items = new List<dynamic>();
            decimal sum = 0;
            foreach (Stavke item in s)
            {
                if (item.RezervacijaID != null)
                Items.Add(new
                {
                    ZaglavljeId = item.ZaglavljeId,
                    Name = context.Projections.Where(x=> x.ProjectionID== item.Reservations.ProjectionID).FirstOrDefault().Movies.Name,
                    Kolicina = item.Kolicina,
                    Cijena = item.Cijena,
                    Iznos = item.Iznos,
                    Datum = item.Zaglavlje.Datum
                });
                else
                    Items.Add(new
                    {
                        ZaglavljeId = item.ZaglavljeId,
                        Name = item.Cjenovnik.Artikli.Naziv,
                        Kolicina = item.Kolicina,
                        Cijena = item.Cijena,
                        Iznos = item.Iznos,
                        Datum = item.Zaglavlje.Datum
                    });
                sum += item.Iznos;
            }            

            return Ok(new { Items, sum });
        }
    }
}
