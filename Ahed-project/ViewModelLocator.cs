using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            using (var db = new EFContext())
                db.Database.Migrate();
            var services = new ServiceCollection();
            services.AddSingleton(x => ServiceConfig.GetServiceConfig());
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginPageViewModel>();
            services.AddTransient<ContentPageViewModel>();
            services.AddTransient<PresetsWindowViewModel>();
            services.AddTransient<ProjectsWindowViewModel>();
            services.AddSingleton<SendDataService>();
            services.AddSingleton<PageService>();
            services.AddSingleton<SelectProjectService>();
            services.AddSingleton<Logs>();
            services.AddSingleton<WebClient>();
            services.AddSingleton<WindowService>();
            services.AddSingleton(x => new JsonWebTokenLocal(_provider.GetRequiredService<ServiceConfig>(), _provider.GetRequiredService<SendDataService>()));
            services.AddSingleton(x => new SendDataService(_provider.GetRequiredService<ServiceConfig>()));

            _provider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();

        public LoginPageViewModel LoginPageViewModel => _provider.GetRequiredService<LoginPageViewModel>();

        public ContentPageViewModel ContentPageViewModel => _provider.GetRequiredService<ContentPageViewModel>();

        public PresetsWindowViewModel PresetsWindowViewModel => _provider.GetRequiredService<PresetsWindowViewModel>();
        public ProjectsWindowViewModel ProjectsWindowViewModel => _provider.GetRequiredService<ProjectsWindowViewModel>();

    }
}
