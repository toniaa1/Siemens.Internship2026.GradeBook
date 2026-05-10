using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _repository;

    public GradeService(IGradeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Grade?> GetActiveItemByIdAsync(int id)
    {
        if (id <=0)
        {
            return null;
        }
        return await _repository.GetByIdAsync(id);
        
    }
    
    public async Task<ItemListResponse> GetAllItemsWithStatisticsAsync()
    {
        var items = await _repository.GetAllAsync();
        var itemList = items.ToList();

        var totalCount = itemList.Count;
        var averageValue = itemList.Any() ? itemList.Average(i => i.Value) : 0;

        var stats = new ItemStatistics(totalCount, averageValue, DateTime.UtcNow);

        return new ItemListResponse(itemList, stats);
    }


    public async Task<IEnumerable<Grade>> GetTopPassingGradesAsync(int n)
    {
        if (n <= 0)
        {
            return Enumerable.Empty<Grade>();
        }

        var allActiveGrades = await _repository.GetAllAsync();

      
        var passingGrades = allActiveGrades
            .Where(g => g.Value >= 5 && g.IsActive)
            .Take(n);

        return passingGrades;
    }
}
