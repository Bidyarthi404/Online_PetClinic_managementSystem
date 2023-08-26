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
    public class LabTimeSlotTablesController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();

        // GET: LabTimeSlotTables
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            var labTimeSlotTables = db.LabTimeSlotTables.Include(l => l.LabTable).Where(l=>l.LabID == lab.LabID);
            return View(labTimeSlotTables.ToList());
        }

        // GET: LabTimeSlotTables/Details/5
        public ActionResult Details(int? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            if (labTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(labTimeSlotTable);
        }

        // GET: LabTimeSlotTables/Create
        public ActionResult Create()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name");
            return View();
        }

        // POST: LabTimeSlotTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LabTimeSlotTable labTimeSlot)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            labTimeSlot.LabID = lab.LabID;
            if (ModelState.IsValid)
            {
                var findtimeslot = db.LabTimeSlotTables.Where(t => t.LabID == lab.LabID && t.FromTime == labTimeSlot.FromTime && t.ToTime ==labTimeSlot.ToTime).FirstOrDefault();
                if(findtimeslot == null)
                { 
                db.LabTimeSlotTables.Add(labTimeSlot);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already in list, Plz check!";
                }
            }

            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name", labTimeSlot.LabID);
            return View(labTimeSlot);
        }

        // GET: LabTimeSlotTables/Edit/5
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
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            if (labTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name", labTimeSlotTable.LabID);
            return View(labTimeSlotTable);
        }

        // POST: LabTimeSlotTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LabTimeSlotTable labTimeSlot)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var findtimeslot = db.LabTimeSlotTables.Where(t => t.LabID == labTimeSlot.LabID && t.FromTime == labTimeSlot.FromTime && t.ToTime == labTimeSlot.ToTime && t.LabTimeSlotID != labTimeSlot.LabTimeSlotID).FirstOrDefault();
                if (findtimeslot == null)
                {
                    db.Entry(labTimeSlot).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already in list, Plz check!";
                }
            }
            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name", labTimeSlot.LabID);
            return View(labTimeSlot);
        }

        // GET: LabTimeSlotTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            if (labTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(labTimeSlotTable);
        }

        // POST: LabTimeSlotTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            db.LabTimeSlotTables.Remove(labTimeSlotTable);
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
