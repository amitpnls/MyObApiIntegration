using System.Web;
using System.Web.Mvc;
using HonorITDemo.Helpers;
using HonorITDemo.Models;
using System.Web.Configuration;
using System.Net.Http;

namespace HonorITDemo.Controllers
{
    public partial class HomeController : Controller
    {
        IAccountRightService _acountRightService = new AccountRightService();

        public ActionResult Index()
        {

            return View();
        }

        //Authentication 
        public ActionResult OAuthCallback(string controllerName="", string actionName = "")
        {
            Common.OAuthCallback();

            if (actionName == "")
                return RedirectToAction("Quotatitons", "Quotation");
            else
                return RedirectToAction(actionName, controllerName);
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
    }
}