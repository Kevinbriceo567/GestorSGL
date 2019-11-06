using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGLClientBase.Models;
using SGLClientBase.Report;
using Rotativa;

namespace SGLClientBase.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {

        private SGLClientBase5Context db = new SGLClientBase5Context();

        // GET: Clients
        public ActionResult Index(string searchBy, string search)
        {
            if(searchBy == "Name")
            {
                return View(db.Clients.Where(x => x.Name.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "Rut")
            {
                return View(db.Clients.Where(x => x.Rut == search || search == null).ToList());
            }
            else
            {
                return View(db.Clients.Where(x => x.PhoneNumber == search || search == null).ToList());
            }
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,BirthDay,EmailAddress,PhoneNumber,Direction,Rut,Observation")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,BirthDay,EmailAddress,PhoneNumber,Direction,Rut,Observation")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
            return View(db.Clients.ToList());
        }

        public ActionResult ClientReport(Client client)
        {
            ClientReport clientReport = new ClientReport();
            byte[] abytes = clientReport.PrepareReport(GetClients());
            return File(abytes, "application/pdf");
        }

        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            Client client = new Client();

            foreach(var item in db.Clients)
            {
                client = new Client();
                client.ID = item.ID;
                client.Name = item.Name;
                client.BirthDay = item.BirthDay;
                client.EmailAddress = item.EmailAddress;
                client.PhoneNumber = item.PhoneNumber;
                client.Direction = item.Direction;
                client.Rut = item.Rut;
                client.Observation = item.Observation;
                clients.Add(client);

            }

            return clients;
        }

        public ActionResult PrintClient()
        {
            var q = new ActionAsPdf("Index");
            return q;
        }


    }
}
