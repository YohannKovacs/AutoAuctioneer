﻿using API.Models.RequestModels;
using API.Services.CarService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _carService.GetAllCars();
        return Ok(response);
    }

    [HttpGet("getowned")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetOwned([FromQuery] Guid id)
    {
        var response = await _carService.GetOwnedCars(id);
        return Ok(response);
    }

    [HttpGet("getbyid")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var response = await _carService.GetCarById(id);
        return Ok(response);
    }

    [HttpPost("store")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Store([FromBody] AddCarRequest request)
    {
        var response = await _carService.StoreCar(request);
        if (response) return Ok(response);

        return BadRequest(response);
    }

    [HttpPut("update")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Update([FromBody] UpdateCarRequest request)
    {
        var response = await _carService.UpdateCar(request);
        if (response) return Ok(response);

        return BadRequest(response);
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> DeleteCar([FromBody] DeleteCarRequest request)
    {
        var response = await _carService.DeleteCar(request);
        if (response) return Ok(response);
        return BadRequest(response);
    }
}