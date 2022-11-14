using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.BafflesClasses
{
    public class BaffleInGlobal:INotifyPropertyChanged
    {
        public BaffleInGlobal()
        {
            Baffle = new BaffleFull();
        }
        private BaffleFull _baffle;
        public BaffleFull Baffle
        {
            get => _baffle;
            set
            {
                _baffle= value;
                OnPropertyChanged(nameof(Baffle));
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
