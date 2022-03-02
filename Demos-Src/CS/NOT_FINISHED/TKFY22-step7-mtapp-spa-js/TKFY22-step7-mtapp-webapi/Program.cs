using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpLogging();
// Add services to the container.

//Mandatory - JS call (of course - IRL - not *)
builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder //.AllowAnyOrigin() - or credentials! - will not work 
           .SetIsOriginAllowed(i => {
               return true;
           })
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
           //.WithOrigins("http://localhost:3000")
           ;

}));

//AddMicrosoftIdentityWebApiAuthentication(builder.Configuration) = AddAuthentication and AddMicrosoftIdentityWebApi
//builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);

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
        opt2.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        opt2.TokenValidationParameters.RoleClaimType = "roles"; //To get roles!
    }
    ).EnableTokenAcquisitionToCallDownstreamApi(cfg => {
        cfg.ClientId = builder.Configuration["AzureAd:ClientId"];
        cfg.TenantId = builder.Configuration["AzureAd:TenantId"];
        cfg.Instance = builder.Configuration["AzureAd:Instance"];
        cfg.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
    })
    .AddMicrosoftGraph(graphBaseUrl: "https://graph.microsoft.com/v1.0", defaultScopes: "user.read")
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("default");
//app.UseCors();// - try and see why it is not working!

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", (ctx) =>
     {

         var qs = ctx.Request.QueryString.ToString();
         var q = HttpUtility.ParseQueryString(qs);
         var ri = q["state"];
         if (ri != null) ctx.Response.Redirect(ri);
         return Task.CompletedTask;
     });
});

app.Run();


//'invalid_client', error_description: 'AADSTS650053: The application 'TKFY22-step6-mtapp-client' asked for scope 'ApiPermission01' that doesn't exist on the resource '00000003-0000-0000-c000-000000000000'. Contact the app vendor.