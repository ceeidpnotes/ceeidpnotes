
using fy21_simplemtapp;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.Url

// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var initialScopes = new[]
{
    // A full string array of the scopes you want the user to consent to...
    "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission02.WebApi02",
    "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission02.WebApi01",
    "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission01"
};

//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd", subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        options.ClientId = builder.Configuration["AzureAd:ClientId"];
        options.TenantId = builder.Configuration["AzureAd:TenantId"];
        options.Instance = builder.Configuration["AzureAd:Instance"];
        options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        options.TokenValidationParameters.RoleClaimType = "roles"; //To get roles!
        options.Events.OnTokenValidated = async context =>
        {
            string? tenantId = context.SecurityToken.Claims.FirstOrDefault(x => x.Type is "tid" or "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            //throw new UnauthorizedAccessException("This tenant is not authorized");
        };
    })
    //.EnableTokenAcquisitionToCallDownstreamApi(initialScopes) //IF we want to get consent BEFORE
    .EnableTokenAcquisitionToCallDownstreamApi() //Incremental, dynamics
    .AddInMemoryTokenCaches(); //For CLIENT part

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Page01policy", policy => policy.RequireClaim("roles", new string[] { "Page01", "Page02" }));
});


// Add services to the container.
// For dynamic consent!
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();


builder.Services.AddHttpClient<ApiPermission01HttpClient>();
builder.Services.AddHttpClient<ApiPermission02HttpClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();


app.Run(); //This is where Urls are initiated; so - can't read them earlier

