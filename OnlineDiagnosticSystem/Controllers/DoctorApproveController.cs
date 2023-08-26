using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineDiagnosticSystem.Controllers
{
    public class DoctorApproveController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();
        // GET: DoctorApprove
        public ActionResult PendingAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == false && d.IsFeeSubmit == false && string.IsNullOrEmpty(d.DoctorComment) == true);
            return View(pendingappointment);
        }


        public ActionResult CompleteAppointment()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == true && d.IsFeeSubmit == true && string.IsNullOrEmpty(d.DoctorComment) != true);
            return View(pendingappointment);
        }

        public ActionResult AllAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID );
            return View(pendingappointment);
        }

        public ActionResult ChangeStatus(int ? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var appoint = db.DoctorAppointTables.Find(id);
            ViewBag.DoctorTimeSlotID = new SelectList (db.DoctorTimeSlotTables.Where(d => d.DoctorID == appoint.DoctorID), "DoctorTimeSlotID","Name",appoint.DoctorTimeSlotID);
            return View(appoint);
        }
        [HttpPost]
        public ActionResult ChangeStatus(DoctorAppointTable app)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if(ModelState.IsValid)
            {
                db.Entry(app).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");
            }
            ViewBag.DoctorTimeSlotID = new SelectList(db.DoctorTimeSlotTables.Where(d => d.DoctorID == app.DoctorID), "DoctorTimeSlotID", "DoctorTimeSlotID",app.DoctorTimeSlotID);
            return View(app);
        }

        public ActionResult CurrentAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var doc = (DoctorTable)Session["Doctor"];
            var curentappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == false && d.IsFeeSubmit == true && string.IsNullOrEmpty(d.DoctorComment) == true);
            return View(curentappointment);
        }
        public ActionResult ProcessAppointment(int ? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var appoint = db.DoctorAppointTables.Find(id);

            return View(appoint);

        }
        [HttpPost]
        public ActionResult ProcessAppointment(DoctorAppointTable app)
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
            return View(app);
        }
    }
}