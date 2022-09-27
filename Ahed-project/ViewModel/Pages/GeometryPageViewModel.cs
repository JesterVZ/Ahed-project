using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.Services.Global;
using DevExpress.DXBinding.Native;
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
        public Dictionary<int, TubeplateLayout> TubePlateLayouts { get; set; }
        private GeometryFull _geometry;
        public GeometryFull Geometry 
        { 
            get =>_geometry; 
            set
            {
                _geometry = value;
                switch (value.head_exchange_type)
                {
                    case "tube_shell":
                        ExchangersSelector = Exchangers.FirstOrDefault(x => x.Key == 0);
                        break;
                    case "annular_space":
                        ExchangersSelector = Exchangers.FirstOrDefault(x => x.Key == 1);
                        break;
                    case "unicus":
                        ExchangersSelector = Exchangers.FirstOrDefault(x => x.Key == 2);
                        break;
                    case "r_series":
                        ExchangersSelector = Exchangers.FirstOrDefault(x => x.Key == 3);
                        break;
                }
                ShellMaterial = Materials.FirstOrDefault(x => x.Value == value.material_shell_side);
                TubesMaterial = Materials.FirstOrDefault(x => x.Value == value.material_tubes_side);
            }
        }

        private KeyValuePair<int, string> _shellMaterial;

        public KeyValuePair<int,string> ShellMaterial
        {
            get => _shellMaterial;
            set
            {
                _shellMaterial = value;
                if (Geometry!=null)
                {
                    if (Geometry.material_shell_side != value.Value)
                    {
                        Geometry.material_shell_side = value.Value;
                    }
                }
            }
        }

        private KeyValuePair<int, string> _tubesMaterial;

        public KeyValuePair<int,string> TubesMaterial
        {
            get => _tubesMaterial;
            set
            {
                _tubesMaterial = value;
                if (Geometry!=null)
                {
                    if (Geometry.material_tubes_side!=value.Value)
                    {
                        Geometry.material_tubes_side = value.Value;
                    }
                }
            }
        }

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
                if (Geometry != null)
                {
                    switch (value.Key)
                    {
                        case 0:
                            if (Geometry.head_exchange_type != "tube_shell")
                            {
                                Geometry.head_exchange_type = "tube_shell";
                            }
                            break;
                        case 1:
                            if (Geometry.head_exchange_type != "annular_space")
                            {
                                Geometry.head_exchange_type = "annular_space";
                            }
                            break;
                        case 2:
                            if (Geometry.head_exchange_type != "unicus")
                            {
                                Geometry.head_exchange_type = "unicus";
                            }
                            break;
                        case 3:
                            if (Geometry.head_exchange_type != "r_series")
                            {
                                Geometry.head_exchange_type = "r_series";
                            }
                            break;
                    }
                }
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
                if (_sealingTypeItem.Value== "O'Rings + Housing")
                {
                    HousingSpaceVis = Visibility.Visible;
                }
                else
                {
                    HousingSpaceVis = Visibility.Hidden;
                }
            }
        }

        private Visibility _housingSpaceVis;
        public Visibility HousingSpaceVis
        {
            get => _housingSpaceVis;
            set
            {
                _housingSpaceVis = value;
            }
        }

        public GeometryPageViewModel()
        {
            Exchangers = new Dictionary<int, string>();
            Materials = new Dictionary<int, string>();
            DivPlateItems = new Dictionary<int, string>();
            SealingTypeItems = new Dictionary<int, string>();
            TubePlateLayouts = new Dictionary<int, TubeplateLayout>();
            TubeplateLayout triangular = new TubeplateLayout
            {
                ImageUrl = "../Visual/Triangilar.png",
                Name = "Triangular"
            };
            TubeplateLayout triangularCentred = new TubeplateLayout
            {
                ImageUrl = "../Visual/Triangular_centred.png",
                Name = "Triangular Centred"
            };
            TubeplateLayout squared = new TubeplateLayout
            {
                ImageUrl = "../Visual/Squared.png",
                Name = "Squared"
            };
            TubeplateLayout squaredCentred = new TubeplateLayout
            {
                ImageUrl = "../Visual/Squared_centred.png",
                Name = "Squared Centred"
            };
            TubeplateLayout rotatedSquared = new TubeplateLayout
            {
                ImageUrl = "../Visual/Rotated_Squared.png",
                Name = "Rotated Squared"
            };
            TubeplateLayout rotatedSquaredCentred = new TubeplateLayout
            {
                ImageUrl = "../Visual/Rotated_Squared_centred.png",
                Name = "Rotated Squared Centred"
            };

            Exchangers.Add(0, "Tube/Shell");
            Exchangers.Add(1, "Annular Space");
            Exchangers.Add(2, "Unicus");
            Exchangers.Add(3, "R Series");
            DivPlateItems.Add(0, "Horizontal");
            DivPlateItems.Add(1, "Mechanised");
            SealingTypeItems.Add(0, "O'Rings + Housing");
            SealingTypeItems.Add(1, "Gasket");
            TubePlateLayouts.Add(0, triangular);
            TubePlateLayouts.Add(1, triangularCentred);
            TubePlateLayouts.Add(2, squared);
            TubePlateLayouts.Add(3, squaredCentred);
            TubePlateLayouts.Add(4, rotatedSquared);
            TubePlateLayouts.Add(5, rotatedSquaredCentred);
            SealingTypeVis = Visibility.Hidden;
            HousingSpaceVis = Visibility.Hidden;
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
