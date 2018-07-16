using CondorExtreme3.Areas.Local.Models;
using CondorExtreme3.DAL;
using CondorExtreme3.Models;
using CondorExtreme3.ModelsLocalDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using CondorExtreme3.Helper;
using System.Threading.Tasks;
using EntityFramework.BulkInsert.Extensions;

namespace CondorExtreme3.Areas.Local.Controllers
{
    
    [Autorization(Autorization.Permissions.Director)]
    public class DirectorController : Controller
    {
        public CondorDBContextChild principal
        {
            get
            {            
               return new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());
            }
         
        }

        public Dictionary<int,string> DctAnalysisTypes {

            get
            {
                return new Dictionary<int, string>
                {
                    {1,"Total Profit" },
                    {2,"Total Profit By Transaction Type " },
                    {3,"Total Profit By Payment Method Online" },
                    {4,"Total Profit By Movie" },
                    {5,"Total Profit By Technology Type" },
                    {6,"Total Profit By Movie Genre" },
                    {7,"Top 10 Seat Reservation Frequency" },
                    {8,"Total Tickets Sold" }
                };
            }

        }

        


        public Dictionary<int, string> DctBindAnalysisToGraph
        {

            get
            {
                return new Dictionary<int, string>
                {
                    {1, "BarChart"},
                    {2, "AreaChart"},
                    {3, "LineChart"},
                    {4, "DonutChart"},
                    {5, "LineChart"},
                    {6, "AreaChart"},
                    {7, "DonutChart"},
                    {8, "BarChart"}
                };
            }

        }
        public Dictionary<int, MapAnalysisToFilter> DctNumbOfParams
        {

            get
            {
                return new Dictionary<int, MapAnalysisToFilter>
                {
                    {1, new MapAnalysisToFilter { ShowMoviesFilter=true, ShowTechTypesFilter=true} },
                    {2, new MapAnalysisToFilter { ShowMoviesFilter=true, ShowTechTypesFilter=true}},
                    {3, new MapAnalysisToFilter { ShowMoviesFilter=true, ShowTechTypesFilter=true}},
                    {4, new MapAnalysisToFilter { ShowMoviesFilter=false, ShowTechTypesFilter=true}},
                    {5, new MapAnalysisToFilter { ShowMoviesFilter=true, ShowTechTypesFilter=false}},
                    {6, new MapAnalysisToFilter { ShowMoviesFilter=false, ShowTechTypesFilter=true}},
                    {7, new MapAnalysisToFilter { ShowMoviesFilter=true, ShowTechTypesFilter=true}},
                    {8, new MapAnalysisToFilter { ShowMoviesFilter=true, ShowTechTypesFilter=true}}
                };
            }

        }

        public List<string> ColorList {
            get
            {
                return new List<string>()
                {
                    "#000066",
                    "#6600CC",
                    "#CC00CC",
                    "#CC0000",
                    "#606060",
                    "#00FFFF",
                    "#FFFF00",
                    "#006600",
                    "#FF8000",
                    "#000000",
                    "#006633",
                    "#660033",
                    "#404040",
                    "#660033",
                    "#1088F0",
                    "#942994",
                    "#553F55",
                    "#F3F312"
                };
            }                              
        }

        private static String Number2String(int number, bool isCaps)

        {

            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));

            return c.ToString();

        }


        public ActionResult Index()
        {

            StatIndexPageVM Model = new StatIndexPageVM();


            Model.ListAnalysisType = new List<SelectListItem>();

            Model.ListAnalysisType.Add(new SelectListItem { Value = "0", Text = "Choose analysis type" });

            Model.ListAnalysisType.AddRange(DctAnalysisTypes
                .Select(x => new SelectListItem
                    {
                        Value = x.Key.ToString(),
                        Text = x.Value

                    }).ToList());

            Model.ListMovies = new List<SelectListItem>();

            Model.ListMovies.Add(new SelectListItem { Value = "0", Text = "All Movies" });


            Model.ListMovies.AddRange(principal.Movies.Select(x => new SelectListItem
            {
                Value = x.MovieID.ToString(),
                Text = x.MovieName + " (" + x.ReleaseYear + ")"

            }).ToList());

          

            Model.ListTechTypes = new List<SelectListItem>();

            Model.ListTechTypes.Add(new SelectListItem { Value = "0", Text = "All Technology Types" });

            Model.ListTechTypes.AddRange(principal.TechnologyTypes.Select(x => new SelectListItem
            {
                Value = x.TechnologyTypeID.ToString(),
                Text = x.Name

            }).ToList());


            Model.NumberOfEmployees = principal.Employees.ToList().Count;
            Model.NumberOfVisitors = principal.Visitors.ToList().Count;
            Model.NumberOfRegisteredVisitors = principal.RegisteredVisitors.ToList().Count;
            Model.NumberOfReservations = principal.Reservations.ToList().Count;


            var Cinema = principal.Cinemas.FirstOrDefault();

            Model.CinemaName= Cinema.Name+" "+ Cinema.Address.City.Name+" stats";


            JavaScriptSerializer JSS = new JavaScriptSerializer();
            var jsonFormater = DctNumbOfParams.Select(x => new {
                AnalysisTypeID = x.Key,
                Filters = x.Value

            }).ToList();

            ViewBag.FilterMap = JSS.Serialize(jsonFormater);

           
            


            return View(Model);
        }


        public ActionResult EmployeeManagement()
        {



            List<EmployeeShowVM> Model = principal.Employees.Where(x=>x.IsDeleted==false).Select(x => new EmployeeShowVM
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Location = x.City.Name+", "+x.City.Country.Name,
                EmailAddress=x.EmailAddress,
                PhoneNumber=x.PhoneNumber,
                BirthDate=x.BirthDate.Day.ToString()+"."+ x.BirthDate.Month.ToString() + "." + x.BirthDate.Year.ToString(),
                Gender= (x.Gender==true)?"Male":"Female",
                HireDate = x.HireDate.Day.ToString() + "." + x.HireDate.Month.ToString() + "." + x.HireDate.Year.ToString(),
                Salary =x.Salary.ToString()+" BAM",
                Username=x.Username,
                Picture=x.Picture             
            }).ToList();

            foreach (var item in Model)
            {
                item.ERoles = principal.EmployeesRoles.Where(x => x.EmployeeID == item.EmployeeID)
                    .Select(x => x.Role.RoleName).ToList();

            }

            return View("EmployeeManagement", Model);
        }

        public ActionResult DetailsEmployee(int EmployeeID)
        {

            Employee x = principal.Employees.Where(X => X.EmployeeID == EmployeeID).FirstOrDefault();
           

            EmployeeShowVM Model = new EmployeeShowVM()
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Location = x.City.Name + ", " + x.City.Country.Name,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                BirthDate = x.BirthDate.Day.ToString() + "." + x.BirthDate.Month.ToString() + "." + x.BirthDate.Year.ToString(),
                Gender = (x.Gender == true) ? "Male" : "Female",
                HireDate = x.HireDate.Day.ToString() + "." + x.HireDate.Month.ToString() + "." + x.HireDate.Year.ToString(),
                Salary = x.Salary.ToString() + " BAM",
                Username = x.Username              
            };


            Model.ERoles = principal.EmployeesRoles.Where(X => X.EmployeeID == Model.EmployeeID)
                    .Select(Y => Y.Role.RoleName).ToList();

            
            return PartialView("DetailsEmployee", Model);
        }

        public ActionResult EditEmployee(int EmployeeID)
        {

            Employee EMP = principal.Employees.Where(x=> x.EmployeeID == EmployeeID).FirstOrDefault();


            EditEmployeeVM Model = new EditEmployeeVM()
            {
                EmployeeID = EMP.EmployeeID,
                FirstName=EMP.FirstName,
                LastName = EMP.LastName,
                CityID=EMP.CityID,
                EmailAddress=EMP.EmailAddress,
                PhoneNumber=EMP.PhoneNumber,                
                Gender=EMP.Gender,                
                Salary=EMP.Salary
                

            };
            Model.Genders = new List<Gender>
            {
                new Gender { Id=1, Name="Male", IsSelected=(EMP.Gender)? true:false },
                new Gender { Id=2, Name="Female", IsSelected=(EMP.Gender==false)? true:false },

            };



            Model.BirthDate = ((EMP.BirthDate.Month.ToString().Length == 1) ? "0" + EMP.BirthDate.Month.ToString() : EMP.BirthDate.Month.ToString()) +"/"+
                            ((EMP.BirthDate.Day.ToString().Length == 1) ? "0" + EMP.BirthDate.Day.ToString() : EMP.BirthDate.Day.ToString()) +"/"+
                            (EMP.BirthDate.Year.ToString());
            Model.HireDate = ((EMP.HireDate.Month.ToString().Length == 1) ? "0" + EMP.HireDate.Month.ToString() : EMP.HireDate.Month.ToString()) + "/" +
                           ((EMP.HireDate.Day.ToString().Length == 1) ? "0" + EMP.HireDate.Day.ToString() : EMP.HireDate.Day.ToString()) + "/" +
                           (EMP.HireDate.Year.ToString());

            Model.ListCities = new List<SelectListItem>();

        

            Model.ListCities = principal.Cities.Select(x => new SelectListItem
            {
                Value=x.CityID.ToString(),
                Text=x.Name

            }).ToList();


            Model.ProjectionManager = (EMP.EmployeesRoles.Where(x => x.Role.RoleName == "ProjectionManager").Any()) ? true : false;
            Model.Employee = (EMP.EmployeesRoles.Where(x => x.Role.RoleName == "Employee").Any()) ? true : false;

            


            return PartialView("EditEmployee", Model);
        }





        public ActionResult SubmitEditEmployee(EditEmployeeVM Model)
        {

            CondorDBContextChild newconn = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());
            Employee EMP = newconn.Employees.Where(x => x.EmployeeID == Model.EmployeeID).FirstOrDefault();

           

            EMP.FirstName = Model.FirstName;
            EMP.LastName = Model.LastName;
            EMP.CityID = Model.CityID;
            EMP.EmailAddress = Model.EmailAddress;
            EMP.PhoneNumber = Model.PhoneNumber;
            EMP.BirthDate = DateTime.Parse(Model.BirthDate);

            EMP.Gender = (Model.SelectedGender == "1") ? true : false;

            
            EMP.HireDate = DateTime.Parse(Model.HireDate);
            EMP.Salary = Model.Salary;            
            EMP.IsDeleted = false;

            newconn.SaveChanges();

            EmployeeRole PM = newconn.EmployeesRoles.Where(x => x.EmployeeID == Model.EmployeeID && x.Role.RoleName == "ProjectionManager").FirstOrDefault();
            EmployeeRole E = newconn.EmployeesRoles.Where(x => x.EmployeeID == Model.EmployeeID && x.Role.RoleName == "Employee").FirstOrDefault();

            int PMID = newconn.Roles.Where(x => x.RoleName == "ProjectionManager").FirstOrDefault().RoleID;
            int EID = newconn.Roles.Where(x => x.RoleName == "Employee").FirstOrDefault().RoleID;


            if (Model.ProjectionManager)
            {
                if (PM == null)
                {
                    newconn.EmployeesRoles.Add(new EmployeeRole { EmployeeID = Model.EmployeeID, RoleID = PMID, IsDeleted = false });
                }
            }
            else
            {
                if (PM != null)
                {
                    newconn.EmployeesRoles.Remove(PM);
                }
            }

            if (Model.Employee)
            {
                if (E == null)
                {
                    newconn.EmployeesRoles.Add(new EmployeeRole { EmployeeID = Model.EmployeeID, RoleID = EID, IsDeleted = false });
                }
            }
            else
            {
                if (E != null)
                {
                    newconn.EmployeesRoles.Remove(E);
                }
            }



            newconn.SaveChanges();




            return RedirectToAction("EmployeeManagement");
        }

        public ActionResult DeleteEmployee(int EmployeeID)
        {


            Employee Model = principal.Employees.Where(x => x.EmployeeID == EmployeeID).FirstOrDefault();

            return PartialView("DeleteEmployee", Model);
        }

        public ActionResult SubmitDeleteEmployee(Employee Model)
        {
            CondorDBContextChild newConn = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());

            Employee e = newConn.Employees.Where(x => x.EmployeeID == Model.EmployeeID).FirstOrDefault();

            e.IsDeleted = true;

            newConn.SaveChanges();


            return RedirectToAction("EmployeeManagement");
        }

        public ActionResult CreateEmployee()
        {


            EditEmployeeVM Model = new EditEmployeeVM();

            Model.ListCities = new List<SelectListItem>();

            Model.ListCities.AddRange(principal.Cities.Select(x => new SelectListItem
            {
                Value = x.CityID.ToString(),
                Text = x.Name

            }).ToList());


            Model.Genders = new List<Gender>
            {
                new Gender { Id=1, Name="Male", IsSelected=null },
                new Gender { Id=2, Name="Female", IsSelected=null }
            };

            Model.ProjectionManager = false;
            Model.Employee = false;


            return PartialView("CreateEmployee", Model);
        }

        public ActionResult SubmitCreateEmployee(EditEmployeeVM Model)
        {

            CondorDBContextChild newconn = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());

            Employee EMP = new Employee();
            Random R = new Random();


            EMP.FirstName = Model.FirstName;
            EMP.LastName = Model.LastName;
            EMP.CityID = Model.CityID;
            EMP.EmailAddress = Model.EmailAddress;
            EMP.PhoneNumber = EMP.PhoneNumber;
            EMP.BirthDate = DateTime.Parse(Model.BirthDate);

            EMP.Gender = (Model.SelectedGender == "1") ? true : false;

            EMP.HireDate = DateTime.Parse(Model.HireDate);

            EMP.Salary = Model.Salary;


            EMP.Username = EMP.FirstName.ToLower();
            EMP.Password = R.Next(10000000, 99999999).ToString();

            newconn.Employees.Add(EMP);
            newconn.SaveChanges();


            int PMID = newconn.Roles.Where(x => x.RoleName == "ProjectionManager").FirstOrDefault().RoleID;
            int EID = newconn.Roles.Where(x => x.RoleName == "Employee").FirstOrDefault().RoleID;

            if (Model.ProjectionManager)
            {
                newconn.EmployeesRoles.Add(new EmployeeRole { EmployeeID = EMP.EmployeeID, RoleID = PMID, IsDeleted = false });
            }
            if (Model.Employee)
            {
                newconn.EmployeesRoles.Add(new EmployeeRole { EmployeeID = EMP.EmployeeID, RoleID = EID, IsDeleted = false });
            }
            newconn.SaveChanges();

            return RedirectToAction("EmployeeManagement");
        }

        public ActionResult CinemaHallManagement()
        {
            List<CinemaHallShowVM> Model = principal.CinemaHalls.Where(x=>x.IsDeleted==false)
                .Select(x => new CinemaHallShowVM
                {
                    CinemaHallID=x.CinemaHallID,
                    Name=x.Name,
                    NumberOfSeats=x.Seats.ToList().Count
   
                }).ToList();

            foreach (var item in Model)
            {
                item.AllTechnologyTypes = new List<string>();
                item.AllTechnologyTypes.AddRange(principal.CinemaHallTechnologyTypes.Where(x => x.CinemaHallID == item.CinemaHallID).Select(x => x.TechnologyType.Name));

            }
            return View("CinemaHallManagement", Model);
        }

        public ActionResult EditCinemaHall(int CinemaHallID)
        {
            CinemaHall CH = principal.CinemaHalls.Where(x => x.CinemaHallID == CinemaHallID).FirstOrDefault();



            EditCinemaHallVM Model = new EditCinemaHallVM
            {
                CinemaHallID = CH.CinemaHallID,
                CinemaHallName = CH.Name             
            };
            //Model.AllTechnologyTypes = principal.TechnologyTypes.Select(x => new KeyValuePair<string, bool>(x.Name,)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            List<TechnologyType> LstTechTypes = principal.TechnologyTypes.ToList();
            Model.AllTechnologyTypes = new Dictionary<string, bool>();


            foreach (var item in LstTechTypes)
            {
                if (principal.CinemaHallTechnologyTypes.Where(x => x.CinemaHallID == CinemaHallID && x.TechnologyTypeID == item.TechnologyTypeID).FirstOrDefault() != null)
                    Model.AllTechnologyTypes.Add(item.Name, true);
                else
                    Model.AllTechnologyTypes.Add(item.Name, false);
            }
            


            return PartialView("EditCinemaHall", Model);
        }

        public ActionResult DeleteCinemaHall(int CinemaHallID)
        {
            CinemaHall Model = principal.CinemaHalls.Where(x => x.CinemaHallID == CinemaHallID).FirstOrDefault();

            return PartialView("DeleteCinemaHall", Model);


        }

        public ActionResult SubmitDeleteCinemaHall(CinemaHall Model)
        {
            CondorDBContextChild newConn = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());

            CinemaHall CH = newConn.CinemaHalls.Where(x => x.CinemaHallID == Model.CinemaHallID).FirstOrDefault();

            CH.IsDeleted = true;

            newConn.SaveChanges();


            return RedirectToAction("CinemaHallManagement");
        }

        public ActionResult CreateCinemaHall()
        {
            AddCinemaHallVM Model = new AddCinemaHallVM();

            Model.NumberOfSeatRows = 10;
            Model.NumberOfSeatColumns = 10;

            List<TechnologyType> LstTechTypes = principal.TechnologyTypes.ToList();
            Model.AllTechnologyTypes = new Dictionary<string, bool>();


            foreach (var item in LstTechTypes)
                Model.AllTechnologyTypes.Add(item.Name, false);
            




            return PartialView("CreateCinemaHall", Model);
        }
        public ActionResult SubmitCreateCinemaHall(FormCollection frmCollection)
        {
            CondorDBContextChild newconn = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());


            CinemaHall CH = new CinemaHall()
            {
                Name= frmCollection["CinemaHallName"],
                CinemaID= newconn.Cinemas.FirstOrDefault().CinemaID,
                IsDeleted=false
            };

            newconn.CinemaHalls.Add(CH);
            newconn.SaveChanges();

            int CHID = newconn.CinemaHalls.OrderByDescending(x => x.CinemaHallID).FirstOrDefault().CinemaHallID;

            int NumberOfSeatRows = Int32.Parse(frmCollection["NumberOfSeatRows"]);
            int NumberOfSeatColumns = Int32.Parse(frmCollection["NumberOfSeatColumns"]);
            int STID= newconn.SeatTypes.OrderBy(x => x.SeatTypeID).FirstOrDefault().SeatTypeID;

           
            List<Seat> allSeats = (from SR in newconn.SeatRows.Take(NumberOfSeatRows)
                           from SC in newconn.SeatColumns.Take(NumberOfSeatColumns)
                           select new
                           {
                               SR.SeatRowID,
                               SC.SeatColumnID
                           }).ToList().Select(x=> new Seat {
                               CinemaHallID = CHID,
                               SeatRowID=x.SeatRowID,
                               SeatColumnID = x.SeatColumnID,
                               IsDeleted = false,
                               SeatTypeID = STID

                           }).ToList();

            newconn.BulkInsert(allSeats, 1000);
            newconn.SaveChanges();




            List<string> TechT = new List<string>();
            for (int i = 1; i < frmCollection.AllKeys.Length - 2; i++)
                TechT.Add(frmCollection.GetKey(i));


            foreach (var iterator in TechT)
            {
                int TechTypeID = newconn.TechnologyTypes.Where(x => x.Name == iterator).FirstOrDefault().TechnologyTypeID;
                newconn.CinemaHallTechnologyTypes.Add(new CinemaHallTechnologyType { CinemaHallID = CHID, TechnologyTypeID = TechTypeID, IsDeleted = false });
            }


            newconn.SaveChanges();
            return RedirectToAction("CinemaHallManagement");
        }

        public ActionResult SubmitEditCinemaHall(FormCollection frmCollection)
        {

            CondorDBContextChild newconn = new CondorDBContextChild(HttpContext.Session["ConnectionString"].ToString());

            int CHid = Int32.Parse(frmCollection["CinemaHallID"]);

            CinemaHall CH = newconn.CinemaHalls.Where(x => x.CinemaHallID == CHid).FirstOrDefault();


            CH.Name = frmCollection["CinemaHallName"];


            Dictionary<string, bool> TechT = new Dictionary<string, bool>();
            for (int i = 2; i < frmCollection.AllKeys.Length; i++)
                    TechT.Add(frmCollection.GetKey(i), true);

            List<string> tempList = TechT.Keys.ToList();

            foreach (var iterator in newconn.TechnologyTypes.ToList())
                if (!tempList.Contains(iterator.Name))
                    TechT.Add(iterator.Name, false);
                
            



            foreach (var item in TechT)
            {
                int TechTypeIDtemp = newconn.TechnologyTypes.Where(x => x.Name == item.Key).FirstOrDefault().TechnologyTypeID;
                if (item.Value)
                {
                    if (newconn.CinemaHallTechnologyTypes.Where(x => x.CinemaHallID == CHid && x.TechnologyTypeID == TechTypeIDtemp).FirstOrDefault() == null)
                        newconn.CinemaHallTechnologyTypes.Add(new CinemaHallTechnologyType { TechnologyTypeID = TechTypeIDtemp, CinemaHallID = CHid, IsDeleted = false });
                }
                else
                {
                    CinemaHallTechnologyType CHTT = newconn.CinemaHallTechnologyTypes.Where(x => x.CinemaHallID == CHid && x.TechnologyTypeID == TechTypeIDtemp).FirstOrDefault();
                    if (CHTT != null)
                        newconn.CinemaHallTechnologyTypes.Remove(CHTT);
                }
            }

            newconn.SaveChanges();
            return RedirectToAction("CinemaHallManagement");
        }


        public ActionResult PerformCalculations(StatIndexPageVM Model)
        {
            string CharGraph = DctBindAnalysisToGraph[Model.AnalysisTypeKey];

            string s = DctAnalysisTypes[Model.AnalysisTypeKey];
            string MethodName = s.Replace(" ", "");

            Type T = GetType();

            MethodInfo MI = T.GetMethod(MethodName);
            object Instance = Activator.CreateInstance(T);


            int numOfparams = 3;
            if (DctNumbOfParams[Model.AnalysisTypeKey].ShowMoviesFilter)
            {
                numOfparams++;
            }
            
            if (DctNumbOfParams[Model.AnalysisTypeKey].ShowTechTypesFilter)
            {
                numOfparams++;
            }
            object[] parameters = new object[numOfparams];

            parameters[0] = HttpContext.Session["ConnectionString"].ToString();
            parameters[1] = DateTime.Parse(Model.DateTimeFrom);
            parameters[2] = DateTime.Parse(Model.DateTimeTo);

            int temp = 3;
            if (DctNumbOfParams[Model.AnalysisTypeKey].ShowMoviesFilter)
            {
                parameters[temp] = Model.MovieID;
                temp++;
            }

            if (DctNumbOfParams[Model.AnalysisTypeKey].ShowTechTypesFilter)
            {
                parameters[temp] = Model.TechTypeID;
            }
       
            object Results = MI.Invoke(Instance, parameters);
            JavaScriptSerializer jss = new JavaScriptSerializer();

            GraphDataVM ModelG = new GraphDataVM
            {
                AnalysisName= s,
                Results = jss.Serialize(Results),
                
               
            };


           
          
          



            return PartialView(CharGraph+"PView",ModelG);
        }

        //Racunanje ukupne zarade datog kina shodno proslijedjenim parametrima
        public object TotalProfit(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? MovieID, int? TechTypeID)
        {

            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);


            List<int> fltMovies = new List<int>();
            List<int> fltTechTypes = new List<int>();

            fltMovies.AddRange((MovieID == 0) ? NewConn.Movies.Select(x => x.MovieID).ToList() : NewConn.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

           

            var profit = from T in NewConn.Tickets
                         join R in NewConn.Reservations on T.ReservationID equals R.ReservationID
                         join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                         join M in NewConn.Movies on P.MovieID equals M.MovieID
                         join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                         join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                         where DDT.DateTimeStart >= DateTimeFrom
                               && DDT.DateTimeStart <= DateTimeTo
                               && fltMovies.Contains(M.MovieID)
                               && fltTechTypes.Contains(TTY.TechnologyTypeID)
                         group T by new
                         {
                             T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                             T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                             T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                         } into Tgroup
                         orderby Tgroup.Key.Day
                         select new
                         {
                             FirstProperty = Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString(),
                             SecondProperty = Tgroup.Sum(x => x.TicketPrice)


                         };
            
          
            Random rnd = new Random();
            //var colorLst = ColorList;
            //var color = colorLst[rnd.Next(0, colorLst.Count - 1)];

            


            AreaLineBarGraphData r = new AreaLineBarGraphData()
            {
                labels = profit.Select(x => x.FirstProperty).ToList(),
                datasets = new List<AreaLineBarGraphDatasets>
                {
                    new AreaLineBarGraphDatasets
                    {
                        label="Total profit",
                        fillColor="#16a085",
                        strokeColor="#16a085",
                        pointColor="#34495e",
                        pointStrokeColor="#34495e",
                        pointHighlightFill= "#fff",
                        pointHighlightStroke= "rgba(220,220,220,1)",
                        data= profit.Select(x=>x.SecondProperty).ToList()
                    }
                }
            };
            

            


            return r as object;
        }



        public object TotalProfitByTransactionType(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? MovieID, int? TechTypeID)
        {
            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);




            List<int> fltMovies = new List<int>();
            List<int> fltTechTypes = new List<int>();

            fltMovies.AddRange((MovieID == 0) ? NewConn.Movies.Select(x => x.MovieID).ToList() : NewConn.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var profitCash = from T in NewConn.Tickets
                             join R in NewConn.Reservations on T.ReservationID equals R.ReservationID
                             join PM in NewConn.PaymentMethods on R.PaymentMethodID equals PM.PaymentMethodID
                             join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                             join M in NewConn.Movies on P.MovieID equals M.MovieID
                             join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                             join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                             where DDT.DateTimeStart >= DateTimeFrom
                                   && DDT.DateTimeStart <= DateTimeTo
                                   && fltMovies.Contains(M.MovieID)
                                   && PM.MethodName=="Cash"
                             && fltTechTypes.Contains(TTY.TechnologyTypeID)
                             group T by new
                             {
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                             } into Tgroup
                             orderby Tgroup.Key.Day
                             select new
                             {
                                 FirstProperty = Tgroup.Key.Month.ToString() + "." + Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Year.ToString(),
                                 SecondProperty = Tgroup.Sum(x => x.TicketPrice)


                             };

            var profitOnline = from T in NewConn.Tickets
                               join R in NewConn.Reservations on T.ReservationID equals R.ReservationID
                               join PM in NewConn.PaymentMethods on R.PaymentMethodID equals PM.PaymentMethodID
                               join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                               join M in NewConn.Movies on P.MovieID equals M.MovieID
                               join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                               join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                               where DDT.DateTimeStart >= DateTimeFrom
                                     && DDT.DateTimeStart <= DateTimeTo
                                     && fltMovies.Contains(M.MovieID)
                                     && PM.MethodName != "Cash"
                               && fltTechTypes.Contains(TTY.TechnologyTypeID)
                               group T by new
                               {
                                   T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                                   T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                                   T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                               } into Tgroup
                               orderby Tgroup.Key.Day
                               select new
                               {
                                   FirstProperty = Tgroup.Key.Month.ToString() + "." + Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Year.ToString(),
                                   SecondProperty = Tgroup.Sum(x => x.TicketPrice)


                               };

            Dictionary<DateTime, decimal> ArrangedCouplesCash = profitCash.ToDictionary(v => DateTime.Parse(v.FirstProperty), v => v.SecondProperty);
            Dictionary<DateTime, decimal> ArrangedCouplesOnline = profitOnline.ToDictionary(v => DateTime.Parse(v.FirstProperty), v => v.SecondProperty);

            DateTime startDate = DateTimeFrom;

            List<DateTime> keysCash = new List<DateTime>(ArrangedCouplesCash.Keys);
            List<DateTime> keysOnline = new List<DateTime>(ArrangedCouplesOnline.Keys);


            for (int i = 0; i < (DateTimeTo - DateTimeFrom).TotalDays - 1; i++)
            {
                if (!keysCash.Contains(startDate))
                {
                    ArrangedCouplesCash.Add(startDate, 0);
                }
                if (!keysOnline.Contains(startDate))
                {
                    ArrangedCouplesOnline.Add(startDate, 0);
                }
                startDate = startDate.AddDays(1);
            }


            Random rnd = new Random();
            var colorLst = ColorList;
            var colorCash = colorLst[rnd.Next(0, colorLst.Count - 1)];
            var colorOnlineT = colorLst[rnd.Next(0, colorLst.Count - 1)];

            AreaLineBarGraphData r = new AreaLineBarGraphData()
            {
                labels = ArrangedCouplesCash.OrderBy(x => x.Key.DayOfYear).Select(x => x.Key.Day + "." + x.Key.Month).ToList(),
                datasets = new List<AreaLineBarGraphDatasets>
                {
                    new AreaLineBarGraphDatasets
                    {
                        label="Cash",
                        fillColor="#f1c40f",
                        strokeColor="#000080",
                        pointColor="#f1c40f",
                        pointStrokeColor="#34495e",
                        pointHighlightFill= "#fff",
                        pointHighlightStroke= "rgba(220,220,220,1)",
                        data= ArrangedCouplesCash.OrderBy(x=>x.Key.DayOfYear).Select(x=>x.Value).ToList()
                    },
                     new AreaLineBarGraphDatasets
                    {
                        label="Online transactions",
                        fillColor="#8e44ad",
                        strokeColor="#000080",
                        pointColor="#8e44ad",
                        pointStrokeColor="rgba(60,141,188,1)",
                        pointHighlightFill= "#fff",
                        pointHighlightStroke= "rgba(60,141,188,1)",
                        data= ArrangedCouplesOnline.OrderBy(x=>x.Key.DayOfYear).Select(x=>x.Value).ToList()
                    }
                }
            };





            return r as object;
        }

        public object TotalProfitByPaymentMethodOnline(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? MovieID, int? TechTypeID)
        {
            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);

            List<string> Colors = new List<string>() { "#000080", "#003C00", "#8b0000" };
            List<int> fltMovies = new List<int>();
            List<int> fltTechTypes = new List<int>();

            fltMovies.AddRange((MovieID == 0) ? NewConn.Movies.Select(x => x.MovieID).ToList() : NewConn.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            Random rnd = new Random();
            var colorLst = ColorList;

            AreaLineBarGraphData r = new AreaLineBarGraphData();
            r.labels = new List<string>();
            r.datasets = new List<AreaLineBarGraphDatasets>();

            bool executed = false;

            List<PaymentMethod> PaymentMethods = NewConn.PaymentMethods.Where(x=>x.MethodName!="Cash").ToList();
            

            int counter = 0;
            foreach (var item in PaymentMethods)
            {

                var colorPM = colorLst[rnd.Next(0, colorLst.Count - 1)];

                var profit = from T in NewConn.Tickets
                             join R in NewConn.Reservations on T.ReservationID equals R.ReservationID         
                             join PM in NewConn.PaymentMethods on R.PaymentMethodID equals PM.PaymentMethodID
                             join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                             join M in NewConn.Movies on P.MovieID equals M.MovieID
                             join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                             join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                             where DDT.DateTimeStart >= DateTimeFrom
                                   && DDT.DateTimeStart <= DateTimeTo
                                   && item.PaymentMethodID == PM.PaymentMethodID
                             && fltMovies.Contains(M.MovieID)
                             && fltTechTypes.Contains(TTY.TechnologyTypeID)
                             group T by new
                             {
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                             } into Tgroup
                             orderby Tgroup.Key.Day
                             select new
                             {
                                 FirstProperty = Tgroup.Key.Month.ToString() + "." + Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Year.ToString(),
                                 SecondProperty = Tgroup.Sum(x => x.TicketPrice)


                             };
                Dictionary<DateTime, decimal> ArrangedCouplesPM = profit.ToDictionary(v => DateTime.Parse(v.FirstProperty), v => v.SecondProperty);
                DateTime startDate = DateTimeFrom;
                List<DateTime> keysPM = new List<DateTime>(ArrangedCouplesPM.Keys);

                for (int i = 0; i < (DateTimeTo - DateTimeFrom).TotalDays - 1; i++)
                {
                    if (!keysPM.Contains(startDate))
                    {
                        ArrangedCouplesPM.Add(startDate, 0);
                    }
                    startDate = startDate.AddDays(1);
                }

                if (executed == false)
                {
                    r.labels = ArrangedCouplesPM.OrderBy(x => x.Key.DayOfYear).Select(x => x.Key.Day + "." + x.Key.Month).ToList();
                    executed = true;
                }
                r.datasets.Add(
                        new AreaLineBarGraphDatasets
                        {
                            label = item.MethodName,
                            fillColor = Colors[counter],
                            strokeColor = Colors[counter],
                            pointColor = Colors[counter],
                            pointStrokeColor = "#c1c7d1",
                            pointHighlightFill = "#fff",
                            pointHighlightStroke = "rgba(220,220,220,1)",
                            data = ArrangedCouplesPM.OrderBy(x => x.Key.DayOfYear).Select(x => x.Value).ToList()
                        }
                    );
                counter++;
            }

            return r as object;
        }


        public object TotalProfitByMovie(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? TechTypeID)
        {
            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);

            List<string> Colors = new List<string>
            {
                "#2980b9",
                "#3498db",
                "#f39c12",
                "#f1c40f",
                "#c0392b",
                "#e74c3c",
                "#27ae60",
                "#2ecc71",
                "#8e44ad",
                "#9b59b6",
                "#2c3e50",
                "#34495e"
            };


            List<int> fltTechTypes = new List<int>();
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());


            var profit = from T in NewConn.Tickets
                         join R in NewConn.Reservations on T.ReservationID equals R.ReservationID
                         join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                         join M in NewConn.Movies on P.MovieID equals M.MovieID
                         join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart                       
                         join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                         where DDT.DateTimeStart >= DateTimeFrom
                                && DDT.DateTimeStart <= DateTimeTo
                                && fltTechTypes.Contains(TTY.TechnologyTypeID)
                         group T by T.Reservation.Projection.Movie into Tgroup
                         orderby Tgroup.Sum(x=>x.TicketPrice) descending
                         select new
                         {
                             FirstProperty = Tgroup.Key.MovieName,
                             SecondProperty = Tgroup.Sum(x => x.TicketPrice)
                         };

            Random rnd = new Random();
            var colorLst = ColorList;
            


            List<PieGraphData> lstProfitMovie = new List<PieGraphData>();


            int counter = 0;
            foreach (var item in profit)
            {
                var clr = colorLst[rnd.Next(0, colorLst.Count - 1)];
                lstProfitMovie.Add(
                        new PieGraphData
                        {
                            value = item.SecondProperty,
                            color = Colors[counter],
                            highlight = "#ecf0f1",
                            label = item.FirstProperty

                        }
                    );
                counter++;
            }

            return lstProfitMovie as object;
        }



        public object TotalProfitByTechnologyType(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? MovieID)
        {
            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);

            List<string> Colors = new List<string>
            {
                "#2980b9",                
                "#f39c12",               
                "#c0392b",               
                "#27ae60",                
                "#8e44ad",               
                "#2c3e50"
              
            };




            List<int> fltMovies = new List<int>();           
            fltMovies.AddRange((MovieID == 0) ? NewConn.Movies.Select(x => x.MovieID).ToList() : NewConn.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
           

            Random rnd = new Random();
            var colorLst = ColorList;

            AreaLineBarGraphData r = new AreaLineBarGraphData();
            r.labels = new List<string>();
            r.datasets = new List<AreaLineBarGraphDatasets>();

            bool executed = false;

            List<TechnologyType> TechnologyTypes = NewConn.TechnologyTypes.ToList();

            int counter = 0;

            foreach (var item in TechnologyTypes)
            {
                //var colorPM = colorLst[rnd.Next(0, colorLst.Count - 1)];

                var profit = from T in NewConn.Tickets
                             join R in NewConn.Reservations on T.ReservationID equals R.ReservationID                                                          
                             join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                             join M in NewConn.Movies on P.MovieID equals M.MovieID
                             join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                             join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                             where DDT.DateTimeStart >= DateTimeFrom
                                   && DDT.DateTimeStart <= DateTimeTo                  
                                   && fltMovies.Contains(M.MovieID)
                                   && TTY.TechnologyTypeID==item.TechnologyTypeID                 
                             group T by new
                             {
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                             } into Tgroup
                             orderby Tgroup.Key.Day
                             select new
                             {
                                 FirstProperty = Tgroup.Key.Month.ToString() + "." + Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Year.ToString(),
                                 SecondProperty = Tgroup.Sum(x => x.TicketPrice)


                             };
                Dictionary<DateTime, decimal> ArrangedCouplesPM = profit.ToDictionary(v => DateTime.Parse(v.FirstProperty), v => v.SecondProperty);
                DateTime startDate = DateTimeFrom;
                List<DateTime> keysPM = new List<DateTime>(ArrangedCouplesPM.Keys);

                for (int i = 0; i < (DateTimeTo - DateTimeFrom).TotalDays - 1; i++)
                {
                    if (!keysPM.Contains(startDate))
                    {
                        ArrangedCouplesPM.Add(startDate, 0);
                    }
                    startDate = startDate.AddDays(1);
                }

                if (executed == false)
                {
                    r.labels = ArrangedCouplesPM.OrderBy(x => x.Key.DayOfYear).Select(x => x.Key.Day + "." + x.Key.Month).ToList();
                    executed = true;
                }
                r.datasets.Add(
                        new AreaLineBarGraphDatasets
                        {
                            label = item.Name,
                            fillColor = Colors[counter],
                            strokeColor = Colors[counter],
                            pointColor = Colors[counter],
                            pointStrokeColor = "#c1c7d1",
                            pointHighlightFill = "#fff",
                            pointHighlightStroke = "rgba(220,220,220,1)",
                            data = ArrangedCouplesPM.OrderBy(x => x.Key.DayOfYear).Select(x => x.Value).ToList()
                        }
                    );
                counter++;
            }

            return r as object;
        }




        public object TotalProfitByMovieGenre(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? TechTypeID)
        {
            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);

            List<int> fltTechTypes = new List<int>();
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            Random rnd = new Random();
            var colorLst = ColorList;

            AreaLineBarGraphData r = new AreaLineBarGraphData();
            r.labels = new List<string>();
            r.datasets = new List<AreaLineBarGraphDatasets>();

            bool executed = false;

            List<Genre> Genres = NewConn.Genres.ToList();

            List<string> Colors = new List<string>() { "#3498db", "#f1c40f", "#e74c3c" };


            int counter = 0;
            foreach (var item in Genres)
            {
                var colorPM = colorLst[rnd.Next(0, colorLst.Count - 1)];

                var profit = from T in NewConn.Tickets
                             join R in NewConn.Reservations on T.ReservationID equals R.ReservationID
                             join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                             join M in NewConn.Movies on P.MovieID equals M.MovieID
                             join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                             join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                             join G in NewConn.Genres on M.GenreID equals G.GenreID
                             where DDT.DateTimeStart >= DateTimeFrom
                                   && DDT.DateTimeStart <= DateTimeTo
                                   && fltTechTypes.Contains(TTY.TechnologyTypeID)
                                   && G.GenreID == item.GenreID
                             group T by new
                             {
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                                 T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                             } into Tgroup
                             orderby Tgroup.Key.Day
                             select new
                             {
                                 FirstProperty = Tgroup.Key.Month.ToString() + "." + Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Year.ToString(),
                                 SecondProperty = Tgroup.Sum(x => x.TicketPrice)


                             };
                Dictionary<DateTime, decimal> ArrangedCouplesPM = profit.ToDictionary(v => DateTime.Parse(v.FirstProperty), v => v.SecondProperty);
                DateTime startDate = DateTimeFrom;
                List<DateTime> keysPM = new List<DateTime>(ArrangedCouplesPM.Keys);

                for (int i = 0; i < (DateTimeTo - DateTimeFrom).TotalDays - 1; i++)
                {
                    if (!keysPM.Contains(startDate))
                    {
                        ArrangedCouplesPM.Add(startDate, 0);
                    }
                    startDate = startDate.AddDays(1);
                }

                if (executed == false)
                {
                    r.labels = ArrangedCouplesPM.OrderBy(x => x.Key.DayOfYear).Select(x => x.Key.Day + "." + x.Key.Month).ToList();
                    executed = true;
                }
                r.datasets.Add(
                        new AreaLineBarGraphDatasets
                        {
                            label = item.GenreName,
                            fillColor = Colors[counter],
                            strokeColor = colorPM,
                            pointColor = Colors[counter],
                            pointStrokeColor = "#c1c7d1",
                            pointHighlightFill = "#fff",
                            pointHighlightStroke = "rgba(220,220,220,1)",
                            data = ArrangedCouplesPM.OrderBy(x => x.Key.DayOfYear).Select(x => x.Value).ToList()
                        }
                    );
                counter++;
            }

            return r as object;




        }

        public object Top10SeatReservationFrequency(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? MovieID, int? TechTypeID)
        {
            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);

            List<int> fltMovies = new List<int>();
            List<int> fltTechTypes = new List<int>();

            List<string> Colors = new List<string>
            {
                "#2980b9",
                "#3498db",
                "#f39c12",
                "#f1c40f",
                "#c0392b",
                "#e74c3c",
                "#27ae60",
                "#2ecc71",
                "#8e44ad",
                "#9b59b6",
                "#2c3e50",
                "#34495e"
            };



            fltMovies.AddRange((MovieID == 0) ? NewConn.Movies.Select(x => x.MovieID).ToList() : NewConn.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());

            var Frequency= (from PS in NewConn.ProjectionSeats
                        
                        join S in NewConn.Seats on PS.SeatID equals S.SeatID
                        join CH in NewConn.CinemaHalls on S.CinemaHallID equals CH.CinemaHallID
                        join P in NewConn.Projections on PS.ProjectionID equals P.ProjectionID
                        join M in NewConn.Movies on P.MovieID equals M.MovieID
                        join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                        join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                         
                        where  DDT.DateTimeStart >= DateTimeFrom
                               && DDT.DateTimeStart <= DateTimeTo
                               && fltTechTypes.Contains(TTY.TechnologyTypeID)
                               && fltMovies.Contains(M.MovieID)
                               && PS.IsReserved ==true
                        group PS by PS.Seat into SeatGroup
                        orderby SeatGroup.Count(x=>x.IsReserved) descending 
                        select new
                        {
                            FirstProperty = SeatGroup.Key.CinemaHall.Name+" - seat "+ SeatGroup.Key.SeatRowID+SeatGroup.Key.SeatColumnID.ToString(),
                            SecondProperty = SeatGroup.Count(x => x.IsReserved)
                        }).Take(10);

            Random rnd = new Random();
            var colorLst = ColorList;

            List<PieGraphData> lstSeatFrequency = new List<PieGraphData>();


            int counter = 0;
            foreach (var item in Frequency)
            {
                var clr = colorLst[rnd.Next(0, colorLst.Count - 1)];
                lstSeatFrequency.Add(
                        new PieGraphData
                        {
                            value = item.SecondProperty,
                            color = Colors[counter],
                            highlight = "#ecf0f1",
                            label = item.FirstProperty

                        }
                    );
                counter++;
            }
            return lstSeatFrequency as object;

        }


        public object TotalTicketsSold(string ConnString, DateTime DateTimeFrom, DateTime DateTimeTo, int? MovieID, int? TechTypeID)
        {

            CondorDBContextChild NewConn = new CondorDBContextChild(ConnString);



            List<int> fltMovies = new List<int>();
            List<int> fltTechTypes = new List<int>();

            fltMovies.AddRange((MovieID == 0) ? NewConn.Movies.Select(x => x.MovieID).ToList() : NewConn.Movies.Where(x => x.MovieID == MovieID).Select(x => x.MovieID).ToList());
            fltTechTypes.AddRange((TechTypeID == 0) ? NewConn.TechnologyTypes.Select(x => x.TechnologyTypeID).ToList() : NewConn.TechnologyTypes.Where(x => x.TechnologyTypeID == TechTypeID).Select(x => x.TechnologyTypeID).ToList());



            var ticketsSold = from T in NewConn.Tickets
                              join R in NewConn.Reservations on T.ReservationID equals R.ReservationID
                              join P in NewConn.Projections on R.ProjectionID equals P.ProjectionID
                              join M in NewConn.Movies on P.MovieID equals M.MovieID
                              join DDT in NewConn.DefinedDateTimes on P.DateTimeStart equals DDT.DateTimeStart
                              join TTY in NewConn.TechnologyTypes on P.TechnologyTypeID equals TTY.TechnologyTypeID
                              where DDT.DateTimeStart >= DateTimeFrom
                                    && DDT.DateTimeStart <= DateTimeTo
                                    && fltMovies.Contains(M.MovieID)
                                    && fltTechTypes.Contains(TTY.TechnologyTypeID)
                              group T by new
                              {
                                  T.Reservation.Projection.DefinedDateTime.DateTimeStart.Day,
                                  T.Reservation.Projection.DefinedDateTime.DateTimeStart.Month,
                                  T.Reservation.Projection.DefinedDateTime.DateTimeStart.Year

                              } into Tgroup
                              orderby Tgroup.Key.Day
                              select new
                              {
                                  FirstProperty = Tgroup.Key.Day.ToString() + "." + Tgroup.Key.Month.ToString(),
                                  SecondProperty = (decimal)Tgroup.Count()


                              };

          
            AreaLineBarGraphData r = new AreaLineBarGraphData()
            {
                labels = ticketsSold.Select(x => x.FirstProperty).ToList(),
                datasets = new List<AreaLineBarGraphDatasets>
                {
                    new AreaLineBarGraphDatasets
                    {
                        label="Total profit",
                        fillColor="#2980b9",
                        strokeColor="#2980b9",
                        pointColor="#2980b9",
                        pointStrokeColor="#2980b9",
                        pointHighlightFill= "#fff",
                        pointHighlightStroke= "rgba(220,220,220,1)",
                        data= ticketsSold.Select(x=>x.SecondProperty).ToList()
                    }
                }
            };




            return r as object;
        }


    }
}
