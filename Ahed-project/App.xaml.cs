using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
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

            UserEF active = null;
            using (var context = new EFContext())
            {
                active = context.Users.FirstOrDefault(x => x.IsActive);
            }
            if (active != null)
            {
                string email = active.Email;
                string password = active.Password;
                var result = jwt.AuthenticateUser(email, password);

                vm.FramePage = content;
            }
            else
            {
                vm.FramePage = login;
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
