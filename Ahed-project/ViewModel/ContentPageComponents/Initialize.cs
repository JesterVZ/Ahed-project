using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.BackGroundServices;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly WindowService _windowServise;
        private readonly Logs _logs;
        private readonly SendDataService _sendDataService;
        private readonly WindowTitleService _windowTitleService;
        private readonly SelectProjectService _selectProjectService;
        private readonly SelectProductService _selectProductService;
        private readonly IMapper _mapper;
        private CancellationTokenService _cancellationToken;

        public ObservableCollection<LoggerMessage> LogCollection { get; set; }
        public ObservableCollection<Calculation> CalculationCollection { get; set; }
        public ObservableCollection<string> TubesProcess { get; set; }
        public ObservableCollection<string> ShellProcess { get; set; }
        private List<CalculationFull> CalculationsInfo { get; set; }

        private Calculation selectedCalculation;
        public Calculation SelectedCalculation
        {
            get => selectedCalculation;
            set
            {
                selectedCalculation = value;
                UpdateProjectParamsAccordingToCalculation();
            }
        }
        public string CalculationName { get; set; }
        private string selectedTubesProcess;
        public string SelectedTubesProcess
        {
            get => selectedTubesProcess;
            set => SetValue(ref selectedTubesProcess, value);
        }

        private string selectedShellProcess;
        public string SelectedShellProcess
        {
            get => selectedShellProcess;
            set => SetValue(ref selectedShellProcess, value);
        }

        public string FlowTube { get; set; }
        public string FlowShell { get; set; }
        public string TemperatureTubeInlet { get; set; }
        public string TemperatureTubeOutlet { get; set; }
        public string TemperatureShellInlet { get; set; }
        public string TemperatureShellOutlet { get; set; }
        public string PressureTubeInlet { get; set; }
        public string PressureShellInlet { get; set; }

        private ProjectInfoGet projectInfo = new();
        public ProjectInfoGet ProjectInfo { get => projectInfo; set => SetValue(ref projectInfo, value); }

        private SingleProductGet singleProductTubesGet;
        private SingleProductGet singleProductShellGet;
        public SingleProductGet SingleProductGetTubes
        {
            get => singleProductTubesGet;
            set
            {
                SetValue(ref singleProductTubesGet, value);
                if (selectedCalculation!=null&&selectedCalculation?.calculation_id!="0")
                SaveChoose(SelectedCalculation, SingleProductGetTubes, SingleProductGetShell);
                CreateTubeCharts();
            }
        }
        public SingleProductGet SingleProductGetShell
        {
            get => singleProductShellGet;
            set
            {
                SetValue(ref singleProductShellGet, value);
                if (selectedCalculation != null && selectedCalculation?.calculation_id != "0")
                    SaveChoose(SelectedCalculation, SingleProductGetTubes, SingleProductGetShell);
                CreateShellCharts();
            }
        }
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
            SendDataService sendDataService, SelectProjectService selectProjectService, SelectProductService selectProductService, IMapper mapper,
            CancellationTokenService cancellationToken)
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
            _selectProjectService.ProjectSelected += (project) =>
            {
                ProjectInfo = project;
                _windowTitleService.ChangeTitle(project.name);
            };
            _selectProductService.ProductTubesSelected += (product) => SingleProductGetTubes = product;
            _selectProductService.ProductShellSelected += (product) => SingleProductGetShell = product;
            LogCollection = _logs.logs;
            CalculationCollection = new ObservableCollection<Calculation>();
            CalculationsInfo = new List<CalculationFull>();
            TubesProcess = new ObservableCollection<string>
            {
                "sensible_heat",
                "condensation"
            };
            ShellProcess = new ObservableCollection<string>
            {
                "sensible_heat",
                "condensation"
            };
            _mapper = mapper;

            _cancellationToken = cancellationToken;
        }
    }
}
