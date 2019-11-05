using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SGLClientBase.Models;

namespace SGLClientBase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Client client)
        {
            return View(client);
        }

        public ActionResult Headquarters()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {

            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)){

                var user = "Kevin";

                if(email == "Kevin" && password == "1234")
                {
                    //FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("Index", "Clients");
                }

            }


            return View();

        }

        public ActionResult Index2()
        {
            return View();
        }

    }
}