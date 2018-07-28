using System;
using System.Web;
//using MYOB.AccountRight.SDK;
//using MYOB.AccountRight.SDK.Contracts;
using Newtonsoft.Json;
using System.Web.Configuration;

namespace HonorITDemo.Helpers
{
    public class HttpContextFactory
    {
        private static HttpContext _context;
        public static HttpContext Current
        {
            get
            {
                if (_context != null)
                    return _context;

                if (HttpContext.Current == null)
                    throw new InvalidOperationException("HttpContext not available");

                return HttpContext.Current;
            }
        }

        public static void SetCurrentContext(HttpContext context)
        {
            _context = context;
        }
    }

    public class OAuthInfo
    {
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public OAuthToken Token { get; set; }
        public string AccessToken { get { return Token == null ? string.Empty : Token.AccessToken; } }
        public string RefreshToken { get { return Token == null ? string.Empty : Token.RefreshToken; } }
        public string ExpiresIn { get { return Token == null ? string.Empty : Token.ExpiresIn.ToString() + "ms"; } }
        public string TokenType { get { return Token == null ? string.Empty : Token.TokenType; } }
    }
    public class OAuthToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
    }

    public static class OAuthKeyService
    {
        public static OAuthInfo OAuthInformation
        {
            get
            {
                var info = HttpContextFactory.Current.Session["OAuthInfo"] as OAuthInfo;

                if (info == null)
                {
                    info = new OAuthInfo
                    {
                        AuthorizationUrl = WebConfigurationManager.AppSettings["authorizationUrl"],
                        Key = WebConfigurationManager.AppSettings["clientId"],
                        TokenUrl = WebConfigurationManager.AppSettings["tokenUrl"],
                        Secret = WebConfigurationManager.AppSettings["clientSecret"],
                        RedirectUri = WebConfigurationManager.AppSettings["redirectUrl"],
                        Scope = WebConfigurationManager.AppSettings["scope"]
                    };

                    HttpContextFactory.Current.Session["OAuthInfo"] = info;
                }
                return info;
            }
            set
            {
                HttpContextFactory.Current.Session["OAuthInfo"] = value;
            }
        }
    }

    //public class OAuthKeyService : IOAuthKeyService
    //{
    //    private const string CsTokensFile = "Tokens.json";

    //    private OAuthTokens _tokens;

    //    /// <summary>
    //    /// On creation read any settings from file
    //    /// </summary>
    //    /// <remarks></remarks>
    //    public OAuthKeyService()
    //    {
    //        ReadFromFile();
    //    }

    //    #region IOAuthKeyService Members

    //    /// <summary>
    //    /// Implements the property for OAuthResponse which holdes theTokens
    //    /// </summary>
    //    /// <value>object containing OAuthTokens</value>
    //    /// <returns>Contracts.OAuthTokens</returns>
    //    /// <remarks>Saves to isolated storage when set</remarks>
    //    public OAuthTokens OAuthResponse
    //    {
    //        get { return _tokens; }
    //        set
    //        {
    //            _tokens = value;
    //            SaveToFile();
    //        }
    //    }

    //    #endregion

    //    /// <summary>
    //    /// Method to read Tokens from Isolated storage
    //    /// </summary>
    //    /// <remarks></remarks>
    //    private void ReadFromFile()
    //    {
    //        try
    //        {
    //            // Get an isolated store for user and application 
    //            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(
    //                IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);

    //            var isoStream = new IsolatedStorageFileStream(CsTokensFile, FileMode.Open,
    //                                                          FileAccess.Read, FileShare.Read);

    //            var reader = new StreamReader(isoStream);
    //            // Read the data.

    //            _tokens = JsonConvert.DeserializeObject<OAuthTokens>(reader.ReadToEnd());
    //            reader.Close();

    //            isoStore.Dispose();
    //            isoStore.Close();
    //        }
    //        catch (FileNotFoundException)
    //        {
    //            // Expected exception if a file cannot be found. This indicates that we have a new user.
    //            _tokens = null;
    //        }
    //    }


    //    /// <summary>
    //    /// Method to save tokens to isolated storage
    //    /// </summary>
    //    /// <remarks></remarks>
    //    private void SaveToFile()
    //    {
    //        // Get an isolated store for user and application 
    //        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(
    //            IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);

    //        // Create a file
    //        var isoStream = new IsolatedStorageFileStream(CsTokensFile, FileMode.OpenOrCreate,
    //                                                      FileAccess.Write, isoStore);
    //        isoStream.SetLength(0);
    //        //Position to overwrite the old data.

    //        // Write tokens to file
    //        var writer = new StreamWriter(isoStream);
    //        writer.Write(JsonConvert.SerializeObject(_tokens));
    //        writer.Close();

    //        isoStore.Dispose();
    //        isoStore.Close();
    //    }
    //}


}