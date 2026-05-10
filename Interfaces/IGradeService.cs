using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Interfaces;


public record ItemStatistics(int TotalCount, decimal AverageValue, DateTime RetrievedAt);

public record ItemListResponse(IEnumerable<Grade> Data, ItemStatistics Statistics);

public interface IGradeService
{
    Task<Grade?> GetActiveItemByIdAsync(int id);
    Task<ItemListResponse> GetAllItemsWithStatisticsAsync();
    Task<IEnumerable<Grade>> GetTopPassingGradesAsync(int n);
}