using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IPaymentService
    {
         StandardResponse<string> InitiatePayment(PaymentModel model);
         StandardResponse<PaymentView> ValidatePayment(string transactionReference, int transactionId);
         StandardResponse<PaymentRatesView> GetPaymentRates(int PropertyId);
         StandardResponse<PagedCollection<Transaction>> ListTransactions(PagingOptions pagingOptions,string search);
         string GenerateTenancyAgreement(Transaction transaction, Property property,int TenancyId);
    }
}