using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLIDRefactor
{
    // PDFFormatter.cs
    public class PDFFormatter : IReportFormatter
    {
        public string Format(Report report) => $"PDF Report: {report.Title}\n{report.Content}";
    }

}
