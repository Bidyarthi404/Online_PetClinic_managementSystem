using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineDiagnosticSystem.Controllers
{
    public class LabAppointStatusController : Controller
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
            var pendingappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }
        public ActionResult CurrentAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var currentappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(currentappointment);
        }

        public ActionResult CompleteAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var completeappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(completeappointment);
        }

        public ActionResult CancelAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var Cancelappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && (d.IsComplete == false || d.IsFeeSubmit == false) && d.AppointDate < DateTime.Now);
            return View(Cancelappointment);
        }

        public ActionResult AllAppoint()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var allAppointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID);
            return View(allAppointment);
        }
    }
}