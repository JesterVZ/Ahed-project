﻿using Ahed_project.MasterData;
using Ahed_project.MasterData.CalculateClasses;
using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services.Global;
using DevExpress.Mvvm;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ahed_project.ViewModel.Pages
{
    public class ProjectPageViewModel : BindableBase
    {
        public ProjectPageViewModel()
        {
            Calculations = new ObservableCollection<CalculationFull>();
            ArrowAngle = "0";
            SystemItems = new ObservableCollection<string>()
            {
                "Si",
                "Metric"
            };
            System = SystemItems[0];
        }
        public Visibility TextBoxVisibillity { get; set; }
        public Visibility LabelVisibillity { get; set; }
        public string ArrowAngle { get; set; }

        public string ButtonImagePath { get; set; }

        private bool _isOpen;
        public bool IsOpen { 

            get => _isOpen; 
            set { 
                _isOpen = value;
                if(value == true)
                {
                    ArrowAngle = "180";
                } else
                {
                    ArrowAngle = "0";
                }
            } 
        }
        public bool FieldsState { get; set; } //enabled/disabled

        #region Props
        private ProjectInfoGet _projectInfo = new();
        public ProjectInfoGet ProjectInfo
        {
            get => _projectInfo;
            set { 
                SetValue(ref _projectInfo, value);
                ProjectName = value?.name;
                System = value?.units;
            }
        }

        private string _system;
        public string System
        {
            get => _system;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _system = "Si";
                    ProjectInfo.units = "Si";
                }
                else
                {
                    _system = value;
                    ProjectInfo.units = value;
                }
            }
        }
        public ObservableCollection<string> SystemItems { get; set; }

        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                ProjectInfo.name = value;
                GlobalFunctionsAndCallersService.SetWindowName(value);
            }
        }

        public void Raise()
        {
            RaisePropertiesChanged(nameof(ProjectInfo));
        }

        private CalculationFull _selectedCalculation;
        public CalculationFull SelectedCalculation
        {
            get => _selectedCalculation;
            set
            {
                _selectedCalculation = value;
                GlobalFunctionsAndCallersService.SetCalculation(_selectedCalculation);
            }
        }
        public ObservableCollection<CalculationFull> Calculations { get; set; }
        public string CalculationName { get; set; }
        #endregion
        #region Comms
        public ICommand CreateCalculationCommand => new AsyncCommand(async () =>
        {
            if(CalculationName != "")
            {
                await Task.Factory.StartNew(() => GlobalFunctionsAndCallersService.CreateCalculation(CalculationName));
                CalculationName = "";
            }
            
        });

        public ICommand ToggleCommand => new DelegateCommand(async () =>
        {
            IsOpen = !IsOpen;
        });
        #endregion
    }
}

