using Ahed_project.MasterData.MainClasses;
using Ahed_project.Pages;
using Ahed_project.Services;
using Ahed_project.Services.Global.Interface;
using DevExpress.Mvvm;
using System.Windows.Controls;

namespace Ahed_project.ViewModel.Windows
{
    public class MainViewModel : BindableBase
    {
        private readonly IUnitedStorage _unitedStorage;
        public MainInGlobal Data
        {
            get => _unitedStorage.GetMainData();
            set => _unitedStorage.SetMainData(value);
        }
        public MainViewModel(IUnitedStorage unitedStorage)
        {            
            _unitedStorage= unitedStorage;
            Data.FramePage = new Page();
        }
    }
}
