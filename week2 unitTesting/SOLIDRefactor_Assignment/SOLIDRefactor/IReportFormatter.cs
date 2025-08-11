using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLIDRefactor
{
    // IReportFormatter.cs
    public interface IReportFormatter
    {
        string Format(Report report);
    }

}
