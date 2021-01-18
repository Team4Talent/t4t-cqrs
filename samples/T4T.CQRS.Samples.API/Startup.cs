using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using T4T.CQRS.Autofac;
using T4T.CQRS.Queries.Factories;
using T4T.CQRS.Samples.API.Queries;
using T4T.CQRS.Samples.API.Queries.Factories;

namespace T4T.CQRS.Samples.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Automatically register all the ICommandHandlers and IQueryHandlers in the assembly.
            // You can specify other assemblies as well: new CQRSModule(assembly1, assembly2);
            builder.RegisterModule(new CQRSModule());

            // Control how your handlers are constructed using specific factories.
            // If not, the CQRSModule will wrap your handler with an ExceptionHandlingQueryHandler and a LoggingQueryHandler.
            // The same goes for commands.
            builder.RegisterType<GetTodosQueryHandlerFactory>()
                .Keyed<IQueryHandlerFactory>(typeof(GetTodosQuery));
        }
    }
}
