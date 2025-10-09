using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeTimeTracker.Models;
using EmployeeTimeTracker.Services;
using EmployeeTimeTracker.Utils;

namespace EmployeeTimeTracker
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching data...");

            var service = new TimeEntryService();
            var entries = await service.FetchTimeEntriesAsync();

            if (entries == null || entries.Count == 0)
            {
                Console.WriteLine("No data retrieved from API.");
                return;
            }

            Console.WriteLine($"Fetched {entries.Count} entries.");
            var summary = service.ProcessEmployeeData(entries);

            HtmlReportGenerator.GenerateHtmlTable(summary);
            Console.WriteLine("HTML Report Generated (employee_table.html)");

            PieChartGenerator.GeneratePieChart(summary);
            Console.WriteLine("Pie Chart Generated (employee_pie_chart.png)");

            Console.WriteLine("Completed successfully!");
        }
    }
}
