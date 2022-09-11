using Ahed_project.Pages;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Windows;
using Autofac;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace Ahed_project
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterModule<Module>();
            var cont = builder.Build();
            var login = cont.Resolve<LoginPage>();
            var main = cont.Resolve<MainWindow>();
            var vm = cont.Resolve<MainViewModel>();
            vm.FramePage = login;
            main.Show();
        }
    }
}
