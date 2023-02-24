using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.Controllers;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
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
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IConfigurationProvider _mappingConfigurations;
        private readonly IMediaRepository _mediaRepository;
        private readonly ICodeProvider _codeProvider;
        private readonly IRequestRepository _requestRepository;
        private readonly IPropertyRequestRepository _propertyRequestRepository;
        private readonly UserManager<User> _userManager;
        private readonly IPropertyRequestService _propertyRequestService;
        public readonly IUserEnquiryRepository _userEnquiryRepository;
        public IApplicationRepository _applicationRepository;
        public IPaymentRepository _paymentRepository;

        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations, IMediaRepository mediaRepository, ICodeProvider codeProvider, IRequestRepository requestRepository, IPropertyRequestRepository propertyRequestRepository, Microsoft.AspNetCore.Identity.UserManager<User> userManager, IPropertyRequestService propertyRequestService, IUserEnquiryRepository userEnquiryRepository, IApplicationRepository applicationRepository, IPaymentRepository paymentRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
            _mediaRepository = mediaRepository;
            _codeProvider = codeProvider;
            _requestRepository = requestRepository;
            _propertyRequestRepository = propertyRequestRepository;
            _userManager = userManager;
            _propertyRequestService = propertyRequestService;
            _userEnquiryRepository = userEnquiryRepository;
            _applicationRepository = applicationRepository;
            _paymentRepository = paymentRepository;
        }

        public StandardResponse<PropertyView> CreateProperty(PropertyModel newProperty)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var NewProperty = _mapper.Map<Property>(newProperty);

                NewProperty.Verified = !NewProperty.SellMyself;

                NewProperty.StatusId = (int)Statuses.PENDING;
                NewProperty.IsActive = true;

                NewProperty.CreatedByUserId = UserId;
                NewProperty.RentCollectionTypeId = NewProperty.RentCollectionTypeId <= 0 ? 2 : NewProperty.RentCollectionTypeId;
                NewProperty.TenantTypeId = NewProperty.TenantTypeId <= 0 ? 2 : NewProperty.TenantTypeId;

                NewProperty = _propertyRepository.CreateProperty(NewProperty).Property;

                if (newProperty.IsRequest)
                {
                    var newRequest = new Request { PropertyId = NewProperty.Id, Comment = newProperty.Comment, Budget = newProperty.Budget, StatusId = (int)Statuses.PENDING };
                    _requestRepository.New(newRequest);
                }

                var ThisUser = _userManager.FindByIdAsync(UserId.ToString()).Result;
                if (newProperty.IsForRent || (newProperty.IsForSale && !newProperty.SellMyself))
                {
                    ThisUser.Bank = newProperty.Bank;
                    ThisUser.AccountNumber = newProperty.AccountNumber;
                    var result = _userManager.UpdateAsync(ThisUser).Result;
                }

                foreach (var media in newProperty.MediaFiles)
                {
                    media.PropertyId = NewProperty.Id;

                    media.Name = _codeProvider.New(0, Constants.NEW_PROPERTY_MEDIA_NAME, 0, 10, Constants.PROPERTY_MATAAZ_MEDIA_PREFIX).CodeString;

                    Media NewMedia = _mapper.Map<Media>(media);

                    var Result = _mediaRepository.UploadMedia(NewMedia).Result;

                    if (!Result.Succeeded)
                        return StandardResponse<PropertyView>.Ok().AddStatusMessage(StandardResponseMessages.MEDIA_UPLOAD_FAILED);
                }

                if (newProperty.PropertyRequestMatchId > 0)
                {
                    var result = _propertyRequestService.AddMatch(NewProperty.Id, newProperty.PropertyRequestMatchId);
                }

                var mapped = _mapper.Map<PropertyView>(NewProperty);

                return StandardResponse<PropertyView>.Ok().AddData(mapped).AddStatusMessage(StandardResponseMessages.PROPERTY_CREATION_SUCCESSFUL);

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PropertyView>.Failed().AddStatusMessage(StandardResponseMessages.PROPERTY_CREATION_FAILED);
            }

        }

        public StandardResponse<PropertyView> CreatePropertyAdmin(PropertyModel newProperty)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var NewProperty = _mapper.Map<Property>(newProperty);

                NewProperty.Verified = true;

                NewProperty.StatusId = (int)Statuses.VERIFIED;

                NewProperty.CreatedByUserId = UserId;

                NewProperty.RentCollectionTypeId = NewProperty.RentCollectionTypeId <= 0 ? 2 : NewProperty.RentCollectionTypeId;
                NewProperty.TenantTypeId = NewProperty.TenantTypeId <= 0 ? 2 : NewProperty.TenantTypeId;

                NewProperty = _propertyRepository.CreateProperty(NewProperty).Property;

                if (newProperty.IsRequest)
                {
                    var newRequest = new Request { PropertyId = NewProperty.Id, Comment = newProperty.Comment, Budget = newProperty.Budget, StatusId = (int)Statuses.PENDING };
                    _requestRepository.New(newRequest);
                }

                foreach (var media in newProperty.MediaFiles)
                {
                    media.PropertyId = NewProperty.Id;

                    media.Name = _codeProvider.New(0, Constants.NEW_PROPERTY_MEDIA_NAME, 0, 10, Constants.PROPERTY_MATAAZ_MEDIA_PREFIX).CodeString;

                    Media NewMedia = _mapper.Map<Media>(media);

                    var Result = _mediaRepository.UploadMedia(NewMedia).Result;

                    if (!Result.Succeeded)
                        return StandardResponse<PropertyView>.Ok().AddStatusMessage(StandardResponseMessages.MEDIA_UPLOAD_FAILED);
                }

                if (newProperty.PropertyRequestMatchId > 0)
                {
                    var match = new PropertyRequestMatch()
                    {
                        PropertyId = NewProperty.Id,
                        PropertyRequestId = newProperty.PropertyRequestMatchId
                    };
                    _propertyRequestRepository.AddMatch(match);
                }
                var mapped = _mapper.Map<PropertyView>(NewProperty);

                return StandardResponse<PropertyView>.Ok().AddData(mapped).AddStatusMessage(StandardResponseMessages.PROPERTY_CREATION_SUCCESSFUL);

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PropertyView>.Failed().AddStatusMessage(StandardResponseMessages.PROPERTY_CREATION_FAILED);
            }

        }

        public StandardResponse<PropertyView> GetPropertyById(int Id)
        {
            var thisProperty = _mapper.Map<PropertyView>(_propertyRepository.GetDetailsById(Id));

            return StandardResponse<PropertyView>.Ok().AddData(thisProperty).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }

        public StandardResponse<PagedCollection<PropertyView>> ListAllProperties(PagingOptions pagingOptions, string search, string filter)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().OrderByDescending(a => a.Id)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(filter))
                {
                    AllProperties = AllProperties.Where(a => a.Status.ToLower().Contains(filter.ToLower()));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllProperties)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListAllRentProperties(PagingOptions pagingOptions, string search, string filter)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().Where(p => p.IsForRent).OrderByDescending(a => a.Id)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(filter))
                {
                    AllProperties = AllProperties.Where(a => a.Status.ToLower().Contains(filter.ToLower()));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllProperties)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListAllSaleProperties(PagingOptions pagingOptions, string search, string filter)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().Where(p => p.IsForSale).OrderByDescending(a => a.Id)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(filter))
                {
                    AllProperties = AllProperties.Where(a => a.Status.ToLower().Contains(filter.ToLower()));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllProperties)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListAllPropertiesForRentReview(PagingOptions pagingOptions, string search)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().OrderByDescending(a => a.Id).Where(p => p.IsForRent && p.StatusId == (int)Statuses.PENDING)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllProperties)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListAllPropertiesRentApproved(PagingOptions pagingOptions, string search, string filter)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().OrderByDescending(a => a.Id).Where(p => p.IsForRent && p.Verified && p.StatusId != (int)Statuses.PENDING && p.StatusId != (int)Statuses.REJECTED)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(filter))
                {
                    AllProperties = AllProperties.Where(a => a.Status.ToLower().Contains(filter.ToLower()));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(AdminController.ListPropertiesRentApproved)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyView> Delete(int Id)
        {
            var ThisProperty = _propertyRepository.GetById(Id);
            _propertyRepository.Delete(ThisProperty);
            return StandardResponse<PropertyView>.Ok().AddStatusMessage(StandardResponseMessages.DEELETED);
        }

        public StandardResponse<IEnumerable<PropertyType>> GetAllTypes()
        {
            return StandardResponse<IEnumerable<PropertyType>>.Ok().AddData(_propertyRepository.GetTypes()).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }

        public StandardResponse<PagedCollection<PropertyView>> ListUsersAddedProperties(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var AllProperties = _propertyRepository.ListProperties()
                                   .Result.AsQueryable().OrderByDescending(a => a.Id)
                                   .ProjectTo<PropertyView>(_mappingConfigurations)
                                   .AsEnumerable();

                var response = AllProperties.Where(p => p.CreatedByUser.Id == UserId && p.IsActive);
                var PagedProperties = response.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListMyProperties)), PagedProperties.ToArray(), response.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListUsersAddedPropertiesForSale(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var AllProperties = _propertyRepository.ListProperties()
                                   .Result.AsQueryable().OrderByDescending(a => a.Id)
                                   .ProjectTo<PropertyView>(_mappingConfigurations)
                                   .AsEnumerable();

                var response = AllProperties.Where(p => p.CreatedByUser.Id == UserId && p.IsForSale && p.IsActive);
                var PagedProperties = response.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListMyPropertiesForSale)), PagedProperties.ToArray(), response.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListUsersAddedPropertiesForRent(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var AllProperties = _propertyRepository.ListProperties()
                                   .Result.AsQueryable().OrderByDescending(a => a.Id)
                                   .ProjectTo<PropertyView>(_mappingConfigurations)
                                   .AsEnumerable();

                var response = AllProperties.Where(p => p.CreatedByUser.Id == UserId && p.IsForRent && p.IsActive);
                var PagedProperties = response.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListMyPropertiesForRent)), PagedProperties.ToArray(), response.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> ListUserDrafts(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var AllProperties = _propertyRepository.ListProperties()
                                   .Result.AsQueryable().OrderByDescending(a => a.Id)
                                   .ProjectTo<PropertyView>(_mappingConfigurations)
                                   .AsEnumerable();

                var response = AllProperties.Where(p => p.CreatedByUser.Id == UserId && p.IsDraft && p.IsActive);
                var PagedProperties = response.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllProperties)), PagedProperties.ToArray(), response.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> PropertyForSales(PagingOptions pagingOptions, string search, PropertyFilterOptions filterOptions)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().OrderByDescending(a => a.Id).Where(p => p.Status.Id == (int)Statuses.VERIFIED && p.IsForSale)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                AllProperties = FilterProperties(AllProperties, filterOptions);

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllPropertiesForSale)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<PropertyView>> PropertyForRent(PagingOptions pagingOptions, string search, PropertyFilterOptions filterOptions)
        {
            try
            {
                var AllProperties = _propertyRepository.ListProperties()
                                    .Result.AsQueryable().OrderByDescending(a => a.Id).Where(p => p.Status.Id == (int)Statuses.VERIFIED && p.IsForRent)
                                    .ProjectTo<PropertyView>(_mappingConfigurations)
                                    .AsEnumerable();

                if (!string.IsNullOrEmpty(search))
                {
                    AllProperties = AllProperties.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()) || p.State.ToLower().Contains(search.ToLower()));
                }

                AllProperties = FilterProperties(AllProperties, filterOptions);

                var PagedProperties = AllProperties.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<PropertyView>.Create(Link.ToCollection(nameof(PropertyController.ListAllPropertiesForRent)), PagedProperties.ToArray(), AllProperties.Count(), pagingOptions);

                return StandardResponse<PagedCollection<PropertyView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<PagedCollection<PropertyView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
        public StandardResponse<bool> Deactivate(int id)
        {
            var result = _propertyRepository.DeActivate(id);
            if (result)
            {
                return StandardResponse<bool>.Ok(result);
            }
            return StandardResponse<bool>.Error(StandardResponseMessages.ERROR_OCCURRED);
        }

        public StandardResponse<PropertyView> IncrementViews(int PropertyId)
        {
            try
            {
                var property = _propertyRepository.GetById(PropertyId);
                property.Views += 1;
                var response = _propertyRepository.Update(property);
                return StandardResponse<PropertyView>.Ok(_mapper.Map<PropertyView>(response));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyView>.Failed();
            }
        }

        public StandardResponse<PropertyView> IncrementEnquiries(int PropertyId)
        {
            try
            {
                var property = _propertyRepository.GetById(PropertyId);
                property.Enquiries += 1;
                var response = _propertyRepository.Update(property);
                return StandardResponse<PropertyView>.Ok(_mapper.Map<PropertyView>(response));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyView>.Failed();
            }
        }

        public StandardResponse<InspectionDateView> CreateDate(InspectionDateModel newDate)
        {
            try
            {
                var mappedDate = _mapper.Map<InspectionDate>(newDate);
                mappedDate.Date = mappedDate.Date;
                foreach (var time in mappedDate.Times)
                {
                    time.IsAvailable = true;
                }
                var result = _propertyRepository.CreateInspectionDate(mappedDate);
                return StandardResponse<InspectionDateView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<InspectionDateView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<bool> DeleteDate(int dateId)
        {
            try
            {
                var result = _propertyRepository.DeleteInspectionDate(dateId);
                return StandardResponse<bool>.Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<bool>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<InspectionTimeView> CreateTime(InspectionTimeModel newTime)
        {
            try
            {
                var mappedDate = _mapper.Map<InspectionTime>(newTime);
                mappedDate.AvailableTime = mappedDate.AvailableTime.ToLocalTime();
                // var inspectionDate = _propertyRepository.ListInspectionDates().Where(date => date.Id == newTime.InspectionDateId).FirstOrDefault();
                mappedDate.IsAvailable = true;
                var result = _propertyRepository.CreateInspectionTime(mappedDate);
                return StandardResponse<InspectionTimeView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<InspectionTimeView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<IEnumerable<InspectionDateView>> GetInspectionDates(int PropertyId)
        {
            try
            {
                var inspectionDates = _propertyRepository.GetInspectionDate(PropertyId)
                                                         .AsQueryable()
                                                         .ProjectTo<InspectionDateView>(_mappingConfigurations)
                                                         .ToList();

                return StandardResponse<IEnumerable<InspectionDateView>>.Ok().AddData(inspectionDates);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<InspectionDateView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyView> UpdateProperty(PropertyModel model)
        {
            try
            {
                var ExistingProperty = _propertyRepository.GetDetailsById(model.Id);
                var MappedProperty = _mapper.Map<Property>(model);

                ExistingProperty.Address = model.Address;
                ExistingProperty.Area = model.Area;
                ExistingProperty.DateModified = DateTime.Now;
                ExistingProperty.Description = model.Description;
                ExistingProperty.IsActive = model.IsActive;
                ExistingProperty.IsDraft = model.IsDraft;
                ExistingProperty.LGA = model.LGA;
                ExistingProperty.NumberOfBathrooms = model.NumberOfBathrooms;
                ExistingProperty.NumberOfBedrooms = model.NumberOfBedrooms;
                ExistingProperty.Price = model.Price;
                ExistingProperty.state = model.State;
                ExistingProperty.Title = model.Title;
                ExistingProperty.Name = model.Name;
                ExistingProperty.StatusId = (int)Statuses.PENDING;
                ExistingProperty.DocumentUrl = !string.IsNullOrEmpty(model.DocumentUrl) ? model.DocumentUrl : ExistingProperty.DocumentUrl;


                var UpdateResult = _propertyRepository.Update(ExistingProperty);
                if (UpdateResult == null)
                    return StandardResponse<PropertyView>.Error(StandardResponseMessages.ERROR_OCCURRED);


                var MappedResponse = _mapper.Map<PropertyView>(UpdateResult);
                return StandardResponse<PropertyView>.Ok().AddData(MappedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PropertyView> Approve(int PropertyId)
        {
            try
            {
                var ExistingProperty = _propertyRepository.GetDetailsById(PropertyId);
                ExistingProperty.StatusId = (int)Statuses.VERIFIED;

                var UpdateResult = _propertyRepository.Update(ExistingProperty);
                if (UpdateResult == null)
                    return StandardResponse<PropertyView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<PropertyView>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }


        public StandardResponse<PropertyView> Reject(int PropertyId, string reason)
        {
            try
            {
                var ExistingProperty = _propertyRepository.GetDetailsById(PropertyId);
                ExistingProperty.StatusId = (int)Statuses.REJECTED;
                ExistingProperty.RejectionReason = reason;

                var UpdateResult = _propertyRepository.Update(ExistingProperty);
                if (UpdateResult == null)
                    return StandardResponse<PropertyView>.Error(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<PropertyView>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PropertyView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<IEnumerable<RentCollectionType>> ListRentCollectionType()
        {
            try
            {
                return StandardResponse<IEnumerable<RentCollectionType>>.Ok(_propertyRepository.ListRentCollectionType());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<RentCollectionType>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<IEnumerable<TenantType>> ListTenantTypes()
        {
            try
            {
                return StandardResponse<IEnumerable<TenantType>>.Ok(_propertyRepository.ListTenantTypes());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<TenantType>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<bool> ScheduleInspection(InspectionModel model)
        {
            try
            {
                var availableInspection = _propertyRepository.GetInspectionTimes(model.InspectionDateId).Where(i => i.Id == model.InspectionTimeId && i.IsAvailable).FirstOrDefault();

                if (availableInspection == null)
                    return StandardResponse<bool>.Error(StandardResponseMessages.INSPECTION_TIME_UNAVAILABE);

                var createdInspection = _propertyRepository.CreateInspection(_mapper.Map<Inspections>(model));
                _propertyRepository.UpdateInspectionTime(model.InspectionTimeId);
                return StandardResponse<bool>.Ok(true);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<bool>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<InspectionView> GetUsersInspectionForProperty(int propertyId)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();

                var mappedInspection = _mapper.Map<InspectionView>(_propertyRepository.ScheduledInspection(propertyId, UserId));

                return StandardResponse<InspectionView>.Ok(mappedInspection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<InspectionView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        private static IEnumerable<PropertyView> FilterProperties(IEnumerable<PropertyView> properties, PropertyFilterOptions filterOptions)
        {
            var propertiesToFilter = properties;
            if (filterOptions == null)
                return properties;

            if ((bool)filterOptions.Commercial)
                properties = propertiesToFilter;

            if ((bool)filterOptions.Residential)
                properties = propertiesToFilter;

            if ((bool)filterOptions.Mixed)
                properties = propertiesToFilter;

            if ((bool)filterOptions.Bungalow)
                properties = propertiesToFilter.Where(p => p.PropertyTypeId == (int)PropertyTypes.BUNGALOW);

            if ((bool)filterOptions.Flat)
                properties = propertiesToFilter.Where(p => p.PropertyTypeId == (int)PropertyTypes.FLAT);

            if ((bool)filterOptions.Duplex)
                properties = propertiesToFilter.Where(p => p.PropertyTypeId == (int)PropertyTypes.DUPLEX);

            if ((bool)filterOptions.Terrace)
                properties = propertiesToFilter.Where(p => p.PropertyTypeId == (int)PropertyTypes.TERRACE);

            if ((int)filterOptions.Bathrooms > 0)
                properties = propertiesToFilter.Where(p => p.NumberOfBathrooms == (int)filterOptions.Bathrooms);

            if ((int)filterOptions.Bedrooms > 0)
                properties = propertiesToFilter.Where(p => p.NumberOfBedrooms == (int)filterOptions.Bedrooms);

            return properties;
        }

        public StandardResponse<IEnumerable<PropertyTitle>> GetTitleTypes()
        {
            return StandardResponse<IEnumerable<PropertyTitle>>.Ok().AddData(_propertyRepository.ListTitleTypes()).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);

        }

        public StandardResponse<bool> CancelEnquiry(int propertyId)
        {
            try
            {
                var UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var thisEnquiry = _userEnquiryRepository.ListUserActiveEnquiries(UserId).LastOrDefault(e => e.PropertyId == propertyId);
                thisEnquiry.Active = false;
                _userEnquiryRepository.Update(thisEnquiry);

                var thisApplication = _applicationRepository.List().LastOrDefault(e => e.PropertyId == propertyId && e.UserId == UserId);

                thisApplication.StatusId = (int)Statuses.INACTIVE;
                _applicationRepository.Update(thisApplication);

                return StandardResponse<bool>.Ok(true);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<bool>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<ReceiptView> GetPaymentReceipt(int propertyId)
        {
            try
            {
                var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var thisUser = _userManager.FindByIdAsync(loggedInUserId.ToString()).Result;
                var property = _propertyRepository.GetById(propertyId);
                var transaction = _paymentRepository.QueryTransactions().Include(x => x.PaymentLog).ThenInclude(x => x.Card).Where(t => t.UserId == loggedInUserId && t.PropertyId == propertyId && t.StatusId == (int)Statuses.COMPLETED).FirstOrDefault();

                if (transaction == null)
                    return StandardResponse<ReceiptView>.Error("Mo transaction found");

                var receipt = new ReceiptView
                {
                    Property = _mapper.Map<PropertyView>(property),
                    FullName = $"{thisUser.FirstName} {thisUser.LastName}",
                    Email = thisUser.Email,
                    PhoneNumber = thisUser.PhoneNumber,
                    PaymentDate = transaction.DateCreated,
                    Amount = transaction.Amount,
                    PaymentLog = transaction.PaymentLog
                };

                return StandardResponse<ReceiptView>.Ok(receipt);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ReceiptView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
   
        
    }
}