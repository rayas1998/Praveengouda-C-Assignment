using EmployeeTimeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracker.Utils
{
    public static class HtmlReportGenerator
    {
        public static void GenerateHtmlTable(List<EmployeeWorkSummary> employees)
        {
            var html = new StringBuilder();

            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><title>Employee Work Hours</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial; background: #f5f5f5; margin: 20px; }");
            html.AppendLine("table { width: 80%; margin:auto; border-collapse: collapse; background: white; }");
            html.AppendLine("th, td { padding: 10px; border: 1px solid #ddd; text-align:left; }");
            html.AppendLine("th { background: #333; color: white; }");
            html.AppendLine(".low-hours { background-color: #ffe6e6; }");
            html.AppendLine("</style></head><body>");
            html.AppendLine("<h1 style='text-align:center;'>Employee Work Hours Summary</h1>");
            html.AppendLine($"<p style='text-align:center;'>Total Employees: {employees.Count} | Total Hours: {employees.Sum(e => e.TotalHours):F2}</p>");
            html.AppendLine("<table><tr><th>Employee Name</th><th>Total Hours Worked</th></tr>");

            foreach (var emp in employees)
            {
                var rowClass = emp.TotalHours < 100 ? "class='low-hours'" : "";
                html.AppendLine($"<tr {rowClass}><td>{emp.Name}</td><td>{emp.TotalHours:F2} hrs</td></tr>");
            }

            html.AppendLine("</table></body></html>");
            File.WriteAllText("employee_table.html", html.ToString());
        }
    }
}
