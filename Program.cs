using System.ComponentModel;
using System.Net.Mime;
using System.Text.Json;
using Catalog.Repositories;
using Catalog.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// 아래는 자동 생성 될 guid(id)와 datetime의 type을 미리 지정해주는 코드. 이걸 설정안하면 내가 예상하지 않은대로 datatype이 설정 될 수 있음
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
  // var settings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
  // return new MongoClient(settings.ConnectionString);
  return new MongoClient(mongoDbSettings.ConnectionString);
}
);


builder.Services.AddHealthChecks()
  .AddMongoDb(
      mongoDbSettings.ConnectionString,
      name: "mongodb",
      timeout: TimeSpan.FromSeconds(3),
      tags: new[] { "ready" }
);

// Add singleton service
// builder.Services.AddSingleton<IItemsRepository, InMemItemsRepository>();
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
  options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseHttpsRedirection();
}


app.UseAuthorization();

app.MapControllers();


app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
  Predicate = (check) => check.Tags.Contains("ready"),
  ResponseWriter = async (context, report) =>
  {
    var result = JsonSerializer.Serialize(
      new
      {
        status = report.Status.ToString(),
        checks = report.Entries.Select(entry => new
        {
          name = entry.Key,
          status = entry.Value.Status.ToString(),
          exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
          duration = entry.Value.Duration.ToString()
        })
      }
    );

    context.Response.ContentType = MediaTypeNames.Application.Json;
    await context.Response.WriteAsync(result);
  }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
  Predicate = (_) => false
});

app.Run();
