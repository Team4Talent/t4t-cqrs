using System.Reflection;
using Autofac;
using T4T.CQRS.Commands;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Queries;
using T4T.CQRS.Queries.Factories;
using Module = Autofac.Module;

namespace T4T.CQRS.Autofac
{
    /// <summary>
    ///     Register all the <see cref="ICommandHandler{T}"/>s and <see cref="IQueryHandler{T4T}"/>s in an assembly.
    /// </summary>
    public class CQRSModule : Module
    {
        private readonly Assembly[] _assemblies;

        /// <summary>
        ///     Register all the handlers in the calling assembly (<code>Assembly.GetCallingAssembly()</code>.
        /// </summary>
        public CQRSModule()
            : this(Assembly.GetCallingAssembly())
        {
            
        }

        /// <summary>
        ///     Register all the handlers in the given assemblies. Also registers the <see cref="AbstractCommandHandlerFactory"/> and <see cref="AbstractQueryHandlerFactory"/>.
        /// </summary>
        /// <param name="handlersAssembly">The assemblies where all the handlers are located.</param>
        public CQRSModule(params Assembly[] handlersAssemblies)
        {
            _assemblies = handlersAssemblies;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assemblies)
                .AsClosedTypesOf(typeof(ICommandHandler<>));

            builder.RegisterAssemblyTypes(_assemblies)
                .AsClosedTypesOf(typeof(IQueryHandler<,>));

            builder.RegisterType<AbstractCommandHandlerFactory>()
                .As<IAbstractCommandHandlerFactory>();

            builder.RegisterType<AbstractQueryHandlerFactory>()
                .As<IAbstractQueryHandlerFactory>();
        }
    }
}
