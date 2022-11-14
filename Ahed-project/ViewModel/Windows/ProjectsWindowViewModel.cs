using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
using Ahed_project.Services.Global.Content;
using Ahed_project.Services.Global.Interface;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Windows
{
    public class ProjectsWindowViewModel : BindableBase
    {
        private readonly IUnitedStorage _storage;
        public ProjectsWindowViewModel(IUnitedStorage storage)
        {
            _storage= storage;
        }

        public ICommand SelectProject => new DelegateCommand(() =>
        {
            _storage.SetProject(SelectedProject);
            _storage.ChangePage(0);
        });

        public ProjectInfoGet SelectedProject { get; set; }

        private ObservableCollection<ProjectInfoGet> _projects;

        public ObservableCollection<ProjectInfoGet> Projects 
        {
            get
            {
                if (_projects == null)
                {
                    _projects = new ObservableCollection<ProjectInfoGet>(_storage.GetProjects());
                }
                return _projects;
            }
            set
            {
                _projects= value;
            }
        }

        private string _searchBox;
        public string SearchBox
        {
            get => _searchBox;
            set
            {
                _searchBox = value;
                SearchCondition();
            }
        }

        public ICommand WindowLoaded => new DelegateCommand(() =>
        {
            SearchCondition();
        });

        public void SearchCondition()
        {
            if (string.IsNullOrEmpty(SearchBox))
            {
                Projects = new ObservableCollection<ProjectInfoGet>(_storage.GetProjects());
            }
            else
            {
                var lowSB = _searchBox.ToLower();
                Projects = new ObservableCollection<ProjectInfoGet>(_storage.GetProjects().Where
                    (x => (x.name?.ToLower().Contains(lowSB) ?? false) || (x.customer?.ToLower().Contains(lowSB) ?? false)
                    || (x.description?.ToLower().Contains(lowSB) ?? false) || (x.category?.ToLower().Contains(lowSB) ?? false)
                    || (x.keywords?.ToLower().Contains(lowSB) ?? false) || (x.comments?.ToLower().Contains(lowSB) ?? false)));
            }
        }
    }
}
