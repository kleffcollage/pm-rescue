using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
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

namespace PropertyMataaz.Services
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        private readonly IConfigurationProvider _mappingConfigurations;
        private readonly IEmailHandler _emailHandler;

        public ReportService(IMapper mapper, IReportRepository reportRepository, IConfigurationProvider mappingConfigurations, IEmailHandler emailHandler)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _mappingConfigurations = mappingConfigurations;
            _emailHandler = emailHandler;
        }

        public StandardResponse<ReportView> CreateReport(ReportModel model)
        {
            try
            {
                var mappedReport = _mapper.Map<Report>(model);
                if(mappedReport.UserId == 0){ 
                    mappedReport.UserId = 1;
                }
                var response = _reportRepository.CreateAndReturn(mappedReport);
                if (response != null)
                    return StandardResponse<ReportView>.Ok().AddData(_mapper.Map<ReportView>(response));

                return StandardResponse<ReportView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ReportView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<ReportView> ContactUs(ReportModel model)
        {
            try
            {
                // send an email to admin with the information  

                List<KeyValuePair<string, string>> EmailParameters = new List<KeyValuePair<string, string>>();
                EmailParameters.Add(new KeyValuePair<string, string>("USERNAME", model.UserName));
                EmailParameters.Add(new KeyValuePair<string, string>("EMAIL", model.Email));
                EmailParameters.Add(new KeyValuePair<string, string>("MESSAGE", model.Description));


                var EmailTemplate = _emailHandler.ComposeFromTemplate("contact-us.html", EmailParameters);
                var SendEmail = _emailHandler.SendEmail("braingrams40@gmail.com","New Message On Property Mataaz", EmailTemplate);

                return StandardResponse<ReportView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ReportView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<ReportView>> GetReports(PagingOptions pagingOptions)
        {
            try
            {
                var all = _reportRepository.Query().ToList();
                var reports  = _reportRepository.Query().Include(x => x.Property).OrderByDescending(a => a.Id).ProjectTo<ReportView>(_mappingConfigurations).AsEnumerable();
                var PagedResponse = reports.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);
                var response = PagedCollection<ReportView>.Create(Link.ToCollection(nameof(ReportController.GetReports)), PagedResponse.ToArray(), reports.Count(), pagingOptions);
                return StandardResponse<PagedCollection<ReportView>>.Ok().AddData(response);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<ReportView>>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<ReportView> GetReportById(int id)
        {
            try
            {
                var response = _reportRepository.GetById(id);
                if (response != null)
                    return StandardResponse<ReportView>.Ok().AddData(_mapper.Map<ReportView>(response));

                return StandardResponse<ReportView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<ReportView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
    }
}