
using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IPropertyService
    {
         StandardResponse<PropertyView> CreateProperty(PropertyModel newProperty);
         StandardResponse<PropertyView> CreatePropertyAdmin(PropertyModel newProperty);
         StandardResponse<PagedCollection<PropertyView>> ListAllProperties(PagingOptions pagingOptions, string search,string filter);
         StandardResponse<PagedCollection<PropertyView>> ListAllSaleProperties(PagingOptions pagingOptions, string search, string filter);
         StandardResponse<PagedCollection<PropertyView>> ListAllRentProperties(PagingOptions pagingOptions, string search, string filter);
        StandardResponse<PropertyView> GetPropertyById(int Id);
        StandardResponse<IEnumerable<PropertyType>> GetAllTypes();
        StandardResponse<IEnumerable<PropertyTitle>> GetTitleTypes();
        StandardResponse<PagedCollection<PropertyView>> ListUsersAddedProperties(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<PropertyView>> ListUserDrafts(PagingOptions pagingOptions);
        StandardResponse<PropertyView> Delete(int Id);
        StandardResponse<bool> Deactivate(int id);
        StandardResponse<PropertyView> IncrementEnquiries(int PropertyId);
        StandardResponse<PropertyView> IncrementViews(int PropertyId);
        StandardResponse<PagedCollection<PropertyView>> PropertyForSales(PagingOptions pagingOptions,string search,PropertyFilterOptions filterOptions);
        StandardResponse<PagedCollection<PropertyView>> PropertyForRent(PagingOptions pagingOptions, string search,PropertyFilterOptions filterOptions);
        StandardResponse<PagedCollection<PropertyView>> ListUsersAddedPropertiesForRent(PagingOptions pagingOptions);
        StandardResponse<PagedCollection<PropertyView>> ListUsersAddedPropertiesForSale(PagingOptions pagingOptions);
        StandardResponse<InspectionDateView> CreateDate(InspectionDateModel newDate);
        StandardResponse<IEnumerable<InspectionDateView>> GetInspectionDates(int PropertyId);
        StandardResponse<PropertyView> UpdateProperty(PropertyModel model);
        StandardResponse<PropertyView> Approve(int PropertyId);
        StandardResponse<PropertyView> Reject(int PropertyId, string reason);
        StandardResponse<IEnumerable<TenantType>> ListTenantTypes();
        StandardResponse<IEnumerable<RentCollectionType>> ListRentCollectionType();
        StandardResponse<bool> ScheduleInspection(InspectionModel model);
        StandardResponse<InspectionView> GetUsersInspectionForProperty(int propertyId);
        StandardResponse<PagedCollection<PropertyView>> ListAllPropertiesRentApproved(PagingOptions pagingOptions, string search,string filter);
        StandardResponse<PagedCollection<PropertyView>> ListAllPropertiesForRentReview(PagingOptions pagingOptions, string search);
        StandardResponse<InspectionTimeView> CreateTime(InspectionTimeModel newTime);
        StandardResponse<bool> CancelEnquiry(int propertyId);
        StandardResponse<bool> DeleteDate(int dateId);
        StandardResponse<ReceiptView> GetPaymentReceipt(int propertyId);
    }
}