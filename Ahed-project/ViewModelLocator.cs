using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.ViewModel;
using AutoMapper;
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
            services.AddTransient<ProductsViewModel>();
            services.AddSingleton<SendDataService>();
            services.AddSingleton<PageService>();
            services.AddSingleton<SelectProjectService>();
            services.AddSingleton<Logs>();
            services.AddSingleton<WebClient>();
            services.AddSingleton<WindowService>();
            services.AddSingleton<JsonWebTokenLocal>();
            services.AddSingleton<SelectProductService>();
            services.AddSingleton<WindowTitleService>();

            //Маппер
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectInfoGet, ProjectInfoSend>()
                .ForMember(x => x.Id, s => s.MapFrom(o => o.project_id))
                .ForMember(x => x.CustomerReference, s => s.MapFrom(o => o.customer_reference))
                .ForMember(x => x.Customer, s => s.MapFrom(o => o.customer))
                .ForMember(x => x.Name, s => s.MapFrom(o => o.name))
                .ForMember(x => x.NumberOfDecimals, s => s.MapFrom(o => o.number_of_decimals))
                .ForMember(x => x.Revision, s => s.MapFrom(o => o.revision))
                .ForMember(x => x.Category, s => s.MapFrom(o => o.category))
                .ForMember(x => x.Comments, s => s.MapFrom(o => o.comments))
                .ForMember(x => x.Contact, s => s.MapFrom(o => o.contact))
                .ForMember(x => x.Description, s => s.MapFrom(o => o.description))
                .ForMember(x => x.Keywords, s => s.MapFrom(o => o.keywords))
                .ForMember(x => x.Units, s => s.MapFrom(o => o.units));
            });
            IMapper mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);

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
        public ProductsViewModel ProductsViewModel => _provider.GetRequiredService<ProductsViewModel>();

    }
}
