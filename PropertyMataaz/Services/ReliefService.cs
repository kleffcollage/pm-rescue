using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Utilities;
using PropertyMataaz.Utilities.Extentions;

namespace PropertyMataaz.Services
{
    public class ReliefService : IReliefService
    {
        private readonly IReliefRepository _reliefRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationProvider _mappingConfigurations;
        private readonly IPropertyRepository _propertyRepository;

        public ReliefService(IReliefRepository reliefRepository, IHttpContextAccessor httpContextAccessor, IConfigurationProvider mappingConfigurations, IPropertyRepository propertyRepository)
        {
            _reliefRepository = reliefRepository;
            _httpContextAccessor = httpContextAccessor;
            _mappingConfigurations = mappingConfigurations;
            _propertyRepository = propertyRepository;
        }

        public StandardResponse<IEnumerable<RentReliefView>> ListMyReliefs()
        {
            int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
            try
            {
                var reliefs = _reliefRepository.ListReliefs().Where(r => r.UserId == UserId).AsQueryable().ProjectTo<RentReliefView>(_mappingConfigurations).AsEnumerable().ToList();
                foreach (var relief in reliefs)
                {
                    var checker = relief.Installments.ToList();
                    // var installments = _reliefRepository.ListInstallments().Where(i => i.RentReliefId == relief.Id).ToList();
                    var thisProperty = _propertyRepository.GetDetailsById(relief.PropertyId);
                    relief.Status = relief.Installments.LastOrDefault().Status;
                    relief.Interest = 15;
                    relief.MonthlyInstallment = relief.Installments.LastOrDefault().Amount;
                    relief.ReliefAmount = thisProperty.Price;
                    relief.TotalRepayment = relief.Installments.Sum( i => i.Amount);
                }

                return StandardResponse<IEnumerable<RentReliefView>>.Ok(reliefs);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<IEnumerable<RentReliefView>>.Failed();
            }
        }
    }
}