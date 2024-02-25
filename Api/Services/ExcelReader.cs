using Core.Abstraction;
using Core.Domain;
using DataAccess.Repositories;
using NPOI.XSSF.UserModel;

namespace Api.Services;

public class ExcelReader(WeatherRepository repository)
{
    public async Task<string> ReadAllFiles(IFormFileCollection fileCollection)
    {
        var tasks = fileCollection.Select(ReadFile);
        var allEntities = await Task.WhenAll(tasks);
        try
        {
            foreach (var weatherEntities in allEntities) await repository.AddNewEntitiesAsync(weatherEntities);
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка сохранения в базу:" + e);
            throw;
        }
        
        return "Success";
    }

    private Task<IEnumerable<WeatherEntity>> ReadFile(IFormFile file)
    {
        try
        {
            lock (file)
            {
                var entities = new List<WeatherEntity>(3000);
                XSSFWorkbook workbook;
                using (var fileStream = file.OpenReadStream())
                {
                    workbook = new XSSFWorkbook(fileStream);
                }

                // Получение листа
                for (var i = 0; i < workbook.Count; i++)
                {
                    var sheet = workbook.GetSheetAt(i);
                    for (var j = 4; j < sheet.LastRowNum; j++)
                    {
                        var row = sheet.GetRow(j);

                        var stringDate = row.GetCell(0).StringCellValue.Split(".");
                        var stringHours = row.GetCell(1).StringCellValue.Split(":");

                        var date = new DateTime(int.Parse(stringDate[2]), int.Parse(stringDate[1]),
                            int.Parse(stringDate[0]),
                            int.Parse(stringHours[0]), int.Parse(stringHours[1]), 0, DateTimeKind.Utc);

                        var temperature = Convert.ToDouble(row.GetCell(2).ToString());

                        var relativeHumidity = Convert.ToDouble(row.GetCell(3).ToString());

                        var dewpoint = Convert.ToDouble(row.GetCell(4).ToString());

                        var atmosphericPressure = Convert.ToInt32(row.GetCell(5).ToString());

                        var windDirection = row.GetCell(6).StringCellValue;

                        var windSpeed = CellConverterToNullableInt(row.GetCell(7).ToString());

                        var cloudiness = CellConverterToNullableInt(row.GetCell(8).ToString());

                        var cloudBoundary = CellConverterToNullableInt(row.GetCell(9).ToString());

                        var horizontalVisibility = row.GetCell(10).ToString();

                        var weatherPhenomena = row.GetCell(11) is null ? null : row.GetCell(11).ToString();

                        var entity = new WeatherEntity
                        {
                            Date = date,
                            Temperature = temperature,
                            RelativeHumidity = relativeHumidity,
                            Dewpoint = dewpoint,
                            AtmosphericPressure = atmosphericPressure,
                            Cloudboundary = cloudBoundary,
                            WindDirection = windDirection,
                            WindSpeed = windSpeed,
                            Cloudiness = cloudiness,
                            HorizontalVisibility = horizontalVisibility,
                            WeatherPhenomena = weatherPhenomena,
                            Id = Guid.NewGuid(),
                        };
                        entities.Add(entity);
                    }
                }

                return Task.FromResult<IEnumerable<WeatherEntity>>(entities);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка чтенеия файла: " + e);
            throw;
        }
    }

    private static int? CellConverterToNullableInt(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : Convert.ToInt32(value);
    }
}