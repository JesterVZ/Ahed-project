using Ahed_project.Services.EF;
using Ahed_project.Services.Global;
using Ahed_project.Services;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Ahed_project.ViewModel.Windows;
using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Channels;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Windows;
using System.Windows.Navigation;
using Ahed_project.ViewModel;

namespace Ahed_project
{
    internal class Module: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterTypes(builder);
        }
        protected void RegisterTypes(Autofac.ContainerBuilder builder)
        {
            using (var db = new EFContext())
                db.Database.Migrate();
            var config = ServiceConfig.GetServiceConfig();
            builder.RegisterInstance(config).As<ServiceConfig>().SingleInstance();
            #region ViewModel
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LoginPageViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ContentPageViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<PresetsWindowViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<OverallCalculationViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ProjectsWindowViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<GeometryWindowViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<GeometryPageViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ProductsViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ProjectPageViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<HeatBalanceViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<TubesFluidViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ShellFluidViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<BufflesPageViewModel>().AsSelf().SingleInstance();
            #endregion
            #region Pages
            builder.RegisterType<_3DPage>().AsSelf().SingleInstance();
            builder.RegisterType<BafflesPage>().AsSelf().SingleInstance();
            builder.RegisterType<ContentPage>().AsSelf().SingleInstance();
            builder.RegisterType<BatchPage>().AsSelf().SingleInstance();
            builder.RegisterType<GeometryPage>().AsSelf().SingleInstance();
            builder.RegisterType<GraphsPage>().AsSelf().SingleInstance();
            builder.RegisterType<HeatBalancePage>().AsSelf().SingleInstance();
            builder.RegisterType<LoginPage>().AsSelf().SingleInstance();
            builder.RegisterType<OverallCalculationPage>().AsSelf().SingleInstance();
            builder.RegisterType<ProjectPage>().AsSelf().SingleInstance();
            builder.RegisterType<QuotePage>().AsSelf().SingleInstance();
            builder.RegisterType<ReportsPage>().AsSelf().SingleInstance();
            builder.RegisterType<ShellFluidPage>().AsSelf().SingleInstance();
            builder.RegisterType<TubesFluidPage>().AsSelf().SingleInstance();
            #endregion
            #region Windows
            builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
            builder.RegisterType<GeometryWindow>().AsSelf();
            builder.RegisterType<Presets>().AsSelf();
            builder.RegisterType<ProductsWindow>().AsSelf();
            builder.RegisterType<ProjectsWindow>().AsSelf();
            #endregion
            #region Services
            builder.RegisterType<SendDataService>().AsSelf().SingleInstance();
            builder.Register(x=>new PageService(x.Resolve<IComponentContext>())).As<PageService>().SingleInstance();
            builder.RegisterType<WebClient>().AsSelf().SingleInstance();
            builder.RegisterType<JsonWebTokenLocal>().AsSelf().SingleInstance();
            builder.RegisterType<WindowTitleService>().AsSelf().SingleInstance();
            var data = new GlobalDataCollectorService();
            builder.RegisterInstance(data).As<GlobalDataCollectorService>().SingleInstance();
            #endregion
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
            builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();

            builder.RegisterType<GlobalFunctionsAndCallersService>().AsSelf().SingleInstance().AutoActivate();
        }
    }
}
