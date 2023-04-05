using Serilog;
using System.IO;

namespace EPubGenerator.Infrastructure
{
    public  class Logger
    {
        public  Serilog.Core.Logger Log { get; }

        public Logger(string dirPath)
        {
           
            Log = new LoggerConfiguration().WriteTo.File(Path.Combine(dirPath, "log.txt")).CreateLogger();
        }

        
    }
}
