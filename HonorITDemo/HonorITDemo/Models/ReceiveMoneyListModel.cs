using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class AccountReceiveMoneyList
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class ContactReceiveMoneyList
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class TaxCodeReceiveMoneyList
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class LineReceiveMoneyList
    {
        public int RowID { get; set; }
        public AccountReceiveMoneyList Account { get; set; }
        public object Job { get; set; }
        public TaxCode TaxCode { get; set; }
        public object Memo { get; set; }
        public double Amount { get; set; }
        public string RowVersion { get; set; }
    }

    public class ItemReceiveMoneyList
    {
        public string UID { get; set; }
        public string DepositTo { get; set; }
        public AccountReceiveMoneyList Account { get; set; }
        public ContactReceiveMoneyList Contact { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime Date { get; set; }
        public double AmountReceived { get; set; }
        public bool IsTaxInclusive { get; set; }
        public double TotalTax { get; set; }
        public string PaymentMethod { get; set; }
        public string Memo { get; set; }
        public object Category { get; set; }
        public List<LineReceiveMoneyList> Lines { get; set; }
        public object ForeignCurrency { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }

    public class ReceiveMoneyListModel
    {
        public List<ItemReceiveMoneyList> Items { get; set; }
        public object NextPageLink { get; set; }
        public int Count { get; set; }
    }
}