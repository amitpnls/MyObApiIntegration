using HonorITDemo.Helpers;
using HonorITDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Security.Cryptography.X509Certificates;
using Xero.Api.Core;
using Xero.Api.Core.Model;
using Xero.Api.Serialization;
using Xero.Api.Example.Applications.Private;
using Xero.Api.Infrastructure.OAuth;
using System;
using Xero.Api.Core.Model.Types;
using System.Configuration;

namespace HonorITDemo.Controllers
{
    public class InvoiceController : Controller
    {
        IAccountRightService _acountRightService = new AccountRightService();

        //Xero Settings
        string XERO_ConsumerKey = ConfigurationManager.AppSettings["XERO_ConsumerKey"].ToString();
        string XERO_ConsumerSecret = ConfigurationManager.AppSettings["XERO_ConsumerSecret"];
        string XERO_SigningCertificatePath = ConfigurationManager.AppSettings["XERO_SigningCertificate"];
        string XERO_SigningCertificatePassword = ConfigurationManager.AppSettings["XERO_SigningCertificatePassword"];
        string XERO_BaseUrl = ConfigurationManager.AppSettings["XERO_BaseUrl"];
        dynamic private_app_api = null;

        // GET: Invoice
        public ActionResult Invoices()
        {

            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                Common.ReGenerateToken();
            }
            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            InvoiceModel invoice = _acountRightService.GetSalesOrderInvoiceListAR(MYOBToken);

            return View("Invoices", invoice);
        }

        [HttpPost]
        public ActionResult Invoices(InvoiceModel model,FormCollection form)
        {
            String selectedIds = form["InvoiceCheckedIds"].ToString();
            string[] selectedIdList = selectedIds.Split(new string[] { "," },
                                  StringSplitOptions.RemoveEmptyEntries);

            if (OAuthKeyService.OAuthInformation.Token == null)
            {
                Common.ReGenerateToken();
                return View("Index", "Home");
            }

            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;

            InvoiceItemModel invoicemodel = _acountRightService.GetSalesOrderInvoiceById(MYOBToken,selectedIdList.First());

            FromMyOb_ToXeroHelper myob_to_xero_invoice = new FromMyOb_ToXeroHelper();

            myob_to_xero_invoice.CreateXeroInvoice_FromMyObInvoice(invoicemodel, MYOBToken);

            //return View("Invoices", invoicemodel);
            return RedirectToAction("Invoices", "Invoice");
        }

        public ActionResult Receipt()
        {
            MyObCustomerPayment receipt = new MyObCustomerPayment();
            string MYOBToken = OAuthKeyService.OAuthInformation.Token.AccessToken;
            receipt = _acountRightService.GetReceiptList(MYOBToken);

            return View(receipt);
        }
    }
}
