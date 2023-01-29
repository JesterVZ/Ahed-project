using Ahed_project.MasterData.BafflesClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class BufflesPageViewModel : BindableBase
    {
        public Dictionary<string, string> Type { get; set; }
        public Dictionary<string, string> BaffleType { get; set; } // No baffles, Standard heat transfer with SUPPORT baffles, Full baffles heat transfer calculation
        public Dictionary<string, string> CutDirection { get; set; }
        public Visibility ColumnVisibility { get; set; }
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
        public BaffleFull Baffle {
            get => _baffle;
            set { 
                _baffle = value;
                SelectedBaffleType = BaffleType.FirstOrDefault(x => x.Key == value?.method);
            } 
        }
        public BufflesPageViewModel()
        {
            Type = new Dictionary<string, string>();
            BaffleType = new Dictionary<string, string>();
            CutDirection = new Dictionary<string, string>();
            Baffle = new BaffleFull();
            ArrowAngle = "0";
            Type.Add("single_segmental", "Single Segmental");
            Type.Add("double_segmental", "Double Segmental");

            CutDirection.Add("horizontal", "Horizontal");
            CutDirection.Add("vertical", "Vertical");

            BaffleType.Add("no_baffles", "No baffles");
            BaffleType.Add("standard_heat_transfer_with_support_baffles", "Standard heat transfer with SUPPORT baffles");
            BaffleType.Add("full_baffles_heat_transfer_calculation", "Full baffles heat transfer calculation");
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
                if(value.Key == "double_segmental")
                {
                    SingleSegmentalIsEnables = 0;
                    DoubleSegmentalIsEnables = 37;
                }
            }
        }

        private KeyValuePair<string, string> _selectedBaffleType;
        public KeyValuePair<string, string> SelectedBaffleType
        {
            get => _selectedBaffleType;
            set
            {
                Baffle.method = value.Key;
                _selectedBaffleType = value;
                if(value.Key == "no_baffles" || value.Key == "standard_heat_transfer_with_support_baffles")
                {
                    ColumnVisibility = Visibility.Hidden;
                } else
                {
                    ColumnVisibility = Visibility.Visible;
                }
            }
        }
        #endregion

        #region commands
        public ICommand ToggleCommand => new DelegateCommand(() =>
        {
            IsOpen = !IsOpen;

        });

        public ICommand CalculateCommand => new DelegateCommand(() => {
            if(Baffle.method == null)
            {
                Baffle.method = "no_baffles";
                Baffle.type = "single_segmental";
                Baffle.buffle_cut_diraction = "horizontal";
            }
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CalculateBaffle(Baffle));
        });
        #endregion



        public void Raise(string name)
        {
            RaisePropertiesChanged(name);
        }
    }
}
