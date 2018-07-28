using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace HonorITDemo.Helpers
{
    internal static class OAuthServiceHelper
    {
        public static void GetAuthorizationCode(this OAuthInfo info)
        {
            info.Key = ConfigurationManager.AppSettings["clientId"];
            info.RedirectUri = ConfigurationManager.AppSettings["redirectUrl"];
            info.Scope = ConfigurationManager.AppSettings["scope"];
            info.AuthorizationUrl= ConfigurationManager.AppSettings["authorizationUrl"];

            var authorizationParams = string.Format("?client_id={0}&redirect_uri={1}&response_type=code&scope={2}", info.Key, HttpUtility.UrlEncode(info.RedirectUri), info.Scope);
            var authorizationUri = info.AuthorizationUrl + authorizationParams;

            HttpContextFactory.Current.Response.Redirect(authorizationUri);
        }
      
        //Custom request Access 
        public static OAuthToken GetAccessToken(OAuthInfo info = null,string code="")
        {
            var accessTokenBody = string.Format("client_id={0}&client_secret={1}&scope={2}&code={3}&redirect_uri={4}&grant_type=authorization_code",
            info.Key, info.Secret, info.Scope, code, info.RedirectUri);

            var reply = DoPost(info.TokenUrl, accessTokenBody);

            var tokenJson = JsonConvert.DeserializeObject<dynamic>(reply);

            info.Token = new OAuthToken
            {
                AccessToken = tokenJson.access_token,
                RefreshToken = tokenJson.refresh_token,
                ExpiresIn = tokenJson.expires_in,
                Scope = tokenJson.scope,
                TokenType = tokenJson.token_type
            };

            return info.Token;
        }
       
        private static string DoPost(string url, string body)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var bytes = Encoding.ASCII.GetBytes(body);
            if (bytes.Length > 0)
            {
                request.ContentLength = bytes.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                {
                    throw new InvalidOperationException("WebReponse not received.");
                }
                using (var sr = new StreamReader(responseStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        //public static void RequestAccessToken(this OAuthInfo info)
        //{
        //    var requestUri = HttpContextFactory.Current.Request.Url;
        //    var queries = HttpUtility.ParseQueryString(requestUri.Query);
        //    var code = queries["code"];

        //    if (string.IsNullOrEmpty(code))
        //    {
        //        info.Token = new OAuthToken
        //        {
        //            AccessToken = requestUri.Query.TrimStart('?')
        //        };
        //        return;
        //    }
        //    var accessTokenBody = string.Format("client_id={0}&client_secret={1}&scope={2}&code={3}&redirect_uri={4}&grant_type=authorization_code",
        //    info.Key, info.Secret, info.Scope, code, info.RedirectUri);

        //    var reply = DoPost(info.TokenUrl, accessTokenBody);
        //    var tokenJson = JsonConvert.DeserializeObject<dynamic>(reply);

        //    info.Token = new OAuthToken
        //    {
        //        AccessToken = tokenJson.access_token,
        //        RefreshToken = tokenJson.refresh_token,
        //        ExpiresIn = tokenJson.expires_in,
        //        Scope = tokenJson.scope,
        //        TokenType = tokenJson.token_type
        //    };
        //}

        //public static OAuthToken RefreshToken(this OAuthInfo info)
        //{
        //    var accessTokenBody = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token",
        //    info.Key, info.Secret, info.RefreshToken);

        //    var reply = DoPost(info.TokenUrl, accessTokenBody);
        //    var tokenJson = JsonConvert.DeserializeObject<dynamic>(reply);

        //    info.Token = new OAuthToken
        //    {
        //        AccessToken = tokenJson.access_token,
        //        RefreshToken = tokenJson.refresh_token,
        //        ExpiresIn = tokenJson.expires_in,
        //        Scope = tokenJson.scope,
        //        TokenType = tokenJson.token_type
        //    };

        //    return info.Token;
        //}
    }
}
