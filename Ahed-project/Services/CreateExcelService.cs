using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.ContentPageComponents;
using Ahed_project.ViewModel.Pages;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ahed_project.Services
{
    public class CreateExcelService
    {
        private static ProjectPageViewModel _projectPageViewModel;
        private static TubesFluidViewModel _tubesFluidViewModel;
        public static ShellFluidViewModel _shellFluidViewModel;
        private static SLDocument Doc;
        public CreateExcelService(TubesFluidViewModel tubesFluidViewModel, ShellFluidViewModel shellFluidViewModel, ProjectPageViewModel projectPageViewModel)
        {
            _projectPageViewModel = projectPageViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;

            Doc = new();
        }
        public async void CreateExcel(DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.FULL:
                    CreateFull();
                break;
            }
        }

        private static void CreateFull()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string path = assembly.Location;
                
                Doc.MergeWorksheetCells("A1", "B1");
                Doc.MergeWorksheetCells("A2", "B2");
                Doc.MergeWorksheetCells("A3", "B3");
                Doc.MergeWorksheetCells("A4", "B4");
                Doc.MergeWorksheetCells("C1", "F1");
                Doc.MergeWorksheetCells("C3", "F3");
                Doc.MergeWorksheetCells("C4", "F4");
                Doc.SetCellValue("A1", "Project name");
                Doc.SetCellValue("C1", _projectPageViewModel.ProjectName);
                Doc.SetCellStyle("A1", "A6", BoldTextStyle());
                Doc.SetCellStyle("A1", "F1", BorderCellsStyle());
                Doc.SetCellStyle("A2", "C2", BorderCellsStyle());
                Doc.SetCellStyle("A3", "F3", BorderCellsStyle());
                Doc.SetCellStyle("A4", "F4", BorderCellsStyle());
                Doc.SetColumnWidth("A", "M", 15);
                Doc.SetCellValue("A2", "Revision Nr");
                Doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
                Doc.SetCellValue("A3", "Process");
                Doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);
                Doc.SetCellValue("A4", "Name");
                Doc.SetCellValue("C4", _tubesFluidViewModel.Product.name);

                CreateHeaders();
                CreateUnits();
                AddData(_tubesFluidViewModel.Product.product_properties);
                Doc.SaveAs("FullReport.xlsx");

            }
            catch(Exception e)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", $"Message: {e.Message}\r\nExcep: {e}")));
            }
            
        }

        private static SLStyle BoldTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 11;
            style.Font.Bold = true;
            style.SetWrapText(true);
            return style;
        }
        private static SLStyle BorderCellsStyle()
        {
            var style = new SLStyle();
            style.SetRightBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetTopBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetLeftBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            style.SetBottomBorder(BorderStyleValues.Thin, System.Drawing.Color.Black);
            return style;
        }
        private static void CreateHeaders()
        {
            Doc.SetCellValue("A8", "Temperature");
            Doc.SetCellValue("B8", "Density");
            Doc.SetCellValue("C8", "Specific heat");
            Doc.SetCellValue("D8", "Thermal conductivity");
            Doc.SetCellValue("E8", "Consistency index");
            Doc.SetCellValue("F8", "Flow index");
            Doc.SetCellValue("G8", "Latent heat");
            Doc.SetCellValue("H8", "Density Gas");
            Doc.SetCellValue("I8", "Specific heat gas at constant pressure (Cp)");
            Doc.SetCellValue("J8", "Thermal Conductivity Gas");
            Doc.SetCellValue("K8", "Dynamic viscosity gas");
            Doc.SetCellValue("L8", "Vapour pressure");
            Doc.SetCellValue("M8", "Mass Vapour Fraction");

            Doc.SetCellStyle("A8", "M8", BoldTextStyle());
        }
        private static void CreateUnits()
        {
            Doc.SetCellValue("A9", "°C");
            Doc.SetCellValue("B9", "kg/m³");
            Doc.SetCellValue("C9", "kcal/kg·°C");
            Doc.SetCellValue("D9", "kcal/m·hr·°C");
            Doc.SetCellValue("E9", "cP");
            Doc.SetCellValue("F9", "");
            Doc.SetCellValue("G9", "kcal/kg");
            Doc.SetCellValue("H9", "kg/m³");
            Doc.SetCellValue("I9", "kcal/kg·°C");
            Doc.SetCellValue("J9", "kcal/m·hr·°C");
            Doc.SetCellValue("K9", "cP");
            Doc.SetCellValue("L9", "bar-a");
            Doc.SetCellValue("M9", "%");
        }
        private static void AddData(IEnumerable<ProductProperties> values)
        {
            ProductProperties[] properties = values.ToArray();
            for (int i = 0; i < properties.Length; i++)
            {
                Doc.SetCellValue($"A{i + 10}", properties[i].liquid_phase_temperature.ToString());
                Doc.SetCellValue($"B{i + 10}", properties[i].liquid_phase_density.ToString());
                Doc.SetCellValue($"C{i + 10}", properties[i].liquid_phase_specific_heat.ToString());
                Doc.SetCellValue($"D{i + 10}", properties[i].liquid_phase_thermal_conductivity.ToString());
                Doc.SetCellValue($"E{i + 10}", properties[i].liquid_phase_consistency_index.ToString());
                Doc.SetCellValue($"F{i + 10}", properties[i].liquid_phase_f_ind.ToString());
                Doc.SetCellValue($"G{i + 10}", properties[i].liquid_phase_dh.ToString());
                Doc.SetCellValue($"H{i + 10}", properties[i].gas_phase_density.ToString());
                Doc.SetCellValue($"I{i + 10}", properties[i].gas_phase_specific_heat.ToString());
                Doc.SetCellValue($"J{i + 10}", properties[i].gas_phase_thermal_conductivity.ToString());
                Doc.SetCellValue($"K{i + 10}", properties[i].gas_phase_dyn_visc_gas.ToString());
                Doc.SetCellValue($"L{i + 10}", properties[i].gas_phase_p_vap.ToString());
                Doc.SetCellValue($"M{i + 10}", properties[i].gas_phase_vapour_frac.ToString());

            }
            Doc.SetCellStyle("A8", $"M{properties.Length + 9}", BorderCellsStyle());
        }
    }
}
