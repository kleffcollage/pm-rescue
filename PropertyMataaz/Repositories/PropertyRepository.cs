using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(PMContext context) : base(context)
        {
        }

        public (bool Succeeded, string ErrorMessage, Property Property) CreateProperty(Property property)
        {
            try
            {
                var MediaFIle = property.MediaFiles;
                property.MediaFiles = null;

                var CreatedProperty = CreateAndReturn(property);


                return (true, null, CreatedProperty);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return (false, null, null);
            }

        }

        public async Task<IEnumerable<Property>> ListProperties()
        {
            try
            {
                var AllProperties = _context.Properties
                        .Include(p => p.PropertyType)
                        .Include(p => p.CreatedByUser)
                        .Include(p => p.MediaFiles)
                        .Include(p => p.Status)
                        .Include(p => p.UserEnquiries);

                return AllProperties;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public Property GetDetailsById(int Id)
        {
            try
            {
                return _context.Properties.Include(p => p.PropertyType)
                        .Include(p => p.CreatedByUser)
                        .Include(p => p.MediaFiles)
                        .Include(p => p.Status)
                        .Include(p => p.UserEnquiries)
                        .Where(p => p.Id == Id).First();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<PropertyType> GetTypes()
        {
            try
            {
                var PropertyTypes = _context.PropertyTypes;

                return PropertyTypes;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<Property> ListUserAddedProperties(int Id)
        {
            try
            {
                var Properties = _context.Properties
                                .Include(p => p.PropertyType)
                                .Include(p => p.CreatedByUser)
                                .Include(p => p.MediaFiles)
                                .Include(p => p.UserEnquiries)
                                .Where(p => p.CreatedByUserId == Id)
                                .Where(p => p.IsActive);
                return Properties;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<Property> ListUserDrafts(int Id)
        {
            try
            {
                var Properties = ListUserAddedProperties(Id).Where(p => p.IsDraft);

                return Properties;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public bool DeActivate(int id)
        {
            try
            {
                var thisProperty = GetById(id);
                thisProperty.IsActive = false;
                Update(thisProperty);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public (bool Succeeded, string ErrorMessage, InspectionDate CreatedDate) CreateInspectionDate(InspectionDate newDate)
        {
            try
            {
                _context.Add<InspectionDate>(newDate);
                _context.SaveChanges();
                return (true, null, newDate);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return (false, e.Message, null);
            }
        }

        public (bool Succeeded, string ErrorMessage) DeleteInspectionDate(InspectionDate thisDate)
        {
            try
            {
                _context.Remove<InspectionDate>(thisDate);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return (false, e.Message);
            }
        }

        public (bool Succeeded, string ErrorMessage, InspectionTime CreatedTime) CreateInspectionTime(InspectionTime newTime)
        {
            try
            {
                _context.Add<InspectionTime>(newTime);
                _context.SaveChanges();
                return (true, null, newTime);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return (false, e.Message, null);
            }
        }

        public IEnumerable<RentCollectionType> ListRentCollectionType()
        {
            try
            {
                return _context.RentCollectionTypes.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<TenantType> ListTenantTypes()
        {
            try
            {
                return _context.TenantTypes.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<InspectionDate> GetInspectionDate(int PropertyId)
        {
            try
            {
                var inspectionDates = _context.InspectionDates.Where(i => i.PropertyId == PropertyId)
                .Include(i => i.Times);
                return inspectionDates;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public bool DeleteInspectionDate(int Id)
        {
            try
            {
                var thisDate = ListInspectionDates().FirstOrDefault(i => i.Id == Id);
                _context.Remove<InspectionDate>(thisDate);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        public IEnumerable<InspectionDate> ListInspectionDates()
        {
            try
            {
                var inspectionDates = _context.InspectionDates
                .Include(i => i.Times);
                return inspectionDates;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<InspectionTime> GetInspectionTimes(int dateId)
        {
            try
            {
                var inspectionTimes = _context.InspectionTimes.Where(i => i.InspectionDateId == dateId);
                return inspectionTimes;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Inspections CreateInspection(Inspections inspection)
        {
            try
            {
                _context.Add<Inspections>(inspection);
                _context.SaveChanges();
                return inspection;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Inspections ScheduledInspection(int PropertyId, int UserId)
        {
            try
            {
                var inspection = _context.Inspections.Where(p => p.PropertyId == PropertyId && p.UserId == UserId)
                .Include(p => p.InspectionDate)
                .Include(p => p.InspectionTime).FirstOrDefault();
                return inspection;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public bool UpdateInspectionTime(int timeId)
        {
            try
            {
                var time = _context.InspectionTimes.Where(i => i.Id == timeId).FirstOrDefault();
                time.IsAvailable = false;
                _context.Update<InspectionTime>(time);
                _context.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        public IEnumerable<PropertyTitle> ListTitleTypes()
        {
            try
            {
                var PropertyTitles = _context.PropertyTitles;

                return PropertyTitles;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }
        // public (bool Succeeded, string ErrorMessage) CreateInspectionTime(InspectionTime newTime)
        // {
        //     try
        //     {
        //         var
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.Error(ex);
        //         return (false, ex.Message)
        //     }
        // }

    }
}