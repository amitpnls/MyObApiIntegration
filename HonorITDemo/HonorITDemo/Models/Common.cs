using HonorITDemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HonorITDemo.Models
{
    public class Common
    {
        static string apiBaseUrl = WebConfigurationManager.AppSettings["authorizationUrl"];
        static string client_id = WebConfigurationManager.AppSettings["clientId"];
        static string client_secret = WebConfigurationManager.AppSettings["clientSecret"];
        static string redirect_uri = WebConfigurationManager.AppSettings["redirectUrl"];
        static string tokenUrl = WebConfigurationManager.AppSettings["tokenUrl"];
        static string scope = WebConfigurationManager.AppSettings["scope"];

        static public void ReGenerateToken()
        {
            CallOAuthAuthentication();
        }

        static public void CallOAuthAuthentication()
        {
            OAuthKeyService.OAuthInformation.GetAuthorizationCode();
        }

        //Authentication 
        static public void OAuthCallback()
        {
            var requestUri = HttpContextFactory.Current.Request.Url;
            var queries = HttpUtility.ParseQueryString(requestUri.Query);
            var code = queries["code"];

            OAuthInfo info = new OAuthInfo()
            {
                Key = client_id,
                Secret = client_secret,
                Scope = scope,
                RedirectUri = redirect_uri,
                TokenUrl = tokenUrl
            };

            //Retrieve Access token
            OAuthToken oauthtoken = OAuthServiceHelper.GetAccessToken(info, code);

            if (OAuthKeyService.OAuthInformation.Token == null)
                OAuthKeyService.OAuthInformation.Token = new OAuthToken();
            OAuthKeyService.OAuthInformation.Token.AccessToken = oauthtoken.AccessToken;
            OAuthKeyService.OAuthInformation.Token.RefreshToken = oauthtoken.RefreshToken;
            OAuthKeyService.OAuthInformation.Token.ExpiresIn = oauthtoken.ExpiresIn;

            //return RedirectToAction("QuoteList", "Home");
        }
    }
}