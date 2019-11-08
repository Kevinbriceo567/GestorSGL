using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PuppeteerSharp;
using Rotativa;
using SGLClientBase.Models;
using SGLClientBase.Report;

namespace SGLClientBase.Controllers
{
    
    [Authorize]
    public class EmployeesController : Controller
    {

        private SGLEmployee6BaseContext db = new SGLEmployee6BaseContext();

        // GET: Employees
        public ActionResult Index(string searchBy, string search)
        {
            if (searchBy == "Email")
            {
                return View(db.Employees.Where(x => x.EmailCompany.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "Rut")
            {
                return View(db.Employees.Where(x => x.Rut == search || search == null).ToList());
            }
            else
            {
                return View(db.Employees.Where(x => x.PhoneNumber == search || search == null).ToList());
            }
        }


        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Rut,BirthDay,EmailCompany,PhoneNumber,DollarSalary,Position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Rut,BirthDay,EmailCompany,PhoneNumber,DollarSalary,Position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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

        public ActionResult Statistics()
        {

            return View(db.Employees.ToList());

        }

        public ActionResult EmployeeReport(Client client)
        {
            EmployeeReport clientReport = new EmployeeReport();
            byte[] abytes = clientReport.PrepareReport(GetEmployees());
            return File(abytes, "application/pdf");
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            Employee employee = new Employee();

            foreach (var item in db.Employees)
            {
                employee = new Employee();
                employee.ID = item.ID;
                employee.FirstName = item.FirstName;
                employee.LastName = item.LastName;
                employee.Rut = item.Rut;
                employee.BirthDay = item.BirthDay;
                employee.EmailCompany = item.EmailCompany;
                employee.PhoneNumber = item.PhoneNumber;
                employee.DollarSalary = item.DollarSalary;
                employee.Position = item.Position;
                employees.Add(employee);

            }

            return employees;
        }

        public ActionResult PrintEmployee()
        {
            var q = new ActionAsPdf("Index");
            return q;
        }


    }
}
