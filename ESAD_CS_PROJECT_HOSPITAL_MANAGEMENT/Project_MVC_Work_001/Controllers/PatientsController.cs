using Microsoft.Ajax.Utilities;
using PagedList;
using Project_MVC_Work_001.Models;
using Project_MVC_Work_001.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC_Work_001.Controllers
{
    public class PatientsController : Controller
    {
        private readonly HospitalDbContext db=new HospitalDbContext();
        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }
        public PartialViewResult patientInformation(int pg = 1)
        {
            var data = db.Patients
                     .Include(x => x.PatientDetails)
                     .Include(x => x.Department)
                     .Include(x => x.Doctor)
                     .OrderBy(x => x.PatientId)
                     .ToPagedList(pg, 5);
            return PartialView("_patientInformation",data);
                     
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            PatientInputModel model = new PatientInputModel();
            model.PatientDetails.Add(new PatientDetails());
            ViewBag.Doctor=db.Doctors.ToList();
            ViewBag.Department=db.Departments.ToList();
            return PartialView("_CreateForm",model);
        }
        [HttpPost]
        public ActionResult Create(PatientInputModel model,string act="")
        {
            if (act == "add")
            {
                model.PatientDetails.Add(new PatientDetails());
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.Value = null;
                }

            }
            if(act =="remove")
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.PatientDetails.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.Value = null;
                }
            }
            if(act =="insert")
            {
                if(ModelState.IsValid)
                {
                    var patient = new Patient()
                    {
                        DoctorId = model.DoctorId,
                        DepatmentId=model.DepatmentId,
                        PatientName=model.PatientName,
                        AdmissioneDate=model.AdmissioneDate,
                        Age=model.Age,
                        MaritialStatus=model.MaritialStatus,
                    };
                    //for image
                    string ex = Path.GetExtension(model.Picture.FileName);
                    string file = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ex;
                    string savepath = Path.Combine(Server.MapPath("~/Images"), file);
                    model.Picture.SaveAs(savepath);
                    patient.Picture = file;

                    db.Patients.Add(patient);
                    db.SaveChanges();
                    //patientDetails
                    foreach (var item in model.PatientDetails)
                    {
                        db.Database.ExecuteSqlCommand($@"spInsertPatientDetails {(int)item.Test},{item.Description},{item.Cost},{patient.PatientId}");
                    }
                    PatientInputModel inputModel = new PatientInputModel()
                    {
                        PatientName="",
                        AdmissioneDate=DateTime.Today
                    };
                    inputModel.PatientDetails.Add(new PatientDetails());
                    ViewBag.Doctor=db.Doctors.ToList();
                    ViewBag.Department=db.Departments.ToList();
                    foreach (var item in ModelState.Values)
                    {
                        item.Value = null;
                    }
                    return View("_CreateForm",inputModel);
                }
            }
            ViewBag.Doctor = db.Doctors.ToList();
            ViewBag.Department = db.Departments.ToList();
            return View("_CreateForm",model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult EditForm(int id)
        {
            var data =db.Patients.FirstOrDefault(x=>x.PatientId==id);
            if(data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x=>x.PatientDetails).Load();
            PatientEditModel editModel = new PatientEditModel()
            {
                PatientId= id,
                DoctorId=data.DoctorId,
                DepatmentId=data.DepatmentId,
                PatientName=data.PatientName,
                Age=data.Age,
                AdmissioneDate=data.AdmissioneDate,
                MaritialStatus=data.MaritialStatus,
                PatientDetails=data.PatientDetails.ToList(),
            };
            ViewBag.Doctor = db.Doctors.ToList();
            ViewBag.Department = db.Departments.ToList();
            ViewBag.currentpicture = data.Picture;
            return PartialView("_EidtForm",editModel);
        }
        [HttpPost]
        public ActionResult Edit(PatientEditModel patientEdit,string act = "")
        {
            if(act == "add")
            {
                patientEdit.PatientDetails.Add(new PatientDetails());
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                patientEdit.PatientDetails.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.Value = null;
                }
            }
            if (act == "update")
            {
                if(ModelState.IsValid)
                {
                    var patient = db.Patients.FirstOrDefault(x => x.PatientId == patientEdit.PatientId);
                    if (patient == null) { return new HttpNotFoundResult(); }
                    patient.PatientName = patientEdit.PatientName;
                    patient.Age = patientEdit.Age;
                    patient.AdmissioneDate = patientEdit.AdmissioneDate;
                    patient.MaritialStatus = patientEdit.MaritialStatus;
                    patient.DoctorId = patientEdit.DoctorId;
                    patient.DepatmentId = patientEdit.DepatmentId;
                    if (patientEdit.Picture != null)
                    {
                        string str = Path.GetExtension(patientEdit.Picture.FileName);
                        string file = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + str;
                        string savepath = Path.Combine(Server.MapPath("~/Images"), file);
                        patientEdit.Picture.SaveAs(savepath);
                        patient.Picture = file;
                    }
                    else
                    {

                    }
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"EXEC DeletePatientDetailsByPatientId {patient.PatientId}");
                    foreach (var item in patientEdit.PatientDetails)
                    {
                        db.Database.ExecuteSqlCommand($"EXEC spInsertPatientDetails {(int)item.Test},{item.Description},{item.Cost},{patient.PatientId}");
                    }
                }
            }
            ViewBag.Doctor = db.Doctors.ToList();
            ViewBag.Department = db.Departments.ToList();
            ViewBag.currentpicture = db.Patients.FirstOrDefault(x => x.PatientId == patientEdit.PatientId)?.Picture;
            return View("_EidtForm",patientEdit);
        } 
        public ActionResult Delete(int id)
        {
            var patient = db.Patients.Find(id);
            if(patient != null)
            {
                var delete=db.PatientDetails.Where(x=>x.PatientId==id).ToList(); 
                db.PatientDetails.RemoveRange(delete);
                db.Patients.Remove(patient);
                db.SaveChanges();
                TempData["DeleteMessage"] = "Patient Delete Successfully!!!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult pagination(string name="")
        {
            List<Patient> modeldata;
            if(string.IsNullOrEmpty(name))
                modeldata=db.Patients.ToList();
            else
                modeldata=db.Patients.Where(n=>n.PatientName.ToLower().Equals(name.ToLower())).ToList();
            return PartialView("_Page", modeldata);
        }
    }
}