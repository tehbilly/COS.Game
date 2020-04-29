using System;
using ISerilogLogger = Serilog.ILogger;

namespace COS.Game
{
    public interface ILogger
    {
        ILogger ForContext(Type source);
        void Debug(string messageTemplate);
        void Info(string messageTemplate);
        void Warn(string messageTemplate);
        void Error(string messageTemplate);
        void Fatal(string messageTemplate);
    }

    internal class SerilogLogger : ILogger
    {
        private readonly ISerilogLogger _logger;

        public SerilogLogger(ISerilogLogger logger)
        {
            _logger = logger;
        }

        public ILogger ForContext(Type source)
        {
            return new SerilogLogger(_logger.ForContext(source));
        }

        public void Debug(string messageTemplate)
        {
            _logger.Debug(messageTemplate);
        }

        public void Info(string messageTemplate)
        {
            _logger.Information(messageTemplate);
        }

        public void Warn(string messageTemplate)
        {
            _logger.Warning(messageTemplate);
        }

        public void Error(string messageTemplate)
        {
            _logger.Error(messageTemplate);
        }

        public void Fatal(string messageTemplate)
        {
            _logger.Fatal(messageTemplate);
        }
    }
}