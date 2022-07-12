using Ahed_project.Services;
using Ahed_project.ViewModel;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<PresetsWindowViewModel>();
            services.AddSingleton<PageService>();
            services.AddSingleton<Logs>();
            services.AddSingleton<WindowService>();
            services.AddSingleton(x=>new JsonWebTokenLocal(ServiceConfig.GetServiceConfig()));
            services.AddSingleton(x => new SendDataService(ServiceConfig.GetServiceConfig()));

            _provider = services.BuildServiceProvider();

            foreach(var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();

        public LoginPageViewModel LoginPageViewModel => _provider.GetRequiredService<LoginPageViewModel>();

        public ContentPageViewModel ContentPageViewModel => _provider.GetRequiredService<ContentPageViewModel>();

        public PresetsWindowViewModel PresetsWindowViewModel => _provider.GetRequiredService<PresetsWindowViewModel>();

    }
}
