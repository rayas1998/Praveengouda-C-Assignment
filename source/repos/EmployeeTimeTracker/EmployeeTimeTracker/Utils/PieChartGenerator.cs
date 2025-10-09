using EmployeeTimeTracker.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracker.Utils
{
    public static class PieChartGenerator
    {
        public static void GeneratePieChart(List<EmployeeWorkSummary> employees)
        {
            if (employees == null || employees.Count == 0)
                return;

            int width = 600;
            int height = 600 + employees.Count * 25; 
            int radius = 250;

            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            double totalHours = employees.Sum(e => e.TotalHours);
            double startAngle = 0;

            var colors = new SKColor[]
            {
                SKColors.Red, SKColors.Blue, SKColors.Yellow, SKColors.Green, SKColors.Purple,
                SKColors.Orange, SKColors.Cyan, SKColors.Magenta, SKColors.Brown, SKColors.LightGray
            };

            int colorIndex = 0;

            var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var rect = new SKRect(width / 2 - radius, radius / 2, width / 2 + radius, radius * 2 + radius / 2);
            foreach (var emp in employees)
            {
                double sweepAngle = emp.TotalHours / totalHours * 360;
                paint.Color = colors[colorIndex % colors.Length];
                canvas.DrawArc(rect, (float)startAngle, (float)sweepAngle, true, paint);
                startAngle += sweepAngle;
                colorIndex++;
            }

            var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 18,
                IsAntialias = true
            };

            int legendX = 50;
            int legendY = radius * 2 + radius / 2 + 20;
            colorIndex = 0;

            foreach (var emp in employees)
            {
                paint.Color = colors[colorIndex % colors.Length];
                canvas.DrawRect(new SKRect(legendX, legendY - 15, legendX + 20, legendY + 5), paint);
                canvas.DrawText($"{emp.Name} ({emp.TotalHours:F2} hrs)", legendX + 25, legendY, textPaint);
                legendY += 25;
                colorIndex++;
            }
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            File.WriteAllBytes("employee_pie_chart.png", data.ToArray());
        }
    }
}
