using Microsoft.Extensions.Logging;

namespace Distributed__Logging__System.Utalites
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }










 
        public static class ErrorHandler
        {
            // General error handling method
            public static void HandleError(ILogger logger, Exception ex, string message)
            {
                // Log the error
                logger.LogError(ex, message);

                // Optionally, throw a custom exception or propagate the original exception
                throw new ApplicationException(message, ex);
            }
        }
    

}
