using DevExpress.Mvvm;
using System.Windows.Controls;

namespace Ahed_project.ViewModel.Windows
{
    public class MainViewModel : BindableBase
    {
        public Page FramePage { get; set; }
        public string Title { get; set; }
        public MainViewModel()
        {
            FramePage = new Page();
        }
    }
}
