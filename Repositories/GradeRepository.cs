using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GradeRepository> _logger;

    public GradeRepository(HttpClient httpClient, ILogger<GradeRepository> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all grades from endpoint.");
            var rawJson = await _httpClient.GetStringAsync("");
            var jsonTree = JsonNode.Parse(rawJson);
            var itemsArray = jsonTree?["items"];

            if (itemsArray != null)
            {
                var grades = itemsArray.Deserialize<IEnumerable<Grade>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });

                return grades ?? Enumerable.Empty<Grade>();
            }

            return Enumerable.Empty<Grade>();
        }

        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "An error occurred while fetching grades.");
            return Enumerable.Empty<Grade>();
        }

        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Failed to parse the JSON data.");
            return Enumerable.Empty<Grade>();
        }
    }
    public virtual async Task<Grade?> GetByIdAsync(int id)
    {

        var allGrades = await GetAllAsync();
        var specificGrade = allGrades.FirstOrDefault(g => g.Id == id && g.IsActive);
        return specificGrade;
    }

    
}
