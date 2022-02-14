using fy21_simplemtapp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace fy21_simplemtapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly IConfiguration Config;
        public string Api01Consent { get; }
        public string Api02Consent { get; }
        public string WebUIConsent { get; }

        private readonly string _ApiScope = "https://graph.microsoft.com/.default";
        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            Config = config;
            //Look: https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-admin-consent
            //Url For Admin Consent
            Api01Consent = "https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=" + 
                Config["Api01:ClientId"] +
                "&redirect_uri=" + Config["Api01:RedirectUri"] +
                "&state=" + Config["AzureAD:RedirectUri"] +
                "&scope=" + _ApiScope;
            Api02Consent = "https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=" + 
                Config["Api02:ClientId"] +
                "&redirect_uri=" + Config["Api02:RedirectUri"] +
                "&state=" + Config["AzureAD:RedirectUri"] +
                "&scope=" + "User.Read " + Config["Api01:Scope"];
            WebUIConsent = "https://login.microsoftonline.com/organizations/v2.0/adminconsent?client_id=" + 
                Config["AzureAD:ClientId"] +
                "&redirect_uri=" + Config["AzureAD:RedirectUri"] +
                "&state=" + randomString(5) +
                "&scope=" + Config["Api02:Scope"] + " " + Config["Api01:Scope"];
        }

        private static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
        }

        public async Task OnGet()
        {

        }
    }
}