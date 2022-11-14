using Ahed_project.MasterData;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.Services.Global.Content;
using Ahed_project.Services.Global.Interface;
using DevExpress.DXBinding.Native;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Ahed_project.ViewModel.Pages
{
    public class GeometryPageViewModel : BindableBase
    {
        public bool IsOpen { get; set; }
        public double GridColumnWidth { get; set; }
        public Dictionary<string, string> Exchangers { get; set; }
        public Dictionary<string, string> Orientations { get; set; }
        public Dictionary<int, Material> Materials { get; set; }
        public Dictionary<string, string> TubeProfile { get; set; }
        public Dictionary<string, TubeplateLayout> TubePlateLayouts { get; set; }
        private string _tube_plate_layout_number_of_passes { get; set; }
        public string Tube_plate_layout_number_of_passes
        {
            get => _tube_plate_layout_number_of_passes;
            set
            {
                _tube_plate_layout_number_of_passes = value;
                List<string> divplate = new List<string>(); ;
                switch (value)
                {
                    case "1":
                        divplate.Add("None");
                        break;
                    case "2":
                        divplate.Add("Horizontal");
                        divplate.Add("Vertical");
                        divplate.Add("Mechanised");
                        break;
                    case "3":
                        divplate.Add("Horizontal");
                        divplate.Add("Vertical");
                        divplate.Add("Mechanised");
                        break;
                    case "4":
                        divplate.Add("Horizontal");
                        divplate.Add("Horizontal + Vertical");
                        divplate.Add("Type 3");
                        divplate.Add("Mechanised");
                        break;
                    case "5":
                        divplate.Add("Horizontal");
                        divplate.Add("Mechanised");
                        break;
                    case "6":
                        divplate.Add("Type 1");
                        divplate.Add("Type 2");
                        divplate.Add("Type 3");
                        divplate.Add("Mechanised");
                        break;
                    case "7":
                        divplate.Add("Mechanised");
                        break;
                    case "8":
                        divplate.Add("Type 1");
                        divplate.Add("Type 2");
                        divplate.Add("Type 3");
                        divplate.Add("Mechanised");
                        break;
                    case "9":
                        divplate.Add("Mechanised");
                        break;
                }
                DivPlateItems = new ObservableCollection<string>(divplate);
                if (DivPlateItems.Count>0)
                {
                    DivPlateItem = DivPlateItems.First();
                }
            }
        }

        private KeyValuePair<int, Material> _shellMaterial;

        public KeyValuePair<int, Material> ShellMaterial
        {
            get => _shellMaterial;
            set
            {
                _shellMaterial = value;
                if (value.Value != null && Data.Geometry != null)
                {
                    Data.Geometry.material_shell_side = value.Value.name_short;
                }
            }
        }

        private KeyValuePair<int, Material> _tubesMaterial;

        public KeyValuePair<int, Material> TubesMaterial
        {
            get => _tubesMaterial;
            set
            {
                _tubesMaterial = value;
                if (value.Value != null && Data.Geometry != null)
                {
                    Data.Geometry.material_tubes_side = value.Value.name_short;
                }
            }
        }
        private KeyValuePair<string, TubeplateLayout> _tubeLayout;
        public KeyValuePair<string, TubeplateLayout> TubeLayout
        {
            get => _tubeLayout;
            set
            {
                _tubeLayout = value;
                if (Data.Geometry != null)
                {
                    if (value.Value != null)
                    {
                        Data.Geometry.tube_plate_layout_tube_layout = value.Key;
                    }

                }
            }

        }

        private KeyValuePair<string, string> _orientation;
        public KeyValuePair<string, string> Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                Data.Geometry.orientation = value.Key;
            }
        }
        private bool _oppositeSide;
        public bool OppositeSide
        {
            get => _oppositeSide;
            set
            {
                _oppositeSide = value;
                if (Data.Geometry != null)
                {
                    if (value == true)
                    {
                        Data.Geometry.shell_nozzle_orientation = "opposite_side";
                    }

                }

            }
        }
        private bool _sameSide;
        public bool SameSide
        {
            get => _sameSide;
            set
            {
                _sameSide = value;
                if (Data.Geometry != null)
                {
                    if (value == true)
                    {
                        Data.Geometry.shell_nozzle_orientation = "same_side";
                    }
                }
            }
        }
        private KeyValuePair<string, string> _exchangersSelector;

        public KeyValuePair<string, string> ExchangersSelector
        {
            get
            {
                return _exchangersSelector;
            }
            set
            {
                if (value.Key == "annular_space")
                {
                    GridColumnWidth = 120;
                }
                else
                {
                    GridColumnWidth = 0;
                }
                _exchangersSelector = value;
                if (Data.Geometry != null)
                {
                    Data.Geometry.head_exchange_type = value.Key;
                }
            }
        }

        private KeyValuePair<string, string> _tubeProfileSelector;
        public KeyValuePair<string, string> TubeProfileSelector
        {
            get => _tubeProfileSelector;
            set
            {
                _tubeProfileSelector = value;
                if (Data.Geometry != null)
                {
                    Data.Geometry.tube_profile_tubes_side = value.Key;
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
                
                if (value == true)
                {
                    _storage.SetBaffle("3", "0");
                    Data.Geometry.bundle_type = "fixed";
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
                if (value == true)
                {
                    _storage.SetBaffle("6", "0");
                    Data.Geometry.bundle_type = "removable";
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
                if (Data.Geometry != null)
                {
                    if (value == true)
                    {
                        Data.Geometry.roller_expanded = "1";
                    }
                    else
                    {
                        Data.Geometry.roller_expanded = "0";
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
                /*
                
                if (Geometry != null)
                {
                    Geometry.tube_plate_layout_div_plate_layout = value;
                }
                */
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

        public Dictionary<string, string> SealingTypeItems { get; set; }

        private KeyValuePair<string, string> _sealingTypeItem;

        public KeyValuePair<string, string> SealingTypeItem
        {
            get => _sealingTypeItem;
            set
            {
                _sealingTypeItem = value;
                if (Data.Geometry != null)
                {
                    Data.Geometry.tube_plate_layout_sealing_type = value.Key;
                }
                if (_sealingTypeItem.Value == "O'Rings + Housing")
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

        private readonly IUnitedStorage _storage;

        public GeometryPageViewModel(IUnitedStorage storage)
        {
            _storage = storage;
            Exchangers = new Dictionary<string, string>();
            Orientations = new Dictionary<string, string>();
            Materials = new Dictionary<int, Material>();
            DivPlateItems = new ObservableCollection<string>();
            SealingTypeItems = new Dictionary<string, string>();
            TubePlateLayouts = new Dictionary<string, TubeplateLayout>();
            TubeProfile = new Dictionary<string, string>();
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
            Tube_plate_layout_number_of_passes = "1";
            Exchangers.Add("tube_shell", "Tube/Shell");
            Exchangers.Add("annular_space", "Annular Space");
            Exchangers.Add("unicus", "Unicus");
            Exchangers.Add("r_series", "R Series");

            Orientations.Add("horizontal", "Horizontal");
            Orientations.Add("vertical", "Vertical");

            //DivPlateItems.Add(0, "Horizontal");
            //DivPlateItems.Add(1, "Mechanised");

            SealingTypeItems.Add("o_rings_housing", "O'Rings + Housing");
            SealingTypeItems.Add("gasket", "Gasket");

            TubeProfile.Add("smooth_tube", "Smooth Tube");
            TubeProfile.Add("hard_corrugation", "Hard Corrugation");

            TubePlateLayouts.Add("triangular", triangular);
            TubePlateLayouts.Add("triangular_centered", triangularCentred);
            TubePlateLayouts.Add("squared", squared);
            TubePlateLayouts.Add("squared_centered", squaredCentred);
            TubePlateLayouts.Add("rotated_squared", rotatedSquared);
            TubePlateLayouts.Add("rotated_squared_centered", rotatedSquaredCentred);
            TubePlateLayouts.Add("optimize", optimize);
            SealingTypeVis = Visibility.Hidden;
            HousingSpaceVis = Visibility.Hidden;
        }

        public GeometryInGlobal Data
        {
            get => _storage.GetGeometryData();
            set
            {
                ExchangersSelector = Exchangers.FirstOrDefault(x => x.Key == value.Geometry.head_exchange_type);
                Orientation = Orientations.FirstOrDefault(x => x.Value == value.Geometry.orientation);
                TubeProfileSelector = TubeProfile.FirstOrDefault(x => x.Value == value.Geometry.tube_profile_tubes_side);
                ShellMaterial = Materials.FirstOrDefault(x => x.Value.name == value.Geometry.material_shell_side);
                TubesMaterial = Materials.FirstOrDefault(x => x.Value.name == value.Geometry.material_tubes_side);
                TubeLayout = TubePlateLayouts.FirstOrDefault(x => x.Value.Name == value.Geometry.tube_plate_layout_tube_layout);
                DivPlateItem = DivPlateItems.FirstOrDefault(x => x == value.Geometry.tube_plate_layout_div_plate_layout);
                try
                {
                    double outer_diameter_tubes_side = Convert.ToDouble(value.Geometry.outer_diameter_tubes_side, CultureInfo.InvariantCulture);
                    if (outer_diameter_tubes_side <= 25)
                    {
                        _storage.SetBaffle("0", "0.3");
                    }
                    if (outer_diameter_tubes_side > 25)
                    {
                        _storage.SetBaffle("0", "0.4");
                    }
                    RaisePropertiesChanged("GeometryPageViewModel");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }


                switch (value.Geometry.bundle_type)
                {
                    case "Fixed":
                        Fixed = true;
                        Removable = false;
                        value.Geometry.bundle_type = "fixed";
                        break;
                    case "Removable":
                        Removable = true;
                        Fixed = false;
                        value.Geometry.bundle_type = "removable";
                        break;
                    case "":
                        Fixed = true;
                        Removable = false;
                        break;
                }

                switch (value.Geometry.roller_expanded)
                {
                    case "No":
                        RollerExpanded = false;
                        break;
                    case "Yes":
                        RollerExpanded = true;
                        break;
                }

                switch (value.Geometry.shell_nozzle_orientation)
                {
                    case "Opposite side":
                        OppositeSide = true;
                        SameSide = false;
                        break;
                    case "Same side":
                        SameSide = true;
                        OppositeSide = false;
                        break;
                }
                if (value.Geometry.bundle_type == null || value.Geometry.bundle_type == "")
                {
                    Fixed = true;
                    Removable = false;
                }
                if (value.Geometry.shell_nozzle_orientation == null || value.Geometry.shell_nozzle_orientation == "")
                {
                    OppositeSide = true;
                    SameSide = false;
                }
                if (value.Geometry.tube_plate_layout_number_of_passes == null || value.Geometry.tube_plate_layout_number_of_passes == "")
                {
                    Tube_plate_layout_number_of_passes = "1";
                }
                _storage.SetGeometryData(value);
            }
        }

        #region commands

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;

        });

        public ICommand Calculate => new DelegateCommand(() =>
        {
            if (OppositeSide == true) //небольшой костыль, я потом поправлю
            {
                Data.Geometry.shell_nozzle_orientation = "opposite_side";
            }
            if (SameSide == true)
            {
                Data.Geometry.shell_nozzle_orientation = "same_side";
            }

            switch (Data.Geometry.roller_expanded)
            {
                case "No":
                    Data.Geometry.roller_expanded = "0";
                    break;
                case "Yes":
                    Data.Geometry.roller_expanded = "1";
                    break;
            }

            switch (DivPlateItem)
            {
                case "None":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "none";
                    break;
                case "Horizontal":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "horizontal";
                    break;
                case "Vertical":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "vertical";
                    break;
                case "Mechanised":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "mechanised";
                    break;
                case "Type_1":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "type_1";
                    break;
                case "Type_2":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "type_2";
                    break;
                case "Type_3":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "type_3";
                    break;
                case "Horizontal + Vertical":
                    Data.Geometry.tube_plate_layout_div_plate_layout = "horizontal_vertical";
                    break;
            }
            switch(SealingTypeItem.Value)
            {
                case "O'Rings + Housing":
                    Data.Geometry.tube_plate_layout_sealing_type = "o_rings_housing";
                    break;
                case "Gasket":
                    Data.Geometry.tube_plate_layout_sealing_type = "gasket";
                    break;
            }
            Task.Factory.StartNew(() => _storage.CalculateGeometry(Data.Geometry));
        });

        #endregion
    }
}
