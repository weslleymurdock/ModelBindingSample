using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using ModelBindingSample.Controllers;
using ModelBindingSample.Models;
using ModelBindingSample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new IssueModelBinderProvider());
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.Configure<ForwardedHeadersOptions>(x =>
{
    x.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
});

builder.Services.Configure<MongoSettings>(x =>
{
    x.Connection = builder.Configuration.GetSection("MongoSettings:Connection").Value;
    x.Database = builder.Configuration.GetSection("MongoSettings:DatabaseName").Value;
    x.Issues = builder.Configuration.GetSection("MongoSettings:Issues").Value;
});

builder.Services.AddHealthChecks().AddCheck<MongoHealthCheck>("MongoConnectionCheck");
builder.Services.AddSingleton<IMongoContext<Issue>, MongoContext>();
builder.Services.AddScoped(typeof(MongoContext));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = actionContext =>
    {
        o.SuppressModelStateInvalidFilter = true;
        return new BadRequestObjectResult(actionContext.ModelState);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
