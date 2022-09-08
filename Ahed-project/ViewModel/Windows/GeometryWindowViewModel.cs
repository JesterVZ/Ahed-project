using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Windows
{
    public class GeometryWindowViewModel : BindableBase
    {
        public ObservableCollection<GeometryFull> Geometries {get; set;}
        #region coms
        public ICommand WindowLoaded => new DelegateCommand(() =>
        {
            Geometries = new ObservableCollection<GeometryFull>(GlobalDataCollectorService.GeometryCollection);
        });
        #endregion

    }
}
