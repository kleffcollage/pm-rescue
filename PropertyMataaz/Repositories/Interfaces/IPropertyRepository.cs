using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.UtilityModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IPropertyRepository
    {
        (bool Succeeded,string ErrorMessage, Property Property) CreateProperty(Property property);
        Task<IEnumerable<Property>> ListProperties();
        Property GetById(int Id);
        Property GetDetailsById(int Id);  
        IEnumerable<PropertyType> GetTypes();
        IEnumerable<PropertyTitle> ListTitleTypes();
        IEnumerable<Property> ListUserAddedProperties(int Id);
         IEnumerable<Property> ListUserDrafts(int Id);
        void Delete(Property property);
        bool DeActivate(int id);
        Property Update(Property property);
        (bool Succeeded, string ErrorMessage, InspectionDate CreatedDate) CreateInspectionDate(InspectionDate newDate);
        (bool Succeeded, string ErrorMessage, InspectionTime CreatedTime) CreateInspectionTime(InspectionTime newTime);
        (bool Succeeded, string ErrorMessage) DeleteInspectionDate(InspectionDate thisDate);
        public IEnumerable<InspectionDate> GetInspectionDate(int PropertyId);
        IEnumerable<InspectionDate> ListInspectionDates();
        bool DeleteInspectionDate(int Id);
        IEnumerable<RentCollectionType> ListRentCollectionType();
        IEnumerable<TenantType> ListTenantTypes();
        Inspections CreateInspection(Inspections inspection);
        IEnumerable<InspectionTime> GetInspectionTimes(int dateId);
        Inspections ScheduledInspection(int PropertyId, int UserId);
        bool UpdateInspectionTime(int timeId);
    }
}