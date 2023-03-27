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
using System.IO;
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
        private static ShellFluidViewModel _shellFluidViewModel;
        private static HeatBalanceViewModel _heatBalanceViewModel;
        private static SLDocument Doc;
        public CreateExcelService(TubesFluidViewModel tubesFluidViewModel, ShellFluidViewModel shellFluidViewModel, ProjectPageViewModel projectPageViewModel, HeatBalanceViewModel heatBalanceViewModel)
        {
            _projectPageViewModel = projectPageViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;

            Doc = new();
        }
        public void CreateExcel()
        {
            CreateFull();
        }

        private static void CreateFull()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                string path = assembly.Location;
                if (!File.Exists($"{path}\\FullReport.xlsx"))
                {
                    AddTubeData();
                    AddShellData();
                    AddHeatBalanceData();
                    Doc.SaveAs("FullReport.xlsx");
                } else
                {

                }
                

            }
            catch(Exception e)
            {
                Application.Current.Dispatcher.Invoke(() => GlobalDataCollectorService.Logs.Add(new LoggerMessage("Error", $"Message: {e.Message}\r\nExcep: {e}")));
            }
            
        }

        #region styles
        private static SLStyle BoldTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 11;
            style.Font.Bold = true;
            style.SetWrapText(true);
            return style;
        }
        private static SLStyle BoldHeaderTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 14;
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
        #endregion

        #region tube
        private static void AddTubeData()
        {
            Doc.AddWorksheet("TubeReport");
            Doc.DeleteWorksheet("Sheet1");
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

        #endregion

        #region shell
        private static void AddShellData()
        {
            Doc.AddWorksheet("ShellReport");
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
            Doc.SetCellValue("C4", _shellFluidViewModel.Product.name);

            CreateHeaders();
            CreateUnits();
            AddData(_shellFluidViewModel.Product.product_properties);
        }
        #endregion

        #region heat balance
        private static void AddHeatBalanceData()
        {
            Doc.AddWorksheet("HeatBalanceReport");
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
            Doc.SetColumnWidth("A", "M", 15);
            Doc.SetCellValue("A2", "Revision Nr");
            Doc.SetCellValue("C2", _projectPageViewModel.ProjectInfo.revision.ToString());
            Doc.SetCellValue("A3", "Process");
            Doc.SetCellValue("C3", _projectPageViewModel.SelectedCalculation.name);

            Doc.MergeWorksheetCells("A5", "F5");
            Doc.SetCellValue("A5", "Heat Balance");
            Doc.SetCellStyle("A5", BoldHeaderTextStyle());

            Doc.MergeWorksheetCells("A5", "F5");
            Doc.MergeWorksheetCells("C6", "D6");
            Doc.MergeWorksheetCells("E6", "F6");
            Doc.SetCellValue("C6", "Tubes side");
            Doc.SetCellValue("E6", "Shell side");

            Doc.SetCellValue("C7", "In");
            Doc.SetCellValue("D7", "Out");
            Doc.SetCellValue("E7", "In");
            Doc.SetCellValue("F7", "Out");
            Doc.SetCellStyle("C6", "F7", BorderCellsStyle());
            Doc.SetCellStyle("C6", "F7", BoldTextStyle());

            AddNames();
            AddUnits();
            AddValues();
        }

        private static void AddUnits()
        {
            Doc.SetCellValue("B11", "kg/hr");
            Doc.SetCellValue("B12", "°C");
            Doc.SetCellValue("B13", "kW");
            Doc.SetCellValue("B14", "bar-a");
            Doc.SetCellValue("B16", "kg/m³");
            Doc.SetCellValue("B17", "kJ/kg•°С");
            Doc.SetCellValue("B18", "W/m•°C");
            Doc.SetCellValue("B19", "cP");
            Doc.SetCellValue("B21", "J/kg");
            Doc.SetCellValue("B23", "kg/m³");
            Doc.SetCellValue("B24", "kJ/kg•°C");
            Doc.SetCellValue("B25", "W/m•°C");
            Doc.SetCellValue("B26", "cP");
            Doc.SetCellValue("B27", "bar-a");
            Doc.SetCellValue("B28", "%");
        }

        private static void AddNames()
        {
            Doc.SetCellValue("A8", "Fluid Name");
            Doc.SetCellValue("A9", "Flow Type");
            Doc.SetCellValue("A10", "Process");
            Doc.SetCellValue("A11", "Flow");
            Doc.SetCellValue("A12", "Temperature");
            Doc.SetCellValue("A13", "Duty");
            Doc.SetCellValue("A14", "Pressure");
            Doc.SetCellValue("A15", "Liquid Phase");
            Doc.SetCellValue("A16", "Density");
            Doc.SetCellValue("A17", "Specific heat");
            Doc.SetCellValue("A18", "Therm. Cond.");
            Doc.SetCellValue("A19", "Consistency index");
            Doc.SetCellValue("A20", "Flow index");
            Doc.SetCellValue("A21", "Latent heat");
            Doc.SetCellValue("A22", "Gas Phase");
            Doc.SetCellValue("A23", "Density Gas");
            Doc.SetCellValue("A24", "Specific heat gas at constant pressure (Cp)");
            Doc.SetCellValue("A25", "Thermal Conductivity Gas");
            Doc.SetCellValue("A26", "Dynamic viscosity gas");
            Doc.SetCellValue("A27", "Vapour pressure");
            Doc.SetCellValue("A28", "Mass Vapour Fraction");
            Doc.SetCellStyle("A8", "F28", BorderCellsStyle());
            Doc.SetCellStyle("A8", "A28", BoldTextStyle());
        }

        private static void AddValues()
        {
            Doc.MergeWorksheetCells("C8", "D8");
            Doc.SetCellValue("C8", _tubesFluidViewModel.Product.name);
            Doc.MergeWorksheetCells("E8", "F8");
            Doc.SetCellValue("E8", _shellFluidViewModel.Product.name);
            Doc.MergeWorksheetCells("C9", "F9");
            Doc.SetCellValue("C9", "Counter current");
            Doc.MergeWorksheetCells("C10", "D10");
            Doc.SetCellValue("C10", _heatBalanceViewModel.Calculation.process_tube);
            Doc.MergeWorksheetCells("E10", "F10");
            Doc.SetCellValue("E10", _heatBalanceViewModel.Calculation.process_shell);
            Doc.MergeWorksheetCells("C11", "D11");
            Doc.SetCellValue("C11", _heatBalanceViewModel.Calculation.flow_tube);
            Doc.MergeWorksheetCells("E11", "F11");
            Doc.SetCellValue("E11", _heatBalanceViewModel.Calculation.flow_shell);
            Doc.SetCellValue("C12", _heatBalanceViewModel.Calculation.temperature_tube_inlet);
            Doc.SetCellValue("D12", _heatBalanceViewModel.Calculation.temperature_tube_outlet);
            Doc.SetCellValue("E12", _heatBalanceViewModel.Calculation.temperature_shell_inlet);
            Doc.SetCellValue("F12", _heatBalanceViewModel.Calculation.temperature_shell_outlet);
            Doc.MergeWorksheetCells("C13", "D13");
            Doc.MergeWorksheetCells("E13", "F13");
            Doc.SetCellValue("C13", _heatBalanceViewModel.Calculation.duty_tube);
            Doc.SetCellValue("E13", _heatBalanceViewModel.Calculation.duty_shell);
            Doc.SetCellValue("C16", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet);
            Doc.SetCellValue("D16", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet);
            Doc.SetCellValue("E16", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet);
            Doc.SetCellValue("F16", _heatBalanceViewModel.Calculation.liquid_density_shell_outlet);
            Doc.SetCellValue("C17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_inlet);
            Doc.SetCellValue("D17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_outlet);
            Doc.SetCellValue("E17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_inlet);
            Doc.SetCellValue("F17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_outlet);
            Doc.SetCellValue("C18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_inlet);
            Doc.SetCellValue("D18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_outlet);
            Doc.SetCellValue("E18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_inlet);
            Doc.SetCellValue("F18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_outlet);
            Doc.SetCellValue("C19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_inlet);
            Doc.SetCellValue("D19", _heatBalanceViewModel.Calculation.liquid_consistency_index_tube_outlet);
            Doc.SetCellValue("E19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_inlet);
            Doc.SetCellValue("F19", _heatBalanceViewModel.Calculation.liquid_consistency_index_shell_outlet);
            Doc.SetCellValue("C20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_inlet);
            Doc.SetCellValue("D20", _heatBalanceViewModel.Calculation.liquid_flow_index_tube_outlet);
            Doc.SetCellValue("E20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet);
            Doc.SetCellValue("F20", _heatBalanceViewModel.Calculation.liquid_flow_index_shell_inlet);
            Doc.SetCellValue("C21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_inlet);
            Doc.SetCellValue("D21", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_outlet);
            Doc.SetCellValue("E21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            Doc.SetCellValue("F21", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            Doc.SetCellValue("C23", _heatBalanceViewModel.Calculation.gas_density_tube_inlet);
            Doc.SetCellValue("D23", _heatBalanceViewModel.Calculation.gas_density_tube_outlet);
            Doc.SetCellValue("E23", _heatBalanceViewModel.Calculation.gas_density_shell_inlet);
            Doc.SetCellValue("F23", _heatBalanceViewModel.Calculation.gas_density_shell_outlet);
            Doc.SetCellValue("C24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_inlet);
            Doc.SetCellValue("D24", _heatBalanceViewModel.Calculation.gas_specific_heat_tube_outlet);
            Doc.SetCellValue("E24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_inlet);
            Doc.SetCellValue("F24", _heatBalanceViewModel.Calculation.gas_specific_heat_shell_outlet);
            Doc.SetCellValue("C25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_inlet);
            Doc.SetCellValue("D25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_tube_outlet);
            Doc.SetCellValue("E25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_inlet);
            Doc.SetCellValue("F25", _heatBalanceViewModel.Calculation.gas_thermal_conductivity_shell_outlet);
            Doc.SetCellValue("C26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_inlet);
            Doc.SetCellValue("D26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_outlet);
            Doc.SetCellValue("E26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_inlet);
            Doc.SetCellValue("F26", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_outlet);

        }
        #endregion

    }
}
