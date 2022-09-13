using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Windows;
using Autofac;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Ahed_project
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterModule<Module>();
            var cont = builder.Build();
            var login = cont.Resolve<LoginPage>();
            var content = cont.Resolve<ContentPage>();
            var main = cont.Resolve<MainWindow>();
            var vm = cont.Resolve<MainViewModel>();
            var jwt = cont.Resolve<JsonWebTokenLocal>();

            UserEF active = null;
            using (var context = new EFContext())
            {
                active = context.Users.FirstOrDefault(x => x.IsActive);
            }
            if (active != null)
            {
                string email = active.Email;
                string password = active.Password;
                var result = await Task.Factory.StartNew(() => jwt.AuthenticateUser(email, password));

                vm.FramePage = content;
            } else
            {
                vm.FramePage = login;
            }
            main.Show();
        }

    }
}
