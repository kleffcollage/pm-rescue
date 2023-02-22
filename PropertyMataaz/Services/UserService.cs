using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyMataaz.ContentServer;
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
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailHandler _emailHandler;
        private readonly ICodeProvider _codeProvider;
        private readonly RoleManager<Role> _roleManager;
        private readonly Globals _globals;
        private readonly IUtilityMethods _utilityMethods;
        public IConfigurationProvider _mappingConfigurations;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserEnquiryRepository _userEnquiryRepository;
        private readonly IMediaRepository _mediaRepository;
        public UserService(IMapper mapper, IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, IEmailHandler emailHandler, ICodeProvider codeProvider, IOptions<Globals> globals, IUtilityMethods utilityMethods, IConfigurationProvider mappingConfigurations, IHttpContextAccessor httpContextAccessor, IUserEnquiryRepository userEnquiryRepository, RoleManager<Role> roleManager, IMediaRepository mediaRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailHandler = emailHandler;
            _codeProvider = codeProvider;
            _utilityMethods = utilityMethods;
            _globals = globals.Value;
            _mapper = mapper;
            _mappingConfigurations = mappingConfigurations;
            _httpContextAccessor = httpContextAccessor;
            _userEnquiryRepository = userEnquiryRepository;
            _roleManager = roleManager;
            _mediaRepository = mediaRepository;
        }
        public StandardResponse<UserView> CreateUser(Register newUser)
        {
            try
            {
                var ExistingUser = _userManager.FindByEmailAsync(newUser.Email).Result;

                if (ExistingUser != null)
                    return StandardResponse<UserView>.Error(StandardResponseMessages.USER_ALREADY_EXISTS);

                var User = _mapper.Map<Register, User>(newUser);

                var Result = _userRepository.CreateUser(User).Result;
                if (!Result.Succeeded)
                    return StandardResponse<UserView>.Error(Result.ErrorMessage);

                var EmailConfirmationToken = _codeProvider.New(Result.CreatedUser.Id, Constants.NEW_EMAIL_VERIFICATION_CODE).CodeString;
                var ConfirmationLink = $"{_globals.FrontEndBaseUrl}{_globals.EmailVerificationUrl}{EmailConfirmationToken}";

                var mapped = _mapper.Map<UserView>(Result.CreatedUser);

                List<KeyValuePair<string, string>> EmailParameters = new List<KeyValuePair<string, string>>();
                EmailParameters.Add(new KeyValuePair<string, string>(Constants.EMAIL_STRING_REPLACEMENT_FULLNAME, mapped.FullName));
                EmailParameters.Add(new KeyValuePair<string, string>(Constants.EMAIL_STRING_REPLACEMENTS_CODE, EmailConfirmationToken.ToUpper()));

                var EmailTemplate = _emailHandler.ComposeFromTemplate(Constants.NEW_USER_WELCOME_EMAIL_FILENAME, EmailParameters);
                var SendEmail = _emailHandler.SendEmail(Result.CreatedUser.Email, Constants.NEW_USER_WELCOME_EMAIL_SUBJECT, EmailTemplate);


                return StandardResponse<UserView>.Ok(mapped);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<UserView> RequestVerificationMail(string email)
        {
            try
            {
                var ExistingUser = _userManager.FindByEmailAsync(email).Result;

                if (ExistingUser == null)
                    return StandardResponse<UserView>.Error(StandardResponseMessages.USER_NOT_FOUND);

                var EmailConfirmationToken = _codeProvider.New(ExistingUser.Id, Constants.NEW_EMAIL_VERIFICATION_CODE).CodeString;
                var ConfirmationLink = $"{_globals.FrontEndBaseUrl}{_globals.EmailVerificationUrl}{EmailConfirmationToken}";

                var mapped = _mapper.Map<UserView>(ExistingUser);

                List<KeyValuePair<string, string>> EmailParameters = new List<KeyValuePair<string, string>>();
                EmailParameters.Add(new KeyValuePair<string, string>(Constants.EMAIL_STRING_REPLACEMENT_FULLNAME, mapped.FullName));
                EmailParameters.Add(new KeyValuePair<string, string>(Constants.EMAIL_STRING_REPLACEMENTS_CODE, EmailConfirmationToken.ToUpper()));

                var EmailTemplate = _emailHandler.ComposeFromTemplate(Constants.NEW_USER_WELCOME_EMAIL_FILENAME, EmailParameters);
                var SendEmail = _emailHandler.SendEmail(ExistingUser.Email, Constants.NEW_USER_WELCOME_EMAIL_SUBJECT, EmailTemplate);
                return StandardResponse<UserView>.Ok(mapped);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<UserView> CreateAdminUser(Register newUser)
        {
            try
            {
                var ExistingUser = _userManager.FindByEmailAsync(newUser.Email).Result;

                if (ExistingUser != null)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_ALREADY_EXISTS);

                var roleExists = _roleManager.RoleExistsAsync("ADMIN").Result;

                if (!roleExists)
                {
                    var role = new Role()
                    {
                        Name = "ADMIN"
                    };
                    var roleCreated = _roleManager.CreateAsync(role);
                }
                var User = _mapper.Map<Register, User>(newUser);
                User.IsAdmin = true;
                User.UserName = newUser.Email;
                User.Password = $"{newUser.FirstName}1234";

                var Result = _userRepository.CreateUser(User).Result;
                if (!Result.Succeeded)
                    return StandardResponse<UserView>.Error(Result.ErrorMessage);

                var result = _userManager.AddToRoleAsync(Result.CreatedUser, "ADMIN").Result.Succeeded;

                var mapped = _mapper.Map<UserView>(Result.CreatedUser);

                return StandardResponse<UserView>.Ok(mapped);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }

        }

        public StandardResponse<UserView> AuthenticateAdmin(LoginModel userToLogin)
        {
            try
            {
                var User = _userManager.FindByEmailAsync(userToLogin.Email).Result;
                if (User == null)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_NOT_FOUND);

                var isInRole = _userManager.IsInRoleAsync(User, "ADMIN").Result;

                var roles = _userManager.GetRolesAsync(User).Result;

                User = _mapper.Map<LoginModel, User>(userToLogin);

                if (!isInRole)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_NOT_PERMITTED);


                var Result = _userRepository.Authenticate(User).Result;

                if (!Result.Succeeded)
                    return StandardResponse<UserView>.Failed().AddStatusMessage((Result.ErrorMessage ?? StandardResponseMessages.ERROR_OCCURRED));

                var mapped = _mapper.Map<UserView>(Result.LoggedInUser);

                return StandardResponse<UserView>.Ok(mapped);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<UserView>.Failed();
            }
        }

        public StandardResponse<UserView> Authenticate(LoginModel userToLogin)
        {
            var User = _userManager.FindByEmailAsync(userToLogin.Email).Result;
            if (User == null)
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_NOT_FOUND);

            User = _mapper.Map<LoginModel, User>(userToLogin);

            var Result = _userRepository.Authenticate(User).Result;

            if (!Result.Succeeded)
                return StandardResponse<UserView>.Failed().AddStatusMessage((Result.ErrorMessage ?? StandardResponseMessages.ERROR_OCCURRED));

            var mapped = _mapper.Map<UserView>(Result.LoggedInUser);

            return StandardResponse<UserView>.Ok(mapped);
        }

        public StandardResponse<PagedCollection<UserView>> ListUsers(PagingOptions pagingOptions, string search)
        {
            var Users = _userRepository.ListUsers().Result.Users.ProjectTo<UserView>(_mappingConfigurations).AsEnumerable();

            if (!String.IsNullOrEmpty(search))
            {
                Users = Users.Where(u => u.Email.ToLower().Contains(search.ToLower()) || u.FullName.ToLower().Contains(search.ToLower()));
            }

            var PagedUsers = Users.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

            var PagedResponse = PagedCollection<UserView>.Create(Link.ToCollection(nameof(UserController.ListUsers)), PagedUsers.ToArray(), Users.Count(), pagingOptions);


            return StandardResponse<PagedCollection<UserView>>.Ok(PagedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }

        public StandardResponse<UserView> GetUser(int userId)
        {
            var User = _userRepository.ListUsers().Result.Users.FirstOrDefault(x => x.Id == userId);
            if (User == null)
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_NOT_FOUND);

            var mapped = _mapper.Map<UserView>(User);

            return StandardResponse<UserView>.Ok(mapped);
        }

        public StandardResponse<UserView> DeleteUser(string email)
        {
            try
            {
                var User = _userManager.FindByEmailAsync(email).Result;
                var Result = _userRepository.DeleteUser(User).Result;
                if (!Result.Suceeded)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<UserView>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public StandardResponse<UserView> VerifyUser(string token, string email)
        {
            try
            {
                var UserToVerify = _userManager.FindByEmailAsync(email).Result;

                Code ThisCode = _codeProvider.GetByCodeString(token.ToLower());

                if (UserToVerify == null)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_NOT_FOUND);

                if (UserToVerify.EmailConfirmed)
                    return StandardResponse<UserView>.Ok().AddStatusMessage(StandardResponseMessages.ALREADY_ACTIVATED);

                if (ThisCode.IsExpired || ThisCode.ExpiryDate < DateTime.Now || ThisCode.Key != Constants.NEW_EMAIL_VERIFICATION_CODE)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.EMAIL_VERIFICATION_FAILED);

                UserToVerify.EmailConfirmed = true;
                var Verified = _userManager.UpdateAsync(UserToVerify).Result;

                if (!Verified.Succeeded)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);

                _codeProvider.SetExpired(ThisCode);

                return StandardResponse<UserView>.Ok().AddStatusMessage(StandardResponseMessages.EMAIL_VERIFIED);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return StandardResponse<UserView>.Failed();
            }
        }

        public StandardResponse<UserView> InitiatePasswordReset(string email)
        {
            try
            {
                var ThisUser = _userManager.FindByEmailAsync(email).Result;

                var Token = _userManager.GeneratePasswordResetTokenAsync(ThisUser).Result;

                Code PasswordResetCode = _codeProvider.New(ThisUser.Id, Constants.PASSWORD_RESET_CODE);

                PasswordResetCode.Token = Token;
                _codeProvider.Update(PasswordResetCode);

                var ConfirmationLink = $"{_globals.FrontEndBaseUrl}{_globals.PasswordResetUrl}{PasswordResetCode.CodeString}";

                List<KeyValuePair<string, string>> EmailParameters = new List<KeyValuePair<string, string>>();
                EmailParameters.Add(new KeyValuePair<string, string>(Constants.EMAIL_STRING_REPLACEMENTS_URL, ConfirmationLink));
                EmailParameters.Add(new KeyValuePair<string, string>(Constants.EMAIL_STRING_REPLACEMENTS_EXPIRYDATE, PasswordResetCode.ExpiryDate.ToShortDateString()));


                var EmailTemplate = _emailHandler.ComposeFromTemplate(Constants.PASSWORD_RESET_EMAIL_FILENAME, EmailParameters);
                var SendEmail = _emailHandler.SendEmail(ThisUser.Email, Constants.PASSWORD_RESET_EMAIL_SUBJECT, EmailTemplate);

                return StandardResponse<UserView>.Ok().AddStatusMessage(StandardResponseMessages.PASSWORD_RESET_EMAIL_SENT);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public StandardResponse<UserView> CompletePasswordReset(PasswordReset payload)
        {
            Code ThisCode = _codeProvider.GetByCodeString(payload.Code);

            var ThisUser = _userManager.FindByIdAsync(ThisCode.UserId.ToString()).Result;

            if (ThisUser == null)
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.USER_NOT_FOUND);

            if (ThisCode.IsExpired || ThisCode.ExpiryDate < DateTime.Now || ThisCode.Key != Constants.PASSWORD_RESET_CODE)
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.PASSWORD_RESET_FAILED);

            var Result = _userManager.ResetPasswordAsync(ThisUser, ThisCode.Token, payload.NewPassword).Result;

            if (!Result.Succeeded)
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);

            return StandardResponse<UserView>.Ok().AddStatusMessage(StandardResponseMessages.PASSWORD_RESET_COMPLETE);
        }

        public async void UploadSample(string base64String)
        {
            FileDocument uploadResult = FileDocument.Create();
            uploadResult = await
                BaseContentServer
                    .Build(BaseContentServer.ContentServerTypeEnum.GOOGLEDRIVE, _globals, _utilityMethods)
                    .UploadDocumentAsync(FileDocument.Create("", "", $"", FileDocumentType.GetDocumentType(MIMETYPE.PDF)));

        }

        public StandardResponse<PagedCollection<UserView>> ListAldminUsers(PagingOptions pagingOptions, string search)
        {
            var Users = _userRepository.ListUsers().Result.Users.Where(u => u.IsAdmin).
            ProjectTo<UserView>(_mappingConfigurations).AsEnumerable();

            if (!String.IsNullOrEmpty(search))
            {
                Users = Users.Where(u => u.Email.ToLower().Contains(search.ToLower()) || u.FullName.ToLower().Contains(search.ToLower()));
            }
            var PagedUsers = Users.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

            var PagedResponse = PagedCollection<UserView>.Create(Link.ToCollection(nameof(UserController.ListUsers)), PagedUsers.ToArray(), Users.Count(), pagingOptions);


            return StandardResponse<PagedCollection<UserView>>.Ok(PagedResponse).AddStatusMessage(StandardResponseMessages.SUCCESSFUL);
        }

        public StandardResponse<UserView> AddEnquiry(int propertyId)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var userEnquiries = _userEnquiryRepository.ListUserActiveEnquiries(UserId).Where(e => e.UserId == UserId && e.PropertyId == propertyId && e.Active).FirstOrDefault();
                if (userEnquiries != null)
                    return StandardResponse<UserView>.Ok();

                var result = _userRepository.AddEnquiry(propertyId, UserId).Result;
                if (!result.Succeeded)
                {
                    return StandardResponse<UserView>.Error(StandardResponseMessages.ERROR_OCCURRED);
                }
                return StandardResponse<UserView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<UserView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<UserView> CancelEnquiry(int propertyId)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                bool result = _userRepository.CancelEnquiry(propertyId, UserId).Result;
                return StandardResponse<UserView>.Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<UserView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<PagedCollection<UserEnquiry>> ListUserEnquiries(PagingOptions pagingOptions)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var userEnquiries = _userEnquiryRepository.ListUserActiveEnquiries(UserId).AsEnumerable();
                var pagedEnquiries = userEnquiries.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value);

                var PagedResponse = PagedCollection<UserEnquiry>.Create(Link.ToCollection(nameof(UserController.ListMyEnquiries)), pagedEnquiries.ToArray(), userEnquiries.Count(), pagingOptions);
                return StandardResponse<PagedCollection<UserEnquiry>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<UserEnquiry>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }


        public StandardResponse<PagedCollection<UserEnquiryView>> ListAllEnquiries(PagingOptions pagingOptions)
        {
            try
            {
                var Enquiries = _userEnquiryRepository.ListAllActiveEnquiries().OrderByDescending(x => x.DateCreated).ProjectTo<UserEnquiryView>(_mappingConfigurations);
                var pagedEnquiries = Enquiries.Skip(pagingOptions.Offset.Value).Take(pagingOptions.Limit.Value).AsEnumerable();

                var PagedResponse = PagedCollection<UserEnquiryView>.Create(Link.ToCollection(nameof(AdminController.ListAllEnquiries)), pagedEnquiries.ToArray(), Enquiries.Count(), pagingOptions);
                return StandardResponse<PagedCollection<UserEnquiryView>>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(PagedResponse);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<PagedCollection<UserEnquiryView>>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<UserEnquiryView> GetEnquiryById(int Id)
        {
            try
            {
                var Enquiry = _userEnquiryRepository.Query().Include(e => e.User)
                                                      .Include(e => e.Property)
                                                      .Include(e => e.Property.MediaFiles)
                                                      .Include(e => e.Property.PropertyType).FirstOrDefault(x => x.Id == Id);
                var mapped = _mapper.Map<UserEnquiryView>(Enquiry);

                return StandardResponse<UserEnquiryView>.Ok().AddStatusMessage(StandardResponseMessages.SUCCESSFUL).AddData(mapped);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<UserEnquiryView>.Error(StandardResponseMessages.ERROR_OCCURRED);
            }
        }

        public StandardResponse<MetricsView> Metrics()
        {
            try
            {
                var response = new MetricsView()
                {
                    Users = _userRepository.UsersCount(),
                    NewUsers = _userRepository.NewUsersCount(),
                    ActiveUsers = _userRepository.UsersCount()
                };
                return StandardResponse<MetricsView>.Ok().AddData(response);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<MetricsView>.Failed();
            }
        }

        public StandardResponse<UserView> UpdateUser(UpdateUserModel model)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var thisUser = _userRepository.ListUsers().Result.Users.FirstOrDefault(u => u.Id == UserId);
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    thisUser.PhoneNumber = model.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(model.Bank))
                {
                    thisUser.Bank = model.Bank;
                }

                if (!string.IsNullOrEmpty(model.AccountNumber))
                {
                    thisUser.AccountNumber = model.AccountNumber;
                }

                if (model.ProfilePicture != null)
                {
                    var media = _mapper.Map<Media>(model.ProfilePicture);
                    media.Name = thisUser.FirstName + "_" + thisUser.LastName + "_" + "ProfilePicture";
                    media.PropertyId = null;
                    var Result = _mediaRepository.UploadMedia(media).Result;

                    if (Result.Succeeded)
                        thisUser.ProfilePictureId = Result.UploadedMedia.Id;
                }

                var up = _userManager.UpdateAsync(thisUser).Result;

                if (!up.Succeeded)
                    return StandardResponse<UserView>.Failed(up.Errors.FirstOrDefault().Description);

                return StandardResponse<UserView>.Ok(_mapper.Map<UserView>(thisUser));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<UserView>.Failed();
            }
        }

        public StandardResponse<UserView> ResetPassword(PasswordResetModel payload)
        {
            try
            {
                int UserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<int>();
                var ThisUser = _userManager.FindByIdAsync(UserId.ToString()).Result;

                var Token = _userManager.GeneratePasswordResetTokenAsync(ThisUser).Result;
                ThisUser.Password = payload.ExistingPassword;
                var Result = _userRepository.Authenticate(ThisUser).Result;

                if (!Result.Succeeded)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(Result.ErrorMessage ?? StandardResponseMessages.ERROR_OCCURRED);

                var NextResult = _userManager.ResetPasswordAsync(ThisUser, Token, payload.NewPassword).Result;

                if (!NextResult.Succeeded)
                    return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);

                return StandardResponse<UserView>.Ok().AddStatusMessage(StandardResponseMessages.PASSWORD_RESET_COMPLETE);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StandardResponse<UserView>.Failed().AddStatusMessage(StandardResponseMessages.ERROR_OCCURRED);
            }
        }
    }
}
