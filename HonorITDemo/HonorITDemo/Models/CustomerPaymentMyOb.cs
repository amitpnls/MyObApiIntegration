using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class AccountMyOb
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class CustomerMyOb
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class InvoiceMyOb
    {
        public int RowID { get; set; }
        public string Number { get; set; }
        public string UID { get; set; }
        public double AmountApplied { get; set; }
        public string Type { get; set; }
        public string URI { get; set; }
    }

    public class CustomerPaymentMyOb
    {
        public string UID { get; set; }
        public string DepositTo { get; set; }
        public AccountMyOb Account { get; set; }
        public CustomerMyOb Customer { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime Date { get; set; }
        public double AmountReceived { get; set; }
        public string PaymentMethod { get; set; }
        public string Memo { get; set; }
        public List<InvoiceMyOb> Invoices { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }


}