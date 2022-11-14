using Ahed_project.MasterData.GeometryClasses;
using Ahed_project.Services.Global;
using Ahed_project.Services.Global.Content;
using Ahed_project.Services.Global.Interface;
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
        private IUnitedStorage _storage;
        public GeometryWindowViewModel(IUnitedStorage storage)
        {
            _storage= storage;
        }
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
            BeforeSearch = new List<GeometryFull>(_storage.GetGeometries());
            Geometries = new ObservableCollection<GeometryFull>(BeforeSearch);
        });

        public ICommand SelectGeometryCommand => new DelegateCommand(() =>
        {
            _storage.UpdateGeometry(SelectedGeometry);
            _storage.ChangePage(4);
        });
        #endregion

    }
}
