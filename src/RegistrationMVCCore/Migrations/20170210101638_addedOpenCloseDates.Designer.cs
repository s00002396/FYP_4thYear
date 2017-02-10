using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RegistrationMVCCore.Model;

namespace RegistrationMVCCore.Migrations
{
    [DbContext(typeof(OurDbContext))]
    [Migration("20170210101638_addedOpenCloseDates")]
    partial class addedOpenCloseDates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RegistrationMVCCore.Model.Guardian_Table", b =>
                {
                    b.Property<int>("GuardianID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("PhoneNo");

                    b.HasKey("GuardianID");

                    b.ToTable("guardians");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.NotePatient_Table", b =>
                {
                    b.Property<int>("NoteID");

                    b.Property<int>("PPS_No");

                    b.Property<DateTime>("NoteDate");

                    b.HasKey("NoteID", "PPS_No");

                    b.HasAlternateKey("NoteID");

                    b.ToTable("notePatient");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.Notes_Table", b =>
                {
                    b.Property<int>("NoteID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Details");

                    b.Property<string>("NoteTitle");

                    b.HasKey("NoteID");

                    b.ToTable("notes");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.Patient_Table", b =>
                {
                    b.Property<int>("PPS_No")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLineOne");

                    b.Property<string>("City");

                    b.Property<DateTime>("CloseDate");

                    b.Property<string>("County");

                    b.Property<string>("EirCode");

                    b.Property<int>("GuardianID");

                    b.Property<string>("Name");

                    b.Property<int>("OccID");

                    b.Property<DateTime>("OpenDate");

                    b.Property<int>("SchoolID");

                    b.HasKey("PPS_No");

                    b.ToTable("patientAccount");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.SchoolList_Table", b =>
                {
                    b.Property<int>("SchoolID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SchoolAddress");

                    b.Property<string>("SchoolName");

                    b.Property<int>("SchoolPhone");

                    b.HasKey("SchoolID");

                    b.ToTable("schoolLists");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.Task_Patient_OT_Table", b =>
                {
                    b.Property<int>("TaskID");

                    b.Property<int>("PPS_No");

                    b.Property<int>("OccID");

                    b.HasKey("TaskID", "PPS_No", "OccID");

                    b.HasAlternateKey("TaskID");


                    b.HasAlternateKey("PPS_No", "TaskID");


                    b.HasAlternateKey("OccID", "PPS_No", "TaskID");

                    b.ToTable("otTasks");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.Task_Table", b =>
                {
                    b.Property<int>("TaskID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Completed");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("TaskType");

                    b.HasKey("TaskID");

                    b.ToTable("tasks");
                });

            modelBuilder.Entity("RegistrationMVCCore.Model.UserAccount", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CofirmPassword");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("UserID");

                    b.ToTable("userAccount");
                });
        }
    }
}
