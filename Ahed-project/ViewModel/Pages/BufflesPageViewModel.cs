using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel.Pages
{
    public class BufflesPageViewModel : BindableBase
    {
        public Dictionary<string, string> Type;
        public BufflesPageViewModel()
        {
            Type = new Dictionary<string, string>();

            Type.Add("single_segmental", "Single Segmental");
            Type.Add("double_segmental", "Double Segmental");
        }
    }
}
