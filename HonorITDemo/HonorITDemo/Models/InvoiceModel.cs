using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class CustomerInvoiceItem
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class ItemInvoiceItem
    {
        public string UID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string URI { get; set; }
    }

    public class TaxCodeInvoiceItem
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class LineInvoiceItem
    {
        public int RowID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double ShipQuantity { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountPercent { get; set; }
        public double CostOfGoodsSold { get; set; }
        public double Total { get; set; }
        public ItemInvoiceItem Item { get; set; }
        public object Job { get; set; }
        public TaxCodeInvoiceItem TaxCode { get; set; }
        public string RowVersion { get; set; }
    }

    public class TermsInvoiceItem
    {
        public string PaymentIsDue { get; set; }
        public double DiscountDate { get; set; }
        public double BalanceDueDate { get; set; }
        public double DiscountForEarlyPayment { get; set; }
        public double MonthlyChargeForLatePayment { get; set; }
        public DateTime DiscountExpiryDate { get; set; }
        public double Discount { get; set; }
        public DateTime DueDate { get; set; }
        public double FinanceCharge { get; set; }
    }

    public class FreightTaxCodeInvoiceItem
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class CategoryInvoiceItem
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class InvoiceItemModel
    {
        public string UID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public CustomerInvoiceItem Customer { get; set; }
        public DateTime? PromisedDate { get; set; }
        public double BalanceDueAmount { get; set; }
        public string Status { get; set; }
        public List<LineInvoiceItem> Lines { get; set; }
        public string ShipToAddress { get; set; }
        public TermsInvoiceItem Terms { get; set; }
        public bool IsTaxInclusive { get; set; }
        public double Subtotal { get; set; }
        public double Freight { get; set; }
        public FreightTaxCodeInvoiceItem FreightTaxCode { get; set; }
        public double TotalTax { get; set; }
        public double TotalAmount { get; set; }
        public CategoryInvoiceItem Category { get; set; }
        public object Salesperson { get; set; }
        public string Comment { get; set; }
        public string ShippingMethod { get; set; }
        public string JournalMemo { get; set; }
        public string ReferralSource { get; set; }
        public string InvoiceDeliveryStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public object Order { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }







    /// <summary>
    /// ////////////////////Invoice Model
    /// </summary>

    public class CustomerInvoice
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string URI { get; set; }
        public string DisplayID { get; set; }
    }

    public class TermsInvoice
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

    public class FreightTaxCodeInvoice
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class SalespersonInvoice
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class OrderInvoice
    {
        public string UID { get; set; }
        public string Number { get; set; }
        public string URI { get; set; }
    }
    public class CategoryInvoice
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }
    public class LineInvoice
    {
        public int RowID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double ShipQuantity { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountPercent { get; set; }
        public double CostOfGoodsSold { get; set; }
        public double Total { get; set; }
        public ItemInvoice Item { get; set; }
        public object Job { get; set; }
        public TaxCode TaxCode { get; set; }
        public string RowVersion { get; set; }
    }
    public class ItemInvoice
    {
        public string UID { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public CustomerInvoice Customer { get; set; }
        public DateTime? PromisedDate { get; set; }
        public double BalanceDueAmount { get; set; }
        public string Status { get; set; }
        public List<LineInvoice> Lines { get; set; }
        public string ShipToAddress { get; set; }
        public TermsInvoice Terms { get; set; }
        public bool IsTaxInclusive { get; set; }
        public double Subtotal { get; set; }
        public double Freight { get; set; }
        public FreightTaxCodeInvoice FreightTaxCode { get; set; }
        public double TotalTax { get; set; }
        public double TotalAmount { get; set; }
        public CategoryInvoice Category { get; set; }
        public SalespersonInvoice Salesperson { get; set; }
        public string Comment { get; set; }
        public string ShippingMethod { get; set; }
        public string JournalMemo { get; set; }
        public object ReferralSource { get; set; }
        public string InvoiceDeliveryStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public OrderInvoice Order { get; set; }
        public string OnlinePaymentMethod { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }

    public class InvoiceModel
    {
        public List<ItemInvoice> Items { get; set; }
        public object NextPageLink { get; set; }
        public int Count { get; set; }

        public string InvoiceCheckedIds { get; set; }
    }
}