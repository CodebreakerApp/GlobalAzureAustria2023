using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Logging.AddConsole();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    //  options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.AddContext<AppJsonSerializerContext>();
});

//builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

//builder.Services.Configure<JsonOptions>(options =>
//{
//    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
//    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

builder.Services.AddSingleton<IGamesRepository, InMemoryGamesRepository>();
builder.Services.AddSingleton<GamesFactory>();
builder.Services.AddTransient<IGamesService, GamesService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGameEndpoints(app.Logger);

app.Run();

[JsonSerializable(typeof(Game<ColorField, ColorResult>))]
[JsonSerializable(typeof(Game[]))]
[JsonSerializable(typeof(ColorResult[]))]
[JsonSerializable(typeof(ColorField[]))]
[JsonSerializable(typeof(ShapeAndColorField[]))]
[JsonSerializable(typeof(ShapeAndColorResult[]))]
[JsonSerializable(typeof(CreateGameRequest))]
[JsonSerializable(typeof(CreateGameResponse))]
[JsonSerializable(typeof(SetMoveRequest))]
[JsonSerializable(typeof(SetMoveResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
