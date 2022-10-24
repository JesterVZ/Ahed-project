﻿using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class BufflesPageViewModel : BindableBase
    {
        public Dictionary<string, string> Type { get; set; }
        public bool IsOpen { get; set; }
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
                if(value.Key == "single_segmental")
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

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;

        });

    }
}
