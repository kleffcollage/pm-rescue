using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nut;
using PropertyMataaz.Controllers;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.PaymentModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Abstrctions;
using PropertyMataaz.Utilities.Constants;
using PropertyMataaz.Utilities.Extentions;

namespace PropertyMataaz.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICodeProvider _codeProvider;
        private readonly IUserRepository _userRepository;
        private readonly Globals _globals;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfigurationProvider _mappingConfigurations;
        private readonly IReliefRepository _reliefRepository;
        private readonly ITenancyRepository _tenancyRepository;
        private readonly IPDFHandler _pdfHandler;
        private readonly IMediaRepository _mediaRepository;

        public PaymentService(IHttpContextAccessor httpContextAccessor, ICodeProvider codeProvider, IPropertyRepository propertyRepository, IUserRepository userRepository, IOptions<Globals> globals, IPaymentRepository paymentRepository, IMapper mapper, UserManager<User> userManager, IConfigurationProvider mappingConfigurations, IReliefRepository reliefRepository, ITenancyRepository tenancyRepository, IPDFHandler pdfHandler, IMediaRepository mediaRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _codeProvider = codeProvider;
            _propertyRepository = propertyRepository;
            _userRepository = userRepository;
            _globals = globals.Value;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _userManager = userManager;
            _mappingConfigurations = mappingConfigurations;
            _reliefRepository = reliefRepository;
            _tenancyRepository = tenancyRepository;
            _pdfHandler = pdfHandler;
            _mediaRepository = mediaRepository;
        }

        public StandardResponse<string> InitiatePayment(PaymentModel model)
        {

            int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
            var user = _userManager.FindByIdAsync(UserId.ToString()).Result;
            var thisRelief = _reliefRepository.ListReliefs().FirstOrDefault(r => r.Id == model.RentReliefId);
            var transactionReference = _codeProvider.New(UserId, "pmpayref", 0, 10).CodeString;
            var thisProperty = _propertyRepository.GetById(model.PropertyId > 0 ? model.PropertyId : thisRelief.PropertyId);
            try
            {
                var newTransaction = new Transaction()
                {
                    Amount = model.Amount,
                    PropertyId = model.PropertyId > 0 ? model.PropertyId : thisRelief.PropertyId,
                    RentReliefId = model.RentReliefId,
                    TransactionReference = transactionReference,
                    Title = model.PropertyId > 0 ? thisProperty.Name : "Rent relief repayment",
                    StatusId = (int)Statuses.PENDING,
                    UserId = UserId,
                    InstallmentId = model.InstallmentId,
                };
                var paymentPayload = new PaymentPayload()
                {
                    amount = model.Amount,
                    tx_ref = transactionReference,
                    payment_options = "Card",
                    redirect_url = $"{_globals.FrontEndBaseUrl}payment/validate",
                    currency = "NGN",
                    customer = new Customer()
                    {
                        name = user.FirstName,
                        email = user.Email,
                        phoneNumber = user.PhoneNumber,
                    },
                    customization = new Customization()
                    {
                        title = thisProperty.Name,
                        description = thisProperty.Description,
                        logo = _globals.LogoURL
                    },
                };
                var result = _paymentRepository.MakePaymentRequest(newTransaction, paymentPayload);

                if (result.status != "success")
                    return StandardResponse<string>.Error(StandardResponseMessages.PAYMENT_INITIATION_FAILED);

                return StandardResponse<string>.Ok(result.data.link);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<string>.Failed();
            }
        }

        public StandardResponse<PaymentView> ValidatePayment(string transactionReference, int transactionId)
        {
            try
            {
                var transaction = _paymentRepository.GetTransactionByReferences(transactionReference);
                if (transaction == null)
                    return StandardResponse<PaymentView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                var response = _paymentRepository.ValidatePayment("", transactionId);
                var paymentLog = _mapper.Map<PaymentLog>(response.data);

                var result = _paymentRepository.CreateAndReturn(paymentLog);

                if (response.status != "success")
                    return StandardResponse<PaymentView>.Error(StandardResponseMessages.PAYMENT_UNSUCCESSFUL).AddData(new PaymentView() { Status = "false" });

                transaction.PaymentLogId = result.Id;
                transaction.StatusId = (int)Statuses.COMPLETED;
                var updatedTransaction = _paymentRepository.UpdateTransaction(transaction);

                var updatedProperty = new Property();
                if (transaction.PropertyId > 0)
                {
                    var Id = transaction.PropertyId;
                    var property = _propertyRepository.GetDetailsById((int)Id);
                    if (property.IsForSale)
                    {
                        property.StatusId = (int)Statuses.SOLD;
                    }
                    else
                    {
                        property.StatusId = (int)Statuses.INACTIVE;
                        var existingTenancy = _tenancyRepository.ListTenancy().FirstOrDefault(t => t.TenantId == transaction.UserId && t.StatusId == (int)Statuses.ACTIVE);
                        if (existingTenancy == null)
                        {
                            var newTenancyRecord = new Tenancy()
                            {
                                TenantId = transaction.UserId,
                                OwnerId = property.CreatedByUserId,
                                PropertyId = property.Id,
                                StatusId = (int)Statuses.ACTIVE,
                                TransactionId = transaction.Id,
                                RentDueDate = DateTime.Now.AddDays(365)
                            };
                            newTenancyRecord = _tenancyRepository.CreateAndReturn(newTenancyRecord);
                            //TODO: Generate Tenancy Receipt and agreement
                            var agreement = GenerateTenancyAgreement(transaction, property, newTenancyRecord.Id);
                        }

                        existingTenancy.RentDueDate = DateTime.Now.AddDays(365);
                        _tenancyRepository.Update(existingTenancy);
                    }
                    updatedProperty = _propertyRepository.Update(property);
                }

                if (transaction.RentReliefId > 0)
                {
                    var thisRelief = _reliefRepository.ListReliefs().FirstOrDefault(r => r.Id == transaction.RentReliefId);
                    var thisInstallment = thisRelief.Installments.FirstOrDefault(i => i.Id == transaction.InstallmentId);
                    thisInstallment.StatusId = (int)Statuses.COMPLETED;
                    thisInstallment.PaidOn = DateTime.Now;
                    var updatedInstallment = _reliefRepository.UpdateInstallment(thisInstallment);
                }

                //TODO: Generate Receipt and agreement and save in google cloud and save url in db

                return StandardResponse<PaymentView>.Ok(StandardResponseMessages.SUCCESSFUL).AddData(new PaymentView()
                { Status = "true", Property = updatedProperty, User = transaction.User, Transaction = transaction, IsRelief = transaction.RentReliefId > 0 });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PaymentView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PaymentRatesView> GetPaymentRates(int PropertyId)
        {
            try
            {
                var thisProperty = _propertyRepository.GetDetailsById(PropertyId);
                var tax = (_globals.TaxPercentage / 100) * thisProperty.Price;
                var response = new PaymentRatesView()
                {
                    Tax = tax,
                    Price = thisProperty.Price,
                    Rates = 0,
                    Total = (int)(tax + thisProperty.Price + 0)
                };
                return StandardResponse<PaymentRatesView>.Ok(response);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PaymentRatesView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<Transaction>> ListTransactions(PagingOptions pagingOptions, string search)
        {
            try
            {
                search = search.ToLower();
                var justTransactions = _paymentRepository.ListAllTransaction();

                if (!string.IsNullOrEmpty(search))
                {
                    justTransactions = justTransactions.Where(x => x.User.FirstName.ToLower().Contains(search) || x.User.FirstName.ToLower().Contains(search) || x.Property.Name.ToLower().Contains(search)).ToArray();

                }
                var transactions = _paymentRepository.ListAllTransaction().AsQueryable().OrderByDescending(a => a.Id).ProjectTo<TransactionView>(_mappingConfigurations).AsEnumerable();

                var PagedResponse = justTransactions.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);
                var PagedCollection = PagedCollection<Transaction>.Create(Link.ToCollection(nameof(AdminController.ListTransactions)), PagedResponse.ToArray(), justTransactions.Count(), pagingOptions);
                return StandardResponse<PagedCollection<Transaction>>.Ok(PagedCollection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<Transaction>>.Failed();
            }
        }

        public string GenerateTenancyAgreement(Transaction transaction, Property property, int TenancyId)
        {

            List<KeyValuePair<string, string>> CustomValues = new List<KeyValuePair<string, string>>();
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_LANDLORD_NAME, $"{property.CreatedByUser.FirstName} {property.CreatedByUser.LastName}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_PROPERTY_ADDRESS, $"{property.Address}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_PROPERTY_NAME, $"{property.Name}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_PROPERTY_NUMOFBEDROOMS, $"{property.NumberOfBedrooms}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_PROPERTY_PRICEINFIGURES, $"{property.FormattedPrice}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_PROPERTY_PRICEINWORDS, $"{Convert.ToInt32(property.Price).ToText(Nut.Language.English)} naira only"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_TENANT_ADDRESS, $"{transaction.User.Address}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_TENANT_NAME, $"{transaction.User.FirstName} {transaction.User.FirstName}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_TENANT_NAME, $"{transaction.User.FirstName} {transaction.User.FirstName}"));
            CustomValues.Add(new KeyValuePair<string, string>("today", $"{DateTime.Now.ToShortDateString()}"));
            CustomValues.Add(new KeyValuePair<string, string>(Constants.TENANCY_REPLACEMENT_PROPERTY_TYPE, $"{property.PropertyType.Name}"));

            var pdfString = _pdfHandler.ComposeFromTemplate("TenancyAggreement.docx", CustomValues);

            var newMedia = new Media()
            {
                Extention = "pdf",
                Base64String = pdfString,
                IsDocument = true,
                IsImage = false,
                IsVideo = false,
                Name = $"Tenancy Agreement",
                PropertyId = property.Id,
                TenancyId = TenancyId,
            };

            var result = _mediaRepository.UploadMedia(newMedia).Result;

            if (result.Succeeded)
                return result.UploadedMedia.Url;

            return "";
        }


    }
}