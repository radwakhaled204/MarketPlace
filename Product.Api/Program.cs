using Product.Api.Domain.Interfaces;
using Product.Api.Infrastructure.CRM;
using Product.Api.Infrastructure.Mappings;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
//builder.Services.AddAutoMapper(typeof(CrmMappingsProfile));

// ✅ Windows Auth (On-Prem) HttpClient
builder.Services.AddHttpClient("Crm", (sp, client) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();

    var baseUrl = cfg["Crm:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl!);

    client.DefaultRequestHeaders.Add("OData-Version", "4.0");
    client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();

    var domain = cfg["Crm:Domain"];
    var user = cfg["Crm:Username"];
    var pass = cfg["Crm:Password"];

    return new HttpClientHandler
    {
        Credentials = new NetworkCredential(user, pass, domain),
        PreAuthenticate = true,
        UseDefaultCredentials = false,
       
        UseProxy = true
    };
});


builder.Services.AddScoped<ICrmProductClient, CrmProductClient>();


builder.Services.AddAutoMapper(typeof(CrmMappingsProfile));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
