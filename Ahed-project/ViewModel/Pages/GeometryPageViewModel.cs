using Ahed_project.MasterData.GeometryClasses;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class GeometryPageViewModel : BindableBase
    {
        public bool IsOpen { get; set; }
        public double GridColumnWidth { get; set; }
        public Dictionary<int, string> Exchangers { get; set; }
        public GeometrySend Geometry { get; set; }
        public bool OppositeSide { get; set; }
        public bool SameSide { get; set; }
        private KeyValuePair<int, string> _exchangersSelector;

        public KeyValuePair<int, string> ExchangersSelector { 
            get { 
                return _exchangersSelector; 
            } 
            set {
                if(value.Key == 1)
                {
                    GridColumnWidth = 120;
                } else
                {
                    GridColumnWidth = 0;
                }
                _exchangersSelector = value; 
            } 
        }

        public GeometryPageViewModel()
        {
            Exchangers = new Dictionary<int, string>();
            Exchangers.Add(0, "Tube/Shell");
            Exchangers.Add(1, "Annular Space");
            Exchangers.Add(2, "Unicus");
            Exchangers.Add(3, "R Series");
        }

        #region coms

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;

        });

        #endregion
    }
}
