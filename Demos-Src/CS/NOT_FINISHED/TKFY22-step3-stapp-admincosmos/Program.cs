/*
 * admin@tkfreeaad.onmicrosoft.com - all rights, admin
 * listtenants@tkfreeaad.onmicrosoft.com - only list tenants, not update
 * user@tkfreeaad.onmicrosoft.com - no access, but can see user profile etc
 */

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using TKFY22_step3_stapp_admincosmos;

var builder = WebApplication.CreateBuilder(args);

// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;


builder.Services.AddDbContext<fTKFY22_step3_stapp_admincosmos.DatabaseContext>(
    o =>
    {
        o.UseCosmos(
                builder.Configuration["DB:Connection"],
                databaseName: builder.Configuration["DB:Database"],
                options =>
                {
                    options.ConnectionMode(ConnectionMode.Gateway); //or Direct
                });
    }
    );


builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options => {
        options.ClientId = builder.Configuration["AzureAd:ClientId"];
        options.TenantId = builder.Configuration["AzureAd:TenantId"];
        options.Instance = builder.Configuration["AzureAd:Instance"];
        options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        options.Domain = builder.Configuration["AzureAd:Domain"];
        options.TokenValidationParameters.RoleClaimType = "roles"; //To get roles!
    })
    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { "User.Read" })
    .AddMicrosoftGraph(graphBaseUrl : "https://graph.microsoft.com/v1.0", defaultScopes : "user.read")
    .AddInMemoryTokenCaches();
    //.AddSessionTokenCaches();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("updateTenantPolicy", policy => policy.RequireClaim("roles", new string[] { "updateTenant" }));
});

// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseAuthentication(); //Try to switch!
app.UseAuthorization();

app.MapRazorPages();

app.Run();
