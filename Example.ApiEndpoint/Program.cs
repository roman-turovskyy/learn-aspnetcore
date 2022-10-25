using AutoMapper;
using Example.Common.Web;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Converters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOData();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
    options.SerializerSettings.Converters.Add(new EnumReferenceJsonConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvcCore(options =>
{
    options.EnableEndpointRouting = false;
    foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
        outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));

    foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
        inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
});

DIContainer.Configure(builder);

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AllowNullCollections = true;
    mc.AddProfile(new MappingProfile());
});

builder.Services.AddSingleton(mappingConfig.CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseMvc(builder =>
{
    builder.Filter().Select().OrderBy().MaxTop(null).Count().Expand();
    builder.EnableDependencyInjection();
});

app.MapControllers();

app.Run();
