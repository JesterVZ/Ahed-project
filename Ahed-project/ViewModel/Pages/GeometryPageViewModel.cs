using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class GeometryPageViewModel : BindableBase
    {
        public bool IsOpen { get; set; }

        public bool IsColumnVisible { get; set; }

        #region coms

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;
            IsColumnVisible = !IsColumnVisible;
        });

        #endregion
    }
}
