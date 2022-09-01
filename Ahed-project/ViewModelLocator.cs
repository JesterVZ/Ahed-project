using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel;
using Ahed_project.ViewModel.ContentPageComponents;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

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
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginPageViewModel>();
            services.AddSingleton<ContentPageViewModel>();
            services.AddSingleton<PresetsWindowViewModel>();
            services.AddSingleton<ProjectsWindowViewModel>();
            services.AddSingleton<GeometryWindowViewModel>();
            services.AddSingleton<GeometryPageViewModel>();
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<ProjectPageViewModel>();
            services.AddSingleton<HeatBalanceViewModel>();
            services.AddSingleton<TubesFluidViewModel>();
            services.AddSingleton<ShellFluidViewModel>();
            services.AddSingleton<SendDataService>();
            services.AddSingleton<PageService>();
            services.AddSingleton<WebClient>();
            services.AddSingleton<WindowService>();
            services.AddSingleton<JsonWebTokenLocal>();
            services.AddSingleton<WindowTitleService>();
            services.AddSingleton<GlobalDataCollectorService>();
            services.AddSingleton<GlobalFunctionsAndCallersService>();

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
        public ContentPageViewModel ContentPageViewModel => _provider.GetRequiredService<ContentPageViewModel>();
        public LoginPageViewModel LoginPageViewModel => _provider.GetRequiredService<LoginPageViewModel>();
        public PresetsWindowViewModel PresetsWindowViewModel => _provider.GetRequiredService<PresetsWindowViewModel>();
        public ProjectsWindowViewModel ProjectsWindowViewModel => _provider.GetRequiredService<ProjectsWindowViewModel>();
        public ProductsViewModel ProductsViewModel => _provider.GetRequiredService<ProductsViewModel>();
        public GeometryWindowViewModel GeometryWindowViewModel => _provider.GetRequiredService<GeometryWindowViewModel>();
        public GeometryPageViewModel GeometryPageViewModel => _provider.GetRequiredService<GeometryPageViewModel>();
        public ProjectPageViewModel ProjectPageViewModel => _provider.GetRequiredService<ProjectPageViewModel>();
        public HeatBalanceViewModel HeatBalanceViewModel => _provider.GetRequiredService<HeatBalanceViewModel>();
        public TubesFluidViewModel TubesFluidViewModel => _provider.GetRequiredService<TubesFluidViewModel>();
        public ShellFluidViewModel ShellFluidViewModel => _provider.GetRequiredService<ShellFluidViewModel>();
        // Глобильные сервисы
        public GlobalDataCollectorService GlobalDataCollectorService => _provider.GetRequiredService<GlobalDataCollectorService>();
        public GlobalFunctionsAndCallersService StartUpService => _provider.GetRequiredService<GlobalFunctionsAndCallersService>();
    }
}
