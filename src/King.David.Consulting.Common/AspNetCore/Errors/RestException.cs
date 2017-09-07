using System;
using System.Net;

namespace King.David.Consulting.Common.AspNetCore.Errors
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, string message = null)
            : base(message)
        {
            Code = code;
        }

        public HttpStatusCode Code { get; }
    }
}
