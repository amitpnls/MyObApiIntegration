using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HonorITDemo.Models;
using System.Configuration;

using System.Security.Cryptography.X509Certificates;
using Xero.Api.Example.Applications.Private;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Core;
using Xero.Api.Serialization;
using Xero.Api.Core.Model;
using Xero.Api.Core.Model.Types;
using Xero.Api.Core.Model.Status;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Text;

namespace HonorITDemo.Helpers
{
    public class FromMyOb_ToXeroHelper
    {
        string username = WebConfigurationManager.AppSettings["username"];
        string password = WebConfigurationManager.AppSettings["password"];
        string baseUrl = WebConfigurationManager.AppSettings["baseUrl"];
        string companyUID = WebConfigurationManager.AppSettings["companyUID"];
        string client_id = WebConfigurationManager.AppSettings["clientId"];

        public string XERO_ConsumerKey = ConfigurationManager.AppSettings["XERO_ConsumerKey"].ToString();
        public string XERO_ConsumerSecret = ConfigurationManager.AppSettings["XERO_ConsumerSecret"].ToString();
        public string XERO_SigningCertificatePath = ConfigurationManager.AppSettings["XERO_SigningCertificate"].ToString();
        public string XERO_SigningCertificatePassword = ConfigurationManager.AppSettings["XERO_SigningCertificatePassword"].ToString();
        public string XERO_BaseUrl = ConfigurationManager.AppSettings["XERO_BaseUrl"].ToString();

        //Create Xero Invoice From MyOb Invoice.
        public void CreateXeroInvoice_FromMyObInvoice(InvoiceItemModel myob_invoice_item, string MYOBToken)
        {
            Invoice invoice = new Invoice();
            invoice.Contact = new Contact();

            try
            {
                //Xero authentication
                X509Certificate2 cert = new X509Certificate2(XERO_SigningCertificatePath, XERO_SigningCertificatePassword);
                XeroCoreApi private_app_api = new XeroCoreApi(XERO_BaseUrl, new PrivateAuthenticator(cert), new Consumer(XERO_ConsumerKey, XERO_ConsumerSecret), null
                , new DefaultMapper(), new DefaultMapper());

                var contact = private_app_api.Contacts.Find().ToList().Where(c => c.EmailAddress == "avanimehta@gmail.com").FirstOrDefault();
                //Contact contact = private_app_api.Contacts.Find().ToList().First();
                invoice.Contact.Name = contact.Name;
                invoice.CurrencyCode = invoice.Contact.DefaultCurrency;

                invoice.Type = InvoiceType.AccountsPayable;
                invoice.Date = DateTime.Now;
                invoice.DueDate = myob_invoice_item.PromisedDate;
                invoice.Status = Xero.Api.Core.Model.Status.InvoiceStatus.Authorised;
                invoice.Number = myob_invoice_item.Number;

                invoice.LineAmountTypes = new LineAmountType();

                //invoice.LineAmountTypes = LineAmountType.Exclusive;

                invoice.LineAmountTypes = myob_invoice_item.IsTaxInclusive == true ? LineAmountType.Inclusive : LineAmountType.Inclusive;
                invoice.Reference = myob_invoice_item.Number;
                invoice.AmountDue = (decimal)myob_invoice_item.BalanceDueAmount;

                invoice.LineItems = new List<LineItem>();
                foreach (var line in myob_invoice_item.Lines)
                {
                    LineItem li = new LineItem();
                    li.Description = line.Description;
                    li.Quantity = (decimal)line.ShipQuantity;
                    li.UnitAmount = (decimal)line.UnitPrice;
                    li.AccountCode = "310";
                    //li.ItemCode = line.Item.Number;
                    li.TaxType = "OUTPUT";
                    invoice.LineItems.Add(li);
                }
                Invoice InvoiceResponse = private_app_api.Invoices.Create(invoice);


                //Do payment in xero from using xero invoice.
                Payment paymentResponse = CreateXeroPaymentFromXeroInvoice(private_app_api, InvoiceResponse.Id);

                if (paymentResponse.ValidationStatus == ValidationStatus.Ok)
                {
                    //var receiptresponse= CreateMyObReceiveMoneyFromXeroPayment(myob_invoice_item, paymentResponse, MYOBToken);
                    var receiptresponse = CreateMyObCustomerPayment(myob_invoice_item, paymentResponse, MYOBToken);
                }
                //Recept
                //Receipt receipt = CreateXeroReceiptFromPayment(private_app_api, InvoiceResponse.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Create Payment in Xero From Xero Invoice
        public Payment CreateXeroPaymentFromXeroInvoice(XeroCoreApi private_app_api, Guid invoiceid)
        {
            Payment paymentresponse = new Payment();
            try
            {
                Invoice invoiceById = private_app_api.Invoices.Find().Where(x => x.Id == invoiceid).First();

                //Find account By Name for Payent Transaction which is mendatory in Xero.
                Xero.Api.Core.Model.Account account = private_app_api.Accounts.Find().ToList().Where(c => c.Name == "Arun Test Bank").FirstOrDefault();

                var payment = new Payment
                {
                    Invoice = new Invoice { Number = invoiceById.Number, Id = invoiceById.Id },
                    Account = new Xero.Api.Core.Model.Account { Id = account.Id, Code = account.Code },
                    Date = DateTime.Now,
                    Amount = invoiceById.AmountDue
                };
                paymentresponse = private_app_api.Payments.Create(payment);
            }
            catch (Exception ex)
            {
                throw;
            }
            return paymentresponse;
        }

        //Create Receipt in myob,using from xero payment and myob invoice info.(Banking/ReceiveMoneyTxn)
        //public ReceiveMoneyMyOb CreateMyObReceiveMoneyFromXeroPayment(InvoiceItemModel invoice_myob, Payment payment_xero, string MYOBToken)
        //{
        //    ReceiveMoneyMyOb receivepayment = new ReceiveMoneyMyOb();

        //    try
        //    {
        //        AccountRightService accountright = new AccountRightService();
        //        string accountrequesturl = "https://ar1.api.myob.com/accountright/3e5400ea-9ba2-4acc-acef-b93e5f8364ef/GeneralLedger/Account/720e0fcc-fcf1-416d-9a69-7353b0079811";

        //        //Get Account details from webapi.
        //        string accountresponse = accountright.WebApiGetRequest(accountrequesturl, MYOBToken);

        //        AccountReceiptMyOb AccountSerialized = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountReceiptMyOb>(accountresponse);
        //        receivepayment.Account = new AccountReceiveMoney();
        //        receivepayment.Account.UID = AccountSerialized.UID;
        //        //receivepayment.Account.DisplayID = AccountSerialized.DisplayID;
        //        //receivepayment.Account.Name = AccountSerialized.Name;
        //        //receivepayment.Account.URI = AccountSerialized.URI;

        //        //receivepayment.AmountReceived = (double)payment_xero.Amount;

        //        receivepayment.DepositTo = "Account";
        //        receivepayment.Memo = "Payment: "+ invoice_myob.Customer.Name;

        //        receivepayment.Contact = new ContactReceiveMoney();
        //        receivepayment.Contact.UID = invoice_myob.Customer.UID;
        //        //receivepayment.Contact.DisplayID = invoice_myob.Customer.DisplayID;
        //        //receivepayment.Contact.Name = invoice_myob.Customer.Name;
        //        //receivepayment.Contact.URI = invoice_myob.Customer.URI;

        //        receivepayment.Date = payment_xero.Date;

        //        receivepayment.Lines = new List<LineReceiveMoney>();
        //        LineReceiveMoney linereceive = new LineReceiveMoney();
        //        linereceive.TaxCode = new TaxCodeReceiveMoney();
        //        linereceive.Account = new AccountReceiveMoney();

        //        foreach (var invoiceline in invoice_myob.Lines)
        //        {
        //            linereceive.Account.UID = AccountSerialized.UID;
        //            linereceive.TaxCode.UID = "b0613ff1-eabe-4aed-9d35-ec7f66e1aac1";
        //            linereceive.Amount = invoiceline.Total;
                    
        //            //linereceive.RowID = invoiceline.RowID;
        //            //linereceive.Job = invoiceline.Job;
        //            //linereceive.TaxCode.Code = "GST";
        //            //linereceive.TaxCode.UID = "3bd2fbea-54d7-4115-8030-b84360048465";

        //            receivepayment.Lines.Add(linereceive);
        //        }
                
        //        string ReceivePaymentSerialized = JsonConvert.SerializeObject(receivepayment);
        //        byte[] pbytes = Encoding.UTF8.GetBytes(ReceivePaymentSerialized);

        //        string bURL = string.Format("{0}{1}/Banking/ReceiveMoneyTxn", baseUrl, companyUID);

        //        HttpWebRequest paymentrequest = (HttpWebRequest)HttpWebRequest.Create(bURL);
        //        paymentrequest.Method = "POST";
        //        paymentrequest.Headers.Add("Authorization", "Bearer " + MYOBToken);
        //        paymentrequest.Headers.Add("x-myobapi-cftoken", Base64Encode(username, password));
        //        paymentrequest.Headers.Add("x-myobapi-key", client_id);
        //        paymentrequest.Headers.Add("x-myobapi-version", "v2");
        //        paymentrequest.Accept = "application/json";
        //        paymentrequest.ContentType = "application/json";
        //        paymentrequest.ContentLength = pbytes.Length;

        //        Stream paymentStream = paymentrequest.GetRequestStream();
        //        paymentStream.Write(pbytes, 0, pbytes.Length);

        //        HttpWebResponse webresponse = (HttpWebResponse)paymentrequest.GetResponse();

        //        paymentStream = webresponse.GetResponseStream();
        //        StreamReader reader = new StreamReader(paymentStream);
        //        string responseServer = reader.ReadToEnd();

        //        reader.Close();
        //        paymentStream.Close();
        //        webresponse.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        return receivepayment;
        //    }
        //    return receivepayment;
        //}
        
        
        //Create Myob Customer Payment on Generated Invoice. 
        public ItemCustomerPayment CreateMyObCustomerPayment(InvoiceItemModel invoice_myob, Payment payment_xero, string MYOBToken)
        {
            ItemCustomerPayment customerpayment = new ItemCustomerPayment();

            try
            {
                AccountRightService accountright = new AccountRightService();
                string accountrequesturl = "https://ar1.api.myob.com/accountright/3e5400ea-9ba2-4acc-acef-b93e5f8364ef/GeneralLedger/Account/720e0fcc-fcf1-416d-9a69-7353b0079811";

                //Get Account details from webapi.
                string accountresponse = accountright.WebApiGetRequest(accountrequesturl, MYOBToken);

                AccountCustomerPayment AccountSerialized = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountCustomerPayment>(accountresponse);
                customerpayment.Account = new AccountCustomerPayment();
                customerpayment.Account.UID = AccountSerialized.UID;

                customerpayment.Customer = new CustomerCustomerPayment();
                customerpayment.Customer.UID = invoice_myob.Customer.UID;

                customerpayment.Date = payment_xero.Date;
                customerpayment.DepositTo = "Account";
                customerpayment.PaymentMethod = "Cheque";

                customerpayment.Invoices = new List<InvoiceCustomerPayment>();
                customerpayment.Invoices.Add(new InvoiceCustomerPayment
                {
                    UID = invoice_myob.UID,
                    AmountApplied = invoice_myob.TotalAmount,
                    Type = "Invoice"
                });

                string CustomerpaymentSerialized = JsonConvert.SerializeObject(customerpayment);
                //string CustomerpaymentSerialized = "{\"DepositTo\": \"Account\", \"Account\": { \"UID\": \"720e0fcc-fcf1-416d-9a69-7353b0079811\" }, \"Customer\": { \"UID\": \"11b8409e-dad2-4df8-bfd2-7eecf691edde\" }, \"Date\": \"2018-09-25T00:00:00\", \"AmountReceived\": 499, \"PaymentMethod\": \"Cheque\", \"Memo\": \"Payment\", \"Invoices\": [ { \"RowID\": 0, \"Number\": null, \"UID\": \"f429e12d-d017-4b0d-8026-303523d65b9c\", \"AmountApplied\": 499, \"Type\": \"Invoice\" } ], \"TransactionUID\": null, \"URI\": null, \"RowVersion\": null}";
                byte[] pbytes = Encoding.UTF8.GetBytes(CustomerpaymentSerialized);

                string bURL = string.Format("{0}{1}/Sale/CustomerPayment", baseUrl, companyUID);

                HttpWebRequest paymentrequest = (HttpWebRequest)HttpWebRequest.Create(bURL);
                paymentrequest.Method = "POST";
                paymentrequest.Headers.Add("Authorization", "Bearer " + MYOBToken);
                paymentrequest.Headers.Add("x-myobapi-cftoken", Base64Encode(username, password));
                paymentrequest.Headers.Add("x-myobapi-key", client_id);
                paymentrequest.Headers.Add("x-myobapi-version", "v2");
                paymentrequest.Accept = "application/json";
                paymentrequest.ContentType = "application/json";
                paymentrequest.ContentLength = pbytes.Length;

                Stream paymentStream = paymentrequest.GetRequestStream();
                paymentStream.Write(pbytes, 0, pbytes.Length);

                HttpWebResponse webresponse = (HttpWebResponse)paymentrequest.GetResponse();

                paymentStream = webresponse.GetResponseStream();
                StreamReader reader = new StreamReader(paymentStream);
                string responseServer = reader.ReadToEnd();

                reader.Close();
                paymentStream.Close();
                webresponse.Close();
            }
            catch (Exception e)
            {
                return customerpayment;
            }
            return customerpayment;
        }

        //Convert usercredentials into Base64Encode string 
        public string Base64Encode(string username, string password = "")
        {
            string credentials = username + ":" + password;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //Create Receipt from Xero Payment,Invoice 
        //public Receipt CreateXeroReceiptFromPayment(XeroCoreApi private_app_api, Guid invoiceid)
        //{
        //    Receipt receipt_response = new Receipt();
        //    try
        //    {
        //        Invoice invoiceById = private_app_api.Invoices.Find().Where(x => x.Id == invoiceid).First();

        //        //Find account By Name for Payent Transaction which is mendatory in Xero.
        //        Xero.Api.Core.Model.Account account = private_app_api.Accounts.Find().ToList().Where(c => c.Name == "Arun Test Bank").FirstOrDefault();

        //        Receipt createreceipt = new Receipt();
        //        createreceipt.Contact = invoiceById.Contact;
        //        createreceipt.Date = DateTime.UtcNow;

        //        createreceipt.LineItems = new List<LineItem>();

        //        foreach (var invoiceline in invoiceById.LineItems)
        //        {
        //            LineItem lineitem = new LineItem();

        //            lineitem.Description = invoiceline.Description;
        //            lineitem.Quantity = (decimal)invoiceline.Quantity;
        //            lineitem.UnitAmount = (decimal)invoiceline.UnitAmount;
        //            lineitem.LineAmount= invoiceline.UnitAmount;
        //            lineitem.AccountCode = "200";
        //            createreceipt.LineItems.Add(lineitem);
        //        }

        //        //Get User
        //        User user = private_app_api.Users.Find().First();

        //        createreceipt.Reference = invoiceById.Reference;
        //        createreceipt.SubTotal = invoiceById.SubTotal;
        //        createreceipt.Total = invoiceById.Total;
        //        createreceipt.LineAmountTypes = LineAmountType.Exclusive;
        //        createreceipt.User = user;

        //        receipt_response = private_app_api.Receipts.Create(createreceipt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return receipt_response;
        //}

        //private Contact CreateContact(Contact contact)
        //{
        //    contact.AccountNumber = "00000000011";
        //    contact.ContactStatus = ContactStatus.Active;
        //    contact.Name = "ABC Test Ac";
        //    contact.FirstName = "Tejas";
        //    contact.LastName = "Dhimmar";
        //    contact.EmailAddress = "tejas@gmail.com";
        //    contact.SkypeUserName = "tejas001";
        //    contact.BankAccountDetails = "98746678912";
        //    contact.IsCustomer = true;
        //    Contact createcontact = private_app_api.Contacts.Create(contact);
        //    return createcontact;
        //}

        //Contact contact = private_app_api.Contacts.Find().ToList().Where(c => c.EmailAddress == "avanimehta@gmail.com").FirstOrDefault();
        //inv.Contact.Name = contact.Name;
        //    inv.CurrencyCode = inv.Contact.DefaultCurrency;

        //    inv.Type = InvoiceType.AccountsPayable;
        //    inv.Date = DateTime.Now;
        //    inv.DueDate = DateTime.Now.AddDays(5);
        //    inv.Status = Xero.Api.Core.Model.Status.InvoiceStatus.Authorised;

        //    inv.LineAmountTypes = new LineAmountType();
        //inv.LineAmountTypes = LineAmountType.Exclusive;

        //    inv.LineItems = new List<LineItem>();
        //    LineItem li = new LineItem();
        //li.Description = "Webdev inv test";
        //    li.Quantity = Convert.ToDecimal("1.0000");
        //    li.UnitAmount = Convert.ToDecimal("75.00");
        //    li.AccountCode = "200";
        //    //li.LineAmount = Convert.ToDecimal("200.00");
        //    //li.ItemCode = "WEBDEVPROJ";

        //    inv.LineItems.Add(li);

        //    Invoice InvoiceResponse = private_app_api.Invoices.Create(inv);

    }
}