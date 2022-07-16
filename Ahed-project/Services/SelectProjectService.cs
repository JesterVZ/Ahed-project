using Ahed_project.MasterData.ProjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.Services
{
    public class SelectProjectService
    {
        public event Action<ProjectInfoGet> ProjectSelected;
        public void SelectProject(ProjectInfoGet project) => ProjectSelected?.Invoke(project);
    }
}
