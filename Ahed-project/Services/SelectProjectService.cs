using Ahed_project.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class SelectProjectService
    {
        public event Action<ProjectInfo> ProjectSelected;
        public void SelectProject(ProjectInfo project) => ProjectSelected?.Invoke(project);
    }
}
