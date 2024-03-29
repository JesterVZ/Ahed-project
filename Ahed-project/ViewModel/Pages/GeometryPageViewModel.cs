﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.Services.Global;
using Ahed_project.Settings;
using Ahed_project.Windows;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class GeometryPageViewModel : BindableBase
    {
        private bool _isOpen;
        public bool IsOpen
        {

            get => _isOpen;
            set
            {
                _isOpen = value;
                if (value == true)
                {
                    ArrowAngle = "180";
                }
                else
                {
                    ArrowAngle = "0";
                }

            }
        }
        public string ArrowAngle { get; set; }
        public double GridColumnWidth { get; set; }
        public Dictionary<string, string> Exchangers { get; set; }
        public Dictionary<string, string> Orientations { get; set; }
        public Dictionary<int, Material> Materials { get; set; }
        public Dictionary<string, string> TubeProfile { get; set; }
        public Dictionary<string, TubeplateLayout> TubePlateLayouts { get; set; }
        public Dictionary<string, string> DivPlateItems { get; set; }
        private string _tube_plate_layout_number_of_passes { get; set; }
        public string Tube_plate_layout_number_of_passes
        {
            get => _tube_plate_layout_number_of_passes;
            set
            {
                _tube_plate_layout_number_of_passes = value;
                //ObservableCollection<string> divplate = new ObservableCollection<string>(); ;
                Dictionary<string, string> divplate = new();
                switch (value)
                {
                    case "1":
                        divplate.Add("none", "None");
                        break;
                    case "2":
                        divplate.Add("horizontal", "Horizontal");
                        divplate.Add("vertical", "Vertical");
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "3":
                        divplate.Add("horizontal", "Horizontal");
                        divplate.Add("vertical", "Vertical");
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "4":
                        divplate.Add("horizontal", "Horizontal");
                        divplate.Add("horizontal_vertical", "Horizontal + Vertical");
                        divplate.Add("type_3", "Type 3");
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "5":
                        divplate.Add("horizontal", "Horizontal");
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "6":
                        divplate.Add("type_1", "Type 1");
                        divplate.Add("type_2", "Type 2");
                        divplate.Add("type_3", "Type 3");
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "7":
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "8":
                        divplate.Add("type_1", "Type 1");
                        divplate.Add("type_2", "Type 2");
                        divplate.Add("type_3", "Type 3");
                        divplate.Add("mechanised", "Mechanised");
                        break;
                    case "9":
                        divplate.Add("mechanised", "Mechanised");
                        break;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DivPlateItems.Clear();
                    DivPlateItems = divplate;
                });
                if (DivPlateItems.Count > 0)
                {
                    DivPlateItem = DivPlateItems.First();
                }
                if (_geometry != null)
                {
                    _geometry.tube_plate_layout_number_of_passes = value;
                }
            }
        }
        private GeometryFull _geometry;
        public GeometryFull Geometry
        {
            get => _geometry;
            set
            {
                _geometry = value;
                var tempExchangeSelector = Exchangers.FirstOrDefault(x => x.Key == value?.head_exchange_type);
                ExchangersSelector = tempExchangeSelector.Key == null ? Exchangers.FirstOrDefault(x => x.Value == value?.head_exchange_type) : tempExchangeSelector;
                var tempOrientation = Orientations.FirstOrDefault(x => x.Key == value?.orientation);
                Orientation = tempOrientation.Key == null ? Orientations.FirstOrDefault(x => x.Value == value?.orientation) : tempOrientation;
                var tempTubesProfile = TubeProfile.FirstOrDefault(x => x.Key == value?.tube_profile_tubes_side);
                TubeProfileSelector = tempTubesProfile.Key == null ? TubeProfile.FirstOrDefault(x => x.Value == value?.tube_profile_tubes_side) : tempTubesProfile;
                ShellMaterial = Materials.FirstOrDefault(x => x.Value.name_short == value?.material_shell_side);
                TubesMaterial = Materials.FirstOrDefault(x => x.Value.name_short == value?.material_tubes_side);
                var tempLayout = TubePlateLayouts.FirstOrDefault(x => x.Key == value?.tube_plate_layout_tube_layout);
                TubeLayout = tempLayout.Key == null ? TubePlateLayouts.FirstOrDefault(x => x.Value.Name == value?.tube_plate_layout_tube_layout) : tempLayout;

                try
                {
                    double outer_diameter_tubes_side = Convert.ToDouble(StringToDoubleChecker.ConvertFromInvariantCulture(_geometry?.outer_diameter_tubes_side));
                    if (outer_diameter_tubes_side <= 25)
                    {
                        GlobalFunctionsAndCallersService.SetDiametralTubeDefaultValue("0.3");
                    }
                    if (outer_diameter_tubes_side > 25)
                    {
                        GlobalFunctionsAndCallersService.SetDiametralTubeDefaultValue("0.4");
                    }
                    RaisePropertiesChanged("GeometryPageViewModel");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }


                switch (value?.bundle_type)
                {
                    case "fixed":
                        Fixed = true;
                        Removable = false;
                        _geometry.bundle_type = "fixed";
                        break;
                    case "removable":
                        Removable = true;
                        Fixed = false;
                        _geometry.bundle_type = "removable";
                        break;
                    case "":
                        Fixed = true;
                        Removable = false;
                        break;
                }

                switch (value?.roller_expanded)
                {
                    case "0":
                        RollerExpanded = false;
                        break;
                    case "1":
                        RollerExpanded = true;
                        break;
                }

                switch (value?.shell_nozzle_orientation)
                {
                    case "opposite_side":
                        OppositeSide = true;
                        SameSide = false;
                        break;
                    case "same_side":
                        SameSide = true;
                        OppositeSide = false;
                        break;
                }
                if (value?.bundle_type == null || value?.bundle_type == "")
                {
                    Fixed = true;
                    Removable = false;
                }
                if (value?.shell_nozzle_orientation == null || value?.shell_nozzle_orientation == "")
                {
                    OppositeSide = true;
                    SameSide = false;
                }
                if (value?.tube_plate_layout_number_of_passes == null || value?.tube_plate_layout_number_of_passes == "")
                {
                    Tube_plate_layout_number_of_passes = "1";
                }
                else
                {
                    Tube_plate_layout_number_of_passes = value.tube_plate_layout_number_of_passes;
                }
                DivPlateItem = DivPlateItems.FirstOrDefault(x => x.Key == value?.tube_plate_layout_div_plate_layout);
                if (value?.tube_plate_layout_sealing_type == null)
                {
                    SealingTypeItem = SealingTypeItems.First();
                }
                SealingTypeItem = SealingTypeItems.FirstOrDefault(x => x.Key == value?.tube_plate_layout_sealing_type);
                RaisePropertiesChanged("GeometryPageViewModel");
            }
        }

        private KeyValuePair<int, Material> _shellMaterial;

        public KeyValuePair<int, Material> ShellMaterial
        {
            get => _shellMaterial;
            set
            {
                _shellMaterial = value;
                if (value.Value != null && Geometry != null)
                {
                    Geometry.material_shell_side = value.Value.name_short;
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
                if (value.Value != null && Geometry != null)
                {
                    Geometry.material_tubes_side = value.Value.name_short;
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
                if (Geometry != null)
                {
                    if (value.Value != null)
                    {
                        Geometry.tube_plate_layout_tube_layout = value.Key;
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
                if (Geometry != null)
                {
                    Geometry.orientation = value.Key;
                }
            }
        }
        private bool _oppositeSide;
        public bool OppositeSide
        {
            get => _oppositeSide;
            set
            {
                _oppositeSide = value;
                if (Geometry != null)
                {
                    if (value == true)
                    {
                        Geometry.shell_nozzle_orientation = "opposite_side";
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
                if (Geometry != null)
                {
                    if (value == true)
                    {
                        Geometry.shell_nozzle_orientation = "same_side";
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
                GlobalFunctionsAndCallersService.ChengeRow(value.Key);
                if (Geometry != null)
                {
                    Geometry.head_exchange_type = value.Key;
                }
                if (value.Key == "r_series" || value.Key == "unicus")
                {
                    HousingSpaceVis = Visibility.Hidden;
                    ShowScrapingFrequencyAndMotorPower = Visibility.Visible;
                }
                else
                {
                    HousingSpaceVis = HousingSpaceVis == Visibility.Hidden ? Visibility.Visible : HousingSpaceVis;
                    ShowScrapingFrequencyAndMotorPower = Visibility.Collapsed;
                }
            }
        }

        private Visibility _showScrapingFrequencyAndMotorPower = Visibility.Collapsed;
        public Visibility ShowScrapingFrequencyAndMotorPower
        {
            get => _showScrapingFrequencyAndMotorPower;
            set => _showScrapingFrequencyAndMotorPower = value;
        }

        private KeyValuePair<string, string> _tubeProfileSelector;
        public KeyValuePair<string, string> TubeProfileSelector
        {
            get => _tubeProfileSelector;
            set
            {
                _tubeProfileSelector = value;
                if (Geometry != null)
                {
                    Geometry.tube_profile_tubes_side = value.Key;
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
                    GlobalFunctionsAndCallersService.SetDiametralShellDefaultValue("3");
                    Geometry.bundle_type = "fixed";
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
                    GlobalFunctionsAndCallersService.SetDiametralShellDefaultValue("6");
                    Geometry.bundle_type = "removable";
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
                if (Geometry != null)
                {
                    if (value == true)
                    {
                        Geometry.roller_expanded = "1";
                    }
                    else
                    {
                        Geometry.roller_expanded = "0";
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

        private KeyValuePair<string, string> _divPlateItem;
        public KeyValuePair<string, string> DivPlateItem
        {
            get => _divPlateItem;
            set
            {
                _divPlateItem = value;


                if (value.Value == "Mechanised")
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
                if (Geometry != null)
                {
                    Geometry.tube_plate_layout_sealing_type = value.Key;
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

        public void Refresh()
        {
            RaisePropertiesChanged(String.Empty);
        }

        public GeometryPageViewModel()
        {
            Exchangers = new Dictionary<string, string>();
            Orientations = new Dictionary<string, string>();
            Materials = new Dictionary<int, Material>();
            SealingTypeItems = new();
            TubePlateLayouts = new Dictionary<string, TubeplateLayout>();
            DivPlateItems = new();
            TubeProfile = new Dictionary<string, string>();
            ArrowAngle = "0";
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
                Name = "Squared Centered (90º)"
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
            OppositeSide = true;
        }

        #region commands

        public ICommand ToggleCommand => new DelegateCommand(() =>
        {
            IsOpen = !IsOpen;

        });

        public ICommand ShowTemplate => new DelegateCommand(() =>
        {
            GeometryTemplateWindow window = new GeometryTemplateWindow(Geometry.image_geometry);
            window.ShowDialog();
        });

        public ICommand Calculate => new DelegateCommand<bool>((byButton) =>
        {
            if (OppositeSide == true) //небольшой костыль, я потом поправлю
            {
                Geometry.shell_nozzle_orientation = "opposite_side";
            }
            if (SameSide == true)
            {
                Geometry.shell_nozzle_orientation = "same_side";
            }

            switch (Geometry.roller_expanded)
            {
                case "No":
                    Geometry.roller_expanded = "0";
                    break;
                case "Yes":
                    Geometry.roller_expanded = "1";
                    break;
            }

            if (Geometry.head_exchange_type == null)
            {
                ExchangersSelector = Exchangers.First();
            }
            if (Geometry.material_tubes_side == null)
            {
                TubesMaterial = Materials.First();
            }
            if (Geometry.material_shell_side == null)
            {
                ShellMaterial = Materials.First();
            }
            if (Geometry.orientation == null)
            {
                Orientation = Orientations.First();
            }
            if (Geometry.tube_profile_tubes_side == null)
            {
                TubeProfileSelector = TubeProfile.First();
            }
            if (Geometry.tube_plate_layout_tube_layout == null)
            {
                TubeLayout = TubePlateLayouts.First();
            }
            if (Geometry.tube_plate_layout_sealing_type == null)
            {
                SealingTypeItem = SealingTypeItems.First();
            }
            Geometry.tube_plate_layout_div_plate_layout = DivPlateItem.Key;
            Geometry.tube_plate_layout_sealing_type = SealingTypeItem.Key;
            Task.Run(() => GlobalFunctionsAndCallersService.CalculateGeometry(Geometry, byButton));
        });

        #endregion

        private int _oldCount = 2;
        public void ShowFull(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            var type = typeof(GeometryPageViewModel);
            var field = type.GetProperty(name);
            object value = null;
            if (field == null)
            {
                type = typeof(GeometryFull);
                field = type.GetProperty(name);
                value = field.GetValue(Geometry);
            }
            else
            {
                value = field.GetValue(this);
            }
            if (value == null)
                return;
            if (Config.NumberOfDecimals != 0)
            {
                _oldCount = Config.NumberOfDecimals;
            }
            Config.NumberOfDecimals = 0;
            if (type == typeof(GeometryFull))
            {
                Geometry.OnPropertyChanged(name, false);
            }
            else
            {
                RaisePropertyChanged(name);
            }
        }

        public void RaiseDeep(TextBox tb)
        {
            if (String.IsNullOrEmpty(tb.Name))
            {
                return;
            }
            Config.NumberOfDecimals = _oldCount;
            var type = typeof(GeometryPageViewModel);
            var field = type.GetProperty(tb.Name);
            if (tb.IsReadOnly == false)
            {
                if (field == null)
                {
                    type = typeof(GeometryFull);
                    field = type.GetProperty(tb.Name);
                    var value = field.GetValue(Geometry)?.ToString();
                    if (value != tb.Text)
                    {
                        try
                        {
                            field.SetValue(Geometry, tb.Text);
                        }
                        catch (ArgumentException e)
                        {

                        }
                        Geometry.OnPropertyChanged(tb.Name);
                    }
                }
                else
                {
                    try
                    {
                        field.SetValue(this, tb.Text);
                    }
                    catch (ArgumentException e)
                    {

                    }
                    Refresh();
                }
            }
            else
            {
                if (field == null)
                {
                    Geometry.OnPropertyChanged(tb.Name, false);
                }
                else
                {
                    Refresh();
                }
            }
        }
    }
}
