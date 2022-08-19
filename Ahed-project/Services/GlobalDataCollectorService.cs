using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.MasterData.ProjectClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class GlobalDataCollectorService
    {
        public GlobalDataCollectorService()
        {
            ProjectPageContent = new ProjectInfoGet();
            CalculationsInfo = new List<CalculationFull>();
            SelectedCalculation = new CalculationFull();
            Logs = new ObservableCollection<LoggerMessage>();
            AllProducts = new Dictionary<string, List<SingleProductGet>>();
            Nodes = new ObservableCollection<Node>();
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
                Task.Factory.StartNew(StartUpService.SetupUserDataAsync);
            }
        }
        #endregion
        #region Project
        public static ProjectInfoGet ProjectPageContent { get; set; }

        public static List<CalculationFull> CalculationsInfo { get; set; }

        public static CalculationFull SelectedCalculation { get; set; }
        #endregion
        #region Products
        public static Dictionary<string, List<SingleProductGet>> AllProducts { get;set;}
        public static ObservableCollection<Node> Nodes { get; set; }
        #endregion
    }
}
