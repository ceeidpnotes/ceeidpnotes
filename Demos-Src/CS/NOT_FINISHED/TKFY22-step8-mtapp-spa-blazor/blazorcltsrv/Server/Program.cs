using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
//
builder.Services.AddAuthentication("Bearer").AddMicrosoftIdentityWebApi(
    opt1 =>
    {
        opt1.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents();
        //opt1.Events.OnMessageReceived = async ctx => { }; //All Messages
        opt1.Events.OnTokenValidated = ctx =>
        {
            string? tenantId = ctx.Principal?.Claims.FirstOrDefault(x => x.Type is "tid" or "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            if (tenantId is null)
                throw new UnauthorizedAccessException("No tenant information");
            string[] validTenants = new string[] { "AAA", "BBB", "2feaa5b1-2722-4933-afca-4d14140d5ef0", "07b42849-58ba-42ca-a954-1ce79ed62182" };

            if (!validTenants.Contains(tenantId))
                throw new UnauthorizedAccessException("This tenant is not authorized");
            return Task.CompletedTask;
        };
    },
    opt2 =>
    {
        opt2.ClientId = builder.Configuration["AzureAd:ClientId"];
        opt2.TenantId = builder.Configuration["AzureAd:TenantId"];
        opt2.Instance = builder.Configuration["AzureAd:Instance"];
        //opt2.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        opt2.TokenValidationParameters.RoleClaimType = "roles"; //To get roles!
    }
    )
    //.EnableTokenAcquisitionToCallDownstreamApi(cfg => {
    //    cfg.ClientId = builder.Configuration["AzureAd:ClientId"];
    //    cfg.TenantId = builder.Configuration["AzureAd:TenantId"];
    //    cfg.Instance = builder.Configuration["AzureAd:Instance"];
    //    cfg.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
    //})
    //.AddMicrosoftGraph(graphBaseUrl: "https://graph.microsoft.com/v1.0", defaultScopes: "user.read")
    //.AddInMemoryTokenCaches()
    ;



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//app.MapRazorPages();
//app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    /*
     * https://localhost:7188/?admin_consent=True
     * &tenant=2feaa5b1-2722-4933-afca-4d14140d5ef0
     * &state=http%3a%2f%2flocalhost%3a7188
     * &scope=https%3a%2f%2fgraph.microsoft.com%2fUser.Read+https%3a%2f%2fgraph.microsoft.com%2f.default#
    */
    //endpoints.MapGet("/?admin_consent={consent}&tenat={tenant}&state={state}&scope={scope}", (HttpContext ctx, string consent, string tenant, string state, string scope) =>
    //{
    //    //var qs = ctx.Request.QueryString.ToString();
    //    //var q = HttpUtility.ParseQueryString(qs);
    //    //var ri = q["state"];
    //    if (state!= null) ctx.Response.Redirect(state);
    //    return Task.CompletedTask;
    //});
});


app.MapFallbackToFile("index.html");

app.Run();
