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
using System.Windows.Media.Animation;

namespace Ahed_project.ViewModel.Pages
{
    public class GeometryPageViewModel : BindableBase
    {
        public bool IsOpen { get; set; }
        public double GridColumnWidth { get; set; }
        public Dictionary<string, string> Exchangers { get; set; }
        public Dictionary<int, string> Orientations { get; set; }
        public Dictionary<int, string> Materials { get; set; }
        public Dictionary<int, string> TubeProfile { get; set; }
        public Dictionary<int, TubeplateLayout> TubePlateLayouts { get; set; }
        private string _tube_plate_layout_number_of_passes { get; set; }
        public string Tube_plate_layout_number_of_passes {
            get => _tube_plate_layout_number_of_passes;
            set {
                _tube_plate_layout_number_of_passes = value;
                List<string> divplate = new List<string>(); ;
                switch (value)
                {
                    case "1":
                        divplate.Add("None");
                        break;
                    case "2":
                        divplate.Add( "Horizontal");
                        divplate.Add( "Vertical");
                        divplate.Add( "Mechanised");
                        break;
                    case "3":
                        divplate.Add( "Horizontal");
                        divplate.Add( "Vertical");
                        divplate.Add( "Mechanised");
                        break;
                    case "4":
                        divplate.Add("Horizontal");
                        divplate.Add("Horizontal + Vertical");
                        divplate.Add( "Type 3");
                        divplate.Add( "Mechanised");
                        break;
                    case "5":
                        divplate.Add("Horizontal");
                        divplate.Add( "Mechanised");
                        break;
                    case "6":
                        divplate.Add( "Type 1");
                        divplate.Add( "Type 2");
                        divplate.Add( "Type 3");
                        divplate.Add( "Mechanised");
                        break;
                    case "7":
                        divplate.Add( "Mechanised");
                        break;
                    case "8":
                        divplate.Add("Type 1");
                        divplate.Add( "Type 2");
                        divplate.Add( "Type 3");
                        divplate.Add( "Mechanised");
                        break;
                    case "9":
                        divplate.Add("Mechanised");
                        break;
                }
                DivPlateItems = new ObservableCollection<string>(divplate);
            } 
        }
        private GeometryFull _geometry;
        public GeometryFull Geometry 
        { 
            get =>_geometry; 
            set
            {
                _geometry = value;
                ExchangersSelector = Exchangers.FirstOrDefault(x=>x.Key==value.head_exchange_type);
                Orientation = Orientations.FirstOrDefault(x => x.Value == value.orientation);
                TubeProfileSelector = TubeProfile.FirstOrDefault(x => x.Value == value.tube_profile_tubes_side);
                ShellMaterial = Materials.FirstOrDefault(x => x.Value == value.material_shell_side);
                TubesMaterial = Materials.FirstOrDefault(x => x.Value == value.material_tubes_side);
                TubeLayout = TubePlateLayouts.FirstOrDefault(x => x.Value.Name == value.tube_plate_layout_tube_layout);
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
        private KeyValuePair<int, TubeplateLayout> _tubeLayout;
        public KeyValuePair<int, TubeplateLayout> TubeLayout
        {
            get => _tubeLayout;
            set
            {
                _tubeLayout = value;
                if(Geometry != null)
                {
                    if(value.Value != null)
                    {
                        Geometry.tube_plate_layout_tube_layout = value.Value.Name;
                    }
                    
                }
            }

        }

        private KeyValuePair<int, string> _orientation;
        public KeyValuePair<int, string> Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                if (Geometry != null)
                {
                    if(Geometry.orientation != value.Value)
                    {
                        Geometry.orientation = value.Value;
                    }
                }
            }
        }
        private bool _oppositeSide;
        public bool OppositeSide {
            get => _oppositeSide;
            set
            {
                _oppositeSide = value;
                if(Geometry != null)
                {
                    if(value == true)
                    {
                        Geometry.shell_nozzle_orientation = "Opposide side";
                    }
                    
                }
                
            }
        }
        private bool _sameSide;
        public bool SameSide {
            get => _sameSide;
            set
            {
                _sameSide = value;
                if(Geometry != null)
                {
                    if(value == true)
                    {
                        Geometry.shell_nozzle_orientation = "Same side";
                    }
                }
            }
        }
        private KeyValuePair<string, string> _exchangersSelector;

        public KeyValuePair<string, string> ExchangersSelector { 
            get { 
                return _exchangersSelector; 
            } 
            set {
                if(value.Key == "annular_space")
                {
                    GridColumnWidth = 120;
                } else
                {
                    GridColumnWidth = 0;
                }
                _exchangersSelector = value;
                if (Geometry != null)
                {
                    Geometry.head_exchange_type = value.Key;
                }
            }
        }

        private KeyValuePair<int, string> _tubeProfileSelector;
        public KeyValuePair<int, string> TubeProfileSelector
        {
            get => _tubeProfileSelector;
            set
            {
                _tubeProfileSelector = value;
                if(Geometry != null)
                {
                    if(Geometry.tube_profile_tubes_side != value.Value)
                    {
                        Geometry.tube_profile_tubes_side = value.Value;
                    }
                }
            }
        }
        private bool _fixed;
        public bool Fixed
        {
            get => _fixed;
            set
            {
                _fixed = value;
                if(value == true)
                {
                    if(Geometry != null)
                    {
                        Geometry.bundle_type = "Fixed";
                    }
                    
                }
                
            }
        }
        private bool _removable;
        public bool Removable
        {
            get => _removable;
            set
            {
                _removable = value;
                if(Geometry != null)
                {
                    if (value == true)
                    {
                        Geometry.bundle_type = "Removable";

                    }
                }
                
            }
        }
        private bool _rollerExpanded;
        public bool RollerExpanded
        {
            get => _rollerExpanded;
            set
            {
                _rollerExpanded = value;
                if(Geometry != null)
                {
                    if (value == true)
                    {
                        Geometry.roller_expanded = "Yes";
                    }
                    else
                    {
                        Geometry.roller_expanded = "No";
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
        public ObservableCollection<string> DivPlateItems { get; set; }

        private string _divPlateItem;
        public string DivPlateItem
        {
            get => _divPlateItem;
            set
            {
                _divPlateItem = value;
                if (Geometry != null)
                {
                    Geometry.tube_plate_layout_div_plate_layout = value;
                }
                if (value == "Mechanised")
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
                if(Geometry != null)
                {
                    Geometry.tube_plate_layout_sealing_type = value.Value;
                }
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
            Exchangers = new Dictionary<string, string>();
            Orientations = new Dictionary<int, string>();
            Materials = new Dictionary<int, string>();
            DivPlateItems = new ObservableCollection<string>();
            SealingTypeItems = new Dictionary<int, string>();
            TubePlateLayouts = new Dictionary<int, TubeplateLayout>();
            TubeProfile = new Dictionary<int, string>();
            TubeplateLayout optimize = new TubeplateLayout
            {
                ImageUrl = "",
                Name = "<Optimize>"
            };
            TubeplateLayout triangular = new TubeplateLayout
            {
                ImageUrl = "../Visual/Triangilar.png",
                Name = "Triangular (30º)"
            };
            TubeplateLayout triangularCentred = new TubeplateLayout
            {
                ImageUrl = "../Visual/Triangular_centred.png",
                Name = "Triangular Centered (30º)"
            };
            TubeplateLayout squared = new TubeplateLayout
            {
                ImageUrl = "../Visual/Squared.png",
                Name = "Squared (90º)"
            };
            TubeplateLayout squaredCentred = new TubeplateLayout
            {
                ImageUrl = "../Visual/Squared_centred.png",
                Name = "Squared Centred (90º)"
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

            Exchangers.Add("tube_shell", "Tube/Shell");
            Exchangers.Add("annular_space", "Annular Space");
            Exchangers.Add("unicus", "Unicus");
            Exchangers.Add("r_series", "R Series");

            Orientations.Add(0, "Horizontal");
            Orientations.Add(1, "Vertical");

            //DivPlateItems.Add(0, "Horizontal");
            //DivPlateItems.Add(1, "Mechanised");

            SealingTypeItems.Add(0, "O'Rings + Housing");
            SealingTypeItems.Add(1, "Gasket");

            TubeProfile.Add(0, "Smooth Tube");
            TubeProfile.Add(1, "Hard Corrugation");

            TubePlateLayouts.Add(0, triangular);
            TubePlateLayouts.Add(1, triangularCentred);
            TubePlateLayouts.Add(2, squared);
            TubePlateLayouts.Add(3, squaredCentred);
            TubePlateLayouts.Add(4, rotatedSquared);
            TubePlateLayouts.Add(5, rotatedSquaredCentred);
            TubePlateLayouts.Add(6, optimize);
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
