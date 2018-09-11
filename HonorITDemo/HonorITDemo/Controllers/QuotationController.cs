using System.Web.Mvc;

using HonorITDemo.Helpers;
using HonorITDemo.Models;


namespace HonorITDemo.Controllers
{
    public class QuotationController : Controller
    {
        IAccountRightService _acountRightService = new AccountRightService();

        // GET: Quotation
        [HttpGet]
        public ActionResult Quotatitons()
        {
            //IAccountRightService service = new AccountRightService();

            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                Common.ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            QuotesModel quotes = _acountRightService.GetQuoteList(MYOBToken);

            TempData["QuotesList"] = quotes;

            return View("Quotations", quotes);
        }

        public ActionResult CreatePurchaseOrderAR()
        {
            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                Common.ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            //QuotesModel quotes = service.GetQuoteList(MYOBToken);

            QuotesModel quotes = new QuotesModel();

            quotes = TempData["QuotesList"] as QuotesModel;

            _acountRightService.CreatePurchaseOrderFromQuotes(quotes, MYOBToken);

            return Redirect("PurchaseOrderAR");
        }
    }
}