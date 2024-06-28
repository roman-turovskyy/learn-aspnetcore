using AutoMapper;
using Example.Common.Web;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Newtonsoft.Json.Converters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
    options.SerializerSettings.Converters.Add(new EnumReferenceJsonConverter());
})
    .AddOData(options => options.Select().Filter().OrderBy())
    .AddODataNewtonsoftJson(); // https://stackoverflow.com/a/76231680

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

app.Run();
