namespace Diabits.Web.Models;


public record DailyGlucoseResponse(
    List<DailyGlucoseReading> Readings,
    List<DailyBucketPoint> Buckets,
    DailyGlucoseStats Stats,
    DailyGlucoseRanges Ranges,
    List<DailyRangePoint> WeeklyRange
);

public record DailyGlucoseReading(DateTime Time, double Value);
public record DailyBucketPoint(DateTime Time, double? Value);
public record DailyGlucoseStats(double Average, double Min, double Max, int Count);
public record DailyGlucoseRanges(double VeryLow, double Low, double InRange, double High, double VeryHigh);
public record DailyRangePoint(DateTime Time, double? Min, double? Max);

public record GlucosePoint(DateTime Time, double? Value, double? Min, double? Max);

public class RangePercentageData
{
    public string Category { get; set; } = string.Empty;
    public double Percentage { get; set; }
}