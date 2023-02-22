

using System.Collections.Generic;
using System.Linq;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.PaymentModels;
using PropertyMataaz.Models.UtilityModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
         Transaction NewTransaction(Transaction transaction);
         public PaymentInitializationResponse MakePaymentRequest(Transaction transaction, PaymentPayload payload);
         Transaction GetTransactionByReferences(string reference);
         PaymentResponse ValidatePayment(string reference,int transactionId);
         PaymentLog CreateAndReturn(PaymentLog paymentLog);
         Transaction UpdateTransaction(Transaction transaction);
         Transaction GetUserTransactionByUserAndPropertyId(int propertyId, int userId);
         IEnumerable<Transaction> ListAllTransaction();
         IQueryable<Transaction> QueryTransactions();
    }
}