using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLIDRefactor
{
    // ReportService.cs
    public class ReportService
    {
        private readonly IReportGenerator _generator;
        private readonly IReportSaver _saver;

        public ReportService(IReportGenerator generator, IReportSaver saver)
        {
            _generator = generator;
            _saver = saver;
        }

        public void ProcessReport(Report report, string path)
        {
            string output = _generator.Generate(report);
            _saver.Save(output, path);
        }
    }

}
