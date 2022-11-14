using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ContentClasses;
using Ahed_project.MasterData.MainClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.MasterData.TabClasses;
using Ahed_project.Pages;
using Ahed_project.Services.EF;
using Ahed_project.Services.Global.Interface;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Ahed_project.ViewModel.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services.Global.Content
{
    public partial class UnitedStorage : IUnitedStorage
    {
        private ContentInGlobal _contentData;
        public ContentInGlobal ContentData
        {
            get => _contentData;
            set => _contentData = value;
        }
        public ContentInGlobal GetContentData() { return ContentData; }
        public void SetContentData(ContentInGlobal data) { ContentData = data; }
        private ProjectInfoGet _project;
        private CalculationFull _calculation;
        public ProjectInfoGet Project
        {
            get => _project;
            set
            {
                _project = value;
                ProjectData.Project = _project;
            }
        }
        public CalculationFull Calculation
        {
            get => _calculation;
            set
            {
                if (value?.calculation_id != _calculation?.calculation_id&&value!=null)
                {
                    using (var context = new EFContext())
                    {
                        var u = context.Users.FirstOrDefault(x => x.Id == _user.Id);
                        u.LastCalculationId = value.calculation_id;
                        context.Users.Update(u);
                        context.SaveChanges();
                    }
                }
                _calculation = value;
                ProjectData.Calculation = _calculation;
                CalculationData.Calculation = _calculation;
                SelectProductTube(value.product_id_tube??0);
                SelectProductShell(value.product_id_shell ?? 0);
            }
        }
        public void SetCalculation(int? id)
        {
            Calculation = Calculations.FirstOrDefault(x => x.calculation_id == id);
        }
        private ProjectInGlobal _projectData;
        public ProjectInGlobal ProjectData
        {
            get { return _projectData; }
            set { _projectData = value; }
        }

        public ProjectInGlobal GetProjectData() { return ProjectData; }
        public void SetProjectData(ProjectInGlobal data) { ProjectData = data; }
        public void SetProject(ProjectInfoGet project)
        {
            bool isNeedToUpdateProj = false;
            if (project?.project_id != null && project?.project_id != Project?.project_id)
            {
                isNeedToUpdateProj = true;
            }
            Project = project;
            if (isNeedToUpdateProj)
            {
                if (!(Calculation == null || Calculation?.calculation_id == 0))
                {
                    _projectData.Calculation = null;
                }
                using (var context = new EFContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == _user.Id);
                    user.LastProjectId = project.project_id;
                    context.Update(user);
                    context.SaveChanges();
                }
                GetCalculations(project.project_id.ToString());
                if (_user.LastCalculationId != null && _user.LastCalculationId != 0)
                {
                    Calculation = Calculations.FirstOrDefault(x => x.calculation_id == _user.LastCalculationId.Value);
                }
                _mainData.Title = $"{project.name} ({Calculation?.name})";
                ProjectData.FieldsState = true;
                Validation(false);
            }
        }
        public List<ProjectInfoGet> GetProjects() { return Projects; }
        // Смена страницы на ContentPage
        public void ChangePage(int n)
        {
            ContentData.SelectedPage = n;
        }

        private MainInGlobal _mainData;
        public MainInGlobal MainData
        {
            get { return _mainData; }
            set { _mainData = value; }
        }
        public MainInGlobal GetMainData() { return MainData; }
        public void SetMainData(MainInGlobal data) { MainData = data; }
        //получение состояний вкладок
        public async Task GetTabState()
        {
            int calculation_id;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == _user.Id);
                calculation_id = user.LastCalculationId ?? 0;
            }
            var template = _sendDataService.ReturnCopy();
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Info", "Загрузка состояний вкладок...")));
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = template.SendToServer(ProjectMethods.GET_TAB_STATE, null, Project.project_id.ToString(), calculation_id.ToString());
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            });
            SetTabState(response);
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Info", "Загрузка состояний вкладок завершена!")));
            _isAllSave = true;
        }

        //сохранение состояния вкладок
        public async void SetTabState(TabsState tabs)
        {
            int calculation_id;
            using (var context = new EFContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == _user.Id);
                calculation_id = user.LastCalculationId ?? 0;
            }
            tabs.calculation_id = calculation_id.ToString();
            tabs.project_id = Project.project_id.ToString();
            string json = JsonConvert.SerializeObject(tabs);
            var template = _sendDataService.ReturnCopy();
            var response = await Task.Factory.StartNew(() => template.SendToServer(ProjectMethods.SET_TAB_STATE, json, Project.project_id.ToString(), calculation_id.ToString()));
        }

        //Сохранение проекта
        public async void SaveProject()
        {
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("info", "Идет сохранение проекта...")));
            var projectInfoSend = _mapper.Map<ProjectInfoSend>(Project);
            string json = JsonConvert.SerializeObject(projectInfoSend);
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.UPDATE, json, Project.project_id.ToString());
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}")));
                    return null;
                }
            });
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            _isProjectSave = true; //проект сохранен

            if (result.logs != null)
                for (int i = 0; i < result.logs.Count; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                }
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
        }

        private PageStates _pageStates;
        public PageStates PageStates
        {
            get => _pageStates;
            set => _pageStates = value;
        }

        public PageStates GetPageStates() { return PageStates; }
        public void SetPageStates(PageStates data) { PageStates = data; }

        private void Validation(bool needSetData)
        {
            var assembly = Assembly.GetExecutingAssembly();
            TabsState tabs = new();


            if (Project != null)
            {
                if (Calculation != null)
                {
                    PageStates.ProjectState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    ChangeTabState(MasterData.Pages.TUBES_FLUID);
                    ChangeTabState(MasterData.Pages.SHELL_FLUID);
                    ChangeTabState(MasterData.Pages.HEAT_BALANCE);
                    ChangeTabState(MasterData.Pages.GEOMETRY);
                    ChangeTabState(MasterData.Pages.BAFFLES);
                    ChangeTabState(MasterData.Pages.OVERALL_CALCULATION);
                    if (TubesData.Product == null)
                    {
                        tabs.tube_fluid = "0";
                        PageStates.TubesFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    }
                    else
                    {
                        tabs.tube_fluid = "1";
                        PageStates.TubesFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    if (ShellsData.Product == null)
                    {
                        tabs.shell_fluid = "0";
                        PageStates.ShellFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    }
                    else
                    {
                        tabs.shell_fluid = "1";
                        PageStates.ShellFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    if (_heatBalanceCalculated)
                    {
                        tabs.head_balance = "1";
                        PageStates.HeatBalanceState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    else
                    {
                        tabs.head_balance = "0";
                        PageStates.HeatBalanceState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    }
                    if (_geometryCalculated)
                    {
                        tabs.geometry = "1";
                        PageStates.GeometryState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                    }
                    else
                    {
                        tabs.geometry = "0";
                        PageStates.GeometryState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                    }
                    if (needSetData)
                        SetTabState(tabs); //отправить состояник вкладок по api
                }
                else
                {
                    //GlobalDataCollectorService.Logs.Add(new LoggerMessage("warning", "Выберите калькуляцию!"));
                    PageStates.ProjectState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                }

            }
            else
            {
                //GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", "Выберите проект!"));
                PageStates.ProjectState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }
        }

        private void ChangeTabState(MasterData.Pages page)
        {
            switch (page)
            {
                case MasterData.Pages.PROJECT:
                    PageStates.ProjectState.IsEnabled = true;
                    break;
                case MasterData.Pages.TUBES_FLUID:
                    PageStates.TubesFluidState.IsEnabled = true;
                    break;
                case MasterData.Pages.SHELL_FLUID:
                    PageStates.ShellFluidState.IsEnabled = true;
                    break;
                case MasterData.Pages.HEAT_BALANCE:
                    PageStates.HeatBalanceState.IsEnabled = true;
                    break;
                case MasterData.Pages.GEOMETRY:
                    PageStates.GeometryState.IsEnabled = true;
                    break;
                case MasterData.Pages.BAFFLES:
                    PageStates.BafflesState.IsEnabled = true;
                    break;
                case MasterData.Pages.OVERALL_CALCULATION:
                    PageStates.OverallCalculationState.IsEnabled = true;
                    break;
                case MasterData.Pages.BATCH:
                    PageStates.BatchState.IsEnabled = true;
                    break;
                case MasterData.Pages.GRAPHS:
                    PageStates.GraphState.IsEnabled = true;
                    break;
                case MasterData.Pages.REPORTS:
                    PageStates.ReportsState.IsEnabled = true;
                    break;
                case MasterData.Pages.QUOTE:
                    PageStates.QuoteState.IsEnabled = true;
                    break;
                case MasterData.Pages.THREE_D:
                    PageStates.ThreeDState.IsEnabled = true;
                    break;
            }
        }

        private void SetTabState(string json) //расставить галочки
        {
            var assembly = Assembly.GetExecutingAssembly();
            TabsState tabs = JsonConvert.DeserializeObject<TabsState>(json);
            if (tabs.tube_fluid != null && tabs.tube_fluid == "1")
            {
                PageStates.TubesFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                PageStates.TubesFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }

            if (tabs.shell_fluid != null && tabs.shell_fluid == "1")
            {
                PageStates.ShellFluidState.ValidationStatusSource= Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                PageStates.ShellFluidState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }

            if (tabs.head_balance != null && tabs.head_balance == "1")
            {
                PageStates.HeatBalanceState.ValidationStatusSource= Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                PageStates.HeatBalanceState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }

            if (tabs.geometry != null && tabs.geometry == "1")
            {
                PageStates.GeometryState.ValidationStatusSource= Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                PageStates.GeometryState.ValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }
        }

        //Создать проект
        public async void CreateNewProject()
        {
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("Info", "Начало создания проекта...")));
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.CREATE, "");
                if (resp.IsSuccessful)
                {
                    return resp.Content;
                }
                else
                {
                    Logs.Logs.Add(new LoggerMessage("Error", $"Message: {resp.ErrorMessage}\r\nCode: {resp.StatusCode}\r\nExcep: {resp.ErrorException}"));
                    return null;
                }
            });
            if (response != null)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response);
                    for (int i = 0; i < result.logs.Count; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
                    }
                    var newProj = JsonConvert.DeserializeObject<ProjectInfoGet>(result.data.ToString());
                    Projects.Add(newProj);
                    _geometryCalculated = false;
                    _heatBalanceCalculated = false;
                    Project = newProj;
                    Validation(true);
                    Application.Current.Dispatcher.Invoke(() => Calculations.Clear());
                    await Task.Factory.StartNew(() => CreateCalculation("Default"));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
