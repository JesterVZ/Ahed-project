﻿using DevExpress.Mvvm;
using System;

namespace Ahed_project.ViewModel.Windows
{
    public class PresetsWindowViewModel : BindableBase
    {
        public PresetsWindowViewModel()
        {

        }

        public Action CloseAction { get; set; }
    }
}
