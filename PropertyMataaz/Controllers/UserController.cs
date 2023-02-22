using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Services;
using PropertyMataaz.Services.Interfaces;
using PropertyMataaz.Models.UtilityModels;
using Microsoft.Extensions.Options;
using PropertyMataaz.Utilities;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Models.AppModels;
using Microsoft.AspNetCore.Authorization;

namespace PropertyMataaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly PagingOptions _defaultPagingOptions;

        public UserController(IUserService userService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _userService = userService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpPost("register", Name = nameof(Register))]
        [ProducesResponseType(200)]
        public ActionResult Register(Register newUser)
        {
            return Ok(_userService.CreateUser(newUser));
        }

        [HttpPost("token", Name = nameof(Login))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> Login(LoginModel user)
        {
            return Ok(_userService.Authenticate(user));
        }

        [HttpGet("reverify/{email}", Name = nameof(ResendVerificationMail))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> ResendVerificationMail(string email)
        {
            return Ok(_userService.RequestVerificationMail(email));
        }

        [HttpGet("verifyUser/{token}/{email}", Name = nameof(Verify))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> Verify(string token, string email)
        {
            return Ok(_userService.VerifyUser(token, email));
        }

        [HttpGet("delete/{email}", Name = nameof(Delete))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> Delete(string email)
        {
            return Ok(_userService.DeleteUser(email));
        }

        [HttpGet("reset/initiate/{email}", Name = nameof(InitiateReset))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> InitiateReset(string email)
        {
            return Ok(_userService.InitiatePasswordReset(email));
        }

        [HttpPost("reset/complete", Name = nameof(CompleteReset))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> CompleteReset(PasswordReset payload)
        {
            return Ok(_userService.CompletePasswordReset(payload));
        }

        [HttpGet("list", Name = nameof(ListUsers))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<UserView>>> ListUsers([FromQuery] PagingOptions pagingOptions = null, [FromQuery] string search = null)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            pagingOptions.Limit = 14;
            return Ok(_userService.ListUsers(pagingOptions, search));
        }

        [HttpGet("enquire/{PropertyId}", Name = nameof(AddEnquiry))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> AddEnquiry(int PropertyId)
        {
            return Ok(_userService.AddEnquiry(PropertyId));
        }

        [HttpGet("enquire/cancel/{PropertyId}", Name = nameof(CancelUserEnquiry))]
        public ActionResult<StandardResponse<UserView>> CancelUserEnquiry(int PropertyId)
        {
            return Ok(_userService.CancelEnquiry(PropertyId));
        }

        [HttpGet("enquiries/user", Name = nameof(ListMyEnquiries))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<PagedCollection<UserEnquiry>>> ListMyEnquiries([FromQuery] PagingOptions pagingOptions = null)
        {
            pagingOptions.Replace(_defaultPagingOptions);
            return Ok(_userService.ListUserEnquiries(pagingOptions));
        }

        [HttpPut("update", Name = nameof(UpdateUser))]
        [Authorize]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> UpdateUser(UpdateUserModel model)
        {
            return Ok(_userService.UpdateUser(model));
        }

        [HttpPost("password/update", Name = nameof(UpdatePassword))]
        [ProducesResponseType(200)]
        public ActionResult<StandardResponse<UserView>> UpdatePassword(PasswordResetModel model)
        {
            return Ok(_userService.ResetPassword(model));
        }
        // [HttpGet("enquiries", Name = nameof(ListAllEnquiries))]
        // [ProducesResponseType(200)]
        // public ActionResult<StandardResponse<PagedCollection<UserEnquiry>>> ListAllEnquiries([FromQuery] PagingOptions pagingOptions = null)
        // {
        //     pagingOptions.Replace(_defaultPagingOptions);
        //     return Ok(_userService.ListAllEnquiries(pagingOptions));
        // }
    }
}
