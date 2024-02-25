namespace Core.Domain;

public class WeatherEntity : BaseEntity
{
    public required DateTime Date { get; init; }
    public required double Temperature  { get; init; }
    public required double RelativeHumidity  { get; init; }
    public required double Dewpoint  { get; init; }
    public required int AtmosphericPressure  { get; init; }
    public string? WindDirection  { get; init; }
    public int? WindSpeed  { get; init; }
    public int? Cloudiness  { get; init; }
    public int?  Cloudboundary  { get; init; }
    public string? HorizontalVisibility  { get; init; }
    public string? WeatherPhenomena  { get; init; }
}