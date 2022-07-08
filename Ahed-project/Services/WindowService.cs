using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services
{
    public class WindowService
    {
        public void OpenModalWindow(Window window) => window.ShowDialog();
    }
}
