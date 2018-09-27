using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using System.Security.Cryptography.X509Certificates;
using Xero.Api.Core;
using Xero.Api.Core.Model;
using Xero.Api.Serialization;
using Xero.Api.Example.Applications.Private;
using Xero.Api.Infrastructure.OAuth;
using System;
using Xero.Api.Core.Model.Types;

namespace HonourIT_Xero_MyOB.Controllers
{
    public class HomeController : Controller
    {
        public string ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"].ToString();
        public string ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
        public string SigningCertificatePath = ConfigurationManager.AppSettings["SigningCertificate"];
        public string SigningCertificatePassword = ConfigurationManager.AppSettings["SigningCertificatePassword"];
        public string BaseUrl = "https://api.xero.com/api.xro/2.0/";

        dynamic private_app_api = null;
        public HomeController()
        {
            //Authentication code 
            X509Certificate2 cert = new X509Certificate2(SigningCertificatePath, SigningCertificatePassword);
            private_app_api = new XeroCoreApi(BaseUrl, new PrivateAuthenticator(cert), new Consumer(ConsumerKey, ConsumerSecret), null
           , new DefaultMapper(), new DefaultMapper());
        }

        #region Invoice list
        public ActionResult InvoiceList()
        {
            IList<Invoice> invoicelist = GetAllInvoiceBill(private_app_api);
            ViewBag.InvoiceListData = invoicelist;

            return View();
        }

        //return all invoice list
        public List<Invoice> GetAllInvoiceBill(XeroCoreApi private_app_api)
        {
            List<Invoice> invoicelist = private_app_api.Invoices.Find().ToList();

            foreach (var invoice in invoicelist)
            {
                //Guid invoiceId = invoice.Id;
                //Invoice invoice = private_app_api.Invoices.Find(invoiceId);
                Xero.Api.Core.File.BinaryFile pdfFile = private_app_api.PdfFiles.Get(Xero.Api.Core.Model.Types.PdfEndpointType.Invoices, invoice.Id);
                string _path = System.IO.Path.Combine(Server.MapPath("~/Pdf/Invoice"), invoice.Id + ".pdf");
                pdfFile.Save(_path);
            }

            return invoicelist;
        }
        #endregion

        #region Purchase order list
        public ActionResult PurchaseOrderList()
        {
            IList<PurchaseOrder> POList = GetAllPurchaseOrder(private_app_api);
            if (POList == null)
                return View();
            else
                ViewBag.POListData = POList;

            return View();
        }

        //return all purchase Order list
        public List<PurchaseOrder> GetAllPurchaseOrder(XeroCoreApi private_app_api)
        {
            List<PurchaseOrder> polist = private_app_api.PurchaseOrders.Find().ToList();

            foreach (var purchase in polist)
            {
                Xero.Api.Core.File.BinaryFile pdfFile = private_app_api.PdfFiles.Get(Xero.Api.Core.Model.Types.PdfEndpointType.PurchaseOrders, purchase.Id);
                string _path = System.IO.Path.Combine(Server.MapPath("~/Pdf/Purchase"), purchase.Id + ".pdf");
                pdfFile.Save(_path);
            }
            return polist;
        }

        #endregion

        #region Payment list
        public ActionResult Payment()
        {
            IList<Payment> payment = GetPayment(private_app_api);
            if (payment == null)
                return View();
            else
                ViewBag.PaymentData = payment;

            return View();
        }

        //return all Payment list
        public List<Payment> GetPayment(XeroCoreApi private_app_api)
        {
            List<Payment> payment = private_app_api.Payments.Find().ToList();
            return payment;
        }

        #endregion

        #region Return downloaded Pdf Invoice/Purchaseorder
        //This will return Pdf from path(~/HonourIT_Xero_MyOB/Pdf/Invoice or Purchase) 
        public ActionResult Download(string fileName, string foldername)
        {
            string mappath = Server.MapPath("~/Pdf/" + foldername);
            string path = Path.Combine(mappath, fileName + ".pdf");

            return File(path, "application/pdf");
        }

        #endregion


        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";

           
           // CreateXeroInvoice(private_app_api);
            //CreateAccount(private_app_api);
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

        //Create Xero Invoice 
        public void CreateXeroInvoice(XeroCoreApi private_app_api)
        {
            Invoice inv = new Invoice();
            inv.Contact = new Contact();

            Contact contact = private_app_api.Contacts.Find().ToList().Where(c => c.EmailAddress == "avanimehta@gmail.com").FirstOrDefault();
            inv.Contact.Name = contact.Name;
            inv.CurrencyCode = inv.Contact.DefaultCurrency;

            inv.Type = InvoiceType.AccountsPayable;
            inv.Date = DateTime.Now;
            inv.DueDate = DateTime.Now.AddDays(5);
            inv.Status = Xero.Api.Core.Model.Status.InvoiceStatus.Authorised;

            inv.LineAmountTypes = new LineAmountType();
            inv.LineAmountTypes = LineAmountType.Exclusive;

            inv.LineItems = new List<LineItem>();
            LineItem li = new LineItem();
            li.Description = "Webdev inv test";
            li.Quantity = Convert.ToDecimal("1.0000");
            li.UnitAmount = Convert.ToDecimal("75.00");
            li.AccountCode = "200";
            //li.LineAmount = Convert.ToDecimal("200.00");
            //li.ItemCode = "WEBDEVPROJ";

            inv.LineItems.Add(li);

            Invoice InvoiceResponse = private_app_api.Invoices.Create(inv);

            //Payment
            Payment paymentdata = CreatePayment(private_app_api, InvoiceResponse.Id);
        }

        public Payment CreatePayment(XeroCoreApi private_app_api, Guid invoiceid)
        {
            //var invoice = Given_an_invoice(invoiceAmount, Account.Code);
            //var bankCode = BankAccount.Code;
            //Guid invoiceid = new Guid("2d3e4fe1-369d-472e-b767-6055c80376cb");
            Invoice invoiceById = private_app_api.Invoices.Find().Where(x => x.Id == invoiceid).First();

            Account account = private_app_api.Accounts.Find().ToList().Where(c => c.Name == "Arun Test Bank").FirstOrDefault();

            var payment = new Payment
            {
                Invoice = new Invoice { Number = invoiceById.Number,Id=invoiceById.Id },
                Account = new Account { Id=account.Id, Code = account.Code},
                Date = DateTime.Now,
                Amount = invoiceById.AmountDue
            };


            try
            {
                Payment paymentresp = private_app_api.Payments.Create(payment);
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }

        //Create account.
        public void CreateAccount(XeroCoreApi private_app_api)
        {
            Account account = new Account();
            Guid accountguid = Guid.NewGuid();
           // account.Id = accountguid;
            account.Name = "Arun Test Bank";
            account.Status = Xero.Api.Core.Model.Status.AccountStatus.Active;
            account.Type = AccountType.Bank;
            account.Code = "091";
           // account.TaxType = "NONE";
            account.Class = AccountClassType.Asset;
           // account.EnablePaymentsToAccount = false;
           // account.ShowInExpenseClaims = false;
            account.BankAccountNumber = "0963258741";
            account.CurrencyCode = "INR";
           // account.ReportingCode = "ASS";
           // account.ReportingCodeName = "Assets";
            account.HasAttachments = false;

            try
            {
                Account accountresponse = private_app_api.Accounts.Create(account);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //<AccountID>c0ae22d3-fb9a-49ff-b9e9-979b5800b1f0</AccountID>
        //<Name>Test Business</Name>
        //<Status>ACTIVE</Status>
        //<Type>BANK</Type>
        //<TaxType>NONE</TaxType>
        //<Class>ASSET</Class>
        //<EnablePaymentsToAccount>false</EnablePaymentsToAccount>
        //<ShowInExpenseClaims>false</ShowInExpenseClaims>
        //<BankAccountNumber>1232456578987</BankAccountNumber>
        //<BankAccountType>BANK</BankAccountType>
        //<CurrencyCode>INR</CurrencyCode>
        //<ReportingCode>ASS</ReportingCode>
        //<ReportingCodeName>Assets</ReportingCodeName>
        //<HasAttachments>false</HasAttachments>
        //<UpdatedDateUTC>2018-08-31T12:31:51.937</UpdatedDateUTC>




        //public ActionResult CreatePurchaseOrder()
        //{
        //    PurchaseOrder  po= CreatePurchaseOrderData(private_app_api);

        //    return View();
        //}

        //Create contact
        //private Contact CreateContact(Contact contact)
        //{
        //    contact.AccountNumber = "12345678912";
        //    contact.ContactStatus = ContactStatus.Active;
        //    contact.Name = "Tejas dhimmar";
        //    contact.FirstName = "Tejas";
        //    contact.LastName = "Dhimmar";
        //    contact.EmailAddress = "tejas@gmail.com";
        //    contact.SkypeUserName = "tejas001";
        //    contact.BankAccountDetails = "98746678912";
        //    contact.IsCustomer = true;
        //    Contact createcontact = private_app_api.Contacts.Create(contact);
        //    return createcontact;
        //}




        //private void GetContactsByName()
        //{
        //    X509Certificate2 cert = new X509Certificate2(SigningCertificatePath, SigningCertificatePassword);
        //    XeroCoreApi private_app_api = new XeroCoreApi(BaseUrl, new PrivateAuthenticator(cert), new Consumer(ConsumerKey, ConsumerSecret), null
        //    , new DefaultMapper(), new DefaultMapper());

        //    //var Contacts = private_app_api.Contacts.Find().ToList().Where(c => c.Name.StartsWith(txtContactNameOrEmail.Text.Trim()) || c.EmailAddress == txtContactNameOrEmail.Text.Trim()).FirstOrDefault();
        //    var Contacts = private_app_api.Contacts.Find().ToList().Where(c => c.Name.StartsWith("Arun") || c.EmailAddress == "tejas.dhimmar@nexuslinkservices.in").FirstOrDefault();
        //    if (Contacts != null)
        //        ///Uncomment the loop in case you want to get the whole list of contacts 
        //        for (int i = 0; i < Contacts.ContactGroups.Count; i++)
        //        {
        //            //MessageBox.Show("The contact " + Contacts[i].FirstName + " " + Contacts[i].LastName + " is Existing with Id " + Contacts[i].Id);
        //        }
        //    //MessageBox.Show("The contact " + Contacts.FirstName + " " + Contacts.LastName + " is Existing with Id " + Contacts.Id);
        //    else
        //    {
        //        //MessageBox.Show("This contat doesn't exist");
        //    }
        //    List<Contact> contactlist = private_app_api.Contacts.Find().ToList();
        //    ViewBag.data = private_app_api.Contacts.Find().ToList();

        //    //Invoice Sample code.
        //    //var org = private_app_api.Organisation;
        //    //Console.WriteLine("Org Name: " + org.Name);
        //    //var invoices = private_app_api.Invoices.OrderByDescending("Total").Find();
        //    //Console.WriteLine("There are " + invoices.Count() + " Invoices, the 5 biggest are:");
        //    //for (int i = 0; i < 5; i++) //just print first 5 invoices for testing
        //    //{
        //    //    var myinvoice = invoices.ElementAt(i);
        //    //    Console.WriteLine(myinvoice.Contact.Name + " " + myinvoice.Total);
        //    //}            .Find();
        //}

        //Create Purchase order data
        //public PurchaseOrder CreatePurchaseOrderData(XeroCoreApi private_app_api)
        //{
        //    Contact contact = new Xero.Api.Core.Model.Contact();
        //    //contact.Id = new Guid("5e6b3d35-9886-42fe-9753-a009cae55600");
        //    contact.ContactNumber = "73918b2493c0347d0a9979632bf88a5d234d996d6ea0eefb16";
        //    Contact getcontact = private_app_api.Contacts.Find(contact.ContactNumber);  /*.Where(x=>x.Id==contact.Id).FirstOrDefault();*/

        //    PurchaseOrder purchaseorder = new PurchaseOrder()
        //    {
        //        Contact = new Xero.Api.Core.Model.Contact()
        //        {
        //            //ContactNumber = "C0001"
        //            ContactNumber = getcontact.ContactNumber
        //        },
        //        Number = "4920249",
        //        LineItems = new List<LineItem>() {
        //            new LineItem() {
        //                UnitAmount=30.03M,
        //                Description="Plant Hire",
        //                Quantity=1,
        //                AccountCode="200",
        //                TaxType = "NONE"
        //            },
        //            new LineItem() {
        //                UnitAmount = 35.33M,
        //                Description = "Car Hire",
        //                Quantity = 1,
        //                AccountCode = "200",
        //                TaxType = "NONE"
        //            }
        //        }
        //    };
        //    var po = private_app_api.PurchaseOrders.Create(purchaseorder);

        //    return po;
        //}
    }
}
