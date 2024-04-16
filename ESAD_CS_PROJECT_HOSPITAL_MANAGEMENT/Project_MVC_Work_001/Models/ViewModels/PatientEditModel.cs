using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC_Work_001.Models.ViewModels
{
    public class PatientEditModel
    {
        public int PatientId { get; set; }
        [Required, StringLength(50), Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Admissione Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AdmissioneDate { get; set; }
        public int Age { get; set; }
        public bool MaritialStatus { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public int DoctorId { get; set; }
        public int DepatmentId { get; set; }
        public virtual List<PatientDetails> PatientDetails { get; set; } = new List<PatientDetails>(); 
    }
}