using Ahed_project.MasterData;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.MasterData.Products;
using Ahed_project.MasterData.ProjectClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ahed_project.Services.Global
{
    public class GlobalDataCollectorService
    {
        public GlobalDataCollectorService()
        {
            Logs = new ObservableCollection<LoggerMessage>();
            AllProducts = new Dictionary<string, List<ProductGet>>();
            Nodes = new ObservableCollection<Node>();
            ProjectsCollection = new List<ProjectInfoGet>();
            GeometryCollection = new ObservableCollection<GeometryFull>();
            Materials = new List<Material>();
        }
        #region Global
        public static ObservableCollection<LoggerMessage> Logs { get; set; }
        #endregion
        #region User
        private static int _userId;
        public static int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                Task.Factory.StartNew(GlobalFunctionsAndCallersService.SetupUserDataAsync);
            }
        }
        #endregion
        #region Heat
        public static bool HeatBalanceCalculated { get; set; }
        #endregion
        #region Project
        public static List<ProjectInfoGet> ProjectsCollection { get; set; }
        public static ProjectInfoGet Project { get; set; }
        #endregion
        #region Geometry
        public static ObservableCollection<GeometryFull> GeometryCollection { get; set; }
        public static bool GeometryCalculated { get; set; }
        #endregion
        #region Products
        public static Dictionary<string, List<ProductGet>> AllProducts { get; set; }
        public static ObservableCollection<Node> Nodes { get; set; }
        #endregion
        #region Materials
        public static List<Material> Materials { get; set; }
        #endregion
    }
}
