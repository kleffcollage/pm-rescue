using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.PaymentModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;
using RestSharp;
using PropertyMataaz.Utilities.Constants;

namespace PropertyMataaz.Repositories
{
    public class PaymentRepository : BaseRepository<PaymentLog>, IPaymentRepository
    {
        private readonly Globals _globals;
        public PaymentRepository(PMContext context, IOptions<Globals> globals) : base(context)
        {
            _globals = globals.Value;
        }

        public Transaction NewTransaction(Transaction transaction)
        {
            try
            {
                _context.Add<Transaction>(transaction);
                _context.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Transaction UpdateTransaction(Transaction transaction)
        {
            try
            {
                _context.Update<Transaction>(transaction);
                _context.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public PaymentInitializationResponse MakePaymentRequest(Transaction transaction, PaymentPayload payload)
        {
            try
            {
                var createdTransaction = NewTransaction(transaction);
                var client = new RestClient(UtilityConstants.FlutterWaveBaseURL);
                var request = new RestRequest("payments", Method.POST);
                request.AddJsonBody(payload);
                request.AddHeader("Authorization", $"Bearer {UtilityConstants.FlutterWaveSEC_KEY}");
                IRestResponse<PaymentInitializationResponse> response = client.Execute<PaymentInitializationResponse>(request);

                return response.Data;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public PaymentResponse ValidatePayment(string reference, int transactionId)
        {
            try
            {
                var client = new RestClient(UtilityConstants.FlutterWaveBaseURL);
                var request = new RestRequest($"transactions/{transactionId}/verify", Method.GET);
                request.AddHeader("Authorization", $"Bearer {UtilityConstants.FlutterWaveSEC_KEY}");

                IRestResponse<PaymentResponse> response = client.Execute<PaymentResponse>(request);

                return response.Data;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Transaction GetTransactionByReferences(string reference)
        {
            try
            {
                var transactions = _context.Transactions.Where(t => t.TransactionReference == reference)
                .Include(t => t.User).Include(t => t.Property);
                return transactions.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }



        public Transaction GetUserTransactionByUserAndPropertyId(int propertyId, int userId)
        {
            try
            {
                var transaction = _context.Transactions.Where(t => t.PropertyId == propertyId && t.UserId == userId).Include(t => t.PaymentLog).OrderBy(t => t.Id).LastOrDefault();
                return transaction;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<Transaction> ListAllTransaction()
        {
            try{
                var transactions = _context.Transactions.Include(t => t.Property).Include(t => t.RentRelief)
                .Include(t => t.Status).Include(t => t.User).AsNoTracking();
                return transactions;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
   
        public IQueryable<Transaction> QueryTransactions()
        {
            try
            {
                var transactions = _context.Transactions.Include(t => t.PaymentLog).Include(t => t.Property).Include(t => t.RentRelief)
                .Include(t => t.Status).Include(t => t.User);
                return transactions;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}