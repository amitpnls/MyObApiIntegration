using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HonorITDemo.Helpers;
using HonorITDemo.Models;
using Newtonsoft.Json;
using System.Web.Configuration;

namespace HonorITDemo.Controllers
{
    public class HomeController : Controller
    {
       string apiBaseUrl = WebConfigurationManager.AppSettings["authorizationUrl"]; 
       string client_id = WebConfigurationManager.AppSettings["clientId"];
       string client_secret = WebConfigurationManager.AppSettings["clientSecret"];
       string redirect_uri = WebConfigurationManager.AppSettings["redirectUrl"];
       string tokenUrl = WebConfigurationManager.AppSettings["tokenUrl"];
       string scope = WebConfigurationManager.AppSettings["scope"];

        public ActionResult Index()
        {
           // CallOAuthAuthentication();

            return View();
        }

        public ActionResult Quotes()
        {
            CallOAuthAuthentication();

            return View();
        }

        [HttpGet]
        public ActionResult QuoteList()
        {
            //Session["tokenstatus"] = 1;
            AccountRightService service = new AccountRightService();
            string MYOBToken = OAuthInformation.Token.AccessToken;

            QuotesModel quotes = service.GetQuoteList(MYOBToken);

            return View(quotes);
        }
   
        public ActionResult PurchaseOrderAR()
        {
            string MYOBToken = OAuthInformation.Token.AccessToken;
            AccountRightService service = new AccountRightService();

            QuotesModel quotes = service.GetQuoteList(MYOBToken);
            
            PurchaseOrderModel rootobj = service.GetPurchaseOrderListAR(MYOBToken);

            return View(rootobj);
        }

        public ActionResult CreatePurchaseOrderAR()
        {
            string MYOBToken = OAuthInformation.Token.AccessToken;
            AccountRightService service = new AccountRightService();

           QuotesModel quotes = service.GetQuoteList(MYOBToken);

            service.CreatePurchaseOrderFromQuotes(quotes,MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

        public ActionResult DeletePurchaseOrderAR()
        {
            string MYOBToken = OAuthInformation.Token.AccessToken;
            AccountRightService service = new AccountRightService();

            PurchaseOrderModel rootobj = service.GetPurchaseOrderListAR(MYOBToken);

            service.DeletePurchaseOrder(rootobj, MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

        public void CallOAuthAuthentication()
        {
            OAuthInformation.GetAuthorizationCode();
        }
     
        //Authentication 
        public ActionResult OAuthCallback()
        {
            var requestUri = HttpContextFactory.Current.Request.Url;
            var queries = HttpUtility.ParseQueryString(requestUri.Query);
            var code = queries["code"];

            OAuthInfo info = new OAuthInfo()
            {
                Key=client_id,
                Secret=client_secret,
                Scope= scope,
                RedirectUri=redirect_uri,
                TokenUrl=tokenUrl
            };

            //Retrieve Access token
            OAuthToken oauthtoken = OAuthServiceHelper.GetAccessToken(info, code);

            if (OAuthInformation.Token == null)
                OAuthInformation.Token = new OAuthToken();
            OAuthInformation.Token.AccessToken = oauthtoken.AccessToken;
            OAuthInformation.Token.RefreshToken = oauthtoken.RefreshToken;
            OAuthInformation.Token.ExpiresIn = oauthtoken.ExpiresIn;

            //Session["tokenstatus"] = 1;


            //if (Convert.ToInt32(Session["tokenstatus"]) == 1)
            //{
            //    return RedirectToAction("QuoteList", "Home");
            //}
            //else if (Convert.ToInt32(Session["tokenstatus"]) == 2)
            //{
            //    return RedirectToAction("PurchaseOrderAR", "Home");
            //}
            //else if (Convert.ToInt32(Session["tokenstatus"]) == 3)
            //{
            //    return RedirectToAction("QuoteList", "Home");
            //}
            return RedirectToAction("QuoteList", "Home");
        }

       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
       
            return View();
        }
        public OAuthInfo OAuthInformation
        {
            get
            {
                var info = HttpContextFactory.Current.Session["OAuthInfo"] as OAuthInfo;

                if (info == null)
                {
                    info = new OAuthInfo
                    {
                        AuthorizationUrl = ConfigurationManager.AppSettings["authorizationUrl"],
                        Key = ConfigurationManager.AppSettings["clientId"],
                        TokenUrl = ConfigurationManager.AppSettings["tokenUrl"],
                        Secret = ConfigurationManager.AppSettings["clientSecret"],
                        RedirectUri = ConfigurationManager.AppSettings["redirectUrl"],
                        Scope = ConfigurationManager.AppSettings["scope"]
                    };

                    HttpContextFactory.Current.Session["OAuthInfo"] = info;
                }
                return info;
            }
            set
            {
                HttpContextFactory.Current.Session["OAuthInfo"] = value;
            }
        }
        //public IOAuthKeyService KeyService
        //{
        //    get
        //    {
        //        var keyService = Session["KeyService"] as IOAuthKeyService ?? new SimpleOAuthKeyService();

        //        return keyService;
        //    }
        //    set { Session["KeyService"] = value; }
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}