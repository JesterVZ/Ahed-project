using Ahed_project.MasterData;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Windows;
using Autofac;
using System.Linq;
using System.Windows;

namespace Ahed_project
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _containers;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterModule<Module>();
            _containers = builder.Build();
            var login = _containers.Resolve<LoginPage>();
            var content = _containers.Resolve<ContentPage>();
            var main = _containers.Resolve<MainWindow>();
            var vm = _containers.Resolve<MainViewModel>();
            var jwt = _containers.Resolve<JsonWebTokenLocal>();
            Application.Current.Exit += new ExitEventHandler((_, _) => GlobalFunctionsAndCallersService.AskAndSave());
            UserEF active = null;
            using (var context = new EFContext())
            {
                active = context.Users.FirstOrDefault(x => x.IsActive);
            }
            vm.FramePage = login;
            if (active != null)
            {
                var result = jwt.TryAuthenticateByToken(active.Token);
                if (result != null)
                {
                    GlobalDataCollectorService.UserId = active.Id;
                    vm.FramePage = content;
                }
            }
            main.Show();
        }

        public static void Refresh()
        {
            var main = _containers.Resolve<MainWindow>();
            main.Show();
            App.Current.Windows[0].Close();
        }
    }
}
