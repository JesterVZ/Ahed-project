﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Windows
{
    public class ProjectsWindowViewModel : BindableBase
    {
        public ProjectsWindowViewModel()
        {

        }

        public ICommand DeleteProject => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.DeleteProject(SelectedProject);
        });

        public ICommand SelectProject => new DelegateCommand(() =>
        {
            GlobalFunctionsAndCallersService.SetProject(SelectedProject);
            GlobalFunctionsAndCallersService.ChangePage(0);
        });

        public ProjectInfoGet SelectedProject { get; set; }

        public ObservableCollection<ProjectInfoGet> Projects { get; set; }

        public ObservableCollection<Node> Nodes
        {
            get => GlobalDataCollectorService.ProjectNodes;
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

        public ICommand SelectProjectCommand => new DelegateCommand<object>((val) =>
        {
            var selected = (Node)val;
            if (selected.Nodes == null && selected.Id != null)
            {
                BeforeSearch = GlobalDataCollectorService.AllProjects[selected.Id];
                SearchCondition();
            }
        });

        private List<ProjectInfoGet> _beforeSearch;
        public List<ProjectInfoGet> BeforeSearch
        {
            get
            {
                if (_beforeSearch == null)
                {
                    _beforeSearch = GlobalDataCollectorService.ProjectsCollection;
                }
                return _beforeSearch;
            }
            set
            {
                _beforeSearch = value;
            }
        }

        public ICommand GetAll => new DelegateCommand(() =>
        {
            Projects = new ObservableCollection<ProjectInfoGet>(GlobalDataCollectorService.AllProjects.SelectMany(x => x.Value));
        });

        public ICommand WindowLoaded => new DelegateCommand(() =>
        {
            SearchCondition();
        });

        public void SearchCondition()
        {
            if (string.IsNullOrEmpty(SearchBox))
            {

                Projects = new ObservableCollection<ProjectInfoGet>(BeforeSearch.OrderByDescending(p => p.updatedAt ?? p.createdAt));
            }
            else
            {
                var lowSB = _searchBox.ToLower();
                Projects = new ObservableCollection<ProjectInfoGet>(BeforeSearch.Where
                    (x => (x.name?.ToLower().Contains(lowSB) ?? false) || (x.customer?.ToLower().Contains(lowSB) ?? false)
                    || (x.description?.ToLower().Contains(lowSB) ?? false) || (x.category?.ToLower().Contains(lowSB) ?? false)
                    || (x.keywords?.ToLower().Contains(lowSB) ?? false) || (x.comments?.ToLower().Contains(lowSB) ?? false)).OrderByDescending(p => p.updatedAt ?? p.createdAt));
            }
        }
    }
}
