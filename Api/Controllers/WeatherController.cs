using Api.Services;
using Core.Domain;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class WeatherController(WeatherRepository weatherRepository, ExcelReader excelReader) : ControllerBase
    {
        [HttpGet("first")]
        public Task<IEnumerable<WeatherEntity?>> GetFirstNWeathers(int takeValue)
        {
            return weatherRepository.GetMultipleWeatherEntitiesAsync(takeValue);
        }

        [HttpGet("withPagination")]
        public Task<IEnumerable<WeatherEntity?>> GetWithPagination(int pageNumber, int pageSize)
        {
            return weatherRepository.GetMultipleWeatherEntitiesByPageAsync(pageNumber, pageSize);
        }

        [HttpGet("byYear")]
        public Task<IEnumerable<WeatherEntity?>> GetByYear(int pageNumber, int pageSize, int year)
        {
            return weatherRepository.GetMultipleWeatherEntitiesByYearAsync(pageNumber, pageSize, year);
        }

        [HttpGet("byMonth")]
        public Task<IEnumerable<WeatherEntity?>> GetByMonth(int pageNumber, int pageSize, int month)
        {
            return weatherRepository.GetMultipleWeatherEntitiesByMonthAsync(pageNumber, pageSize, month);
        }

        [HttpPost("create-weather")]
        public async Task<ActionResult> PostCreateWeather()
        {
            var entity = new WeatherEntity
            {
                Id = Guid.NewGuid(),
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc),
                Temperature = 0,
                RelativeHumidity = 0,
                Dewpoint = 0,
                AtmosphericPressure = 0,
                Cloudboundary = 0,
                HorizontalVisibility = "",
            };
            await weatherRepository.CreateNewEntityAsync(entity);
            return Ok();
        }

        [HttpPost("load-weather")]
        public async Task<ActionResult> PostLoadWeather(IFormFileCollection files)
        {
            if (!files.Any())
            {
                var formCollection = await Request.ReadFormAsync();// Костыльное решение
                files = formCollection.Files;
            }

            if (files.Any(f => f.Length == 0))
            {
                return BadRequest();
            }

            var res = await excelReader.ReadAllFiles(files);

            return Ok(new { message = $"{res}. All {files.Count} the files are successfully uploaded." });
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete()
        {
            int deletedCount = await weatherRepository.DeleteAllAsync();
            return Ok(deletedCount);
        }
    }
}