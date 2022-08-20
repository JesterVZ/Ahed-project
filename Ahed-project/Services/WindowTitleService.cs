using System;

namespace Ahed_project.Services
{
    public class WindowTitleService
    {
        public event Action<string> TitleChanged;
        public void ChangeTitle(string title) => TitleChanged?.Invoke(title);

    }
}
