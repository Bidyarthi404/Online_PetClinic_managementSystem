using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace OnlineDiagnosticSystem.Controllers
{
    public class DoctorTimeSlotTablesController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();

        // GET: DoctorTimeSlotTables
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            var doctorTimeSlotTables = db.DoctorTimeSlotTables.Include(d => d.DoctorTable).Where(d=>d.DoctorID ==doc.DoctorID);
            return View(doctorTimeSlotTables.ToList());
        }

        // GET: DoctorTimeSlotTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            if (doctorTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(doctorTimeSlotTable);
        }

        // GET: DoctorTimeSlotTables/Create
        public ActionResult Create()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: DoctorTimeSlotTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorTimeSlotTable doctorTimeSlotTable)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            doctorTimeSlotTable.DoctorID = doc.DoctorID;
            if (ModelState.IsValid)
            {
                var find = db.DoctorTimeSlotTables.Where(t => t.DoctorID == doc.DoctorID && t.FromTime == doctorTimeSlotTable.FromTime && t.ToTime == doctorTimeSlotTable.ToTime).FirstOrDefault();
               if(find == null) 
                { 
                db.DoctorTimeSlotTables.Add(doctorTimeSlotTable);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already in list, Plz check!";
                }
              
            }
            return View(doctorTimeSlotTable);
        }

        // GET: DoctorTimeSlotTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            if (doctorTimeSlotTable == null)
            {
                return HttpNotFound();
            }

            return View(doctorTimeSlotTable);
        }

        // POST: DoctorTimeSlotTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorTimeSlotTable doctorTimeSlotTable)
        {

            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var find = db.DoctorTimeSlotTables.Where(t => t.DoctorID == doctorTimeSlotTable.DoctorID && t.FromTime == doctorTimeSlotTable.FromTime && t.ToTime == doctorTimeSlotTable.ToTime && t.DoctorTimeSlotID != doctorTimeSlotTable.DoctorTimeSlotID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(doctorTimeSlotTable).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already in list, Plz check!";
                }
                
            }
            return View(doctorTimeSlotTable);
        }

        // GET: DoctorTimeSlotTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            if (doctorTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(doctorTimeSlotTable);
        }

        // POST: DoctorTimeSlotTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorTimeSlotTable doctorTimeSlotTable = db.DoctorTimeSlotTables.Find(id);
            db.DoctorTimeSlotTables.Remove(doctorTimeSlotTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
