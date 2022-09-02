using Ahed_project.MasterData;
using Ahed_project.Services.Global;
using System.IO;
using System.Reflection;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        public void Validation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (GlobalDataCollectorService.Project.name != null && GlobalDataCollectorService.Project.name != string.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", "Введите имя проекта!"));
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }

            if (GlobalDataCollectorService.Project.description != null && GlobalDataCollectorService.Project.description != string.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";

            }
            else
            {
                GlobalDataCollectorService.Logs.Add(new LoggerMessage("warning", "Введите описание проекта!"));
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
            }
            /*
            if (SingleProductGet != null)
            {
                if (SingleProductGet.name != null && SingleProductGet.name != string.Empty)
                {
                    TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
                }
                else
                {
                    _logs.AddMessage("warning", "Введите имя продукта!");
                    TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/warning.svg";
                }
            }
            else
            {
                _logs.AddMessage("Error", "Выберете продукт!");
                TubesFluidValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }
            */
        }
    }
}
