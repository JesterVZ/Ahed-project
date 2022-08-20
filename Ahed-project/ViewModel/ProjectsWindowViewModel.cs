﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace Ahed_project.ViewModel
{
    public class ProjectsWindowViewModel : BindableBase
    {
        public ProjectsWindowViewModel()
        {

        }

        public ICommand SelectProject => new DelegateCommand(() => {
            GlobalFunctionsAndCallersService.SetProject(SelectedProject);
            GlobalFunctionsAndCallersService.ChangePage(0);
        });

        public ProjectInfoGet SelectedProject { get; set; }

        public ObservableCollection<ProjectInfoGet> Projects { get; set; }

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
                Projects = new ObservableCollection<ProjectInfoGet>(GlobalDataCollectorService.ProjectsCollection); 
            }
            else
            {
                var lowSB = _searchBox.ToLower();
                Projects = new ObservableCollection<ProjectInfoGet>(GlobalDataCollectorService.ProjectsCollection.Where
                    (x => (x.name?.ToLower().Contains(lowSB)??false) || (x.customer?.ToLower().Contains(lowSB) ?? false)
                    || (x.description?.ToLower().Contains(lowSB) ?? false) || (x.category?.ToLower().Contains(lowSB) ?? false)
                    || (x.keywords?.ToLower().Contains(lowSB) ?? false) || (x.comments?.ToLower().Contains(lowSB) ?? false)));
            }
        }
    }
}
