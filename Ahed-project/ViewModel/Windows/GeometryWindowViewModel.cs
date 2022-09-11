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
        public List<GeometryFull> BeforeSearch { get; set; }
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
        private string _textBox;
        public string TextBox
        {
            get => _textBox;
            set
            {
                _textBox = value;
                var low = value.ToLower();
                Geometries = new ObservableCollection<GeometryFull>(BeforeSearch.Where(x => x.name.ToLower().Contains(low)));
            }
        }
            #region coms
        public ICommand WindowLoaded => new DelegateCommand(() =>
        {
            BeforeSearch = new List<GeometryFull>(GlobalDataCollectorService.GeometryCollection);
            Geometries = new ObservableCollection<GeometryFull>(BeforeSearch);
        });

        public ICommand SelectGeometryCommand => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SelectGeometry(SelectedGeometry);
            GlobalFunctionsAndCallersService.ChangePage(4);
        });
        #endregion

    }
}
