using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HonorITDemo.Models
{
    public class ParentAccountMyOb
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string URI { get; set; }
    }

    public class TaxCodeMyOb
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string URI { get; set; }
    }

    public class BankingDetailsMyOb
    {
        public string BSBNumber { get; set; }
        public int BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string CompanyTradingName { get; set; }
        public string BankCode { get; set; }
        public bool CreateBankFiles { get; set; }
        public string DirectEntryUserId { get; set; }
        public bool IncludeSelfBalancingTransaction { get; set; }
        public string StatementParticulars { get; set; }
    }

    public class ForeignCurrencyMyOb
    {
        public string UID { get; set; }
        public string Code { get; set; }
        public string CurrencyName { get; set; }
        public string URI { get; set; }
    }

    public class AccountReceiptMyOb
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string DisplayID { get; set; }
        public string Classification { get; set; }
        public string Type { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public ParentAccountMyOb ParentAccount { get; set; }
        public bool IsActive { get; set; }
        public TaxCodeMyOb TaxCode { get; set; }
        public int Level { get; set; }
        public double OpeningBalance { get; set; }
        public double CurrentBalance { get; set; }
        //public BankingDetailsMyOb BankingDetails { get; set; }
        public bool IsHeader { get; set; }
        public ForeignCurrencyMyOb ForeignCurrency { get; set; }
        public string URI { get; set; }
        public string RowVersion { get; set; }
    }
}