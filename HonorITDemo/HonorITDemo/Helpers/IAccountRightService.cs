using HonorITDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Helpers
{
    interface IAccountRightService
    {

        QuotesModel GetQuoteList(string MYOBToken);

        PurchaseOrderModel GetPurchaseOrderListAR(string MYOBToken);

        void CreatePurchaseOrderFromQuotes(QuotesModel quotes, string MYOBToken);

        void DeletePurchaseOrder(PurchaseOrderModel purchaseorder, string MYOBToken);

        SupplierModel GetSupplierListAR(string MYOBToken);

        string WebApiGetRequest(string bURL, string MYOBToken);

        string Base64Encode(string username, string password = "");

    }
}