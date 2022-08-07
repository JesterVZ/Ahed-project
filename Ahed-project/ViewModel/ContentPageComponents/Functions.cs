using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.ViewModel.ContentPageComponents
{
    public partial class ContentPageViewModel
    {
        private void Validation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (ProjectInfo.name != null && ProjectInfo.name != string.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";
            }
            else
            {
                _logs.AddMessage("Error", "Введите имя проекта!");
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/cancel.svg";
                return;
            }

            if (ProjectInfo.description != null && ProjectInfo.description != string.Empty)
            {
                ProjectValidationStatusSource = Path.GetDirectoryName(assembly.Location) + "/Visual/check.svg";

            }
            else
            {
                _logs.AddMessage("warning", "Введите описание проекта!");
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
