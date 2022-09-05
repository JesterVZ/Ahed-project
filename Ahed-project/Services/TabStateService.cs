using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahed_project.MasterData;

namespace Ahed_project.Services
{
    public class TabStateService
    {
        public event Action<Ahed_project.MasterData.Pages> TabChanged;
        public void ChangeTab(Ahed_project.MasterData.Pages page) => TabChanged?.Invoke(page);
    }
}
