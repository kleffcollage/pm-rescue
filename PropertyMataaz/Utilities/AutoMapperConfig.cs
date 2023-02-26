using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;

namespace PropertyMataaz.Utilities
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Register, User>();

            CreateMap<LoginModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<User, UserView>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture.Url));

            CreateMap<User, LeanUserView>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<PropertyModel, Property>();

            CreateMap<Property, PropertyView>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.Enquiries, opt => opt.MapFrom(src => src.UserEnquiries.Count()));

            CreateMap<MediaModel, Media>();

            CreateMap<Media, MediaView>();

            CreateMap<UserEnquiry, UserEnquiryView>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Property.state))
            .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property.Name))
            .ForMember(dest => dest.Lga, opt => opt.MapFrom(src => src.Property.LGA))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Property.Area));


            CreateMap<Request, RequestView>();
            CreateMap<Report, ReportView>();

            CreateMap<PropertyRequestInput, PropertyRequest>();
            CreateMap<PropertyRequest, PropertyRequestView>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));
            CreateMap<PropertyRequestMatch, PropertyRequestMatchView>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<InspectionDateModel, InspectionDate>();
            CreateMap<InspectionTimeModel, InspectionTime>();
            CreateMap<InspectionDate, InspectionDateView>();
            CreateMap<InspectionTime, InspectionTimeView>()
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.AvailableTime));

            CreateMap<ApplicationModel, Application>();
            CreateMap<NextOfKinModel, NextOfKin>();
            CreateMap<Application, ApplicationView>()
            .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.ApplicationType.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<PaymentResponseData, PaymentLog>().ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.account_id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.amount))
            .ForMember(dest => dest.AmountSettled, opt => opt.MapFrom(src => src.amount_settled))
            .ForMember(dest => dest.AppFee, opt => opt.MapFrom(src => src.app_fee))
            .ForMember(dest => dest.AuthModel, opt => opt.MapFrom(src => src.auth_model))
            .ForMember(dest => dest.ChargedAmount, opt => opt.MapFrom(src => src.charged_amount))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.created_at))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.currency))
            .ForMember(dest => dest.DeviceFingerPrint, opt => opt.MapFrom(src => src.device_fingerprint))
            .ForMember(dest => dest.FlutterWavePaymentId, opt => opt.MapFrom(src => src.id))
            .ForMember(dest => dest.FlutterWaveReference, opt => opt.MapFrom(src => src.flw_ref))
            .ForMember(dest => dest.IP, opt => opt.MapFrom(src => src.ip))
            .ForMember(dest => dest.MerchantFee, opt => opt.MapFrom(src => src.merchant_fee))
            .ForMember(dest => dest.Narration, opt => opt.MapFrom(src => src.narration))
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.payment_type))
            .ForMember(dest => dest.ProcessorResponse, opt => opt.MapFrom(src => src.processor_response))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
            .ForMember(dest => dest.TransactionReference, opt => opt.MapFrom(src => src.tx_ref))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PaymentLog, PaymentView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<RentRelief, RentReliefView>();
            CreateMap<Installment, InstallmentView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));


            CreateMap<PaymentResponseCard, Card>().ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
            .ForMember(dest => dest.Expiry, opt => opt.MapFrom(src => src.expiry))
            .ForMember(dest => dest.First6Digits, opt => opt.MapFrom(src => src.first_6digits))
            .ForMember(dest => dest.Issuer, opt => opt.MapFrom(src => src.issuer))
            .ForMember(dest => dest.Last4Digits, opt => opt.MapFrom(src => src.last_4digits))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.token))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.type));

            CreateMap<InspectionModel, Inspections>();
            CreateMap<Inspections, InspectionView>();

            CreateMap<ComplaintsModel, Complaints>();
            CreateMap<Complaints, ComplaintsView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.ComplaintsSubCategory, opt => opt.MapFrom(src => src.ComplaintsSubCategory.Name))
            .ForMember(dest => dest.ComplaintsCategory, opt => opt.MapFrom(src => src.ComplaintsSubCategory.ComplantsCategory.Name));

            CreateMap<CleaningModel, Cleaning>();
            CreateMap<Cleaning, CleaningView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name));

            CreateMap<CleaningQuoteModel, CleaningQuote>();
            CreateMap<CleaningQuote, CleaningQuoteView>();

            CreateMap<LandSearchModel, LandSearch>();
            CreateMap<LandSearch, LandSearchView>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<Transaction, TransactionView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<Tenancy, TenancyView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<ReportModel, Report>();
        }
    }
}
