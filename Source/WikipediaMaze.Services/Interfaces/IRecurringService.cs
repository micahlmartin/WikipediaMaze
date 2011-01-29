using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Services
{
    public interface IRecurringService
    {
        void StartService();
        void StopService();
        int Interval { get; set; }
    }
}
