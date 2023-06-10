using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace BitNetServices.Alpa.EHS.Web
{
    /// <summary>
    /// JSON storage extension to store complex objects
    /// http://benjii.me/2016/07/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core/
    /// </summary>
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static bool HasKey(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value==null;
        }

        public static string RawUrl(this HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Scheme))
            {
                throw new InvalidOperationException("Missing Scheme");
            }
            if (!request.Host.HasValue)
            {
                throw new InvalidOperationException("Missing Host");
            }
            string path = (request.PathBase.HasValue || request.Path.HasValue) ? (request.PathBase + request.Path).ToString() : "/";
            return request.Scheme + "://" + request.Host + path + request.QueryString;
        }
    }

    
}
