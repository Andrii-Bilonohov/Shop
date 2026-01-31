namespace Application.Abstractions.Logger
{
    public interface IExceptionLogger
    {
        void Log(Exception exception, string message, params object[] args);
    }
}
