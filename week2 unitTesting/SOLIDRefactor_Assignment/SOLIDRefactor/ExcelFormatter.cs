using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLIDRefactor
{
    // ExcelFormatter.cs
    public class ExcelFormatter : IReportFormatter
    {
        public string Format(Report report) => $"Excel Report: {report.Title}\n{report.Content}";
    }

}
