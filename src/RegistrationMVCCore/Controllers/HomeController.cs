using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationMVCCore.Model;
using Microsoft.AspNetCore.Http;

namespace RegistrationMVCCore.Controllers
{
    //private OurDbContxt db = new OurDbContxt();
    public class HomeController : Controller
    {
        private OurDbContext _context;

        public HomeController(OurDbContext context)
        {
            _context = context;
        }

        #region Index
        public ActionResult Index()
        {
            var client = _context.userAccount.FirstOrDefault();
            return View(client);
        }
        [HttpPost]
        public ActionResult Index(UserAccount user)
        {
            var account = _context.userAccount.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
            if (account != null)
            {
                HttpContext.Session.SetString("UserID", account.UserID.ToString());
                HttpContext.Session.SetString("Username", account.UserName);
                var test3 = account.UserID;
                //return RedirectToAction("Welcome", "Home", new { id = account.UserID });
                return RedirectToAction("Welcome", null, test3);
            }
            else
            {
                ModelState.AddModelError("", "Username or password is worng");
            }
            return View();
        }
        #endregion

        //public IActionResult Index()
        //{
        //    return View(_context.userAccount.ToList());
        //}

        #region Details
        public ActionResult Details(int? id)
        {
            var patientDetails = (from pA in _context.patientAccount
                                  join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                  join s in _context.schoolLists on pA.SchoolID equals s.SchoolID 
                                  //join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                                  //where otTask.OccID.Equals(occID)
                                  where pA.PPS_No.Equals(id)
                                  //where t.DueDate.Date == today
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s
                                  }).ToList();
            
            //ViewBag.PageTitle = "Details for " + client.Name;
            
            return View(patientDetails);
        }
        #endregion

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
         
        public ActionResult Register()
        {
            return View();
        }

        #region Register
        [HttpPost]
        public ActionResult Register(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                _context.userAccount.Add(user);
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " is successfuly registered. ";
            }
            return View();
        }
        #endregion

        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Login(UserAccount user)
        //{
        //    var account = _context.userAccount.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
        //    if (account !=null)
        //    {
        //        HttpContext.Session.SetString("UserID", account.UserID.ToString());
        //        HttpContext.Session.SetString("Username", account.UserName);
        //        return RedirectToAction("Welcome");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Username or password is worng");
        //    }
        //    return View();
        //}

        #region Welcome
        public ActionResult Welcome(string search, int userID)
        {
            var searchID = Convert.ToInt32(search);
            ViewBag.UserName = HttpContext.Session.GetString("Username");


            if (search != null)
            {
                //ICollection<Patient_Table> mySearch = _context.patientAccount.Where(x => x.Name.Contains(search)|| search == null).ToList();
                //var myNewTasks = (from t in _context.tasks
                //                  join otTask in _context.otTasks on t.TaskID equals otTask.TaskID
                //                  join uA in _context.userAccount on otTask.OccID equals uA.UserID
                //                  join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                //                  where pA.PPS_No.Equals(searchID)

                //                  select new MyViewModel
                //                  {
                //                      vmTaskTable = t,
                //                      vmUserAcc = uA,
                //                      vmTPOT = otTask,
                //                      vmPatientTable = pA
                //                  }).ToList();

                var patientDetails = (from pA in _context.patientAccount
                                      join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                      join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                      //join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                                      //where otTask.OccID.Equals(occID)
                                      where pA.PPS_No.Equals(searchID)
                                      //where t.DueDate.Date == today
                                      select new PatientDetailsViewModel
                                      {
                                          vmPatientTable = pA,
                                          vmGuardian = g,
                                          vmSchools = s
                                      }).ToList();

                //var test5 = myNewTasks;
                //return View(myNewTasks);//redirect to search page!
                return View("Details", patientDetails);
            }
            else if (HttpContext.Session.GetString("UserID")!=null)
            {
                //call up all records wit todays date..                
                DateTime today = DateTime.Today;
                ViewBag.TodaysDate = today;
                var occID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                var myNewTasks = (from t in _context.tasks
                                  join otTask in _context.otTasks on t.TaskID equals otTask.TaskID
                                  join uA in _context.userAccount on otTask.OccID equals uA.UserID
                                  join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                                  //where otTask.OccID.Equals(occID)
                                  where otTask.OccID.Equals(occID)
                                  //where t.DueDate.Date == today
                                  select new MyViewModel
                                  {
                                      vmTaskTable = t,
                                      vmUserAcc = uA,
                                      vmTPOT = otTask,
                                      vmPatientTable = pA
                                  }).ToList();

                //var test = myNewTasks;                
                
                return View(myNewTasks);
            }
            else
            {
                return RedirectToAction("Login");
            }            
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
