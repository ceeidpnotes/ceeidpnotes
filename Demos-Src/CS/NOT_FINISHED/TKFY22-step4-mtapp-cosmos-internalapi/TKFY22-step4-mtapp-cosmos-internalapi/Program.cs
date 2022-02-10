/*Registration: 
Multitenant

M365x578388.onmicrosoft.com
fy21-simplemtapp
Application (client) ID
:
07b0ef27-5ebe-49e0-9a14-a82a145c3998

Directory (tenant) ID
:
2feaa5b1-2722-4933-afca-4d14140d5ef0

https://localhost:49155/ (docker)
https://localhost:49155/signin-oidc (not used)
https://localhost:49155/signout-oidc (not used)

secret - not needed (no API Call - pure login)

--------
Registration on FREE Tier (MT on Free tier)

Application (client) ID: 57092a36-15e8-44e3-b2b6-3bbf6cb20fc0
Directory (tenant) ID: 07b42849-58ba-42ca-a954-1ce79ed62182
https://localhost:49155/ (docker)

 */

using fy21_simplemtapp;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.Url

// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;



builder.Services.AddDbContext<fy21_simplemtapp.Model.DatabaseContext>(
    o=>
    {
        o.UseCosmos(
                builder.Configuration["DB:Connection"],
                databaseName: builder.Configuration["DB:Database"],
                options =>
                {
                    options.ConnectionMode(ConnectionMode.Gateway); //or Direct
                    ///options.WebProxy(new WebProxy());
                    //options.LimitToEndpoint();
                    //options.Region(Regions.WestEurope);
                    //options.GatewayModeMaxConnectionLimit(32);
                    //options.MaxRequestsPerTcpConnection(8);
                    //options.MaxTcpConnectionsPerEndpoint(16);
                    //options.IdleTcpConnectionTimeout(TimeSpan.FromMinutes(1));
                    //options.OpenTcpConnectionTimeout(TimeSpan.FromMinutes(1));
                    //options.RequestTimeout(TimeSpan.FromMinutes(1));
                });
    }
    );

//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd", subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(options =>
{
    options.ClientId = builder.Configuration["AzureAd:ClientId"];
    options.TenantId = builder.Configuration["AzureAd:TenantId"];
    options.Instance = builder.Configuration["AzureAd:Instance"];
    options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
    options.TokenValidationParameters.RoleClaimType = "roles"; //To get roles!
    options.Events.OnTokenValidated = async context =>
    {
        string? tenantId = context.SecurityToken.Claims.FirstOrDefault(x => x.Type is "tid" or "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

        if (string.IsNullOrWhiteSpace(tenantId))
            throw new UnauthorizedAccessException("Unable to get tenantId from token.");


        var db = context.HttpContext.RequestServices.GetRequiredService<fy21_simplemtapp.Model.DatabaseContext>();
        var result = await db.Subscriptions.FirstOrDefaultAsync(o => o.Id == tenantId && o.IsActive == true);
        if (result == null)
            throw new UnauthorizedAccessException("This tenant is not authorized");
    };
})
    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { builder.Configuration["Api02:Scope"], builder.Configuration["ApiInternal:Scope"] }) //For CLIENT part
    .AddInMemoryTokenCaches(); //For CLIENT part

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Page01policy", policy => policy.RequireClaim("roles", new string[] { "Page01", "Page02" }));
});


// Add services to the container.
builder.Services.AddRazorPages();

// Add API
builder.Services.AddControllers();

builder.Services.AddHttpClient<API02HttpClient>();


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

