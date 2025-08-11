// Program.cs
using System;
using SOLIDRefactor;
class Program
{
    static void Main()
    {
        Report report = new Report
        {
            Title = "Sales Report",
            Content = "Data for Q2 2025"
        };

        IReportFormatter formatter = new PDFFormatter(); // or ExcelFormatter
        IReportGenerator generator = new ReportGenerator(formatter);
        IReportSaver saver = new ReportSaver();

        ReportService service = new ReportService(generator, saver);
        service.ProcessReport(report, "report_output.txt");

        Console.WriteLine("Report generated and saved successfully.");
    }
}
