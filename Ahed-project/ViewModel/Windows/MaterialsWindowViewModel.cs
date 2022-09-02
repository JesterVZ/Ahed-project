using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
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
    public class MaterialsWindowViewModel : BindableBase
    {
        public MaterialsWindowViewModel()
        {
            Materials = new ObservableCollection<Material>();
        }

        public ICommand GetMaterials => new DelegateCommand(() =>
        {
            Materials = new ObservableCollection<Material>(GlobalDataCollectorService.Materials);
        });

        public ObservableCollection<Material> Materials {get;set;}

        public Material SelectedItem { get; set; }
    }
}
