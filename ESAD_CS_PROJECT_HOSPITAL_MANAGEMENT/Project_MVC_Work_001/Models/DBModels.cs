using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Project_MVC_Work_001.Models
{
    public enum Test { COVID=1,DENGU,BLOOD,URINE,MRI,DIABETES,HIV,X_RAY,CT_SCAN,}
	public class Doctor
	{
		public int DoctorId {  get; set; }
        [Required, StringLength(50), Display(Name = "Doctor Name")]
        public string DoctorName { get; set;}
		public virtual ICollection<Patient> Patients { get; set; }=new List<Patient>();
	}
	public class Department
	{
		public int DepartmentId { get; set; }
        [Required, StringLength(50), Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
	public class Patient
	{
		public int PatientId { get; set; }
        [Required, StringLength(50), Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Admissione Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AdmissioneDate { get; set; }
		public int Age {  get; set; }
		public bool MaritialStatus {  get; set; }
		public string Picture {  get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set;}
        [ForeignKey("Department")]
        public int DepatmentId {  get; set; }
		public virtual Doctor Doctor { get; set; }
		public virtual Department Department { get; set; }
		public virtual ICollection<PatientDetails> PatientDetails { get; set; }=new List<PatientDetails>();
	}
	public class PatientDetails
	{
		public int PatientDetailsId { get; set; }
		public Test Test { get; set; }
        public string Description { get; set; }
		public decimal Cost {  get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set;}
		public virtual Patient Patient { get; set; }
		
    }
	public class HospitalDbContext : DbContext
	{
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<PatientDetails> PatientDetails { get; set; }
	}
}