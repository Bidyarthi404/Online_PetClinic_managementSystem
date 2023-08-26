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
    public class UserTablesController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();

        // GET: UserTables
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var userTables = db.UserTables.Include(u => u.UserTypeTable);
            return View(userTables.ToList());
        }
        public ActionResult GetAlllabs()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lablist = db.UserTables.Where(d => (d.isVerified == false || d.isVerified == true) && d.UserTypeID == 3);
            return View(lablist);
        }
        public ActionResult GetAllDoctors()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doclist = db.UserTables.Where(d => (d.isVerified == false || d.isVerified == true) && d.UserTypeID == 2);
            return View(doclist);
        }
        public ActionResult GetAllPatients()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patlist = db.UserTables.Where(d => (d.isVerified == false || d.isVerified == true) && d.UserTypeID == 4);
            return View(patlist);
        }
        public ActionResult PendingLabRequests()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patientList = db.UserTables.Where(d => (d.isVerified == false) && d.UserTypeID == 3);
            return View(patientList);
        }
        public ActionResult PendingDocRequests()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patientList = db.UserTables.Where(d => (d.isVerified == false) && d.UserTypeID == 2);
            return View(patientList);
        }
        // GET: UserTables/Details/5
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
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // GET: UserTables/Create
        public ActionResult Create()
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables, "UserTypeID", "UserType");
            return View();
        }

        // POST: UserTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserTypeID,UserName,Password,Email,ContactNo,Description,isVerified")] UserTable userTable)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.UserTables.Add(userTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypeTables, "UserTypeID", "UserType", userTable.UserTypeID);
            return View(userTable);
        }

        // GET: UserTables/Edit/5
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
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables, "UserTypeID", "UserType", userTable.UserTypeID);
            return View(userTable);
        }

        // POST: UserTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserTypeID,UserName,Password,Email,ContactNo,Description,isVerified")] UserTable userTable)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(userTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables, "UserTypeID", "UserType", userTable.UserTypeID);
            return View(userTable);
        }

        // GET: UserTables/Delete/5
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
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // POST: UserTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (String.IsNullOrEmpty(Convert.ToString(Session["Username"])))
            {
                return RedirectToAction("Login", "Home");
            }
            UserTable userTable = db.UserTables.Find(id);
            db.UserTables.Remove(userTable);
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
