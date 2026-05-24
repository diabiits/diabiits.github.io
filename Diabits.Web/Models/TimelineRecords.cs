namespace Diabits.Web.Models;

public record TimelineChartResponse(List<TimelineSeries> Series);
public record TimelineSeries(string Name, TimelineSeriesType Type, List<TimelinePoint> Points);
public record TimelinePoint(DateTime Time, double? Value, string? Name = null);

public enum TimelineSeriesType
{
    Line,
    Area,
    Scatter,
    Bar
}