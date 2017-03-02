using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationMVCCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegistrationMVCCore.Controllers
{
    
    public class HomeController : Controller
    {
        private OurDbContext _context;

        #region HomeController
        public HomeController(OurDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Welcome
        public ActionResult Welcome(string search, int userID)
        {
            var searchID = Convert.ToInt32(search);
            ViewBag.UserName = HttpContext.Session.GetString("Username");


            if (search != null)
            { 
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

        #region Register
        [HttpPost]
        public ActionResult Register(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                _context.userAccount.Add(user);
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " was successfuly registered. ";
            }
            return View();
        }
        #endregion      

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

        #region Details
        public ActionResult Details(int? id)
        {
            var patientDetails = (from pA in _context.patientAccount
                                  join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                  join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                  join uA in _context.userAccount on pA.OccID equals uA.UserID
                                  //join nO in _context.notes on pA.PPS_No equals nO.PPS_No
                                  where pA.PPS_No.Equals(id)
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s,
                                      vmUserAcc = uA//,
                                      //vmNoteTable = nO
                                  }).ToList();

            
            ViewBag.noteList = patientDetails;
           
            return View(patientDetails);
        }
        #endregion

        #region Add Note
        public IActionResult AddNote()
        {
            
            return View();
        }
        #endregion

        #region Create
        public IActionResult CreateStudentRecord()
        {
            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");
            return View();
        }
        [HttpPost]
        public IActionResult CreateStudentRecord(PatientDetailsViewModel student)
            //public IActionResult CreateStudentRecord(Patient_Table student)
        {
            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");
            #region hardCoded
            //Patient_Table newStudent = new Patient_Table()
            //{
            //    //PPS_No = 305,
            //    Name = "Fred Johnson",
            //    AddressLineOne = "Millrun",
            //    City = "Sligo",
            //    County = "Sligo",
            //    SchoolID = 801,
            //    GuardianID = 601,
            //    OccID = 401

            //};
            //_context.patientAccount.Add(newStudent);

            //_context.SaveChanges();
            //return RedirectToAction("Welcome");
            #endregion

            if (ModelState.IsValid)
            {
                _context.patientAccount.Add(student.vmPatientTable);
                //add details to other tables and make identity
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = student.vmPatientTable.Name + " " +  " was successfuly added. ";
            }
            return View();
        }
        #endregion

        #region PatientNotes
        public ActionResult PatientNotes(int? id)
        {
            var vm = new MyViewModel();
          

            var patientDetails = (from pA in _context.patientAccount
                                  join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                  join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                  join uA in _context.userAccount on pA.OccID equals uA.UserID
                                  join nO in _context.notes on pA.PPS_No equals nO.PPS_No
                                  where pA.PPS_No.Equals(id)
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      vmSchools = s,
                                      vmUserAcc = uA,
                                      vmNoteTable = nO
                                  }).ToList();
            var x = patientDetails;
            return PartialView("_PatientNotes", patientDetails);
        }
        #endregion

        #region PatientNotesDetails
        public ActionResult PatientNotesDetails(int? pd, string dt)
        {
            DateTime mm = DateTime.Parse(dt);
            var vm = new MyViewModel();
            var noteDetails = _context.notes.Where(bb => bb.NoteDate == mm);            

            var x = noteDetails;
            return PartialView("_PatientNotes", noteDetails);
        }
        #endregion



        #region Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion

        #region About
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
        #endregion
    }
}
