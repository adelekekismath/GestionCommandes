using Api.Application.Services.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    [HttpGet("kpis")]
    public async Task<ActionResult<DashboardStatsDto>> GetKPIs()
    {
        var kpis = await _dashboardService.GetKPIsAsync();
        return Ok(kpis);
    }
}