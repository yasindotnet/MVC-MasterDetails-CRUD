namespace Project_MVC_Work_001.Migrations
{
    using Project_MVC_Work_001.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project_MVC_Work_001.Models.HospitalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project_MVC_Work_001.Models.HospitalDbContext db)
        {
            db.Doctors.AddRange(new Doctor[]
            {
                new Doctor{DoctorName="Dr.Kibria khan"},
                new Doctor{DoctorName="Dr.Azman Ali"}
            });
            db.Departments.AddRange(new Department[]
            {
                new Department{DepartmentName="Medicine"},
                new Department{DepartmentName="Cardiology"}
            });
            db.SaveChanges();
            Patient patient = new Patient()
            {
                PatientName="Lokman Hakim",
                DoctorId=1,
                DepatmentId=1,
                Age=45,
                AdmissioneDate=new DateTime(2024,02,02),
                MaritialStatus=true,
                Picture= "lokman.jpg"
            };
            patient.PatientDetails.Add( new PatientDetails { Test=Test.DENGU,Description="fever since 10 days",Cost=400});
            patient.PatientDetails.Add(new PatientDetails { Test = Test.COVID, Description = "fever since 10 days", Cost = 500 });
            db.Patients.Add(patient);
            db.SaveChanges();
        }
    }
}
