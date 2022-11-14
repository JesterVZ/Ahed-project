using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ahed_project.MasterData.MainClasses
{
    public class MainInGlobal:INotifyPropertyChanged
    {
        public MainInGlobal() 
        {
            Title= string.Empty;
            FramePage = new Page();
        }
        private string _title;
        public string Title
        {
            get=>_title;
            set
            {
                _title= value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private Page _framePage;
        public Page FramePage 
        {
            get => _framePage;
            set
            {
                _framePage= value;
                OnPropertyChanged(nameof(FramePage));
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
