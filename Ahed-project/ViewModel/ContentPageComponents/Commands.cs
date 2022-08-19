using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global;
using Ahed_project.Windows;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        public ICommand Logout => new AsyncCommand(async () =>
        {
            using (var context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                active.IsActive = false;
                context.Update(active);
                context.SaveChanges();
            }
            _pageService.ChangePage(new LoginPage());
        });

        public ICommand Exit => new DelegateCommand(() =>
        {
            Application.Current.Shutdown();
        });

        public ICommand OpenPresetWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new Presets());
        });
        public ICommand OpenProductsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new ProductsWindow());
        });

        public ICommand OpenProjectsWindow => new DelegateCommand(() =>
        {
            _windowServise.OpenModalWindow(new ProjectsWindow());
        });

        public ICommand ShowProjectInfo => new DelegateCommand(() =>
        {
            if (ProjectInfoVisibility == Visibility.Hidden)
            {
                ProjectInfoVisibility = Visibility.Visible;
            }
            else
            {
                ProjectInfoVisibility = Visibility.Hidden;
            }
        });

        public ICommand NewProjectCommand => new AsyncCommand(async () =>
        {
            GlobalDataCollectorService.Logs.Add(new LoggerMessage("Info", "Начало создания проекта..."));
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""));
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message));
                    }
                    var newProj = JsonConvert.DeserializeObject<ProjectInfoGet>(result.data.ToString());
                    GlobalDataCollectorService.ProjectsCollection.Add(newProj);
                    GlobalFunctionsAndCallersService.SetProject(newProj);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        });

        public ICommand CalculateCommand => new AsyncCommand(async () =>
        {
            if (SelectedCalulationFull?.project_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationSendCalc calculateSend = new CalculationSendCalc
            {
                product_id_tube = SingleProductGetTubes?.product_id ?? 0,
                product_id_shell = SingleProductGetShell?.product_id ?? 0,
                flow_type = "counter_current",
                calculate_field = "flow_shell",
                process_tube = SelectedCalulationFull.process_tube,
                process_shell = SelectedCalulationFull.process_shell,
                flow_tube = SelectedCalulationFull.flow_tube,
                flow_shell = SelectedCalulationFull.flow_shell,
                temperature_tube_inlet = SelectedCalulationFull.temperature_tube_inlet,
                temperature_tube_outlet = SelectedCalulationFull.temperature_tube_outlet,
                temperature_shell_inlet = SelectedCalulationFull.temperature_shell_inlet,
                temperature_shell_outlet = SelectedCalulationFull.temperature_shell_outlet,
                pressure_tube_inlet = SelectedCalulationFull.pressure_tube_inlet,
                pressure_shell_inlet = SelectedCalulationFull.pressure_shell_inlet
            };
            string json = JsonConvert.SerializeObject(calculateSend);
            //var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE, json, ProjectInfo));
            //Responce result = JsonConvert.DeserializeObject<Responce>(response);
            //if (result?.logs != null)
            //{
            //    for (int i = 0; i < result.logs.Count; i++)
            //    {
            //        GlobalDataCollectorService.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message));
            //    }
            //    CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
            //    calculationGet.calculation_id = SelectedCalulationFull.calculation_id;
            //    calculationGet.project_id = SelectedCalulationFull.project_id;
            //    var index = CalculationsInfo.FindIndex(0, CalculationsInfo.Count, x => x.calculation_id == SelectedCalulationFull.calculation_id);
            //    CalculationsInfo[index] = calculationGet;
            //    SelectedCalulationFull = calculationGet;
            //}
        });

        public ICommand CreateShellChartsCommand => new DelegateCommand(() =>
        {
            CreateShellCharts();
        });

        public ICommand CreateTubeChartsCommand => new DelegateCommand(() =>
        {
            CreateTubeCharts();
        });

    }
}
