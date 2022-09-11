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
        public GeometryWindowViewModel() { }
        public ObservableCollection<GeometryFull> Geometries {get; set;}
        public bool IsGeometrySelected { get; set;}
        private GeometryFull _geometry;
        public GeometryFull SelectedGeometry
        {
            get
            {
                return _geometry;
            }
            set
            {
                IsGeometrySelected = true;
                _geometry = value;
            }
        }
        #region coms
        public ICommand WindowLoaded => new DelegateCommand(() =>
        {
            Geometries = new ObservableCollection<GeometryFull>(GlobalDataCollectorService.GeometryCollection);
        });

        public ICommand SelectGeometryCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SelectGeometry(SelectedGeometry);
            GlobalFunctionsAndCallersService.ChangePage(4);
        });
        #endregion

    }
}
