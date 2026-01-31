using Application.Abstractions.Logger;

namespace Infrastructure.Logger
{
    public class ExceptionLogger : IExceptionLogger
    {
        public void Log(Exception ex, string message, params object[] args)
        {
            if (message != null)
                Serilog.Log.Error(ex, message, args);
            else
                Serilog.Log.Error(ex, "An exception occurred");
        }
    }
}
