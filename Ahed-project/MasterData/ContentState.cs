using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Ahed_project.MasterData
{
    public class ContentState : INotifyPropertyChanged
    {
        private Visibility _lockVisibility;
        public Visibility LockVisibillity
        {
            get => _lockVisibility;
            set
            {
                _lockVisibility = value;
                OnPropertyChanged(nameof(LockVisibillity));
            }
        } //виден ли замочек

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                if (_isEnabled == true)
                {
                    LockVisibillity = Visibility.Hidden;

                }
                else
                {
                    LockVisibillity = Visibility.Visible;
                }
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        private string _validationStatusSource;
        public string ValidationStatusSource
        {
            get => _validationStatusSource;
            set
            {
                _validationStatusSource = value;
                OnPropertyChanged(nameof(ValidationStatusSource));
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
