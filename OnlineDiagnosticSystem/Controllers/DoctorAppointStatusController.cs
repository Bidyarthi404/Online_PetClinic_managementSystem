using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineDiagnosticSystem.Controllers
{
    public class DoctorAppointStatusController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();
        // GET: DoctorAppointStatus
        public ActionResult PendingAppoint()
        {
             if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsChecked == false && d.IsFeeSubmit == false && d.DoctorComment.Trim().Length == 0);
            return View(pendingappointment);
        }
        public ActionResult CurrentAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var currentappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsChecked == false && d.IsFeeSubmit == true && d.DoctorComment.Trim().Length == 0);
            return View(currentappointment);
        }

        public ActionResult CompleteAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var currentappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsChecked == true && d.IsFeeSubmit == true && d.DoctorComment.Trim().Length > 0);
            return View(currentappointment);
        }

        public ActionResult CancelAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var cancelappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID && d.DoctorComment.Trim().Length > 0);
            return View(cancelappointment);
        }

        public ActionResult AllAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var cancelappointment = db.DoctorAppointTables.Where(d => d.PatientID == patient.PatientID);
            return View(cancelappointment);
        }
    }
}