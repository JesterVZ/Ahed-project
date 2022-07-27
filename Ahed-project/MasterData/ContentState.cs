using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.MasterData
{
    public class ContentState
    {
        public Visibility LockVisibillity { get; set; } //виден ли замочек

        private bool isEnabled;
        public bool IsEnabled { 
            get => isEnabled;
            set {
                isEnabled = value;
                if (isEnabled == true)
                {
                    LockVisibillity = Visibility.Hidden;
                    
                } else
                {
                    LockVisibillity = Visibility.Visible;
                }

            }
        }
    }
}
