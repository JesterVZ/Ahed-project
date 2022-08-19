using Ahed_project.MasterData.ProjectClasses;
using Ahed_project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel
{
    public class ProjectPageViewModel
    {
        public ProjectInfoGet ProjectInfoGet
        {
            get => GlobalDataCollectorService.ProjectPageContent;
            set
            {
                GlobalDataCollectorService.ProjectPageContent = value;
            }
        }
    }
}
