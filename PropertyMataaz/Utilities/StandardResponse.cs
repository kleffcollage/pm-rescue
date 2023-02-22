using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PropertyMataaz.Models;

namespace PropertyMataaz.Utilities
{
    public class StandardResponse<T> : Link
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string StatusCode { get; set; }
        public object Errors { get; set; }
        [JsonIgnore]
        public Link Self { get; set; }


        public StandardResponse()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplicationService.ViewModel.StandardResponse"/> class.
        /// </summary>
        /// <param name="status">If set to <c>true</c> status.</param>
        private StandardResponse(bool status)
        {
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplicationService.ViewModel.StandardResponse"/> class.
        /// </summary>
        /// <param name="status">If set to <c>true</c> status.</param>
        /// <param name="message">Message.</param>
        private StandardResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplicationService.ViewModel.StandardResponse"/> class.
        /// </summary>
        /// <param name="status">If set to <c>true</c> status.</param>
        /// <param name="message">Message.</param>
        /// <param name="data">Data.</param>
        private StandardResponse(bool status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Initialize StandardResponse(bool status, string message, object data, string statusCode)
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        private StandardResponse(bool status, string message, T data, string statusCode)
        {
            Status = status;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static StandardResponse<T> Create()
        {
            return new StandardResponse<T>();
        }

        /// <summary>
        /// Create the specified status.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="status">If set to <c>true</c> status.</param>
        public static StandardResponse<T> Create(bool status)
        {
            return new StandardResponse<T>(status);
        }

        /// <summary>
        /// Create the specified status and message.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="status">If set to <c>true</c> status.</param>
        /// <param name="message">Message.</param>
        public static StandardResponse<T> Create(bool status, string message)
        {
            return new StandardResponse<T>(status, message);
        }

        /// <summary>
        /// Create the specified status, message and data.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="status">If set to <c>true</c> status.</param>
        /// <param name="message">Message.</param>
        /// <param name="data">Data.</param>
        public static StandardResponse<T> Create(bool status, string message, T data)
        {
            return new StandardResponse<T>(status, message, data);
        }

        /// <summary>
        /// Adds the status code.
        /// </summary>
        /// <returns>The status code.</returns>
        /// <param name="statusCode">Status code.</param>

        public StandardResponse<T> AddStatusCode(string statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        /// <summary>
        /// Adds the status message.
        /// </summary>
        /// <returns>The status message.</returns>
        /// <param name="message">Message.</param>
        public StandardResponse<T> AddStatusMessage(string message)
        {
            this.Message = message;
            return this;
        }

        /// <summary>
        /// Error that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Error(string message)
        {
            return new StandardResponse<T>(false, message);
        }

        /// <summary>
        /// Error that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Error(string message, string statusCode)
        {
            return new StandardResponse<T>(false, message, default(T), statusCode);
        }

        /// <summary>
        ///  Ok that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Ok(string message)
        {
            return new StandardResponse<T>(true, message, default(T), HttpStatusCode.OK.ToString());
        }

        /// <summary>
        ///  Ok that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Ok(T data)
        {
            return new StandardResponse<T>(true, StandardResponseMessages.SUCCESSFUL, data, HttpStatusCode.OK.ToString());
        }

        public static StandardResponse<T> NotFound(string message, string status)
        {
            return new StandardResponse<T>(false, message, default(T), status);
        }
        /// <summary>
        ///  Ok that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Ok()
        {
            return new StandardResponse<T>(true, StandardResponseMessages.SUCCESSFUL, default(T), HttpStatusCode.OK.ToString());
        }

        /// <summary>
        ///  Failed that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Failed()
        {
            return new StandardResponse<T>(false, StandardResponseMessages.UNSUCCESSFUL);
        }

        /// <summary>
        ///  Failed that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Failed(string message)
        {
            return new StandardResponse<T>(false, message);
        }

        /// <summary>
        ///  Failed that return this
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StandardResponse<T> Failed(string message, string statusCode)
        {
            return new StandardResponse<T>(false, message, default(T), statusCode);
        }


        /// <summary>
        /// Build this instance.
        /// </summary>
        /// <returns>The build.</returns>
        public StandardResponse<T> Build()
        {
            return this;
        }

        public StandardResponse<T> AddData(T data)
        {
            this.Data = data;
            return this;
        }
    }
}
