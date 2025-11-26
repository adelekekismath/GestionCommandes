namespace Api.Application.Services.Dashboard;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetKPIsAsync();
}