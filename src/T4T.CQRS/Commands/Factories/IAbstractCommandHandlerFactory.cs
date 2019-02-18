namespace T4T.CQRS.Commands.Factories
{
    /// <summary>
    /// An abstract factory that will return a concrete factory to create an <see cref="ICommandHandler{T}"/>.
    /// </summary>
    public interface IAbstractCommandHandlerFactory
    {
        ICommandHandlerFactory GetFactoryForCommand<T>()
            where T : class;
    }
}
