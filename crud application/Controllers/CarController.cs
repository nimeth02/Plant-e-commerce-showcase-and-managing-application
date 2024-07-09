using crud_application.Models;
using Microsoft.AspNetCore.Mvc;

public class CarController : ControllerBase
{
    private readonly CarService _carService;
    private readonly ILogger<CarController> _logger;

    public CarController(CarService carService, ILogger<CarController> logger)
    {
        _carService = carService;
        _logger = logger;
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetCars()
    {
        var cars = await _carService.GetCars();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCar(string id)
    {
        _logger.LogInformation($"GetCar request received for id:");
        var car = await _carService.GetCar(id);
        if (car == null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    [HttpPost("createCar")]
    public async Task<IActionResult> CreateCar([FromBody] CarModel car)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _carService.CreateCar(car);
        return Ok(car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(string id, [FromBody] CarModel car)
    {
        if (id != car.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _carService.UpdateCar(id, car);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(string id)
    {
        await _carService.DeleteCar(id);
        return NoContent();
    }
}
