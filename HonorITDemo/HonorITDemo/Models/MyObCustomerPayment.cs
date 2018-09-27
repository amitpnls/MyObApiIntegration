using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class AccountCustomerPayment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string URI { get; set; }
    }

    public class CustomerCustomerPayment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string URI { get; set; }
    }

    public class InvoiceCustomerPayment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int RowID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double AmountApplied { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Uri { get; set; }
    }

    public class ItemCustomerPayment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DepositTo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AccountCustomerPayment Account { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomerCustomerPayment Customer { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReceiptNumber { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double AmountReceived { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentMethod { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Memo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<InvoiceCustomerPayment> Invoices { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object TransactionUID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string URI { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RowVersion { get; set; }
    }

    public class MyObCustomerPayment
    {
        public List<ItemCustomerPayment> Items { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object NextPageLink { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
    }
}