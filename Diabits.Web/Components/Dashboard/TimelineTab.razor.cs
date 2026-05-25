using ApexCharts;

using Diabits.Web.Models;

namespace Diabits.Web.Components.Dashboard;

public partial class TimelineTab
{
    // TODO Move menstruation to be a value not on chart, but still visible since it's relevant for an entire day   
    private ApexChartOptions<TimelinePoint> _chartOptions => new()
    {
        Chart = new Chart
        {
            Zoom = new Zoom { AllowMouseWheelZoom = false },
            Height = "100%",
            Width = "100%",
            RedrawOnParentResize = false,
            Animations = new Animations
            {
                Enabled = false
            }
        },
        Legend = new Legend
        {
            Show = true,
            Position = LegendPosition.Top,
            HorizontalAlign = ApexCharts.Align.Left,
            FontSize = "14px",
        },
        DataLabels = new DataLabels { Enabled = false },
        Xaxis = new XAxis
        {
            Type = XAxisType.Datetime,
            Min = SelectedDate.HasValue ? new DateTimeOffset(SelectedDate.Value.Date).ToUnixTimeMilliseconds() : null,
            Max = SelectedDate.HasValue ? new DateTimeOffset(SelectedDate.Value.Date.AddDays(1).AddMinutes(-10)).ToUnixTimeMilliseconds() : null,
            Labels = new XAxisLabels
            {
                Show = true,
                Format = "HH:mm",
                DatetimeUTC = false
            },
            Tooltip = new AxisTooltip { Enabled = false }

        },
        Yaxis = new List<YAxis>
        {
            new YAxis { Min = 0, Max = 1, Show = false, SeriesName = "Menstruation" },
            new YAxis { Min = 0, Max = 1, Show = false, SeriesName = "Sleep" },
            new YAxis { Min = 0, Max = 1, Show = false, SeriesName = "Workout" },

            new YAxis
            {
                Opposite = true,
                Min = 40,
                Max = 180,
                DecimalsInFloat = 0,
                Title = new AxisTitle { Text = "Heart Rate (bpm)" },
                SeriesName = "Heart Rate",
            },

            new YAxis { Min = 0, Max = 1, Show = false, SeriesName = "Medication" },

            new YAxis
            {
                Show = false,
                Min = 0,
                Max = 40,
                DecimalsInFloat = 0,
                Title = new AxisTitle { Text = "Insulin (U)" },
                SeriesName = "Insulin Bolus"
            },

            new YAxis
            {
                Min = 0,
                Max = 25,
                DecimalsInFloat = 1,
                Title = new AxisTitle { Text = "Glucose Level (mmol/L)" },
                SeriesName = "Glucose Level"
            }
        },
        Stroke = new Stroke
        {
            Width = new List<double> { 0, 0, 0, 3, 0, 0, 3 },
            Curve = Curve.Smooth
        },
        Colors = ["#9da4a8", "#91B3C9", "#D4C9D8", "#e27396", "#47565f", "#670000", "#AC0000"],
        Fill = new Fill
        {
            Opacity = new List<double> { 0.3, 0.8, 0.9, 1, 1, 1, 1 },
        },
        Markers = new Markers
        {
            Size = new List<double> { 0, 0, 0, 1, 8, 0, 1 }
        },
        Tooltip = new Tooltip
        {
            Enabled = true,
            Shared = true,
            FollowCursor = true,
            Intersect = false,
            X = new TooltipX { Show = false }
        }
    };
}
