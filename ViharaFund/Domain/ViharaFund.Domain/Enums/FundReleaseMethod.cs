using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum FundReleaseMethod
    {
        [Description("Cheque")]
        Cheque = 1,
        [Description("Cash")]
        Cash,
        [Description("Bank Transfer")]
        BankTransfer,
        [Description("Mobile Payment")]
        MobilePayment,
        [Description("Vendor Account Credit")]
        VendorAccountCredit,
        [Description("Purchase Order")]
        PurchaseOrder,
        [Description("Prepaid Card")]
        PrepaidCard,
        [Description("Payment Gateway")]
        PaymentGateway,
        [Description("Journal Voucher")]
        JournalVoucher
    }
}
