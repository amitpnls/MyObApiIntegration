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
       string apiBaseUrl = WebConfigurationManager.AppSettings["authorizationUrl"]; 
       string client_id = WebConfigurationManager.AppSettings["clientId"];
       string client_secret = WebConfigurationManager.AppSettings["clientSecret"];
       string redirect_uri = WebConfigurationManager.AppSettings["redirectUrl"];
       string tokenUrl = WebConfigurationManager.AppSettings["tokenUrl"];
       string scope = WebConfigurationManager.AppSettings["scope"];

        IAccountRightService _acountRightService = new AccountRightService();

        public ActionResult Index()
        {
            CallOAuthAuthentication();

            return View();
        }
       

        [HttpGet]
        public ActionResult QuoteList()
        {
            //IAccountRightService service = new AccountRightService();

            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            QuotesModel quotes = _acountRightService.GetQuoteList(MYOBToken);

            TempData["QuotesList"] = quotes;

            return View(quotes);
        }

        public ActionResult PurchaseOrderAR()
        {
            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            QuotesModel quotes = _acountRightService.GetQuoteList(MYOBToken);
            
            PurchaseOrderModel rootobj = _acountRightService.GetPurchaseOrderListAR(MYOBToken);

            return View(rootobj);
        }

        public ActionResult CreatePurchaseOrderAR()
        {
            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            //QuotesModel quotes = service.GetQuoteList(MYOBToken);

            QuotesModel quotes = new QuotesModel();

            quotes = TempData["QuotesList"] as QuotesModel;

            _acountRightService.CreatePurchaseOrderFromQuotes(quotes,MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

        public ActionResult DeletePurchaseOrderAR()
        {
            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            PurchaseOrderModel rootobj = _acountRightService.GetPurchaseOrderListAR(MYOBToken);

            _acountRightService.DeletePurchaseOrder(rootobj, MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

        public void ReGenerateToken()
        {
            CallOAuthAuthentication();
        }

        public void CallOAuthAuthentication()
        {
            OAuthKeyService.OAuthInformation.GetAuthorizationCode();
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

            if (OAuthKeyService.OAuthInformation.Token == null)
                OAuthKeyService.OAuthInformation.Token = new OAuthToken();
            OAuthKeyService.OAuthInformation.Token.AccessToken = oauthtoken.AccessToken;
            OAuthKeyService.OAuthInformation.Token.RefreshToken = oauthtoken.RefreshToken;
            OAuthKeyService.OAuthInformation.Token.ExpiresIn = oauthtoken.ExpiresIn;

            return RedirectToAction("QuoteList", "Home");
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