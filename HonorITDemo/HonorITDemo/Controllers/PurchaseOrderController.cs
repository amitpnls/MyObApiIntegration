using System.Web.Mvc;

using HonorITDemo.Helpers;
using HonorITDemo.Models;


namespace HonorITDemo.Controllers
{
    public class PurchaseOrderController : Controller
    {
        IAccountRightService _acountRightService = new AccountRightService();

        // GET: Quotation
        public ActionResult PurchaseOrders()
        {
            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                Common.ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            QuotesModel quotes = _acountRightService.GetQuoteList(MYOBToken);

            PurchaseOrderModel rootobj = _acountRightService.GetPurchaseOrderListAR(MYOBToken);

            return View("PurchaseOrders", rootobj);
        }

        public ActionResult DeletePurchaseOrderAR()
        {
            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                Common.ReGenerateToken();
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            PurchaseOrderModel rootobj = _acountRightService.GetPurchaseOrderListAR(MYOBToken);

            _acountRightService.DeletePurchaseOrder(rootobj, MYOBToken);

            return Redirect("PurchaseOrderAR");
        }

    }
}