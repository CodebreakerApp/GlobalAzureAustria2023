using System.Text.Json.Serialization;

namespace BooksSample;

[JsonSerializable(typeof(Book))]
[JsonSerializable(typeof(WeatherForecast))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
