using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using SGLClientBase.Models;
using SGLClientBase.Report;

namespace SGLClientBase.Controllers
{
    [Authorize]
    public class ProductFinalController : Controller
    {
        private PF1Context db = new PF1Context();

        // GET: ProductFinal
        public ActionResult Index(string searchBy, string search)
        {
            int launchedThisYear = 0;

            foreach (var item in db.ProductFinals.ToList())
            {
                string dateP = item.ReleaseDate.ToString();
                string yearP = dateP.Substring(dateP.Length - 13, 4);
                string thisYear = @DateTime.Now.Year.ToString();

                if (yearP == thisYear)
                {
                    ++launchedThisYear;
                }
            }

            ViewBag.LaunchedTY = launchedThisYear;

            double USDtoEUR = Convert.ToDouble(CurrencyConversion(1m, "USD", "EUR"));
            ViewBag.USDtoEUR = (Math.Truncate(USDtoEUR * 100) / 100);

            if (searchBy == "Name")
            {
                return View(db.ProductFinals.Where(x => x.Name.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "Codename")
            {
                return View(db.ProductFinals.Where(x => x.Codename == search || search == null).ToList());
            }
            else
            {
                return View(db.ProductFinals.ToList());
            }


        }

        // GET: ProductFinal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductFinal productFinal = db.ProductFinals.Find(id);
            if (productFinal == null)
            {
                return HttpNotFound();
            }
            return View(productFinal);
        }

        // GET: ProductFinal/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductFinal img, HttpPostedFileBase ImagePath)
        {
            if (ModelState.IsValid)
            {
                if (ImagePath != null)
                {
                    ImagePath.SaveAs(HttpContext.Server.MapPath("~/Images/") + ImagePath.FileName);
                    img.ImagePath = ImagePath.FileName;
                }
                db.ProductFinals.Add(img);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(img);
        }

        // POST: ProductFinal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        /*
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "ID,Name,Description,Codename,ReleaseDate,DollarPrice,ImagePath")] ProductFinal productFinal)
    {
        if (ModelState.IsValid)
        {
            db.ProductFinals.Add(productFinal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(productFinal);
    }*/

        // GET: ProductFinal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductFinal productFinal = db.ProductFinals.Find(id);
            if (productFinal == null)
            {
                return HttpNotFound();
            }

            return View(productFinal);
        }

        // POST: ProductFinal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Codename,ReleaseDate,DollarPrice,ImagePath")] ProductFinal productFinal, HttpPostedFileBase ImagePath)
        {
            if (ImagePath != null)
            {
                ImagePath.SaveAs(HttpContext.Server.MapPath("~/Images/") + ImagePath.FileName);
                productFinal.ImagePath = ImagePath.FileName;
            }

            if (ModelState.IsValid)
            {
                db.Entry(productFinal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productFinal);
        }


        // GET: ProductFinal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductFinal productFinal = db.ProductFinals.Find(id);
            if (productFinal == null)
            {
                return HttpNotFound();
            }
            return View(productFinal);
        }

        // POST: ProductFinal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductFinal productFinal = db.ProductFinals.Find(id);
            db.ProductFinals.Remove(productFinal);
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

            return View(db.ProductFinals.ToList());
        }



        public ActionResult ProductReport(Client client)
        {
            ProductReport productReport = new ProductReport();
            byte[] abytes = productReport.PrepareReport(GetProducts());
            return File(abytes, "application/pdf");
        }

        public List<ProductFinal> GetProducts()
        {
            List<ProductFinal> products = new List<ProductFinal>();
            ProductFinal product = new ProductFinal();

            foreach (var item in db.ProductFinals)
            {
                product = new ProductFinal();
                product.ID = item.ID;
                product.Name = item.Name;
                product.Description = item.Description;
                product.Codename = item.Codename;
                product.ReleaseDate = item.ReleaseDate;
                product.DollarPrice = item.DollarPrice;
                product.ImagePath = item.ImagePath;
                products.Add(product);

            }

            return products;
        }

        public ActionResult PrintProduct()
        {
            var q = new ActionAsPdf("Index");
            return q;
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
