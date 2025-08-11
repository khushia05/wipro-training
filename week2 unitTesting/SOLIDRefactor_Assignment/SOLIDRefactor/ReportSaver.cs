using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLIDRefactor
{
    // ReportSaver.cs
    public interface IReportSaver
    {
        void Save(string content, string path);
    }

    public class ReportSaver : IReportSaver
    {
        public void Save(string content, string path)
        {
            File.WriteAllText(path, content);
        }
    }

}
