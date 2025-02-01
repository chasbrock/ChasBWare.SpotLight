using System;
using Microsoft.Extensions.DependencyInjection;

namespace ChasBWare.Spotlight.Tests
{
    internal abstract class BaseTestEnvironment
    {
        protected readonly ServiceProvider _serviceProvider;

        protected BaseTestEnvironment() 
        {
            var services = new ServiceCollection();
            services.AddSingleton<IServiceProvider>(sp => sp);
            InitialiseDI(services);

            _serviceProvider= services.BuildServiceProvider();
        } 

        protected abstract void InitialiseDI(IServiceCollection serviceCollection);
     
    }
}
