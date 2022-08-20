using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.Products.SingleProduct;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel
{
    public class HeatBalanceViewModel : BindableBase
    {
        public HeatBalanceViewModel()
        {
            TubesProcess = new ObservableCollection<string>
            {
                "sensible_heat",
                "condensation"
            };
            ShellProcess = new ObservableCollection<string>
            {
                "sensible_heat",
                "condensation"
            };
        }
        public string TubesProductName { get; set; }
        public string ShellProductName { get; set; }
        public CalculationFull Calculation { get; set; }
        public ObservableCollection<string> TubesProcess { get; set; }
        public ObservableCollection<string> ShellProcess { get; set; }

        public ICommand Calculate => new DelegateCommand(() =>
        {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.Calculate(Calculation));
        });
    }
}
