using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{

    public class SupplierPO
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class TermsPO
    {
        public string PaymentIsDue { get; set; }
        public int DiscountDate { get; set; }
        public int BalanceDueDate { get; set; }
        public double DiscountForEarlyPayment { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public int Discount { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class ItemPO
    {
        public string UID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string URI { get; set; }
        public DateTime Date { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public Supplier Supplier { get; set; }
        public string ShipToAddress { get; set; }
        public Terms Terms { get; set; }
        public bool IsTaxInclusive { get; set; }
        public List<Line> Lines { get; set; }
        public bool IsReportable { get; set; }
        public double Subtotal { get; set; }
        public double? Freight { get; set; }
        public FreightTaxCode FreightTaxCode { get; set; }
        public double TotalTax { get; set; }
        public double TotalAmount { get; set; }
        public object Category { get; set; }
        public string Comment { get; set; }
        public string ShippingMethod { get; set; }
        public string JournalMemo { get; set; }
        public object PromisedDate { get; set; }
        public double AppliedToDate { get; set; }
        public string OrderDeliveryStatus { get; set; }
        public double BalanceDueAmount { get; set; }
        public string Status { get; set; }
        public object LastPaymentDate { get; set; }
        public object ForeignCurrency { get; set; }
        public string RowVersion { get; set; }
    }

    public class TaxCodePO
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class LinePO
    {
        public int RowID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int BillQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public double UnitPrice { get; set; }
        public int DiscountPercent { get; set; }
        public double Total { get; set; }
        public Item Item { get; set; }
        public object Job { get; set; }
        public TaxCode TaxCode { get; set; }
        public string RowVersion { get; set; }
    }

    public class FreightTaxCodePO
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class PurchaseOrderModel
    {
       public List<ItemPO> Items { get; set; }
       public object NextPageLink { get; set; }
       public int Count { get; set; }
    }
}