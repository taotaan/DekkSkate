using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DekkSkate;

namespace DekkSkate.Controllers
{
    public class ReportsController : Controller
    {
        private Entities db = new Entities();

        // GET: Reports
        public ActionResult Index()
        {
            return View(db.Reports.ToList());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reports reports = db.Reports.Find(id);
            if (reports == null)
            {
                return HttpNotFound();
            }
            return View(reports);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
        
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Report_Id,Email,Descript,SubmittedAt,IsRead")] Reports reports)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(reports);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reports);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reports reports = db.Reports.Find(id);
            if (reports == null)
            {
                return HttpNotFound();
            }
            return View(reports);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Report_Id,Email,Descript,SubmittedAt,IsRead")] Reports reports)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reports).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reports);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reports reports = db.Reports.Find(id);
            if (reports == null)
            {
                return HttpNotFound();
            }
            return View(reports);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reports reports = db.Reports.Find(id);
            db.Reports.Remove(reports);
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
