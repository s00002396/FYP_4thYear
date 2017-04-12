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
        public ActionResult Welcome(string searchBy, string search, int userID)
        {           
            ViewBag.UserName = HttpContext.Session.GetString("Username");
            #region if (search != null)
            if (search != null)
            {
                if (searchBy == "Social_Security_No")
                { 
                    var patientDetails = (from pA in _context.patientAccount
                                          join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                          join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                          join uA in _context.userAccount on pA.OccID equals uA.UserID
                                         
                                          where pA.Social_Security_No.Equals(search) || search == null
                                          select new PatientDetailsViewModel
                                          {
                                              vmPatientTable = pA,
                                              vmGuardian = g,
                                              vmSchools = s,
                                              vmUserAcc = uA                                             
                                          }).ToList();
                    var ss = patientDetails;
                    return View("Details", patientDetails);
                }
                
                else
                {
                    var patientDetails = (from pA in _context.patientAccount
                                          join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                          join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                          join uA in _context.userAccount on pA.OccID equals uA.UserID
                                          
                                          where pA.Name.StartsWith(search) || search == null
                                          select new PatientDetailsViewModel
                                          {
                                              vmPatientTable = pA,
                                              vmGuardian = g,
                                              vmSchools = s,
                                              vmUserAcc = uA
                                          }).ToList();
                    var ss = patientDetails;
                    return View("Details", patientDetails);
                }
            }//end if (search != null)
            #endregion

            #region else if (HttpContext.Session.GetString("UserID") != null)
            else if (HttpContext.Session.GetString("UserID") != null)
            {                           
                DateTime today = DateTime.Today;
                ViewBag.TodaysDate = today;
                var occID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                var myNewTasks = (from t in _context.tasks
                                  join otTask in _context.otTasks on t.TaskID equals otTask.TaskID
                                  join uA in _context.userAccount on otTask.OccID equals uA.UserID
                                  join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                                 
                                  where otTask.OccID.Equals(occID)
                                
                                  select new MyViewModel
                                  {
                                      vmTaskTable = t,
                                      vmUserAcc = uA,
                                      vmTPOT = otTask,
                                      vmPatientTable = pA
                                  }).ToList();
                        try
                        {
                            if (myNewTasks.FirstOrDefault().vmUserAcc.IsAdmin == true)
                            {
                                ViewBag.Admin = "True";
                                return View(myNewTasks);
                            }
                            else
                            {
                                ViewBag.Admin = "False";
                                return View(myNewTasks);
                            }

                }
                        catch (Exception)
                        {
                    var myNewTasks1 = (from uA in _context.userAccount
                                      //join otTask in _context.otTasks on t.TaskID equals otTask.TaskID
                                      //join uA in _context.userAccount on otTask.OccID equals uA.UserID
                                      //join pA in _context.patientAccount on uA.UserID equals pA.OccID
                                      //where otTask.OccID.Equals(occID)
                                      //where otTask.OccID.Equals(occID) || occID == null
                                      where uA.UserID.Equals(occID)
                                      select new MyViewModel
                                      {
                                          //vmTaskTable = t,
                                          vmUserAcc = uA,
                                          //vmTPOT = otTask,
                                          //vmPatientTable = pA
                                      }).ToList();
                    if (myNewTasks1.FirstOrDefault().vmUserAcc.IsAdmin == true)
                    {
                        ViewBag.Admin = "True";
                        ViewBag.Flag1 = "False";
                        return View(myNewTasks1);
                    }
                    //var uu = myNewTasks1;
                    //return View(myNewTasks);
                    //return RedirectToAction("Welcome");
                        }

                //if (myNewTasks.FirstOrDefault().vmUserAcc.IsAdmin == true)
                //{ 
                //    ViewBag.Admin = "True";
                //}
                //return View(myNewTasks);
                //}
                //else
                //{
                //    return RedirectToAction("Login");
                //}
                // }
                //catch (Exception)
                //{
                //    return RedirectToAction("Welcome");
                //}

            } //end if (HttpContext.Session.GetString("UserID") != null)
            #endregion

            return RedirectToAction("Welcome");
        }
        #endregion

        #region Register
        public ActionResult Register()
        {           
            return View();
        }
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

        #region View All Patients
        public ActionResult ViewAllPatients(int? id)
        {
            var patientDetails = (from pA in _context.patientAccount
                                  join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                  join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                  join uA in _context.userAccount on pA.OccID equals uA.UserID
                                  //join nO in _context.notes on pA.PPS_No equals nO.PPS_No
                                  //where pA.PPS_No.Equals(id)
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


        #region Task Details
        public ActionResult TaskDetails(int? id, int? id2, DateTime duedate)
        {
            var tester = 7;
            var taskDetails = (from tS in _context.otTasks
                                  join tT in _context.tasks on tS.TaskID equals tT.TaskID
                                  join pA in _context.patientAccount on tS.PPS_No equals pA.PPS_No
                                  where tS.PPS_No.Equals(id) && tS.TaskID.Equals(id2) && tS.DueDate.Equals(duedate)
                                  select new PatientDetailsViewModel
                                  {
                                      vmTPOT = tS,
                                      vmTaskTable = tT,
                                      vmPatientTable = pA,
                                  }).ToList();
                        
            return View(taskDetails);
            //return View(patientDetails);
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
        public IActionResult AddNote(int? id)
        {
            ViewBag.PatientID = id;
            return PartialView("_AddNote");
            //return View();
        }
        [HttpPost]
        public IActionResult AddNote(PatientDetailsViewModel student, int id)
        {
            ViewBag.PatientID = id;

            if (ModelState.IsValid)
            {
                Notes_Table newStudent = new Notes_Table()
                {
                    NoteTitle = student.vmNoteTable.NoteTitle,
                    NoteDate = DateTime.Now,
                    Details = student.vmNoteTable.Details,
                    PPS_No = ViewBag.PatientID
                };
                var c = newStudent;
                _context.notes.Add(newStudent);
                _context.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Note was successfuly added. ";
            }
            //return View();
            //return PartialView("_AddNote");
            //return RedirectToAction("Welcome");
            return RedirectToAction("Details", new {id = id });
        }
        #endregion

        #region Add Task
        public IActionResult AddTask(int? id)
        {
            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");

            List<UserAccount> OTList = _context.userAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserID", "FirstName");

            List<Task_Table> taskList = _context.tasks.ToList();
            ViewBag.taskList = new SelectList(taskList, "TaskID", "TaskType");

            ViewBag.PatientID = id;
            return PartialView("_AddATask");
        }
        [HttpPost]
        public IActionResult AddTask(PatientDetailsViewModel student, int id)
        {
            ViewBag.PatientID = id;

            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");

            List<UserAccount> OTList = _context.userAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserID", "FirstName");

            if (ModelState.IsValid)
            {
                Task_Patient_OT_Table newTask = new Task_Patient_OT_Table()
                {
                    TaskID = student.vmTaskTable.TaskID,
                    OccID = student.vmPatientTable.OccID,                    
                    PPS_No = ViewBag.PatientID,
                    DueDate = student.vmTaskTable.DueDate,
                    Completed = false
                };
                //var c = newStudent;
                _context.otTasks.Add(newTask);
                _context.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Note was successfuly added. ";
            }
            //return PartialView("_AddNote");
            //return RedirectToAction("Welcome");
            return RedirectToAction("Details", new { id = id });
        }
        #endregion

        #region Complete Task
        [HttpPost]
        //public IActionResult CompleteTask(PatientDetailsViewModel student, int id)
        public IActionResult CompleteTask(int id, int pid)
        {
            ViewBag.PatientID = id;

            Task_Patient_OT_Table otp = _context.otTasks.FirstOrDefault(ot => ot.OTTaskID == pid && ot.PPS_No == id);

            if (ModelState.IsValid)
            {               
                otp.Completed = true;
                _context.otTasks.Update(otp);
                _context.SaveChanges();
                ViewBag.Message = "Updated. ";
                ModelState.Clear();
            }
            //return PartialView("_AddNote");
            //return RedirectToAction("TaskDetails",new { id = otp.PPS_No, id2 = otp.TaskID, duedate = otp.DueDate });
            return RedirectToAction("Welcome");
        }
        #endregion

        #region Create A New Student GET
        public IActionResult CreateStudentRecord()
        {
            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");

            List<UserAccount> OTList = _context.userAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserID", "FirstName");

            return View();
        }
        #endregion

        #region Create A New Student POST
        [HttpPost]
        public IActionResult CreateStudentRecord(PatientDetailsViewModel student)
        {
            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");
            var date = DateTime.Now.ToString("dd/MM/yyyy");
            List<UserAccount> OTList = _context.userAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserID", "FirstName");
            //var today = @ViewBag.TodaysDate.ToString("dd/MM/yyyy");
            if (ModelState.IsValid)
            {
                _context.guardians.Add(student.vmGuardian);
                _context.SaveChanges();



                var id = _context.guardians.LastOrDefault();


                //var noteDetails = (from p in _context.guardians
                //                   where p.Name == student.vmGuardian.Name
                //                   select p.GuardianID).Single();

                //******************************************************************
                Patient_Table newStudent = new Patient_Table()
                {
                    Social_Security_No = student.vmPatientTable.Social_Security_No,
                    DoB = student.vmPatientTable.DoB,
                    Name = student.vmPatientTable.Name,
                    AddressLineOne = student.vmPatientTable.AddressLineOne,
                    City = student.vmPatientTable.City,
                    County = student.vmPatientTable.County,
                    SchoolID = student.vmPatientTable.SchoolID,
                    GuardianID = Convert.ToInt32(id.GuardianID),
                    OccID = student.vmPatientTable.OccID,
                    OpenDate = Convert.ToDateTime(date)

                };
                _context.patientAccount.Add(newStudent);
                
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = student.vmPatientTable.Name + " " + " was successfuly added. ";
            }
            return View();
        }
        #endregion

        #region ReassignPatient GET
        public IActionResult ReassignPatient(int id, string search)
        {
            var searchID = Convert.ToInt32(search);
            List<SchoolList_Table> schoolList = _context.schoolLists.ToList();
            ViewBag.schoolList = new SelectList(schoolList, "SchoolID", "SchoolName");
            ViewBag.PatientNumber = id;

            List<UserAccount> OTList = _context.userAccount.ToList();
            ViewBag.OTList = new SelectList(OTList, "UserID", "FirstName");

            #region remove
            
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
            #endregion
            return PartialView("_ReassignPatient");
            //return PartialView("_ReassignPatient", patientDetails);
        }
        #endregion

        #region ReassignPatient POST
        [HttpPost]
        public IActionResult ReassignPatient(int id, PatientDetailsViewModel student)
        {
            Patient_Table patient = _context.patientAccount.Single(p => p.PPS_No == id);
            //Task_Patient_OT_Table otp = _context.otTasks.Where(pp => pp.PPS_No == id).Single();
            Task_Patient_OT_Table otp = _context.otTasks.FirstOrDefault(pp => pp.PPS_No == id);
            var res = _context.otTasks.Where(pp => pp.PPS_No == id);
            //var tyg = res.FirstOrDefault().OccID = 401;
            var gg = 7;
            var myNewTasks = (from oT in _context.otTasks
                              
                              where oT.PPS_No == id
                              select new PatientDetailsViewModel
                              {                                
                                  vmTPOT = oT,
                              }).ToList();
            var test = myNewTasks;
            if (ModelState.IsValid)
            {           
                patient.OccID = student.vmPatientTable.OccID;
                _context.patientAccount.Update((patient));
                _context.SaveChanges();
            }

            return RedirectToAction("Welcome");
        }
        #endregion

        #region Remove OT
        public IActionResult RemoveOT(int? id)
        {
            ViewBag.PatientID = id;
            var myNewTasks = (from uA in _context.userAccount
                              //join gA in _context.guardians on pA.GuardianID equals gA.GuardianID
                              //join uA in _context.userAccount on pA.OccID equals uA.UserID
                              ////join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                              //where pA.OccID.Equals(occID)
                              //where otTask.OccID.Equals(occID)
                              //where t.DueDate.Date == today
                              select new PatientDetailsViewModel
                              {
                                  //vmGuardian = gA,
                                  vmUserAcc = uA,
                                  //vm = otTask,
                                  //vmPatientTable = pA
                              }).ToList();

            return View(myNewTasks);

            //if (HttpContext.Session.GetString("UserID") != null)
            //{
            //    //call up all records wit todays date..                
            //    DateTime today = DateTime.Today;
            //    ViewBag.TodaysDate = today;
            //    var occID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            //    var myNewTasks = (from pA in _context.patientAccount
            //                      join gA in _context.guardians on pA.GuardianID equals gA.GuardianID
            //                      join uA in _context.userAccount on pA.OccID equals uA.UserID
            //                      //join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
            //                      where pA.OccID.Equals(occID)
            //                      //where otTask.OccID.Equals(occID)
            //                      //where t.DueDate.Date == today
            //                      select new PatientDetailsViewModel
            //                      {
            //                          vmGuardian = gA,
            //                          vmUserAcc = uA,
            //                          //vm = otTask,
            //                          vmPatientTable = pA
            //                      }).ToList();

            //    return View(myNewTasks);
            //}
            //return View();
        }
        #endregion
        #region Get Patients for specific therapist
        public ActionResult Therapist_Patient_Details(int? id)
        {
            var patientDetails = (from pA in _context.patientAccount
                                  join g in _context.guardians on pA.GuardianID equals g.GuardianID
                                  //join s in _context.schoolLists on pA.SchoolID equals s.SchoolID
                                  join uA in _context.userAccount on pA.OccID equals uA.UserID
                                  //join nO in _context.notes on pA.PPS_No equals nO.PPS_No
                                  where pA.OccID.Equals(id)
                                  select new PatientDetailsViewModel
                                  {
                                      vmPatientTable = pA,
                                      vmGuardian = g,
                                      //vmSchools = s,
                                      vmUserAcc = uA//,
                                      //vmNoteTable = nO
                                  }).ToList();


            ViewBag.noteList = patientDetails;

            return View(patientDetails);
        }
        #endregion
        #region Delete Patient
        public IActionResult DeletePatient(int? id)
        {
            ViewBag.PatientID = id;

            if (HttpContext.Session.GetString("UserID") != null)
            {
                //call up all records wit todays date..                
                DateTime today = DateTime.Today;
                ViewBag.TodaysDate = today;
                var occID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                var myNewTasks = (from pA in _context.patientAccount
                                  join gA in _context.guardians on pA.GuardianID equals gA.GuardianID
                                  join uA in _context.userAccount on pA.OccID equals uA.UserID
                                  //join pA in _context.patientAccount on otTask.PPS_No equals pA.PPS_No
                                  where pA.OccID.Equals(occID)
                                  //where otTask.OccID.Equals(occID)
                                  //where t.DueDate.Date == today
                                  select new PatientDetailsViewModel
                                  {
                                      vmGuardian = gA,
                                      vmUserAcc = uA,
                                      //vm = otTask,
                                      vmPatientTable = pA
                                  }).ToList();              

                return View(myNewTasks);
            }
            return View();
        }
        //[HttpPost]
        public IActionResult DeletePatient1(int? id)
        {
            ViewBag.PatientID = id;

            Patient_Table removePatient = _context.patientAccount.Where(c => c.PPS_No == id).Single();
            Guardian_Table removeGuardian = _context.guardians.Where(c => c.GuardianID == removePatient.GuardianID).Single();
            try
            {
                Notes_Table removeNotes = _context.notes.Where(c => c.PPS_No == removePatient.PPS_No).Single();
                _context.notes.Remove(removeNotes);
            }
            catch (Exception)
            {

            }
            //Notes_Table removeNotes = _context.notes.Where(c => c.PPS_No == removePatient.PPS_No).Single();


            if (ModelState.IsValid)
            {   
                _context.patientAccount.Remove(removePatient);
                _context.guardians.Remove(removeGuardian);
                //_context.notes.Remove(removeNotes);
                _context.SaveChanges();
                ViewBag.Message = "Patient was successfuly removed. ";
            }

            return RedirectToAction("DeletePatient");
        }
        #endregion

        #region PatientNotes
        public ActionResult PatientNotes(int? id)
        {
            var vm = new MyViewModel();
            ViewBag.PatId = id;

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

        #region Register/Error
        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View();
        //}

        //public ActionResult Register()
        //{
        //    return View();
        //}
        #endregion
    }
}
