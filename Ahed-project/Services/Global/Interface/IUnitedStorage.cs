using Ahed_project.MasterData;
using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ContentClasses;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.MainClasses;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.MasterData.ShellClasses;
using Ahed_project.MasterData.TabClasses;
using Ahed_project.MasterData.TubesClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services.Global.Interface
{
    public interface IUnitedStorage
    {
        //Разделяем именно по c# докам в реализации partial class
        #region Constructor
        void SelectUser(int userId);
        #endregion
        #region Global
        Task SetupUserDataAsync();
        Task GetTabState();
        void SetTabState(TabsState tabs);
        void ChangePage(int n);
        void SetWindowName(string name);
        #endregion
        #region Geometry
        void UpdateGeometry(GeometryFull geometry);
        ObservableCollection<GeometryFull> GetGeometries();
        GeometryInGlobal GetGeometryData();
        PageStates GetPageStates();
        void SetPageStates(PageStates data);
        void CalculateGeometry(GeometryFull geometry);
        void SetGeometryData(GeometryInGlobal data);
        #endregion
        #region Content
        ContentInGlobal GetContentData();
        void SetContentData(ContentInGlobal data);
        #endregion
        #region Project
        void SaveProject();
        ProjectInGlobal GetProjectData();
        void SetProjectData(ProjectInGlobal data);
        MainInGlobal GetMainData();
        void SetMainData(MainInGlobal data);
        void CreateNewProject();
        void SetProject(ProjectInfoGet project);
        List<ProjectInfoGet> GetProjects();
        #endregion
        #region Calculation
        CalculationInGlobal GetCalculation();
        void SetCalculation(CalculationInGlobal calculation);
        List<CalculationFull> GetCalculations();
        void SetCalculations(List<CalculationFull> calculations);
        void CreateCalculation(string name);
        void CalculateCalculation(CalculationFull calculation);
        void CalculateTemperature(string pressure_shell_inlet_value, CalculationFull calc, bool shell);
        void SetCalculation(int? id);
        #endregion
        #region Tubes
        TubesInGlobal GetTubesData();
        void UpdateTubesData(TubesInGlobal tubesData);
        void SelectProductTube(int id);
        #endregion
        #region Shells
        ShellInGlobal GetShellsData();
        void UpdateShellsData(ShellInGlobal data);
        void SelectProductShell(int id);
        #endregion
        #region Startup
        ObservableCollection<Node> GetNodes();
        Dictionary<string, List<ProductGet>> GetProducts();
        #endregion
        #region Baffles
        BaffleInGlobal GetBafflesData();
        void SetBafflesData(BaffleInGlobal bafflesData);
        void CalculateBaffle(BaffleFull baffle);
        void SetBaffle(string diametral_clearance_shell_baffle, string diametral_clearance_tube_baffle);
        #endregion
        #region Materials
        void GetMaterials();
        List<Material> GetMaterialsCollection();
        #endregion
    }
}
