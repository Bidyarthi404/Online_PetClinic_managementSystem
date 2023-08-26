using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineDiagnosticSystem.Controllers
{
    public class LabApproveController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();
        public ActionResult PendingAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var pendingappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }


        public ActionResult CompleteAppointment()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var Completeappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(Completeappointment);
        }

        public ActionResult CurrentAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var Completeappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(Completeappointment);
        }


        public ActionResult AllAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var allappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID);
            return View(allappointment);
        }

        public ActionResult ChangeStatus(int? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var appoint = db.LabAppointTables.Find(id);
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appoint.LabID), "LabTimeSlotID", "Name", appoint.LabTimeSlotID);
            return View(appoint);
        }
        [HttpPost]
        public ActionResult ChangeStatus(LabAppointTable app)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(app).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");
            }
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == app.LabID), "LabTimeSlotID", "Name", app.LabTimeSlotID);
            return View(app);
        }

        public ActionResult ProcessApointment(int ? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<PatientAppointReportMV> detaillist = new List<PatientAppointReportMV>();
           
            var appoint = db.LabAppointTables.Find(id);
            var testdetails = db.LabTestDetailsTables.Where(p => p.LabTestID == appoint.LabTestID);
            foreach (var item in testdetails)
            {
                var details = new PatientAppointReportMV() { 
                    DetailName = item.Name,
                    LabAppointID = appoint.LabAppointID,
                    LabTestDetailID = item.LabTestDetailID,
                    MaxValue = item.MaxValue,
                    MinValue = item.MinValue,
                    PatientValue = 0
                };
                detaillist.Add(details);
            }
            ViewBag.TestName = appoint.LabTestTable.Name;
            return View(detaillist);
        }

        [HttpPost]
        public ActionResult ProcessApointment(FormCollection collection)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            String[] LabTestDetailID = collection["item.LabTestDetailID"].Split(',');
            String[] LabAppointID = collection["item.LabAppointID"].Split(',');
            String[] DetailName = collection["item.DetailName"].Split(',');
            String[] MinValue = collection["item.MinValue"].Split(',');
            String[] MaxValue = collection["item.MaxValue"].Split(',');
            String[] PatientValue = collection["item.PatientValue"].Split(',');
            List<PatientAppointReportMV> detaillist = new List<PatientAppointReportMV>();
            Boolean issubmit = false;

            for(int i = 0;i<LabTestDetailID.Length;i++)
            {
                var details = new PatientAppointReportMV()
                {
                    DetailName = DetailName[i],
                    LabAppointID = Convert.ToInt32(LabAppointID[i]),
                    LabTestDetailID = Convert.ToInt32(LabTestDetailID[i]),
                    MaxValue = Convert.ToInt32(MaxValue[i]),
                    MinValue= Convert.ToInt32(MinValue[i]),
                    PatientValue = Convert.ToInt32(PatientValue[i]),
                };
                if(details.PatientValue > 0)
                {
                    issubmit = true;
                }
                detaillist.Add(details);
            }
            if(issubmit == true)
            { 
            foreach (var item in detaillist)
            {
                var pdetails = new PatientTestDetailTable()
                {
                    LabAppointID = item.LabAppointID,
                    LabTestDetailID = item.LabTestDetailID,
                    PatientValue = item.PatientValue,
                };
                db.PatientTestDetailTables.Add(pdetails);
                db.SaveChanges();
            }
            int appointid = Convert.ToInt32(LabAppointID[0]);
            var appoint = db.LabAppointTables.Find(appointid);
            appoint.IsComplete = true;
            db.Entry(appoint).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("PendingAppoint");
            }
            else
            {
                ViewBag.Message = "Please Enter Patient Test Details!";
            }
            return View(detaillist);
        }

        public ActionResult ViewDetails(int ? id)
        {
            ViewBag.TestName = db.LabAppointTables.Find(id).LabTestTable.Name;
            ViewBag.Lab = db.LabAppointTables.Find(id).LabTable.Name;
            ViewBag.Patient = db.LabAppointTables.Find(id).PatientTable.Name;
            ViewBag.AppointmentNo = db.LabAppointTables.Find(id).TransectionNo;
            ViewBag.LabLogo = db.LabAppointTables.Find(id).LabTable.Photo;
            ViewBag.PatientPhoto = db.LabAppointTables.Find(id).PatientTable.Photo;
            return View(db.PatientTestDetailTables.Where(p => p.LabAppointID == id).ToList());
        }
    }
}