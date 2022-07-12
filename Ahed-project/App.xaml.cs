using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ahed_project.Services.EF;
using Microsoft.EntityFrameworkCore;

namespace Ahed_project
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ViewModelLocator.Init();
            var db = new EFContext();
            db.Database.Migrate();
            base.OnStartup(e);
        }
    }
}
