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

      
        //This Will return Quots List from api,If quotes is empty it will return null.
        public QuotesModel GetQuoteList(string MYOBToken)
        {
            QuotesModel quotes = new QuotesModel();
            string bURL = string.Format("{0}{1}/Sale/Quote/Service", baseUrl, companyUID);
            
            try
            {
                string jsonResponseString=WebApiGetRequest(bURL, MYOBToken);
                if (jsonResponseString.Contains("\"Count\": null"))
                {
                    return quotes;
                }
                //Convert quotes json string into Quotes Model class
                quotes = Newtonsoft.Json.JsonConvert.DeserializeObject<QuotesModel>(jsonResponseString);

              return quotes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //This Will return Purchsae order list from api,If purchsae order is empty it will return null.
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

                //Convert purchase order json string into PurchaseOrderModel class
                rootobj = Newtonsoft.Json.JsonConvert.DeserializeObject<PurchaseOrderModel>(jsonResponseString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rootobj;
        }

        //Create Purchase order from QuotesJson Account right api 
        public void CreatePurchaseOrderFromQuotes(QuotesModel quotes,string MYOBToken)
        {
            //Get Supplier Detail
            SupplierModel supplierinfo = GetSupplierListAR(MYOBToken);

            if (supplierinfo == null)
            {
                return;
            }

            //currently using first supplier from list,and assign that supplier to all purchase order.
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

                purchaseStream = webresponse.GetResponseStream();
                StreamReader reader = new StreamReader(purchaseStream);
                string responseServer = reader.ReadToEnd();

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

        //Delete Purchase Order 
        public void DeletePurchaseOrder(PurchaseOrderModel purchaseorder, string MYOBToken)
        {
            foreach (var purchase in purchaseorder.Items)
            {
                try
                {
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

        //This Will return Supplier list from api,If supplier is empty it will return null.
        public SupplierModel GetSupplierListAR(string MYOBToken)
        {
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

                return rootobj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        
        //Convert usercredentials into Base64Encode string 
        public string Base64Encode(string username, string password = "")
        {
            string credentials = username+":"+password;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}