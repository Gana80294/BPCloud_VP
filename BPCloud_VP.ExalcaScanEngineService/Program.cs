using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BPCloud_VP.ExalcaScanEngineService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
    
        private static void Main() => ServiceBase.Run(new ServiceBase[1]
        {
          (ServiceBase) new Service1()
        });
    }
}
