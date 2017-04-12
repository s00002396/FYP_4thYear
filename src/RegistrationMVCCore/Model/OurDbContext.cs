using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationMVCCore.Model
{
    public class OurDbContext:DbContext
    {
        public OurDbContext()
        {

        }
        public OurDbContext(DbContextOptions<OurDbContext> options):base(options)
        {

        }
        public DbSet<UserAccount> userAccount { get; set; }
        public DbSet<Patient_Table> patientAccount { get; set; }
        public DbSet<Guardian_Table> guardians { get; set; }
        
        public DbSet<NotePatient_Table> notePatient { get; set; }
        //public DbSet<Task_Patient_OT_Table> otTask { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotePatient_Table>()
                .HasKey(k => new { k.NoteID, k.PPS_No });

            //modelBuilder.Entity<Task_Patient_OT_Table>()
            //    .HasKey(k => new { k.TaskID, k.PPS_No,k.OccID });
            modelBuilder.Entity<Task_Patient_OT_Table>()
                .HasKey(k => new { k.OTTaskID });
        }

        public DbSet<Notes_Table> notes { get; set; }
        public DbSet<SchoolList_Table> schoolLists { get; set; }
        public DbSet<Task_Patient_OT_Table> otTasks { get; set; }
        public DbSet<Task_Table> tasks { get; set; }

    }
}
