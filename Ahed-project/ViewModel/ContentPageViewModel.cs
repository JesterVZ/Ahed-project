using Ahed_project.MasterData;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.EF;
using Ahed_project.Windows;
using AutoMapper;
using DevExpress.Mvvm;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly WindowService _windowServise;
        private readonly Logs _logs;
        private readonly SendDataService _sendDataService;
        private readonly WindowTitleService _windowTitleService;
        private readonly SelectProjectService _selectProjectService;
        private readonly SelectProductService _selectProductService;
        private readonly BackGroundService _backGroundService;
        private readonly IMapper _mapper;

        public ObservableCollection<LoggerMessage> LogCollection { get; set; }

        private ProjectInfoGet projectInfo = new ProjectInfoGet();
        public ProjectInfoGet ProjectInfo { get => projectInfo; set => SetValue(ref projectInfo, value); }

        private SingleProductGet singleProductGet;
        public SingleProductGet SingleProductGet { get => singleProductGet; set => SetValue(ref singleProductGet, value); }
        public string ProjectValidationStatusSource { get; set; }
        public string TubesFluidValidationStatusSource { get; set; }
        public string ShellFluidValidationStatusSource { get; set; }
        public string HeatBalanceValidationStatusSource { get; set; }
        public string GeometryValidationStatusSource { get; set; }
        public string BafflesValidationStatusSource { get; set; }
        public string OverallValidationStatusSource { get; set; }
        public string BatchValidationStatusSource { get; set; }
        public string GraphsValidationStatusSource { get; set; }
        public string ReportsValidationStatusSource { get; set; }
        public string QuoteValidationStatusSource { get; set; }
        public string ThreeDValidationStatusSource { get; set; }

        public ContentPageViewModel(PageService pageService, WindowService windowService, Logs logs, WindowTitleService windowTitleService,
            SendDataService sendDataService, SelectProjectService selectProjectService, SelectProductService selectProductService, IMapper mapper, BackGroundService backGroundService)
        {
            //инициализация
            ProjectState = new ContentState();
            TubesFluidState = new ContentState();
            ShellFluidState = new ContentState();
            HeatBalanceState = new ContentState();
            GeometryState = new ContentState();
            BafflesState = new ContentState();
            OverallCalculationState = new ContentState();
            BatchState = new ContentState();
            GraphState = new ContentState();
            ReportsState = new ContentState();
            QuoteState = new ContentState();
            ThreeDState = new ContentState();

            ProjectState.IsEnabled = true;
            TubesFluidState.IsEnabled = true;
            ShellFluidState.IsEnabled = true;
            HeatBalanceState.IsEnabled = true;
            GeometryState.IsEnabled = false;
            BafflesState.IsEnabled = false;
            OverallCalculationState.IsEnabled = true;
            BatchState.IsEnabled = false;
            GraphState.IsEnabled = false;
            ReportsState.IsEnabled = false;
            QuoteState.IsEnabled = false;
            ThreeDState.IsEnabled = false;

            _pageService = pageService;
            _windowServise = windowService;
            _logs = logs;
            _sendDataService = sendDataService;
            _selectProjectService = selectProjectService;
            _selectProductService = selectProductService;
            _windowTitleService = windowTitleService;
            _selectProjectService.ProjectSelected += (project) => { 
                ProjectInfo = project;
                _windowTitleService.ChangeTitle(project.name);
            };
            _selectProductService.ProductSelected += (product) => SingleProductGet = product;
            LogCollection = _logs.logs;
            _mapper = mapper;
            _backGroundService = backGroundService;

            
        }

        private void Validation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (ProjectInfo.name != null && ProjectInfo.name != String.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            } else
            {
                _logs.AddMessage("Error", "Введите имя проекта!");
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }

            if(ProjectInfo.description != null && ProjectInfo.description != String.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                
            }else
            {
                _logs.AddMessage("warning", "Введите описание проекта!");
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }

            if(SingleProductGet != null)
            {
                if (SingleProductGet.name != null && SingleProductGet.name != String.Empty)
                {
                    TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                }
                else
                {
                    _logs.AddMessage("warning", "Введите имя продукта!");
                    TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                }
            } else
            {
                _logs.AddMessage("Error", "Выберете продукт!");
                TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }
            
        }

        public ICommand Logout => new AsyncCommand(async () => {
            using (var context = new EFContext())
            {
                var active = context.Users.FirstOrDefault(x => x.IsActive);
                active.IsActive = false;
                context.Update(active);
                context.SaveChanges();
            }
            _pageService.ChangePage(new LoginPage());
        });

        public ICommand Exit => new DelegateCommand(() => {
            System.Windows.Application.Current.Shutdown();
        });

        public ICommand OpenPresetWindow => new DelegateCommand(() => {
            _windowServise.OpenModalWindow(new Presets());
        });
        public ICommand OpenProductsWindow => new DelegateCommand(() => {
            _windowServise.OpenModalWindow(new ProductsWindow());
        });

        public ICommand OpenProjectsWindow => new DelegateCommand(() => {
            _windowServise.OpenModalWindow(new ProjectsWindow());
        });

        public ICommand ShowProjectInfo => new DelegateCommand(() => {
            if (ProjectInfoVisibility == Visibility.Hidden)
            {
                ProjectInfoVisibility = Visibility.Visible;
            } else
            {
                ProjectInfoVisibility = Visibility.Hidden;
            }
        });

        public ICommand SelectLastProject => new AsyncCommand(async () => {
            _backGroundService.Start();
            _logs.AddMessage("Info", "Загрузка последних проектов...");
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.GET_PROJECTS, ""));
            if (response.Result is string)
            {
                try
                {
                    Responce result = JsonConvert.DeserializeObject<Responce>(response.Result.ToString());
                    List<ProjectInfoGet> projects = JsonConvert.DeserializeObject<List<ProjectInfoGet>>(result.data.ToString());
                    if (projects.Count > 0)
                    {
                        _selectProjectService.SelectProject(projects.Last<ProjectInfoGet>());
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

        public ICommand SaveComand => new AsyncCommand(async () => {
            _logs.AddMessage("Info", "Идет сохранение проекта...");
            var projectInfoSend = _mapper.Map<ProjectInfoSend>(ProjectInfo);
            string json = JsonConvert.SerializeObject(projectInfoSend);
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.UPDATE, json, ProjectInfo));
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

        public ICommand NewProjectCommand => new AsyncCommand(async () => {
            _logs.AddMessage("Info", "Начало создания проекта...");
            var response = await Task.Factory.StartNew(() => _sendDataService.SendToServer(ProjectMethods.CREATE, ""));
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
            } else if (response.Result is Exception)
            {
                MessageBox.Show(response.Result.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        });

        private Visibility projectInfoVisibility = Visibility.Hidden;

        public Visibility ProjectInfoVisibility { get => projectInfoVisibility; set => SetValue(ref projectInfoVisibility, value); }

        public ContentState ProjectState { get; set; }
        public ContentState TubesFluidState { get; set; }
        public ContentState ShellFluidState { get; set; }
        public ContentState HeatBalanceState { get; set; }
        public ContentState GeometryState { get; set; }
        public ContentState BafflesState { get; set; }
        public ContentState OverallCalculationState { get; set; }
        public ContentState BatchState { get; set; }
        public ContentState GraphState { get; set; }
        public ContentState ReportsState { get; set; }
        public ContentState QuoteState { get; set; }
        public ContentState ThreeDState {get; set;}

    }
}
