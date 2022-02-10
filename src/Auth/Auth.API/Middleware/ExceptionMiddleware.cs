using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Auth.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException validationException)
            {
                await HandleValidationExceptionAsync(httpContext, validationException);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, -1);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string message, int errorCode)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var result = JsonConvert.SerializeObject(new HttpRequestError
            {
                ErrorCode = errorCode,
                Error = message
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return context.Response.WriteAsync(result);
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            dynamic expando = new ExpandoObject();
            IDictionary<string, object> validationErrorObject = expando;
            
            foreach (var validationExceptionError in validationException.Errors)
            {
                validationErrorObject[validationExceptionError.PropertyName] = new string[] { validationExceptionError.ErrorMessage };
            }
            
            var result = JsonConvert.SerializeObject(validationErrorObject, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return context.Response.WriteAsync(result);
        }
    }
    
    public class HttpRequestError
    {
        public int ErrorCode { get; set; }
        public string Error { get; set; }
    }
}