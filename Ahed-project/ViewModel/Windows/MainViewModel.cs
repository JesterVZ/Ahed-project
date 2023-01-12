using Ahed_project.Pages;
using Ahed_project.Services;
using DevExpress.Mvvm;
using System;
using System.Windows.Controls;

namespace Ahed_project.ViewModel.Windows
{
    public class MainViewModel : BindableBase
    {
        public Page FramePage { get; set; }
        public string Title { get; set; }
        public MainViewModel()
        {
            FramePage = new Page();
        }

        public Action CloseWindow { get; set; }
    }
}
