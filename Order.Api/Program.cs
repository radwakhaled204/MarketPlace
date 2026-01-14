using Order.Api.Domain.Entities;
using Order.Api.Infrastructure.Mappings;
using Order.Api.Infrastructure.Repositories;
using System.Net;
using Order.Api.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(CrmMappingsProfile));
builder.Services.AddScoped<ICrmOrderClient, Order.Api.Infrastructure.Repositories.CrmOrderClient>();
//builder.Services.AddScoped<Order.Api.Domain.Interfaces.ICrmOrderItemClient, Order.Api.Infrastructure.CRM.CrmOrderItemClient>();

//builder.Services.AddScoped<Order.Api.Application.Services.OrderItemsService>();
builder.Services.AddScoped<Order.Api.Application.Services.OrdersService>();
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





var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
