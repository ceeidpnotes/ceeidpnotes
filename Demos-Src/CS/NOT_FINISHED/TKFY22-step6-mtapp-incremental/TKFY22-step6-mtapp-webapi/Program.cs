using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration) //Default - AzureAd
    .EnableTokenAcquisitionToCallDownstreamApi()
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


//'invalid_client', error_description: 'AADSTS650053: The application 'TKFY22-step6-mtapp-client' asked for scope 'ApiPermission01' that doesn't exist on the resource '00000003-0000-0000-c000-000000000000'. Contact the app vendor.