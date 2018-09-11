using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    //Custom Add Supplier
    public class Supplier
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class Customer
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class Terms
    {
        public string PaymentIsDue { get; set; }
        public int DiscountDate { get; set; }
        public int BalanceDueDate { get; set; }
        public double DiscountForEarlyPayment { get; set; }
        public double MonthlyChargeForLatePayment { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public double Discount { get; set; }
        public DateTime DueDate { get; set; }
        public double FinanceCharge { get; set; }
    }

    public class Account
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class Job
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string URI { get; set; }
    }

    public class TaxCode
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    [Serializable]
    public class Line
    {
        //public int RowId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double Total { get; set; }
        public Account Account { get; set; }
        public Job Job { get; set; }
        public TaxCode TaxCode { get; set; }
        public string RowVersion { get; set; }
    }

    public class FreightTaxCode
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class Category
    {
        public string UID { get; set; }
        public string DisplayID { get; set; }
        public string Name { get; set; }
        public string URI { get; set; }
    }

    public class Salesperson
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class Item
    {
        //Custom Add
        public Supplier Supplier { get; set; }
        public string UID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string ShipToAddress { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public Customer Customer { get; set; }
        public Terms Terms { get; set; }
        public bool IsTaxInclusive { get; set; }
        public List<Line> Lines { get; set; }
        public double Subtotal { get; set; }
        public double Freight { get; set; }
        public FreightTaxCode FreightTaxCode { get; set; }
        public double TotalTax { get; set; }
        public double TotalAmount { get; set; }
        public Category Category { get; set; }
        public Salesperson Salesperson { get; set; }
        public string Comment { get; set; }
        public string ShippingMethod { get; set; }
        public string JournalMemo { get; set; }
        public DateTime PromisedDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string ReferralSource { get; set; }
        public double BalanceDueAmount { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }

    public class QuotesModel
    {
        public object NextPageLink { get; set; }
        public int Count { get; set; }
        public List<Item> Items { get; set; }
    }
}