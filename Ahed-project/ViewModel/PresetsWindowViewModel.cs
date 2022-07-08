using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel
{
    public class PresetsWindowViewModel : BindableBase
    {
        public Action CloseAction { get; set; }
    }
}
