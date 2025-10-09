using EmployeeTimeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeTimeTracker.Services
{
    public class TimeEntryService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

        public TimeEntryService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<TimeEntry>> FetchTimeEntriesAsync()
        {
            var response = await _httpClient.GetAsync(ApiUrl);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<TimeEntry>>(jsonString, options);
        }

        public List<EmployeeWorkSummary> ProcessEmployeeData(List<TimeEntry> timeEntries)
        {
            var employeeGroups = timeEntries
                .Where(e => !string.IsNullOrEmpty(e.EmployeeName))
                .GroupBy(e => e.EmployeeName);

            var result = new List<EmployeeWorkSummary>();

            foreach (var group in employeeGroups)
            {
                double totalHours = 0;
                int validEntries = 0;

                foreach (var entry in group)
                {
                    if (DateTime.TryParse(entry.StarTimeUtc, out var start) &&
                        DateTime.TryParse(entry.EndTimeUtc, out var end))
                    {
                        if (end > start)
                        {
                            totalHours += (end - start).TotalHours;
                            validEntries++;
                        }
                    }
                }

                if (validEntries > 0)
                {
                    result.Add(new EmployeeWorkSummary
                    {
                        Name = group.Key,
                        TotalHours = Math.Round(totalHours, 2)
                    });
                }
            }

            return result.OrderByDescending(x => x.TotalHours).ToList();
        }
    }
}
