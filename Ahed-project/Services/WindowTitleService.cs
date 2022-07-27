using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class WindowTitleService
    {
        public event Action<string> TitleChanged;
        public void ChangeTitle(string title) => TitleChanged?.Invoke(title);

    }
}
