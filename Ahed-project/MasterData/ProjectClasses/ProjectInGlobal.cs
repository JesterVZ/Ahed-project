using Ahed_project.MasterData.CalculateClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.ProjectClasses
{
    public class ProjectInGlobal :INotifyPropertyChanged
    {

        public ProjectInGlobal()
        {
            Project = new ProjectInfoGet();
            Calculation = new CalculationFull();
            Calculations = new List<CalculationFull>();
        }

        private ProjectInfoGet _project;
        public ProjectInfoGet Project
        {
            get => _project;
            set
            {
                _project = value;
                OnPropertyChanged(nameof(Project));
            }
        }
        private CalculationFull _calculation;

        public CalculationFull Calculation
        {
            get => _calculation;
            set
            {
                _calculation = value;
                OnPropertyChanged(nameof(Calculation));
            }
        }

        private bool _fieldsState;
        public bool FieldsState
        {
            get => _fieldsState;
            set
            {
                _fieldsState = value;
                OnPropertyChanged(nameof(FieldsState));
            }
        }

        private List<CalculationFull> _calculations;
        public List<CalculationFull> Calculations
        {
            get => _calculations;
            set
            {
                _calculations = value;
                OnPropertyChanged(nameof(Calculations));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
