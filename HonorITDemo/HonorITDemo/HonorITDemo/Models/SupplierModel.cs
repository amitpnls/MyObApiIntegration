using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class AddressSupplier
    {
        public int Location { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string ContactName { get; set; }
        public string Salutation { get; set; }
    }

    public class CustomField1Supplier
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class CustomField2Supplier
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class CustomField3Supplier
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class CreditSupplier
    {
        public double Limit { get; set; }
        public double Available { get; set; }
        public double PastDue { get; set; }
    }

    public class TaxCodeSupplier
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class FreightTaxCodeSupplier
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class TermsSupplier
    {
        public string PaymentIsDue { get; set; }
        public int DiscountDate { get; set; }
        public int BalanceDueDate { get; set; }
        public double DiscountForEarlyPayment { get; set; }
        public double VolumeDiscount { get; set; }
    }

    public class BuyingDetailsSupplier
    {
        public string PurchaseLayout { get; set; }
        public string PrintedForm { get; set; }
        public string PurchaseOrderDelivery { get; set; }
        public object ExpenseAccount { get; set; }
        public string PaymentMemo { get; set; }
        public string PurchaseComment { get; set; }
        public double SupplierBillingRate { get; set; }
        public string ShippingMethod { get; set; }
        public bool IsReportable { get; set; }
        public double CostPerHour { get; set; }
        public CreditSupplier Credit { get; set; }
        public string ABN { get; set; }
        public string ABNBranch { get; set; }
        public string TaxIdNumber { get; set; }
        public TaxCodeSupplier TaxCode { get; set; }
        public FreightTaxCode FreightTaxCode { get; set; }
        public bool UseSupplierTaxCode { get; set; }
        public Terms Terms { get; set; }
    }

    public class PaymentDetailsSupplier
    {
        public string BSBNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string StatementText { get; set; }
        public object Refund { get; set; }
    }

    public class ItemSupplier
    {

        public string UID { get; set; }
        public string CompanyName { get; set; }
        public bool IsIndividual { get; set; }
        public string DisplayID { get; set; }
        public bool IsActive { get; set; }
        public List<AddressSupplier> Addresses { get; set; }
        public string Notes { get; set; }
        public object Identifiers { get; set; }
        public object CustomList1 { get; set; }
        public object CustomList2 { get; set; }
        public object CustomList3 { get; set; }
        public CustomField1Supplier CustomField1 { get; set; }
        public CustomField2Supplier CustomField2 { get; set; }
        public CustomField3Supplier CustomField3 { get; set; }
        public double CurrentBalance { get; set; }
        public BuyingDetailsSupplier BuyingDetails { get; set; }
        public PaymentDetailsSupplier PaymentDetails { get; set; }
        public object ForeignCurrency { get; set; }
        public DateTime LastModified { get; set; }
        public object PhotoURI { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }

    public class SupplierModel
    {
        public List<ItemSupplier> Items { get; set; }
        public object NextPageLink { get; set; }
        public int Count { get; set; }
    }
}