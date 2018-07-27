using HonorITDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace HonorITDemo.Helpers
{
    public class AccountRightService
    {
        string apiBaseUrl = WebConfigurationManager.AppSettings["authorizationUrl"];
        string client_id = WebConfigurationManager.AppSettings["clientId"];
        string client_secret = WebConfigurationManager.AppSettings["clientSecret"];
        string redirect_uri = WebConfigurationManager.AppSettings["redirectUrl"];
        string tokenUrl = WebConfigurationManager.AppSettings["tokenUrl"];
        string scope = WebConfigurationManager.AppSettings["scope"];
        string username = WebConfigurationManager.AppSettings["username"];
        string password = WebConfigurationManager.AppSettings["password"];
        string baseUrl = WebConfigurationManager.AppSettings["baseUrl"];
        string companyUID = WebConfigurationManager.AppSettings["companyUID"];

      
        public QuotesModel GetQuoteList(string MYOBToken)
        {
            QuotesModel quotes = new QuotesModel();
            // string MYOBToken = OAuthInformation.Token.AccessToken;
            string bURL = string.Format("{0}{1}/Sale/Quote/Service", baseUrl, companyUID);
            
            try
            {
                string jsonResponseString=WebApiGetRequest(bURL, MYOBToken);
                if (jsonResponseString.Contains("\"Count\": null"))
                {
                    return quotes;
                }
                quotes = Newtonsoft.Json.JsonConvert.DeserializeObject<QuotesModel>(jsonResponseString);
            }
            catch (Exception e)
            {
                throw;
            }
            return quotes;
        }

        //Get Purchase order List for Account Rights Api.
        public PurchaseOrderModel GetPurchaseOrderListAR(string MYOBToken)
        {
            PurchaseOrderModel rootobj = new PurchaseOrderModel();
            string bURL = string.Format("{0}{1}/Purchase/Order/Service", baseUrl, companyUID);

            try
            {
                string jsonResponseString = WebApiGetRequest(bURL, MYOBToken);
                //convert jsonstring to json class
                if (jsonResponseString.Contains("\"Count\": null"))
                {
                    return rootobj;
                }
                rootobj = Newtonsoft.Json.JsonConvert.DeserializeObject<PurchaseOrderModel>(jsonResponseString);
            }
            catch (Exception)
            {
                throw;
            }
            return rootobj;
        }

        //Create Purchase Order from QuotesJson Account right api 
        public void CreatePurchaseOrderFromQuotes(QuotesModel quotes,string MYOBToken)
        {
            //Get Supplier Detail
            SupplierModel supplierinfo = GetSupplierListAR(MYOBToken);
            var supplier = supplierinfo.Items.FirstOrDefault();

            foreach (var quote in quotes.Items)
            {
             try
              {
                quote.Supplier = new Supplier();
                quote.Supplier.UID = supplier.UID;
                quote.Supplier.Name = supplier.CompanyName;
                quote.Supplier.DisplayID = supplier.DisplayID;
                quote.Supplier.URI = supplier.URI;

                string quoteJson = JsonConvert.SerializeObject(quote);

                byte[] pbytes = Encoding.UTF8.GetBytes(quoteJson);

               //string bURL = "https://api.myob.com/accountright/32e7d2a5-a89f-4be8-af70-b2aeb789c203/Purchase/Order/Service";

                string bURL = string.Format("{0}{1}/Purchase/Order/Service", baseUrl, companyUID);

                HttpWebRequest purchaserequest = (HttpWebRequest)HttpWebRequest.Create(bURL);
                purchaserequest.Method = "POST";
                purchaserequest.Headers.Add("Authorization", "Bearer " + MYOBToken);
                purchaserequest.Headers.Add("x-myobapi-cftoken", Base64Encode(username, password));
                purchaserequest.Headers.Add("x-myobapi-key", client_id);
                purchaserequest.Headers.Add("x-myobapi-version", "v2");
                purchaserequest.Accept = "application/json";
                purchaserequest.ContentType = "application/json";
                purchaserequest.ContentLength = pbytes.Length;

                Stream purchaseStream = purchaserequest.GetRequestStream();
                purchaseStream.Write(pbytes, 0, pbytes.Length);

                HttpWebResponse webresponse = (HttpWebResponse)purchaserequest.GetResponse();
                string returnresponse = webresponse.StatusCode.ToString();

                purchaseStream = webresponse.GetResponseStream();
                StreamReader reader = new StreamReader(purchaseStream);
                string responseServer = reader.ReadToEnd();
                // MessageBox.Show(responseServer);
                reader.Close();
                purchaseStream.Close();
                webresponse.Close();

              }
              catch (Exception e)
              {
                  continue;
              }
            }
        }

        //Delete Purchase Order Account right api 
        public void DeletePurchaseOrder(PurchaseOrderModel purchaseorder, string MYOBToken)
        {
            foreach (var purchase in purchaseorder.Items)
            {
                try
                {
                    //string url = string.Format("https://api.myob.com/accountright/32e7d2a5-a89f-4be8-af70-b2aeb789c203/Purchase/Order/Item/{0}", purchase.UID);
                    string bURL = string.Format("{0}{1}/Purchase/Order/Item/{2}", baseUrl, companyUID, purchase.UID);

                    HttpWebRequest purchaserequest = (HttpWebRequest)HttpWebRequest.Create(bURL);
                    purchaserequest.Method = "DELETE";
                    purchaserequest.Headers.Add("Authorization", "Bearer " + MYOBToken);
                    purchaserequest.Headers.Add("x-myobapi-cftoken", Base64Encode(username, password));
                    purchaserequest.Headers.Add("x-myobapi-key", client_id);
                    purchaserequest.Headers.Add("x-myobapi-version", "v2");
                    purchaserequest.Accept = "application/json";
                    purchaserequest.ContentType = "application/json";

                    HttpWebResponse webresponse = (HttpWebResponse)purchaserequest.GetResponse();
                    string returnresponse = webresponse.StatusCode.ToString();

                    using (StreamReader reader = new StreamReader(webresponse.GetResponseStream()))
                    {
                        returnresponse = reader.ReadToEnd();
                    }
                    webresponse.Close();
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        //Get Supplier List
        public SupplierModel GetSupplierListAR(string MYOBToken)
        {
            //string bURL = "https://ar1.api.myob.com/accountright/32e7d2a5-a89f-4be8-af70-b2aeb789c203/Contact/Supplier";
            string bURL = string.Format("{0}{1}/Contact/Supplier", baseUrl, companyUID);
            SupplierModel rootobj = new SupplierModel();

            try
            {
                string jsonResponseString = WebApiGetRequest(bURL, MYOBToken);

                if (jsonResponseString.Contains("\"Count\": null"))
                {
                    return rootobj;
                }
                rootobj = Newtonsoft.Json.JsonConvert.DeserializeObject<SupplierModel>(jsonResponseString);
            }
            catch (Exception)
            {
                throw;
            }
            return rootobj;
        }

        //Get WebApi Response
        public string WebApiGetRequest(string bURL, string MYOBToken)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(bURL);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + MYOBToken);
            request.Headers.Add("x-myobapi-cftoken", Base64Encode(username, password));
            request.Headers.Add("x-myobapi-key", client_id);
            request.Headers.Add("x-myobapi-version", "v2");
            request.Accept = "application/json";
            request.ContentType = "application/json";
            var quotesResponse = (HttpWebResponse)request.GetResponse();

            string jsonResponse = "";
            using (StreamReader reader = new StreamReader(quotesResponse.GetResponseStream()))
            {
                jsonResponse = reader.ReadToEnd();
            }
            return jsonResponse;
        }

        public string Base64Encode(string username, string password = "")
        {
            string credentials = username+":"+password;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}