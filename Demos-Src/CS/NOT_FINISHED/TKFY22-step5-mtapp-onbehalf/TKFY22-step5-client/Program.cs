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

            if (string.IsNullOrWhiteSpace(tenantId))
                throw new UnauthorizedAccessException("Unable to get tenantId from token.");


            var db = context.HttpContext.RequestServices.GetRequiredService<fy21_simplemtapp.Model.DatabaseContext>();
            var result = await db.Subscriptions.FirstOrDefaultAsync(o => o.Id == tenantId && o.IsActive == true);
            if (result == null)
                throw new UnauthorizedAccessException("This tenant is not authorized");
        };
    })
    .EnableTokenAcquisitionToCallDownstreamApi() //For CLIENT part
        //.AddDownstreamWebApi("Api01", builder.Configuration)
        //.AddDownstreamWebApi("Api02", builder.Configuration)
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
builder.Services.AddHttpClient<API01HttpClient>();


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

