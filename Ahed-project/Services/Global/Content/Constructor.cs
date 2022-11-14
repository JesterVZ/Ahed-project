using Ahed_project.MasterData;
using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.MainClasses;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.MasterData.ShellClasses;
using Ahed_project.MasterData.TabClasses;
using Ahed_project.MasterData.TubesClasses;
using Ahed_project.Migrations;
using Ahed_project.Services.EF;
using Ahed_project.Services.EF.Model;
using Ahed_project.Services.Global.Interface;
using Ahed_project.Services.Interfaces;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using Ahed_project.ViewModel.Windows;
using AutoMapper;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Material = Ahed_project.MasterData.Material;

namespace Ahed_project.Services.Global.Content
{
    /// <summary>
    /// Сервис для прогрузки данных после логина в потоке отдельном планируется
    /// </summary>
    public partial class UnitedStorage : IUnitedStorage
    {
        private bool _isAllSave { get; set; }
        private bool _isProjectSave { get; set; } // сохранен ли проект\
        private bool _isTubetSave { get; set; } // сохранен ли tube
        private bool _isShelltSave { get; set; } // сохранен ли shell
        private bool _isHeatBalancetSave { get; set; } // сохранен ли heat balance
        private bool _isGeometrytSave { get; set; } // сохранена ли geometry
        private bool _heatBalanceCalculated = false;

        private readonly ISendDataService _sendDataService;
        private readonly IMapper _mapper;
        private UserEF _user;
        public void SelectUser(int userId)
        {
            using var context = new EFContext();
            _user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
        }
        private readonly LogsStorage Logs;
        public UnitedStorage(ISendDataService sendDataService, LogsStorage logs)
        {
            _sendDataService = sendDataService;
            _geometryCollection = new ObservableCollection<GeometryFull>();
            _calculationData = new CalculationInGlobal();
            _mainData = new MainInGlobal();
            _projectData = new ProjectInGlobal();
            _shellsData = new ShellInGlobal();
            _tubesData = new TubesInGlobal();
            _geometryData = new GeometryInGlobal();
            Calculations = new List<CalculationFull>();
            ContentData = new();
            _nodes = new ObservableCollection<Node>();
            _materrialsCollection = new List<Material>();
            Projects = new List<ProjectInfoGet>();
            PageStates = new MasterData.ContentClasses.PageStates();
            _allProducts = new Dictionary<string, List<ProductGet>>();
            Logs = logs;
            _project = new ProjectInfoGet();
            _calculation = new CalculationFull();
        }


        //Обновить продукты в рассчете
        public async void UpdateCalculationProducts()
        {
            if (Calculation == null || Calculation.calculation_id == 0)
            {
                MessageBox.Show("Не выбран рассчет, следует выбрать для внесения данных", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            CalculationUpdate calculationUpdate = new()
            {
                product_id_tube = Calculation.product_id_tube ?? 0,
                product_id_shell = Calculation.product_id_shell ?? 0,
                name = Calculation.name
            };
            string json = JsonConvert.SerializeObject(calculationUpdate);
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.UPDATE_CHOOSE, json, Calculation.project_id.ToString(), Calculation.calculation_id.ToString());
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
            Responce result = JsonConvert.DeserializeObject<Responce>(response);
            for (int i = 0; i < result.logs.Count; i++)
            {
                Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage(result.logs[i].type, result.logs[i].message)));
            }
            Application.Current.Dispatcher.Invoke(() => Logs.Logs.Add(new LoggerMessage("success", "Сохранение выполнено успешно!")));
            Validation(false);
        }


        private List<Material> _materrialsCollection { get; set; }

        public List<Material> GetMaterialsCollection() { return _materrialsCollection; }

        //Загрузка материалов
        public async void GetMaterials()
        {
            var response = await Task.Factory.StartNew(() =>
            {
                var resp = _sendDataService.SendToServer(ProjectMethods.GET_MATERIALS);
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
                    var materials = JsonConvert.DeserializeObject<IEnumerable<Material>>(response);
                    _materrialsCollection = materials.ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void SetWindowName(string name)
        {
            MainData.Title = $"{name} ({Calculation?.name})";
        }
    }
}
