using T4T.CQRS.Commands;

namespace T4T.CQRS.Extensions
{
    public static class ICommandHandlerExtensions
    {
        public static ICommandHandler<T> WithExceptionHandling<T>(this ICommandHandler<T> commandHandler)
            where T : class
        {
            return new ExceptionHandlingCommandHandler<T>(commandHandler);
        }
    }
}
