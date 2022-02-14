using fy21_simplemtapp;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration) //Default - AzureAd
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddMicrosoftGraph(graphBaseUrl: "https://graph.microsoft.com/v1.0", defaultScopes: "user.read")
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddHttpClient<API01HttpClient>();

//Allowing CORS for all domains and methods for the purpose of sample
builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("default");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
