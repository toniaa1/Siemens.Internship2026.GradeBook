using Microsoft.AspNetCore.Mvc;
using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Services;

namespace Siemens.Internship2026.GradeBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;
    private readonly ILogger<GradeController> _logger;
    public GradeController(IGradeService gradeService, ILogger<GradeController> logger)
    {
        _gradeService = gradeService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET api/grade called");

        var result = await _gradeService.GetAllItemsWithStatisticsAsync();

        return Ok(result);
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET api/grade/{Id} called", id);

        if (id <= 0)
        {
            _logger.LogWarning("Invalid id provided: {Id}", id);
            return BadRequest("Id must be a positive integer.");
        }

        var item = await _gradeService.GetActiveItemByIdAsync(id);

        if (item == null)
        {
            return NotFound($"Item with Id {id} was not found.");
        }

        return Ok(item);
    }

    [HttpGet("passing")]
    public async Task<IActionResult> GetPassingGrades([FromQuery] int n)
    {
        _logger.LogInformation("GET api/grade/passing called with N={N}", n);

        if (n <= 0)
        {
            return BadRequest("Parameter 'n' must be greater than zero.");
        }

        var result = await _gradeService.GetTopPassingGradesAsync(n);

        return Ok(result);
    }
}
