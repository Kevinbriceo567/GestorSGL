using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGLClientBase.Models;

namespace SGLClientBase.Controllers
{
    [Authorize]
    public class ProductsNewController : Controller
    {
        private DBProductModels db = new DBProductModels();

        // GET: ProductsNew
        public ActionResult Index(string searchBy, string search)
        {
            double USDtoEUR = Convert.ToDouble(CurrencyConversion(1m, "USD", "EUR"));
            ViewBag.USDtoEUR = (Math.Truncate(USDtoEUR * 100) / 100);

            if (searchBy == "Name")
            {
                return View(db.Products.Where(x => x.Name.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "Codename")
            {
                return View(db.Products.Where(x => x.Codename == search || search == null).ToList());
            }
            else
            {
                return View(db.Products.ToList());
            }

           
        }

        /*
        // GET: ProductsNew/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // GET: ProductsNew/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsNew/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Codename,ReleaseDate,DollarPrice,ImagePath")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }
        */


        // GET: Products
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {

            string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            product.ImagePath = "~/Image/" + fileName;

            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);

            product.ImageFile.SaveAs(fileName);

            using (DBProductModels db = new DBProductModels())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }

            ModelState.Clear();


            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Product product = new Product();

            using(DBProductModels db = new DBProductModels())
            {
                product = db.Products.Where(x => x.ID == id).FirstOrDefault();
            }

            return View(product);
        }

        // GET: ProductsNew/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsNew/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Codename,ReleaseDate,DollarPrice,ImagePath")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: ProductsNew/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsNew/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

            double USDtoEUR = Convert.ToDouble(CurrencyConversion(1m, "USD", "EUR"));
            double USDtoJPY = Convert.ToDouble(CurrencyConversion(1m, "USD", "JPY"));
            double USDtoCAD = Convert.ToDouble(CurrencyConversion(1m, "USD", "CAD"));

            ViewBag.USDtoEUR = (Math.Truncate(USDtoEUR * 100) / 100);
            ViewBag.USDtoJPY = (Math.Truncate(USDtoJPY * 100) / 100);
            ViewBag.USDtoCAD = (Math.Truncate(USDtoCAD * 100) / 100);

            return View(db.Products.ToList());
        }


        public const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";

        public string CurrencyConversion(decimal amount, string fromCurrency, string toCurrency)
        {
            string url = string.Format(urlPattern, fromCurrency, toCurrency);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                decimal exchangeRate = (decimal)token.SelectToken("rate");

                return (amount * exchangeRate).ToString();
            }
    }
}
}
