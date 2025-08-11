using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLIDRefactor
{
    // ReportGenerator.cs
    public interface IReportGenerator
    {
        string Generate(Report report);
    }

    public class ReportGenerator : IReportGenerator
    {
        private readonly IReportFormatter _formatter;

        public ReportGenerator(IReportFormatter formatter)
        {
            _formatter = formatter;
        }

        public string Generate(Report report)
        {
            return _formatter.Format(report);
        }
    }

}
