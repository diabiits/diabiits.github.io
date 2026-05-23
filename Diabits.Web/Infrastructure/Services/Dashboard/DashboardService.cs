using Diabits.Web.Infrastructure.Api;
using Diabits.Web.Models;
using System.Globalization;

namespace Diabits.Web.Infrastructure.Services.Dashboard;

public class DashboardService(ApiClient apiClient)
{
    public async Task<TimelineChartResponse> GetTimelineAsync(DateTime date)
    {
        //TODO formatting date?
        var dateStr = date.ToString("yyyy-MM-ddTHH:mm:ss",  CultureInfo.InvariantCulture);

        return await apiClient.GetAsync<TimelineChartResponse>($"Dashboard/timeline?date={dateStr}")
            ?? throw new HttpRequestException("No data returned");
    }

    public async Task<DailyGlucoseResponse> GetDailyGlucoseAsync(DateTime date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");

        return await apiClient.GetAsync<DailyGlucoseResponse>($"Dashboard/glucose/daily?date={dateStr}")
            ?? throw new HttpRequestException("No data returned");
    }
}