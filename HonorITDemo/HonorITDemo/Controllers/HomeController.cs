﻿using System.Web;
using System.Web.Mvc;
using HonorITDemo.Helpers;
using HonorITDemo.Models;
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
            CallOAuthAuthentication();

            return View();
        }
       

        [HttpGet]
        public ActionResult QuoteList()
        {
            AccountRightService service = new AccountRightService();

            if (OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthInformation.Token.AccessToken;

            QuotesModel quotes = service.GetQuoteList(MYOBToken);

            return View(quotes);
        }
       

        public ActionResult PurchaseOrderAR()
        {
            AccountRightService service = new AccountRightService();

            if (OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthInformation.Token.AccessToken;

            QuotesModel quotes = service.GetQuoteList(MYOBToken);
            
            PurchaseOrderModel rootobj = service.GetPurchaseOrderListAR(MYOBToken);

            return View(rootobj);
        }

        public ActionResult CreatePurchaseOrderAR()
        {
            AccountRightService service = new AccountRightService();

            if (OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthInformation.Token.AccessToken;

            QuotesModel quotes = service.GetQuoteList(MYOBToken);

            service.CreatePurchaseOrderFromQuotes(quotes,MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

        public ActionResult DeletePurchaseOrderAR()
        {
            if (OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthInformation.Token.AccessToken;

            AccountRightService service = new AccountRightService();

            PurchaseOrderModel rootobj = service.GetPurchaseOrderListAR(MYOBToken);

            service.DeletePurchaseOrder(rootobj, MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

        public void ReGenerateToken()
        {
            CallOAuthAuthentication();
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

            return RedirectToAction("QuoteList", "Home");
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
                        AuthorizationUrl = apiBaseUrl,
                        Key = client_id,
                        TokenUrl = tokenUrl,
                        Secret = client_secret,
                        RedirectUri = redirect_uri,
                        Scope = scope
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