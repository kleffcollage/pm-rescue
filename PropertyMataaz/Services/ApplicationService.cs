using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PropertyMataaz.Controllers;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Extentions;

namespace PropertyMataaz.Services
{
    public class ApplicationService : IApplicationService
    {
        public IApplicationRepository _applicationRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public IMapper _mapper;
        public IConfigurationProvider _mappingConfigurations;
        public UserManager<User> _userManager;
        private readonly IMediaRepository _mediaRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IReliefRepository _reliefRepository;

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations, UserManager<User> userManager, IMediaRepository mediaRepository, IPaymentRepository paymentRepository, IReliefRepository reliefRepository)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
            _userManager = userManager;
            _mediaRepository = mediaRepository;
            _paymentRepository = paymentRepository;
            _reliefRepository = reliefRepository;
        }

        public StandardResponse<ApplicationView> CreateApplication(ApplicationModel model)
        {
            var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
            try
            {
                var thisUser = _userManager.Users.Where(u => u.Id == loggedInUserId).FirstOrDefault();
                thisUser.MiddleName = model.Register.MiddleName;
                thisUser.MaritalStatus = model.Register.MaritalStatus;
                thisUser.WorkAddress = model.Register.WorkAddress;
                thisUser.Occupation = model.Register.Occupation;
                thisUser.Employer = model.Register.Employer;
                thisUser.Nationality = model.Register.Nationality;
                thisUser.Address = model.Register.Address;
                thisUser.DateOfBirth = model.Register.DateOfBirth;
                thisUser.AnnualIncome  = model.Register.AnnualIncome ?? null;
                var updatedUser = _userManager.UpdateAsync(thisUser).Result;

                if (model.Register.PassportPhotograph != null)
                {
                    var media = _mapper.Map<Media>(model.Register.PassportPhotograph);
                    media.Name = thisUser.FirstName + "_" + thisUser.LastName + "_" + "Passport";
                    media.PropertyId = null;
                    var Result = _mediaRepository.UploadMedia(media).Result;

                    if (Result.Succeeded)
                        thisUser.PassportPhotographId = Result.UploadedMedia.Id;

                }

                if (model.Register.WorkId != null)
                {
                    var media = _mapper.Map<Media>(model.Register.WorkId);
                    media.Name = thisUser.FirstName + "_" + thisUser.LastName + "_" + "Work_Id";
                    media.PropertyId = null;
                    var Result = _mediaRepository.UploadMedia(media).Result;

                    if (Result.Succeeded)
                        thisUser.WorkIdId = Result.UploadedMedia.Id;

                }

                var up = _userManager.UpdateAsync(thisUser).Result;

                if (model.ApplicationTypeId == (int)ApplicationTypes.RELIEF)
                {
                    if ((model.PayBackDate - DateTime.Now.Date).TotalDays < 30)
                    {
                        return StandardResponse<ApplicationView>.Failed("Please select a payback date that is more thana 30 days");
                    }
                }

                var mappedApplication = _mapper.Map<Application>(model);
                mappedApplication.UserId = loggedInUserId;
                mappedApplication.StatusId = (int)Statuses.ACTIVE;
                mappedApplication.UserId = loggedInUserId;

                var created = _applicationRepository.CreateAndReturn(mappedApplication);

                if (created == null)
                    return StandardResponse<ApplicationView>.Failed();

                return StandardResponse<ApplicationView>.Ok().AddData(_mapper.Map<ApplicationView>(created));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ApplicationView>.Failed();
            }
        }

        public StandardResponse<PagedCollection<ApplicationView>> ListByProperty(int PropertyId, PagingOptions pagingOptions)
        {
            try
            {
                var applications = _applicationRepository.List().Where(p => p.PropertyId == PropertyId).OrderByDescending(p => p.DateCreated)
                                                         .AsQueryable()
                                                         .AsEnumerable();
                var PagedApplications = applications.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);
                var mappedApplication = new List<ApplicationView>();

                foreach (var application in applications)
                {
                    var mapped = _mapper.Map<ApplicationView>(application);
                    mappedApplication.Add(mapped);
                }

                var PagedResponse = PagedCollection<ApplicationView>.Create(Link.ToCollection(nameof(ApplicationController.ListActiveApplications)), mappedApplication.ToArray(), applications.Count(), pagingOptions);

                return StandardResponse<PagedCollection<ApplicationView>>.Ok().AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ApplicationView>>.Failed();
            }
        }

        public StandardResponse<ApplicationView> GetById(int id)
        {
            try
            {
                var application = _applicationRepository.List().Where(a => a.Id == id).FirstOrDefault();

                if (application == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.APPLICATION_NOT_FOUND);

                return StandardResponse<ApplicationView>.Ok().AddData(_mapper.Map<ApplicationView>(application));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public StandardResponse<PagedCollection<Application>> ListPendingReliefApplications(PagingOptions pagingOptions)
        {
            try
            {
                var applications = _applicationRepository.List()
                                                         .AsQueryable()
                                                         .AsEnumerable().Where(a => a.ReliefAmount > 0);
                var PagedApplications = applications.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<Application>.Create(Link.ToCollection(nameof(AdminController.ListReliefApplications)), PagedApplications.ToArray(), applications.Count(), pagingOptions);

                return StandardResponse<PagedCollection<Application>>.Ok().AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<Application>>.Failed();
            }
        }

        public StandardResponse<PagedCollection<Application>> ListRentApplications(PagingOptions pagingOptions)
        {
            try
            {
                var applications = _applicationRepository.List()
                                                         .AsQueryable()
                                                         .Where(a => a.ApplicationTypeId == (int)ApplicationTypes.RENT)
                                                         .AsEnumerable();
                var PagedApplications = applications.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<Application>.Create(Link.ToCollection(nameof(AdminController.ListReliefApplications)), PagedApplications.ToArray(), applications.Count(), pagingOptions);

                return StandardResponse<PagedCollection<Application>>.Ok().AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<Application>>.Failed();
            }
        }

        public StandardResponse<PagedCollection<ApplicationView>> ListAcceptedReliefApplications(PagingOptions pagingOptions)
        {
            try
            {
                var applications = _applicationRepository.Query().OrderByDescending(a => a.Id).Where(a => a.StatusId == (int)Statuses.ACCEPTED && a.ReliefAmount > 0)
                                                         .ProjectTo<ApplicationView>(_mappingConfigurations);

                var PagedApplications = applications.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<ApplicationView>.Create(Link.ToCollection(nameof(AdminController.ListReliefApplications)), PagedApplications.ToArray(), applications.Count(), pagingOptions);

                return StandardResponse<PagedCollection<ApplicationView>>.Ok().AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ApplicationView>>.Failed();
            }
        }

        public StandardResponse<PagedCollection<ApplicationView>> ListReviewedReliefApplications(PagingOptions pagingOptions)
        {
            try
            {
                var applications = _applicationRepository.Query().OrderByDescending(a => a.Id).Where(a => a.StatusId == (int)Statuses.REVIEWED && a.ReliefAmount > 0)
                                                         .ProjectTo<ApplicationView>(_mappingConfigurations);

                var PagedApplications = applications.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<ApplicationView>.Create(Link.ToCollection(nameof(AdminController.ListReliefApplications)), PagedApplications.ToArray(), applications.Count(), pagingOptions);

                return StandardResponse<PagedCollection<ApplicationView>>.Ok().AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ApplicationView>>.Failed();
            }
        }

        public StandardResponse<PagedCollection<ApplicationView>> ListApprovedReliefApplications(PagingOptions pagingOptions)
        {
            try
            {
                var applications = _applicationRepository.Query().OrderByDescending(a => a.Id).Where(a => a.StatusId == (int)Statuses.APPROVED && a.ReliefAmount > 0)
                                                         .ProjectTo<ApplicationView>(_mappingConfigurations);
                var PagedApplications = applications.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<ApplicationView>.Create(Link.ToCollection(nameof(AdminController.ListReliefApplications)), PagedApplications.ToArray(), applications.Count(), pagingOptions);

                return StandardResponse<PagedCollection<ApplicationView>>.Ok().AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ApplicationView>>.Failed();
            }
        }
        public StandardResponse<ApplicationView> Approve(int id)
        {
            try
            {
                var application = _applicationRepository.List().Where(a => a.Id == id).FirstOrDefault();

                if (application == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.APPLICATION_NOT_FOUND);

                if(application.StatusId == (int)Statuses.APPROVED)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.APPLICATION_ALREADY_APPROVE);

                var approvedApplication = _applicationRepository.Approve(application);
                if (application.ApplicationTypeId == (int)ApplicationTypes.RELIEF)
                {
                    var relief = new RentRelief()
                    {
                        PropertyId = (int)approvedApplication.PropertyId,
                        UserId = approvedApplication.UserId,
                    };
                    var createdRelief = _reliefRepository.CreateAndReturn(relief);
                    var loanAmount = approvedApplication.ReliefAmount;
                    var loanInterest = approvedApplication.ReliefAmount / 100 * 7.5;
                    var totalLoanAmount = loanAmount + loanInterest;
                    int numberOfInstallments = 0;
                    var installments = new List<Installment>();
                    if (approvedApplication.RepaymentFrequency.ToLower() == "weekly")
                    {
                        var numberOfDaysTillLoanDueDate = (approvedApplication.PayBackDate - DateTime.Now.Date).TotalDays;
                        numberOfInstallments = (int)(numberOfDaysTillLoanDueDate / 7);
                        for (int i = 1; i <= numberOfInstallments; i++)
                        {
                            var ins = new Installment()
                            {
                                Amount = totalLoanAmount / numberOfInstallments,
                                DateCreated = DateTime.Now,
                                RentReliefId = createdRelief.Id,
                                StatusId = (int)Statuses.PENDING,
                                DateDue = DateTime.Now.AddDays(i * 7),
                            };
                            installments.Add(ins);
                        }
                    }
                    else
                    {
                        var numberOfDaysTillLoanDueDate = (approvedApplication.PayBackDate - DateTime.Now.Date).TotalDays;
                        numberOfInstallments = (int)(numberOfDaysTillLoanDueDate / 30);
                        for (int i = 1; i <= numberOfInstallments; i++)
                        {
                            var ins = new Installment()
                            {
                                Amount = totalLoanAmount / numberOfInstallments,
                                DateCreated = DateTime.Now,
                                RentReliefId = createdRelief.Id,
                                StatusId = (int)Statuses.PENDING,
                                DateDue = DateTime.Now.AddDays(i * 30),
                            };
                            installments.Add(ins);
                        }
                        _reliefRepository.CreateBulkInstallments(installments);
                    }

                }


                //TODO: Create new rent relief record and installments based on the payment frequency and amount 

                if (approvedApplication == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                // var rejectedApplications = _applicationRepository.List().Where(a => a.PropertyId == application.PropertyId && a.Id != application.Id);

                // foreach (var app in rejectedApplications)
                // {
                //     _applicationRepository.Reject(app);
                // }

                return StandardResponse<ApplicationView>.Ok(_mapper.Map<ApplicationView>(approvedApplication));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ApplicationView>.Failed();
            }
        }

        public StandardResponse<ApplicationView> AcceptReliefApplication(int id)
        {
            try
            {
                var application = _applicationRepository.List().Where(a => a.Id == id).FirstOrDefault();

                if (application == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.APPLICATION_NOT_FOUND);

                application.StatusId = (int)Statuses.ACCEPTED;
                var AcceptedApplication = _applicationRepository.Update(application);

                if (AcceptedApplication == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<ApplicationView>.Ok(_mapper.Map<ApplicationView>(AcceptedApplication));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ApplicationView>.Failed(); ;
            }

        }

        public StandardResponse<ApplicationView> ReviewReliefApplication(int id)
        {
            try
            {
                var application = _applicationRepository.List().Where(a => a.Id == id).FirstOrDefault();

                if (application == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.APPLICATION_NOT_FOUND);

                application.StatusId = (int)Statuses.REVIEWED;
                var AcceptedApplication = _applicationRepository.Update(application);

                if (AcceptedApplication == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<ApplicationView>.Ok(_mapper.Map<ApplicationView>(AcceptedApplication));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ApplicationView>.Failed(); ;
            }

        }

        public StandardResponse<ApplicationView> Reject(int id)
        {
            try
            {
                var application = _applicationRepository.List().Where(a => a.Id == id).FirstOrDefault();

                if (application == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.APPLICATION_NOT_FOUND);

                var rejectedApplication = _applicationRepository.Reject(application);

                if (rejectedApplication == null)
                    return StandardResponse<ApplicationView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<ApplicationView>.Ok(_mapper.Map<ApplicationView>(rejectedApplication));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ApplicationView>.Failed(); ;
            }
        }


        public StandardResponse<IEnumerable<ApplicationType>> ListTypes()
        {
            try
            {
                var result = _applicationRepository.ListTypes();
                return StandardResponse<IEnumerable<ApplicationType>>.Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<ApplicationType>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<ApplicationStatusView> GetApplication(int propertyId)
        {
            var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
            try
            {
                var existingApplication = _applicationRepository.GetPropertyApplicationsForUser(propertyId, loggedInUserId);
                if (existingApplication == null)
                    return StandardResponse<ApplicationStatusView>.Ok(new ApplicationStatusView() { HasApplied = false, HasPaid = false, ApplicationStatus = null });

                var existingTransaction = _paymentRepository.GetUserTransactionByUserAndPropertyId(propertyId, loggedInUserId);

                if (existingTransaction == null || existingTransaction.PaymentLog == null)
                    return StandardResponse<ApplicationStatusView>.Ok(new ApplicationStatusView() { HasApplied = true, HasPaid = false, ApplicationStatus = existingApplication.Status.Name });

                if (existingTransaction.PaymentLog.Status != "success")
                    return StandardResponse<ApplicationStatusView>.Ok(new ApplicationStatusView() { HasApplied = true, HasPaid = false, ApplicationStatus = existingTransaction.Status.Name });

                return StandardResponse<ApplicationStatusView>.Ok(new ApplicationStatusView() { HasApplied = true, HasPaid = true, ApplicationStatus = existingTransaction.Status.Name });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ApplicationStatusView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
    }
}


