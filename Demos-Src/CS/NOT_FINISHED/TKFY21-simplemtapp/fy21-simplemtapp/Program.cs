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

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd", subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(options => {
    options.ClientId = builder.Configuration["AzureAd:ClientId"];
    options.TenantId = builder.Configuration["AzureAd:TenantId"];
    options.Instance = builder.Configuration["AzureAd:Instance"];
    options.Events.OnTokenValidated = context =>
    {
        string? tenantId = context.SecurityToken.Claims.FirstOrDefault(x => x.Type is "tid" or "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

        if (string.IsNullOrWhiteSpace(tenantId))
            throw new UnauthorizedAccessException("Unable to get tenantId from token.");

        /* Do we know that tenant or do we have information about "saas" app claims"*/
        
        //Normally - some db, CosmosDB recommended (fast and inexpensive)
        //var dbContext = context.HttpContext.RequestServices.GetRequiredService<SampleDbContext>();
        //var authorizedTenant = await dbContext.AuthorizedTenants.FirstOrDefaultAsync(t => t.TenantId == tenantId);
        
        //Here - hardcoded table

        string[] validTenants = new string[] { "AAA", "BBB", "67b8b473-b8e0-4cdf-bef6-c9b14beaba48", "72f988bf-86f1-41af-91ab-2d7cd011db47", "07b42849-58ba-42ca-a954-1ce79ed62182" };

        if (!validTenants.Contains(tenantId))
            throw new UnauthorizedAccessException("This tenant is not authorized");
        return Task.FromResult(0);
    };
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

