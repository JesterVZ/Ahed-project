using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class ReportsPageViewModel : BindableBase
    {
        #region commands
        public ICommand CreateReport => new DelegateCommand(() =>
        {
            Task.Run(() => GlobalFunctionsAndCallersService.CreateFullReport());
        });
        #endregion
    }
}
