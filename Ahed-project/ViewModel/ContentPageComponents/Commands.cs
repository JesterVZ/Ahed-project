using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Services.EF;
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
        public Action<int> ChangePage { get; set; }
        public ICommand Logout => new AsyncCommand(async () =>
        {
            using (var context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                active.IsActive = false;
                context.Update(active);
                context.SaveChanges();
            }
            _cancellationToken.Stop();
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

        public ICommand SelectLastProject => new AsyncCommand(async () =>
        {
            _logs.AddMessage("Info", "Загрузка последних проектов...");
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, ""), _cancellationToken.GetToken());
            if (response.Result is string)
            {

                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    List<ProjectInfoGet> projects = JsonConvert.DeserializeObject<List<ProjectInfoGet>>(result.data.ToString());
                    if (projects.Count > 0)
                    {
                        int userId = Convert.ToInt32(Application.Current.Resources["UserId"]);
                        int id = 0;
                        using (var context = new EFContext())
                        {
                            var user = context.Users.FirstOrDefault(x => x.Id == userId);
                            id = user.LastProjectId??0;
                        }
                        if (id != 0)
                            _selectProjectService.SelectProject(projects.FirstOrDefault(x => x.project_id == id));
                        _logs.AddMessage("success", "Загрузка проекта выполнена успешно!");
                    }
                    Validation();
                }
                catch (Exception e)
                {
                    _logs.AddMessage("Error", e.Message.ToString());
                }
            }
            else if (response.Result is Exception)
            {
                _logs.AddMessage("Error", response.Result.ToString());
            }
        });

        public ICommand SaveComand => new AsyncCommand(async () =>
        {
            _logs.AddMessage("Info", "Идет сохранение проекта...");
            var projectInfoSend = _mapper.Map<ProjectInfoSend>(ProjectInfo);
            string json = JsonConvert.SerializeObject(projectInfoSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE, json, ProjectInfo), _cancellationToken.GetToken());
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                    _logs.AddMessage("success", "Сохранение выполнено успешно!");
                    _windowTitleService.ChangeTitle(ProjectInfo.name);
                    Validation();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        });

        public ICommand NewProjectCommand => new AsyncCommand(async () =>
        {
            _logs.AddMessage("Info", "Начало создания проекта...");
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""), _cancellationToken.GetToken());
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        });

        public ICommand CreateCalculationCommand => new AsyncCommand(async () => {
            CalculationSend calculationSend = new CalculationSend
            {
                Name = CalculationName
            };
            string json = JsonConvert.SerializeObject(calculationSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE_CALCULATION, json, ProjectInfo), _cancellationToken.GetToken());
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                    CalculationGet calculationGet = JsonConvert.DeserializeObject<CalculationGet>(result.data.ToString());
                    CalculationCollection.Add(new Calculation
                    {
                        calculation_id = calculationGet.calculation_id.ToString(),
                        name = calculationGet.name.ToString(),
                    });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        public ICommand CalculateCommand => new AsyncCommand(async () => {
            if (SelectedCalulationFull?.project_id == 0)
            {
                MessageBox.Show("Выберите рассчет", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string json = JsonConvert.SerializeObject(SelectedCalulationFull);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CALCULATE, json, ProjectInfo), _cancellationToken.GetToken());
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        _logs.AddMessage(result.logs[i].type, result.logs[i].message);
                    }
                    CalculationFull calculationGet = JsonConvert.DeserializeObject<CalculationFull>(result.data.ToString());
                    calculationGet.calculation_id = SelectedCalulationFull.calculation_id;
                    calculationGet.project_id = SelectedCalulationFull.project_id;
                    var index= CalculationsInfo.FindIndex(0,CalculationsInfo.Count,x => x.calculation_id == SelectedCalulationFull.calculation_id);
                    CalculationsInfo[index] = calculationGet;
                    SelectedCalulationFull = calculationGet;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
