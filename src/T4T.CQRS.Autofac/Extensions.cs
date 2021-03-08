using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Queries.Factories;

namespace T4T.CQRS.Autofac
{
    public static class Extensions
    {
        public static void RegisterDefaultFactoryForCommandsInAssembly<TFactory>(
            this ContainerBuilder builder, 
            Assembly assembly, 
            string @namespace = null)
            where TFactory : ICommandHandlerFactory
        {
            var types = GetTypesFromAssemblyAndNamespace(assembly, @namespace);

            foreach (var type in types)
                builder.RegisterType<TFactory>().Keyed<ICommandHandlerFactory>(type);
        }

        public static void RegisterDefaultFactoryForQueriesInAssembly<TFactory>(
            this ContainerBuilder builder, 
            Assembly assembly, 
            string @namespace = null)
            where TFactory : IQueryHandlerFactory
        {
            var types = GetTypesFromAssemblyAndNamespace(assembly, @namespace);

            foreach (var type in types)
                builder.RegisterType<TFactory>().Keyed<IQueryHandlerFactory>(type);
        }

        internal static IEnumerable<Type> GetTypesFromAssemblyAndNamespace(Assembly assembly, string @namespace = null)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.IsClass);

            if (!string.IsNullOrEmpty(@namespace))
                types = types.Where(t => t.Namespace == @namespace);

            return types;
        }
    }
}
