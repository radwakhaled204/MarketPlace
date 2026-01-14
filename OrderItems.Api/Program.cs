using OrderItems.Api.Application.Services;
using OrderItems.Api.Domain.Interfaces;
using OrderItems.Api.Infrastructure.Mappings;
using OrderItems.Api.Infrastructure.Repositories;
using System.Net;
using static OrderItems.Api.Application.Services.OrderItemService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(CrmMappingsProfile));

builder.Services.AddScoped<OrderItemService>();
builder.Services.AddScoped<ICrmOrderItemClient, CrmOrderItemClient>();
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
