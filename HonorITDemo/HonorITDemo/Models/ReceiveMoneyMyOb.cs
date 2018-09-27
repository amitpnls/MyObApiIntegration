using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class ContactReceiveMoney
    {
        //public string Type { get; set; }
        public string UID { get; set; }
        //public string Name { get; set; }
        //public string DisplayID { get; set; }
        //public string URI { get; set; }
    }

    public class CategoryReceiveMoney
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class AccountReceiveMoney
    {
        public string UID { get; set; }
        //public string Name { get; set; }
        //public string DisplayID { get; set; }
        //public string URI { get; set; }
    }

    public class TaxCodeReceiveMoney
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class LineReceiveMoney
    {
        //public int RowID { get; set; }
        public AccountReceiveMoney Account { get; set; }
        //public object Job { get; set; }
        public TaxCodeReceiveMoney TaxCode { get; set; }
        //public string Memo { get; set; }
        public double Amount { get; set; }
        public string RowVersion { get; set; }
    }

    public class ReceiveMoneyMyOb
    {
        //public string UID { get; set; }
        public string DepositTo { get; set; }
        public AccountReceiveMoney Account { get; set; }
        public ContactReceiveMoney Contact { get; set; }
        //public string ReceiptNumber { get; set; }
        public DateTime Date { get; set; }
        //public double AmountReceived { get; set; }
        //public bool IsTaxInclusive { get; set; }
        //public double TotalTax { get; set; }
        //public string PaymentMethod { get; set; }
        public string Memo { get; set; }
        //public CategoryReceiveMoney Category { get; set; }
        public List<LineReceiveMoney> Lines { get; set; }
        //public object ForeignCurrency { get; set; }
        //public string URI { get; set; }
        public string RowVersion { get; set; }
    }

    public class ReceiveMoneyMyObList
    {
        public List<ReceiveMoneyMyOb> ReceiveMoneyMyOb_list { get; set; }
        public object NextPageLink { get; set; }
        public int Count { get; set; }
    }
}