using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ServiceManaco.SecurityToken
{
    public static class CommonManager
    {
        public static string GetIP(HttpRequestBase request)
        {
            string ip = request.Headers["X-Forwarded-For"]; // AWS compatibility

            if (string.IsNullOrEmpty(ip))
            {
                ip = request.UserHostAddress;
            }

            return ip;
        }
    }
}