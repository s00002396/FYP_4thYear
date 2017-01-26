using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RegistrationMVCCore.Model;

namespace RegistrationMVCCore.Migrations
{
    [DbContext(typeof(OurDbContext))]
    [Migration("20170119181911_NewTable")]
    partial class NewTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RegistrationMVCCore.Model.Patient", b =>
                {
                    b.Property<int>("PPS_No")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLineOne");

                    b.Property<string>("City");

                    b.Property<string>("County");

                    b.Property<string>("EirCode");

                    b.Property<int>("GuardianID");

                    b.Property<string>("Name");

                    b.Property<int>("NotesID");

                    b.Property<int>("SchoolID");

                    b.HasKey("PPS_No");

                    b.ToTable("patientAccount");
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
