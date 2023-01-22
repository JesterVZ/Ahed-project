using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class ReportsPageViewModel : BindableBase
    {
        #region commands
        public ICommand CreateReport => new DelegateCommand(() => {
            Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CreateFullReport());
        });
        #endregion
    }
}
