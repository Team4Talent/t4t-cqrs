namespace T4T.CQRS.Commands
{
    public interface ICommandHandlerFactory
    {
        /// <summary>
        /// Creates an <see cref="ICommandHandler{T}"/> for the given command <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command.</typeparam>
        /// <returns>An <see cref="ICommandHandler{T}"/> for type <typeparamref name="T"/>.</returns>
        ICommandHandler<T> CreateCommandHandler<T>()
            where T : class;
    }
}
