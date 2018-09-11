//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Xero.Api.Core.Model;
//using Xero.Api.Core.Model.Status;
//using Xero.Api.Core.Model.Types;

//namespace HonourIT_Xero_MyOB.Models
//{
//    public class ContactModel
//    {
//            public string AccountNumber { get; set; }
//            public string AccountsPayableTaxType { get; set; }
//            public string AccountsReceivableTaxType { get; set; }
//            public List<Xero.Api.Core.Model.Address> Addresses { get; set; }
//            public Balances Balances { get; set; }
//            public string BankAccountDetails { get; set; }
//            public BatchPayments BatchPayments { get; set; }
//            public BrandingTheme BrandingTheme { get; set; }
//            public List<ContactGroup> ContactGroups { get; set; }
//            public string ContactNumber { get; set; }
//            public List<ContactPerson> ContactPersons { get; set; }
//            public ContactStatus ContactStatus { get; set; }
//            public string DefaultCurrency { get; set; }
//            public decimal? Discount { get; set; }
//            public string EmailAddress { get; set; }
//            public string FirstName { get; set; }
//            public bool? HasAttachments { get; set; }
//            public Guid Id { get; set; }
//            public bool? IsCustomer { get; set; }
//            public bool? IsSupplier { get; set; }
//            public string LastName { get; set; }
//            public string Name { get; set; }
//            public PaymentTerms PaymentTerms { get; set; }
//            public List<Phone> Phones { get; set; }
//            public string PurchaseAccountCode { get; set; }
//            public List<PurchasesTrackingCategory> PurchasesTrackingCategories { get; set; }
//            public string SalesAccountCode { get; set; }
//            public List<SalesTrackingCategory> SalesTrackingCategories { get; set; }
//            public string SkypeUserName { get; set; }
//            public string TaxNumber { get; set; }
//            public string Website { get; set; }
//            public string XeroNetworkKey { get; set; }
//    }


//    public class AddressModel 
//    {
//        public string AddressLine1 { get; set; }
//        public string AddressLine2 { get; set; }
//        public string AddressLine3 { get; set; }
//        public string AddressLine4 { get; set; }
//        public AddressType AddressType { get; set; }
//        public string AttentionTo { get; set; }
//        public string City { get; set; }
//        public string Country { get; set; }
//        public string PostalCode { get; set; }
//        public string Region { get; set; }
//    }

//    public class BalancesModel
//    {
//        public Balance AccountsPayable { get; set; }
//        public Balance AccountsReceivable { get; set; }
//    }
//    public class BalanceModel
//    {
//        public decimal Outstanding { get; set; }
//        public decimal Overdue { get; set; }
//    }
//    public class BatchPaymentsModel
//    {
//        public string BankAccountName { get; set; }
//        public string BankAccountNumber { get; set; }
//        public string Code { get; set; }
//        public string Details { get; set; }
//        public string Reference { get; set; }
//    }
//}