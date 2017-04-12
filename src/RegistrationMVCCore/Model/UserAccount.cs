using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationMVCCore.Model
{
    public class UserAccount
    {
        [Key]
        public int UserID { get; set; }
        [Required(ErrorMessage ="First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="Password does not match")]
        [DataType(DataType.Password)]
        public string CofirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class Patient_Table
    {
        [Key, Display(Name ="PPS Number")]
        public int PPS_No { get; set; }
        public string DoB { get; set; }
        public string Social_Security_No { get; set; }
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Display(Name = "Address line 1")]
        public string AddressLineOne { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string EirCode { get; set; }
        public int SchoolID { get; set; }
        public int GuardianID { get; set; }
        public int OccID { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }


    }
    public class Guardian_Table
    {
        [Key]
        public int GuardianID { get; set; }
        public string Name { get; set; }
        public int PhoneNo { get; set; }
    }
    public class NotePatient_Table
    {
        [Key, Column(Order = 0)]
        public int NoteID { get; set; }
        [Key, Column(Order = 1)]
        public int PPS_No { get; set; }

        public DateTime NoteDate { get; set; }
    }
    public class Notes_Table
    {
        [Key]
        public int NoteId { get; set; }
        public DateTime NoteDate { get; set; }
        public int PPS_No { get; set; }
        public string NoteTitle { get; set; }
        public string Details { get; set; }
    }
    public class SchoolList_Table
    {
        [Key]
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public int SchoolPhone { get; set; }
    }
    public class Task_Patient_OT_Table
    {
        [Key]
        public int OTTaskID { get; set; }

        //[Key, Column(Order = 0)]
        public int TaskID { get; set; }

        //[Key, Column(Order = 1)]
        public int PPS_No { get; set; }

        //[Key, Column(Order = 2)]
        public int OccID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
    }
    public class Task_Table
    {
        [Key]
        public int TaskID { get; set; }
        public string TaskType { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }

    }
    public class MyViewModel
    {
        public Task_Table vmTaskTable { get; set; }
        public Task_Patient_OT_Table vmTPOT { get; set; }
        public Patient_Table vmPatientTable { get; set; }
        public UserAccount vmUserAcc { get; set; }
    }
    
     public class PatientDetailsViewModel
    {
        //public Task_Table vmTaskTable { get; set; }

        public SchoolList_Table vmSchools { get; set; }
        public Patient_Table vmPatientTable { get; set; }
        public Guardian_Table vmGuardian { get; set; }
        public UserAccount vmUserAcc { get; set; }
        public Notes_Table vmNoteTable { get; set; }
        public Task_Patient_OT_Table vmTPOT { get; set; }
        public Task_Table vmTaskTable { get; set; }
    }
    
}
