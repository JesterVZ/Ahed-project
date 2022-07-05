using Ahed_project.Services;
using Ahed_project.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            var services = new ServiceCollection();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginPageViewModel>();
            services.AddTransient<ContentPageViewModel>();
            services.AddSingleton<PageService>();

            _provider = services.BuildServiceProvider();

            foreach(var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();

        public LoginPageViewModel LoginPageViewModel => _provider.GetRequiredService<LoginPageViewModel>();

        public ContentPageViewModel ContentPageViewModel => _provider.GetRequiredService<ContentPageViewModel>();

    }
}
