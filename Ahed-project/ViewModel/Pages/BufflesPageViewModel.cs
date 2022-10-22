using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel.Pages
{
    public class BufflesPageViewModel : BindableBase
    {
        public Dictionary<string, string> Type { get; set; }
        public double SingleSegmentalIsEnables { get; set; }
        public double DoubleSegmentalIsEnables { get; set; }
        public BufflesPageViewModel()
        {
            Type = new Dictionary<string, string>();

            Type.Add("single_segmental", "Single Segmental");
            Type.Add("double_segmental", "Double Segmental");
        }

        private KeyValuePair<string, string> _selectedType;
        public KeyValuePair<string, string> SelectedType
        {
            get => _selectedType; 
            set
            {
                _selectedType = value;
            }
        }
    }
}
