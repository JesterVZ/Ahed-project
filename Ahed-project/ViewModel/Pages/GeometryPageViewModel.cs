using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class GeometryPageViewModel : BindableBase
    {
        public bool IsOpen { get; set; }
        public double GridColumnWidth { get; set; }
        public Dictionary<int, string> Exchangers { get; set; }
        public Dictionary<int, string> Materials { get; set; }
        public GeometryFull Geometry { get; set; }
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

        private Visibility _sealingTypeVis;

        public Visibility SealingTypeVis
        {
            get => _sealingTypeVis;
            set
            {
                _sealingTypeVis = value;
            }
        }

        public Dictionary<int,string> DivPlateItems { get; set; }

        private KeyValuePair<int, string> _divPlateItem;
        public KeyValuePair<int,string> DivPlateItem
        {
            get => _divPlateItem;
            set
            {
                _divPlateItem = value;
                if (_divPlateItem.Value == "Mechanised")
                {
                    SealingTypeVis = Visibility.Visible;
                }
                else
                {
                    SealingTypeVis = Visibility.Hidden;
                }
            }
        }

        public Dictionary<int,string> SealingTypeItems { get; set; }

        private KeyValuePair<int, string> _sealingTypeItem;

        public KeyValuePair<int,string> SealingTypeItem
        {
            get => _sealingTypeItem;
            set
            {
                _sealingTypeItem = value;
            }
        }

        public GeometryPageViewModel()
        {
            Exchangers = new Dictionary<int, string>();
            Materials = new Dictionary<int, string>();
            DivPlateItems = new Dictionary<int, string>();
            SealingTypeItems = new Dictionary<int, string>();
            Exchangers.Add(0, "Tube/Shell");
            Exchangers.Add(1, "Annular Space");
            Exchangers.Add(2, "Unicus");
            Exchangers.Add(3, "R Series");
            DivPlateItems.Add(0, "Horizontal");
            DivPlateItems.Add(1, "Mechanised");
            SealingTypeItems.Add(0, "O'Rings + Housing");
            SealingTypeItems.Add(1, "Gasket");
            SealingTypeVis = Visibility.Hidden;
        }



        #region coms

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;

        });

        public ICommand Calculate => new DelegateCommand(() =>
        {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CalculateGeometry(Geometry));
        });

        #endregion
    }
}
