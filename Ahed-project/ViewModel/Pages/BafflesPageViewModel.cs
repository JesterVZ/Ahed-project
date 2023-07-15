using Ahed_project.MasterData;
using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.Services.Global;
using Ahed_project.Settings;
using DevExpress.Mvvm;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class BafflesPageViewModel : BindableBase
    {
        public Dictionary<string, string> Type { get; set; }
        public Dictionary<string, EnabledLabel> BaffleType { get; set; } // No baffles, Standard heat transfer with SUPPORT baffles, Full baffles heat transfer calculation
        public Dictionary<string, string> CutDirection { get; set; }
        public Visibility ColumnVisibility { get; set; }

        private int _inlet_baffle_spacing_is_edit;
        public int inlet_baffle_spacing_is_edit
        {
            get => _inlet_baffle_spacing_is_edit;
            set
            {
                _inlet_baffle_spacing_is_edit = value;
                Baffle.inlet_baffle_spacing_is_edit = value;
            }
        }

        private int _outlet_baffle_spacing_is_edit;
        public int outlet_baffle_spacing_is_edit
        {
            get => _outlet_baffle_spacing_is_edit;
            set
            {
                _outlet_baffle_spacing_is_edit = value;
                Baffle.outlet_baffle_spacing_is_edit = value;
            }
        }

        private int _number_of_baffles_is_edit;
        public int number_of_baffles_is_edit
        {
            get => _number_of_baffles_is_edit;
            set
            {
                _number_of_baffles_is_edit = value;
                Baffle.number_of_baffles_is_edit = value;
            }
        }


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
        public double SingleSegmentalIsEnables { get; set; }
        public double DoubleSegmentalIsEnables { get; set; }
        private BaffleFull _baffle;
        public BaffleFull Baffle
        {
            get => _baffle;
            set
            {
                _baffle = value;
                SelectedBaffleType = BaffleType.FirstOrDefault(x => x.Key == value?.method);
                NumberOfBaffles = _baffle?.number_of_baffles;
                _notEditedLbi = StringToDoubleChecker.ConvertToDouble(value.inlet_baffle_spacing);
                _notEditedLbo = StringToDoubleChecker.ConvertToDouble(value.outlet_baffle_spacing);
                _notEditedNumberOfBaffles = StringToDoubleChecker.ConvertToDouble(value.number_of_baffles);
            }
        }
        public BafflesPageViewModel()
        {
            Type = new Dictionary<string, string>();
            BaffleType = new Dictionary<string, EnabledLabel>();
            CutDirection = new Dictionary<string, string>();
            Baffle = new BaffleFull();
            ArrowAngle = "0";
            Type.Add("single_segmental", "Single Segmental");
            Type.Add("double_segmental", "Double Segmental");

            CutDirection.Add("horizontal", "Horizontal");
            CutDirection.Add("vertical", "Vertical");

            BaffleType.Add("no_baffles", new EnabledLabel() { Value = "No baffles", Enabled = true });
            BaffleType.Add("standard_heat_transfer_with_support_baffles", new EnabledLabel() { Value = "Standard heat transfer with SUPPORT baffles", Enabled = true });
            BaffleType.Add("full_baffles_heat_transfer_calculation", new EnabledLabel() { Value = "Full baffles heat transfer calculation", Enabled = false });
            ColumnVisibility = Visibility.Hidden;
            SingleSegmentalIsEnables = 37;
            DoubleSegmentalIsEnables = 0;
        }
        #region KeyValuePairs
        private KeyValuePair<string, string> _selectedCutDirection;
        public KeyValuePair<string, string> SelectedCutDirection
        {
            get => _selectedCutDirection;
            set
            {
                Baffle.buffle_cut_diraction = value.Key;
                _selectedCutDirection = value;
            }
        }

        private KeyValuePair<string, string> _selectedType;
        public KeyValuePair<string, string> SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                Baffle.type = value.Key;
                if (value.Key == "single_segmental")
                {
                    SingleSegmentalIsEnables = 37;
                    DoubleSegmentalIsEnables = 0;
                }
                if (value.Key == "double_segmental")
                {
                    SingleSegmentalIsEnables = 0;
                    DoubleSegmentalIsEnables = 37;
                }
            }
        }

        private KeyValuePair<string,EnabledLabel> _selectedBaffleType;
        public KeyValuePair<string,EnabledLabel> SelectedBaffleType
        {
            get => _selectedBaffleType;
            set
            {
                if (Baffle != null)
                {
                    Baffle.method = value.Key;
                }
                _selectedBaffleType = value;
                if (value.Key == "no_baffles" || value.Key == "standard_heat_transfer_with_support_baffles")
                {
                    ColumnVisibility = Visibility.Hidden;
                }
                else
                {
                    ColumnVisibility = Visibility.Visible;
                }
            }
        }
        #endregion

        private string _numberOfBaffles;
        public string NumberOfBaffles
        {
            get => _numberOfBaffles;
            set
            {
                _numberOfBaffles = value;
                Baffle.number_of_baffles = value;
                var converted = StringToDoubleChecker.ConvertToDouble(value);
                if (converted == 1)
                {
                    BaffleType.Clear();
                    BaffleType.Add("no_baffles", new EnabledLabel() { Value = "No baffles", Enabled = true });
                    SelectedBaffleType = BaffleType.First();
                }
                else
                {
                    if (BaffleType.Count == 1)
                    {
                        BaffleType.Add("standard_heat_transfer_with_support_baffles", new EnabledLabel() { Value = "Standard heat transfer with SUPPORT baffles", Enabled = true });
                        BaffleType.Add("full_baffles_heat_transfer_calculation", new EnabledLabel() { Value = "Full baffles heat transfer calculation", Enabled = false });
                    }
                }
                RaisePropertyChanged(nameof(BaffleType));
            }
        }

        #region commands
        public ICommand ToggleCommand => new DelegateCommand(() =>
        {
            IsOpen = !IsOpen;

        });

        private double _notEditedLbi = 0;
        private double _notEditedLbo = 0;
        private double _notEditedNumberOfBaffles = 0;

        public ICommand CalculateCommand => new DelegateCommand(() =>
        {
            if (Baffle.method == null)
            {
                Baffle.method = "no_baffles";
                Baffle.type = "single_segmental";
                Baffle.buffle_cut_diraction = "horizontal";
            }
            Baffle.inlet_baffle_spacing_is_edit = StringToDoubleChecker.ConvertToDouble(Baffle.inlet_baffle_spacing) != _notEditedLbi ? 1 : 0;
            Baffle.outlet_baffle_spacing_is_edit = StringToDoubleChecker.ConvertToDouble(Baffle.outlet_baffle_spacing) != _notEditedLbo ? 1 : 0;
            Baffle.number_of_baffles_is_edit = StringToDoubleChecker.ConvertToDouble(Baffle.number_of_baffles)!=_notEditedNumberOfBaffles? 1 : 0;
            Task.Run(() => GlobalFunctionsAndCallersService.CalculateBaffle(Baffle));
        });

        public ICommand RestoreDefaults => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.RestoreDefaultBaffles();
        });
        #endregion

        public void Refresh()
        {
            RaisePropertiesChanged(String.Empty);
        }

        public void Raise(string name)
        {
            RaisePropertiesChanged(name);
        }

        private int _oldCount = 2;
        public void ShowFull(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            var type = typeof(BafflesPageViewModel);
            var field = type.GetProperty(name);
            object value = null;
            if (field == null)
            {
                type = typeof(BaffleFull);
                field = type.GetProperty(name);
                value = field.GetValue(Baffle);
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
            if (type == typeof(BaffleFull))
            {
                Baffle.OnPropertyChanged(name);
            }
            else
            {
                Raise(name);
            }
        }

        public void RaiseDeep(string name, bool isReadOnly, string text, int alternateValue)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            Config.NumberOfDecimals = _oldCount;
            var type = typeof(BafflesPageViewModel);
            var field = type.GetProperty(name);
            if (!isReadOnly)
            {
                if (field == null)
                {
                    type = typeof(BaffleFull);
                    field = type.GetProperty(name);
                    try
                    {
                        field.SetValue(Baffle, text);
                    }
                    catch
                    {
                        field.SetValue(Baffle, alternateValue);
                    }
                    Baffle.OnPropertyChanged(name);
                }
                else
                {
                    try
                    {
                        field.SetValue(this, text);
                    }
                    catch
                    {
                        field.SetValue(this, alternateValue);
                    }
                    Refresh();
                }
            }
            else
            {
                if (field == null)
                {
                    Baffle.OnPropertyChanged(name);
                }
                else
                {
                    Refresh();
                }
            }
        }

        private Visibility _notAnnularVisibility;

        public Visibility NotAnnularVisibility
        {
            get => _notAnnularVisibility;
            set => _notAnnularVisibility = value;
        }

        private Visibility _annularVisibility;

        public Visibility AnnularVisibility
        {
            get => _annularVisibility;
            set => _annularVisibility = value;
        }
    }
}
