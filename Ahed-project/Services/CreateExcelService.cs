using Ahed_project.MasterData;
using Ahed_project.MasterData.Products;
using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Pages;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;

namespace Ahed_project.Services
{

    public class CreateExcelService
    {
        private static ProjectPageViewModel _projectPageViewModel;
        private static TubesFluidViewModel _tubesFluidViewModel;
        private static ShellFluidViewModel _shellFluidViewModel;
        private static HeatBalanceViewModel _heatBalanceViewModel;
        private static GeometryPageViewModel _geometryPageViewModel;
        private static BafflesPageViewModel _bafflesPageViewModel;
        private static OverallCalculationViewModel _overallCalculationViewModel;
        private static SLDocument Doc;
        public CreateExcelService(TubesFluidViewModel tubesFluidViewModel, ShellFluidViewModel shellFluidViewModel, ProjectPageViewModel projectPageViewModel, HeatBalanceViewModel heatBalanceViewModel, GeometryPageViewModel geometryPageViewModel, BafflesPageViewModel bafflesPageViewModel, OverallCalculationViewModel overallCalculationViewModel)
        {
            _projectPageViewModel = projectPageViewModel;
            _tubesFluidViewModel = tubesFluidViewModel;
            _shellFluidViewModel = shellFluidViewModel;
            _heatBalanceViewModel = heatBalanceViewModel;
            _geometryPageViewModel = geometryPageViewModel;
            _bafflesPageViewModel = bafflesPageViewModel;
            _overallCalculationViewModel = overallCalculationViewModel;

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
                var directory = Directory.GetCurrentDirectory();
                if (File.Exists($"{directory}\\FullReport.xlsx")){
                    File.Delete($"{directory}\\FullReport.xlsx");
                }
                AddTubeData();
                AddShellData();
                AddHeatBalanceData();
                AddGeometryData();
                AddOverallData();
                AddTemaSheetData();
                Doc.SaveAs("FullReport.xlsx");
                var p = new System.Diagnostics.Process();
                p.StartInfo = new ProcessStartInfo($"{directory}\\FullReport.xlsx")
                {
                    UseShellExecute = true,
                };
                p.Start();


            }
            catch (Exception e)
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

        private static SLStyle BoldTemaHeaderTextStyle()
        {
            var style = new SLStyle();
            style.Font.FontSize = 9;
            style.Font.Bold = true;
            style.SetWrapText(true);
            style.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.LightGray, System.Drawing.Color.Blue);
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
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetWrapText(true);
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
            Doc.SetCellValue("C27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_inlet);
            Doc.SetCellValue("D27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_outlet);
            Doc.SetCellValue("E27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_inlet);
            Doc.SetCellValue("F27", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_outlet);
            Doc.SetCellValue("C28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_tube_inlet);
            Doc.SetCellValue("D28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_tube_outlet);
            Doc.SetCellValue("E28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_shell_inlet);
            Doc.SetCellValue("F28", _heatBalanceViewModel.Calculation.gas_mass_vapour_fraction_shell_outlet);
        }
        #endregion

        #region geometry
        private static void AddGeometryData()
        {
            Doc.AddWorksheet("GeometryReport");
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

            AddGeometryTitle("A6", "E6", "Tube & Shell Geometry");

            Doc.SetCellValue("C7", "Inner Side");
            Doc.SetCellValue("D7", "Tube Side");
            Doc.SetCellValue("E7", "Shell Side");

            Doc.SetCellStyle("C7", "E7", BorderCellsStyle());
            Doc.SetCellStyle("C7", "F7", BoldTextStyle());
            AddGeometryNames();
            AddGeometryTitle("A25", "E25", "Tubeplate Layout");
            AddGeometryTubeplateNames();
            AddGeometryTitle("A37", "E37", "Nozzles");
            AddGeometryNozzlesNames();
            AddGeometryTitle("A50", "E50", "Baffles");
            AddGeometryBafflesNames();
            AddGeometryValues();
            AddGeometryUnits();

            DownloadImage(_geometryPageViewModel.Geometry.image_geometry);

        }

        private static void DownloadImage(string url)
        {
            WebClient client = new();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory($"{path}\\Apora");

            client.DownloadFile(new Uri(_geometryPageViewModel.Geometry.image_geometry), @$"{path}\\Apora\\geometry_image.png");
            SLPicture pic = new SLPicture(@$"{path}\\Apora\\geometry_image.png");
            pic.ResizeInPixels(300, 300);
            pic.SetPosition(6, 5);
            Doc.InsertPicture(pic);
        }

        private static void AddGeometryNames()
        {
            Doc.SetCellValue("A8", "Outer Diameter");
            Doc.SetCellValue("A9", "Thickness");
            Doc.SetCellValue("A10", "Inner Diameter");
            Doc.SetCellValue("A11", "Material");
            Doc.SetCellValue("A12", "Number of Tubes");
            Doc.SetCellValue("A13", "Tube Inner Length (Lti)");
            Doc.SetCellValue("A14", "Orientation");
            Doc.SetCellValue("A15", "Wetted Perimeter (per pass)");
            Doc.SetCellValue("A16", "Hydraulic Diameter");
            Doc.SetCellValue("A17", "Area / Module");
            Doc.SetCellValue("A18", "Volume / Module");
            Doc.SetCellValue("A19", "Tube Profile");
            Doc.SetCellValue("A20", "Roughness (ε)");
            Doc.SetCellValue("A21", "Bundle Type");
            Doc.SetCellValue("A22", "Roller Expanded");
            Doc.SetCellStyle("A8", "E22", BorderCellsStyle());
            Doc.SetCellStyle("A8", "A22", BoldTextStyle());
        }

        private static void AddGeometryTubeplateNames()
        {
            Doc.SetCellValue("A26", "Tube Pitch");
            Doc.SetCellValue("A27", "Tube Layout");
            Doc.SetCellValue("A28", "Number of Passes");
            Doc.SetCellValue("A29", "Div. Plate Layout");
            Doc.SetCellValue("A30", "Div. Plate Thickness");
            Doc.SetCellValue("A31", "Flow cross section");
            Doc.SetCellValue("A32", "Perimeter");
            Doc.SetCellValue("A33", "Max. Nr. Tubes");
            Doc.SetCellValue("A34", "Tube Distribution");
            Doc.SetCellValue("A35", "Tube-Tube Spacing");
            Doc.SetCellStyle("A26", "E35", BorderCellsStyle());
            Doc.SetCellStyle("A26", "A35", BoldTextStyle());
        }

        private static void AddGeometryNozzlesNames()
        {
            Doc.SetCellValue("A38", "Inlet nozzle OD");
            Doc.SetCellValue("A39", "Inlet nozzle wall");
            Doc.SetCellValue("A40", "Inlet nozzle ID");
            Doc.SetCellValue("A41", "In Length");
            Doc.SetCellValue("A42", "Outlet nozzle OD");
            Doc.SetCellValue("A43", "Outlet nozzle wall");
            Doc.SetCellValue("A44", "Outlet nozzle ID");
            Doc.SetCellValue("A45", "Out Length");
            Doc.SetCellValue("A46", "Number of Parallel Lines");
            Doc.SetCellValue("A47", "Number of Modules per Block");
            Doc.SetCellValue("A48", "Shell nozzle orientation");
            Doc.SetCellStyle("A38", "E48", BorderCellsStyle());
            Doc.SetCellStyle("A38", "A48", BoldTextStyle());
        }
        private static void AddGeometryBafflesNames()
        {
            Doc.SetCellValue("A51", "Nr. Baffles");
            Doc.SetCellValue("A52", "Baffle cut (% ID)");
            Doc.SetCellValue("A53", "Inlet baffle spacing (Lbi)");
            Doc.SetCellValue("A54", "Central baffle spacing (Lbc)");
            Doc.SetCellValue("A55", "Outlet baffle spacing (Lbo)");
            Doc.SetCellValue("A56", "Baffle thickness");
            Doc.SetCellValue("A57", "Pairs of sealing strips (Nss)");
            Doc.SetCellStyle("A51", "E57", BorderCellsStyle());
            Doc.SetCellStyle("A51", "A57", BoldTextStyle());
        }

        private static void AddGeometryTitle(string position1, string position2, string title)
        {
            Doc.MergeWorksheetCells(position1, position2);
            Doc.SetCellValue(position1, title);
            Doc.SetCellStyle(position1, BoldHeaderTextStyle());
        }

        private static void AddGeometryValues()
        {
            Doc.SetCellValue("C8", _geometryPageViewModel.Geometry.outer_diameter_inner_side);
            Doc.SetCellValue("D8", _geometryPageViewModel.Geometry.outer_diameter_tubes_side);
            Doc.SetCellValue("E8", _geometryPageViewModel.Geometry.outer_diameter_shell_side);
            Doc.SetCellValue("C9", _geometryPageViewModel.Geometry.thickness_inner_side);
            Doc.SetCellValue("D9", _geometryPageViewModel.Geometry.thickness_tubes_side);
            Doc.SetCellValue("E9", _geometryPageViewModel.Geometry.thickness_shell_side);
            Doc.SetCellValue("C10", _geometryPageViewModel.Geometry.inner_diameter_inner_side);
            Doc.SetCellValue("D10", _geometryPageViewModel.Geometry.inner_diameter_tubes_side);
            Doc.SetCellValue("E10", _geometryPageViewModel.Geometry.inner_diameter_shell_side);
            Doc.MergeWorksheetCells("C11", "D11");
            Doc.SetCellValue("C11", _geometryPageViewModel.Geometry.material_tubes_side);
            Doc.SetCellValue("E11", _geometryPageViewModel.Geometry.material_shell_side);
            Doc.SetCellValue("D12", _geometryPageViewModel.Geometry.number_of_tubes);
            Doc.SetCellValue("D13", _geometryPageViewModel.Geometry.tube_inner_length);
            Doc.MergeWorksheetCells("C14", "E14");
            Doc.SetCellValue("D14", _geometryPageViewModel.Geometry.tube_inner_length);
            Doc.SetCellValue("D15", _geometryPageViewModel.Geometry.wetted_perimeter_tubes_side);
            Doc.SetCellValue("E15", _geometryPageViewModel.Geometry.wetted_perimeter_shell_side);
            Doc.SetCellValue("D16", _geometryPageViewModel.Geometry.hydraulic_diameter_tubes_side);
            Doc.SetCellValue("E16", _geometryPageViewModel.Geometry.hydraulic_diameter_shell_side);
            Doc.SetCellValue("D17", _geometryPageViewModel.Geometry.area_module);
            Doc.SetCellValue("D18", _geometryPageViewModel.Geometry.volume_module_tubes_side);
            Doc.SetCellValue("E18", _geometryPageViewModel.Geometry.volume_module_shell_side);
            Doc.MergeWorksheetCells("C19", "D19");
            Doc.SetCellValue("C19", _geometryPageViewModel.Geometry.tube_profile_tubes_side);
            Doc.SetCellValue("D20", _geometryPageViewModel.Geometry.roughness_tubes_side);
            Doc.SetCellValue("E20", _geometryPageViewModel.Geometry.roughness_shell_side);
            Doc.MergeWorksheetCells("C21", "E21");
            Doc.SetCellValue("C21", _geometryPageViewModel.Geometry.bundle_type);
            Doc.SetCellValue("D22", _geometryPageViewModel.Geometry.roller_expanded);
            Doc.SetCellValue("D26", _geometryPageViewModel.Geometry.tube_plate_layout_tube_pitch);
            Doc.SetCellValue("D27", _geometryPageViewModel.Geometry.tube_plate_layout_tube_layout);
            Doc.SetCellValue("D28", _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);
            Doc.SetCellValue("D29", _geometryPageViewModel.Geometry.tube_plate_layout_div_plate_layout);
            Doc.SetCellValue("D30", _geometryPageViewModel.Geometry.tube_plate_layout_div_plate_thickness);
            Doc.SetCellValue("D31", _geometryPageViewModel.Geometry.tube_plate_layout_tubes_cross_section_pre_pass);
            Doc.SetCellValue("E31", _geometryPageViewModel.Geometry.tube_plate_layout_shell_cross_section);
            Doc.SetCellValue("D32", _geometryPageViewModel.Geometry.tube_plate_layout_perimeter);
            Doc.SetCellValue("D33", _geometryPageViewModel.Geometry.tube_plate_layout_max_nr_tubes);
            Doc.SetCellValue("D34", _geometryPageViewModel.Geometry.tube_plate_layout_tube_distribution);
            Doc.SetCellValue("D35", _geometryPageViewModel.Geometry.tube_plate_layout_tube_tube_spacing);
            Doc.SetCellValue("C38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_inner_side);
            Doc.SetCellValue("D38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side);
            Doc.SetCellValue("E38", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side);
            //Doc.SetCellValue("C39", _geometryPageViewModel.Geometry.nozzles);
            //Doc.SetCellValue("D39", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_tubes_side);
            //Doc.SetCellValue("E39", _geometryPageViewModel.Geometry.nozzles_in_outer_diam_shell_side);
            Doc.SetCellValue("C40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_inner_side);
            Doc.SetCellValue("D40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_tubes_side);
            Doc.SetCellValue("E40", _geometryPageViewModel.Geometry.nozzles_in_inner_diam_shell_side);
            Doc.SetCellValue("D41", _geometryPageViewModel.Geometry.nozzles_in_length_tubes_side);
            Doc.SetCellValue("E41", _geometryPageViewModel.Geometry.nozzles_in_length_shell_side);
            Doc.SetCellValue("C42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_inner_side);
            Doc.SetCellValue("D42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_tubes_side);
            Doc.SetCellValue("E42", _geometryPageViewModel.Geometry.nozzles_out_outer_diam_shell_side);
            Doc.SetCellValue("D44", _geometryPageViewModel.Geometry.nozzles_out_inner_diam_tubes_side);
            Doc.SetCellValue("E44", _geometryPageViewModel.Geometry.nozzles_out_inner_diam_shell_side);
            Doc.SetCellValue("D45", _geometryPageViewModel.Geometry.nozzles_out_length_tubes_side);
            Doc.SetCellValue("E45", _geometryPageViewModel.Geometry.nozzles_out_length_shell_side);
            Doc.SetCellValue("D46", _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_tubes_side);
            Doc.SetCellValue("E46", _geometryPageViewModel.Geometry.nozzles_number_of_parallel_lines_shell_side);
            Doc.SetCellValue("D47", _geometryPageViewModel.Geometry.nozzles_number_of_modules_pre_block);
            Doc.SetCellValue("D48", _geometryPageViewModel.Geometry.shell_nozzle_orientation);
            Doc.MergeWorksheetCells("C49", "E49");
            Doc.SetCellValue("D49", _bafflesPageViewModel.Baffle.number_of_baffles);
            Doc.MergeWorksheetCells("C50", "E50");
            Doc.SetCellValue("D50", _bafflesPageViewModel.Baffle.buffle_cut);
            Doc.MergeWorksheetCells("C51", "E51");
            Doc.SetCellValue("D51", _bafflesPageViewModel.Baffle.inlet_baffle_spacing);
            Doc.MergeWorksheetCells("C52", "E52");
            Doc.SetCellValue("D52", _bafflesPageViewModel.Baffle.central_baffle_spacing);
            Doc.MergeWorksheetCells("C53", "E53");
            Doc.SetCellValue("D53", _bafflesPageViewModel.Baffle.outlet_baffle_spacing);
            Doc.MergeWorksheetCells("C54", "E54");
            Doc.SetCellValue("D54", _bafflesPageViewModel.Baffle.baffle_thickness);
            Doc.MergeWorksheetCells("C55", "E55");
            Doc.SetCellValue("D55", _bafflesPageViewModel.Baffle.pairs_of_sealing_strips);
        }

        private static void AddGeometryUnits()
        {
            Doc.SetCellValue("B8", "mm");
            Doc.SetCellValue("B9", "mm");
            Doc.SetCellValue("B10", "mm");
            Doc.SetCellValue("B13", "mm");
            Doc.SetCellValue("B15", "mm");
            Doc.SetCellValue("B16", "mm");
            Doc.SetCellValue("B17", "m²");
            Doc.SetCellValue("B18", "l");
            Doc.SetCellValue("B20", "μm");
            Doc.SetCellValue("B26", "mm");
            Doc.SetCellValue("B30", "mm");
            Doc.SetCellValue("B31", "m²");
            Doc.SetCellValue("B32", "mm");
            Doc.SetCellValue("B35", "mm");
            Doc.SetCellValue("B38", "mm");
            Doc.SetCellValue("B39", "mm");
            Doc.SetCellValue("B40", "mm");
            Doc.SetCellValue("B41", "mm");
            Doc.SetCellValue("B42", "mm");
            Doc.SetCellValue("B43", "mm");
            Doc.SetCellValue("B44", "mm");
            Doc.SetCellValue("B45", "mm");
            Doc.SetCellValue("B53", "mm");
            Doc.SetCellValue("B54", "mm");
            Doc.SetCellValue("B55", "mm");
            Doc.SetCellValue("B56", "mm");
            Doc.SetColumnWidth("B", 15);

        }

        #endregion

        #region overall
        private static void AddOverallData()
        {
            Doc.AddWorksheet("Heat Transfer Report");
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

            AddGeometryTitle("A6", "F6", "Flow Data");
            AddGeometryTitle("A25", "F25", "Heat Transfer Data");
            AddGeometryTitle("A38", "F38", "Areas");
            AddGeometryTitle("A45", "F45", "Logarithmic Mean Temperature Difference (LMTD)");
            AddGeometryTitle("A50", "F50", "Pressure Drop");
            AddGeometryTitle("A52", "F52", "Tubes Side");
            AddGeometryTitle("A60", "F60", "Shell Side");
            AddGeometryTitle("A70", "F70", "Vibrations");
            Doc.MergeWorksheetCells("C7", "D7");
            Doc.MergeWorksheetCells("E7", "F7");
            Doc.SetCellValue("C7", "Tube Side");
            Doc.SetCellValue("E7", "Shell Side");
            Doc.SetCellValue("C8", "Inlet");
            Doc.SetCellValue("D8", "Outlet");
            Doc.SetCellValue("E8", "Inlet");
            Doc.SetCellValue("F8", "Outlet");
            Doc.SetCellStyle("C7", "F8", BorderCellsStyle());
            Doc.SetCellStyle("C7", "F8", BoldTextStyle());
            AddHeatTransferFlowDataNames();
            AddHeatTransferHeatTransferDataNames();
            AddHeatTransferAreaNames();
            AddHeatTransferTempNames();
            AddHeatTransferPressureDropNames();
            AddHeatTransferVibrationsNames();
            AddHeatTransferData();
        }

        private static void AddHeatTransferFlowDataNames()
        {
            Doc.SetCellValue("A9", "Fluid Name");
            Doc.SetCellValue("A10", "Flow");
            Doc.SetCellValue("A11", "Temperature");
            Doc.SetCellValue("A12", "Duty");
            Doc.SetCellValue("A13", "Fluid Velocity");
            Doc.SetCellValue("A14", "Shear Rate");
            Doc.SetCellValue("A15", "Flow Type");
            Doc.SetCellValue("A16", "Liquid Phase");
            Doc.SetCellValue("A17", "   App. viscosity");
            Doc.SetCellValue("A18", "   Reynolds");
            Doc.SetCellValue("A19", "Prandtl");
            Doc.SetCellValue("A20", "Gas Phase");
            Doc.SetCellValue("A21", "   Dynamic viscosity gas");
            Doc.SetCellValue("A22", "   Reynolds");
            Doc.SetCellValue("A23", "   Prandtl");
            Doc.SetCellStyle("A9", "F23", BorderCellsStyle());
            Doc.SetCellStyle("A9", "A23", BoldTextStyle());
        }

        private static void AddHeatTransferHeatTransferDataNames()
        {
            Doc.SetCellValue("A26", "Wall temperature");
            Doc.SetCellValue("A27", "Average metal temp.");
            Doc.SetCellValue("A28", "Wall Consistency Index");
            Doc.SetCellValue("A29", "Nusselt");
            Doc.SetCellValue("A30", "K Side");
            Doc.SetCellValue("A31", "Fouling Factor");
            Doc.SetCellValue("A33", "K Unfouled");
            Doc.SetCellValue("A34", "K Fouled");
            Doc.SetCellValue("A35", "K Global Fouled");
            Doc.SetCellValue("A36", "K Effective");
            Doc.SetCellStyle("A26", "F36", BorderCellsStyle());
            Doc.SetCellStyle("A26", "A36", BoldTextStyle());
        }
        private static void AddHeatTransferAreaNames()
        {
            Doc.SetCellValue("A39", "Surface Area Required");
            Doc.SetCellValue("A40", "Area / Module");
            Doc.SetCellValue("A41", "Nr. Modules");
            Doc.SetCellValue("A42", "Fitted Area");
            Doc.SetCellValue("A43", "Excess Area");

            Doc.SetCellStyle("A39", "F43", BorderCellsStyle());
            Doc.SetCellStyle("A39", "A43", BoldTextStyle());
        }
        private static void AddHeatTransferTempNames()
        {
            Doc.SetCellValue("A46", "LMTD");
            Doc.SetCellValue("A47", "LMTD Correction Factor 'F'");
            Doc.SetCellValue("A48", "Adjusted LMTD");

            Doc.SetCellStyle("A46", "F48", BorderCellsStyle());
            Doc.SetCellStyle("A46", "A48", BoldTextStyle());
        }
        private static void AddHeatTransferPressureDropNames()
        {
            Doc.SetCellValue("A54", "Modules");
            Doc.SetCellValue("A55", "Inlet Nozzles");
            Doc.SetCellValue("A56", "Outlet Nozzles");
            Doc.SetCellValue("A57", "Bends");
            Doc.SetCellValue("A58", "Total");
            Doc.SetCellValue("C53", "v  m/s");
            Doc.SetCellValue("D53", "ρ·V²  kg/m·s²");
            Doc.SetCellValue("E53", "ΔP  bar-a");
            Doc.SetCellStyle("C53", "E53", BorderCellsStyle());
            Doc.SetCellStyle("C53", "E53", BoldTextStyle());

            Doc.SetCellStyle("A54", "E58", BorderCellsStyle());
            Doc.SetCellStyle("A54", "A58", BoldTextStyle());

            Doc.SetCellValue("A62", "Modules");
            Doc.SetCellValue("A63", "Inlet Nozzles");
            Doc.SetCellValue("A64", "Outlet Nozzles");
            Doc.SetCellValue("A65", "Pressure drop in all central baffle spaces (∆Pc)");
            Doc.SetCellValue("A66", "Pressure drop in all baffle windows (∆Pw)");
            Doc.SetCellValue("A67", "Pressure drop in the entrance and exit baffle spaces (∆Pe)");
            Doc.SetCellValue("A68", "Total");
            Doc.SetCellValue("C61", "v  m/s");
            Doc.SetCellValue("D61", "ρ·V²  kg/m·s²");
            Doc.SetCellValue("E61", "ΔP  bar-a");
            Doc.SetCellStyle("C61", "E61", BorderCellsStyle());
            Doc.SetCellStyle("C61", "E61", BoldTextStyle());

            Doc.SetCellStyle("A62", "E68", BorderCellsStyle());
            Doc.SetCellStyle("A62", "A68", BoldTextStyle());
        }
        private static void AddHeatTransferVibrationsNames()
        {
            Doc.SetCellValue("A72", "Span Length");
            Doc.SetCellValue("A73", "Span Length / TEMA Lb,max");
            Doc.SetCellValue("A74", "Tubes Natural Frequency");
            Doc.SetCellValue("A75", "Shell Acoustic Frequency (gases)");
            Doc.SetCellValue("A76", "Fluidelastic Instability");
            Doc.SetCellValue("A77", "Cross Flow Velocity");
            Doc.SetCellValue("A78", "Critical Velocity");
            Doc.SetCellValue("A79", "Average Cross Flow Velocity Ratio");
            Doc.SetCellValue("A80", "Vibration Amplitude");
            Doc.SetCellValue("A81", "Vortex Shedding Ratio");
            Doc.SetCellValue("A82", "Turbulent Buffeting Ratio");
            Doc.SetCellValue("A83", "Acoustic Vibrations (gases)");
            Doc.SetCellValue("A84", "Acoustic Vibration Exists");
            Doc.SetCellValue("A85", "Vibration Exists");
            Doc.SetCellStyle("A72", "E85", BorderCellsStyle());
            Doc.SetCellStyle("A72", "A85", BoldTextStyle());
        }

        private static void AddHeatTransferData()
        {
            Doc.MergeWorksheetCells("C9", "D9");
            Doc.MergeWorksheetCells("E9", "F9");
            Doc.MergeWorksheetCells("C10", "D10");
            Doc.MergeWorksheetCells("E10", "F10");
            Doc.MergeWorksheetCells("C12", "D12");
            Doc.MergeWorksheetCells("E12", "F12");
            Doc.MergeWorksheetCells("C27", "D27");
            Doc.MergeWorksheetCells("E27", "F27");
            Doc.MergeWorksheetCells("C31", "D31");
            Doc.MergeWorksheetCells("E31", "F31");
            Doc.MergeWorksheetCells("C32", "D32");
            Doc.MergeWorksheetCells("E32", "F32");
            Doc.MergeWorksheetCells("C33", "D33");
            Doc.MergeWorksheetCells("E33", "F33");
            Doc.MergeWorksheetCells("C34", "D34");
            Doc.MergeWorksheetCells("E34", "F34");
            Doc.MergeWorksheetCells("C35", "F35");
            Doc.MergeWorksheetCells("C36", "F36");
            Doc.MergeWorksheetCells("C39", "F39");
            Doc.MergeWorksheetCells("C40", "F40");
            Doc.MergeWorksheetCells("C41", "F41");
            Doc.MergeWorksheetCells("C42", "F42");
            Doc.MergeWorksheetCells("C43", "F43");
            Doc.MergeWorksheetCells("C46", "F46");
            Doc.MergeWorksheetCells("C47", "F47");
            Doc.MergeWorksheetCells("C48", "F48");
            Doc.MergeWorksheetCells("A65", "D65");
            Doc.MergeWorksheetCells("A66", "D66");
            Doc.MergeWorksheetCells("A67", "D67");
            Doc.SetCellValue("C9", _overallCalculationViewModel.Overall.fluid_name_tube);
            Doc.SetCellValue("E9", _overallCalculationViewModel.Overall.fluid_name_shell);
            Doc.SetCellValue("C10", _overallCalculationViewModel.Overall.flow_tube);
            Doc.SetCellValue("E10", _overallCalculationViewModel.Overall.flow_shell);
            Doc.SetCellValue("C11", _overallCalculationViewModel.Overall.temperature_tube_inlet);
            Doc.SetCellValue("D11", _overallCalculationViewModel.Overall.temperature_tube_outlet);
            Doc.SetCellValue("E11", _overallCalculationViewModel.Overall.temperature_shell_inlet);
            Doc.SetCellValue("F11", _overallCalculationViewModel.Overall.temperature_shell_outlet);
            Doc.SetCellValue("C12", _overallCalculationViewModel.Overall.duty_tube);
            Doc.SetCellValue("E12", _overallCalculationViewModel.Overall.duty_shell);
            Doc.SetCellValue("C13", _overallCalculationViewModel.Overall.fluid_velocity_tube_inlet);
            Doc.SetCellValue("D13", _overallCalculationViewModel.Overall.fluid_velocity_tube_outlet);
            Doc.SetCellValue("E13", _overallCalculationViewModel.Overall.fluid_velocity_shell_inlet);
            Doc.SetCellValue("F13", _overallCalculationViewModel.Overall.fluid_velocity_shell_outlet);
            Doc.SetCellValue("C14", _overallCalculationViewModel.Overall.shear_rate_tube_inlet);
            Doc.SetCellValue("D14", _overallCalculationViewModel.Overall.shear_rate_tube_outlet);
            Doc.SetCellValue("E14", _overallCalculationViewModel.Overall.shear_rate_shell_inlet);
            Doc.SetCellValue("F14", _overallCalculationViewModel.Overall.shear_rate_shell_outlet);
            Doc.SetCellValue("C15", _overallCalculationViewModel.Overall.flow_type_tube_inlet);
            Doc.SetCellValue("D15", _overallCalculationViewModel.Overall.flow_type_tube_outlet);
            Doc.SetCellValue("E15", _overallCalculationViewModel.Overall.flow_type_shell_inlet);
            Doc.SetCellValue("F15", _overallCalculationViewModel.Overall.flow_type_shell_outlet);

            Doc.SetCellValue("C17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_inlet);
            Doc.SetCellValue("D17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_tube_outlet);
            Doc.SetCellValue("E17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet);
            Doc.SetCellValue("F17", _overallCalculationViewModel.Overall.liquid_phase_app_viscosity_shell_inlet);
            Doc.SetCellValue("C18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_tube_inlet);
            Doc.SetCellValue("D18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_tube_outlet);
            Doc.SetCellValue("E18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_shell_inlet);
            Doc.SetCellValue("F18", _overallCalculationViewModel.Overall.liquid_phase_reynolds_shell_outlet);
            Doc.SetCellValue("C19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_tube_inlet);
            Doc.SetCellValue("D19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_tube_outlet);
            Doc.SetCellValue("E19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_shell_inlet);
            Doc.SetCellValue("F19", _overallCalculationViewModel.Overall.liquid_phase_prandtl_shell_outlet);

            Doc.SetCellValue("C21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_tube_inlet);
            Doc.SetCellValue("D21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_tube_outlet);
            Doc.SetCellValue("E21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_shell_inlet);
            Doc.SetCellValue("F21", _overallCalculationViewModel.Overall.gas_phase_app_viscosity_shell_outlet);
            Doc.SetCellValue("C22", _overallCalculationViewModel.Overall.gas_phase_reynolds_tube_inlet);
            Doc.SetCellValue("D22", _overallCalculationViewModel.Overall.gas_phase_reynolds_tube_outlet);
            Doc.SetCellValue("E22", _overallCalculationViewModel.Overall.gas_phase_reynolds_shell_inlet);
            Doc.SetCellValue("F22", _overallCalculationViewModel.Overall.gas_phase_reynolds_shell_outlet);
            Doc.SetCellValue("C23", _overallCalculationViewModel.Overall.gas_phase_prandtl_tube_inlet);
            Doc.SetCellValue("D23", _overallCalculationViewModel.Overall.gas_phase_prandtl_tube_outlet);
            Doc.SetCellValue("E23", _overallCalculationViewModel.Overall.gas_phase_prandtl_shell_inlet);
            Doc.SetCellValue("F23", _overallCalculationViewModel.Overall.gas_phase_prandtl_shell_outlet);

            Doc.SetCellValue("C26", _overallCalculationViewModel.Overall.wall_temperature_tube_inlet);
            Doc.SetCellValue("D26", _overallCalculationViewModel.Overall.wall_temperature_tube_outlet);
            Doc.SetCellValue("E26", _overallCalculationViewModel.Overall.wall_temperature_shell_inlet);
            Doc.SetCellValue("F26", _overallCalculationViewModel.Overall.wall_temperature_shell_outlet);

            //TODO: Добавить свойство average metall temp

            Doc.SetCellValue("C28", _overallCalculationViewModel.Overall.wall_consistency_index_tube_inlet);
            Doc.SetCellValue("D28", _overallCalculationViewModel.Overall.wall_consistency_index_tube_outlet);
            Doc.SetCellValue("E28", _overallCalculationViewModel.Overall.wall_consistency_index_shell_inlet);
            Doc.SetCellValue("F28", _overallCalculationViewModel.Overall.wall_consistency_index_shell_inlet);
            Doc.SetCellValue("C29", _overallCalculationViewModel.Overall.nusselt_tube_inlet);
            Doc.SetCellValue("D29", _overallCalculationViewModel.Overall.nusselt_tube_outlet);
            Doc.SetCellValue("E29", _overallCalculationViewModel.Overall.nusselt_shell_inlet);
            Doc.SetCellValue("F29", _overallCalculationViewModel.Overall.nusselt_shell_outlet);
            Doc.SetCellValue("C30", _overallCalculationViewModel.Overall.k_side_tube_inlet);
            Doc.SetCellValue("D30", _overallCalculationViewModel.Overall.k_side_tube_outlet);
            Doc.SetCellValue("E30", _overallCalculationViewModel.Overall.k_side_shell_inlet);
            Doc.SetCellValue("F30", _overallCalculationViewModel.Overall.k_side_shell_outlet);
            Doc.SetCellValue("C31", _overallCalculationViewModel.Overall.fouling_factor_tube);
            Doc.SetCellValue("E31", _overallCalculationViewModel.Overall.fouling_factor_shell);
            Doc.SetCellValue("C32", "Inlet");
            Doc.SetCellValue("E32", "Outlet");
            Doc.SetCellValue("C33", _overallCalculationViewModel.Overall.k_unfouled_inlet);
            Doc.SetCellValue("E33", _overallCalculationViewModel.Overall.k_unfouled_outlet);
            Doc.SetCellValue("C34", _overallCalculationViewModel.Overall.k_fouled_inlet);
            Doc.SetCellValue("E34", _overallCalculationViewModel.Overall.k_fouled_outlet);
            Doc.SetCellValue("C35", _overallCalculationViewModel.Overall.k_global_fouled);
            Doc.SetCellValue("C36", _overallCalculationViewModel.Overall.k_effective);
            Doc.SetCellValue("C39", _overallCalculationViewModel.Overall.surface_area_required);
            Doc.SetCellValue("C40", _overallCalculationViewModel.Overall.area_module);
            Doc.SetCellValue("C41", _overallCalculationViewModel.Overall.nr_modules);
            Doc.SetCellValue("C42", _overallCalculationViewModel.Overall.area_fitted);
            Doc.SetCellValue("C43", _overallCalculationViewModel.Overall.excess_area);
            Doc.SetCellValue("C46", _overallCalculationViewModel.Overall.LMTD);
            Doc.SetCellValue("C47", _overallCalculationViewModel.Overall.LMTD_correction_factor);
            Doc.SetCellValue("C48", _overallCalculationViewModel.Overall.adjusted_LMTD);
            Doc.SetCellValue("C54", _overallCalculationViewModel.Overall.pressure_drop_tube_side_modules_V);
            Doc.SetCellValue("E54", _overallCalculationViewModel.Overall.pressure_drop_tube_side_modules_P);
            Doc.SetCellValue("C55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_V);
            Doc.SetCellValue("D55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_pV);
            Doc.SetCellValue("E55", _overallCalculationViewModel.Overall.pressure_drop_tube_side_inlet_nozzles_P);
            Doc.SetCellValue("C56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_V);
            Doc.SetCellValue("D56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_pV);
            Doc.SetCellValue("E56", _overallCalculationViewModel.Overall.pressure_drop_tube_side_outlet_nozzles_P);
            Doc.SetCellValue("C57", _overallCalculationViewModel.Overall.pressure_drop_tube_side_bends_V);
            Doc.SetCellValue("E57", _overallCalculationViewModel.Overall.pressure_drop_tube_side_bends_P);
            Doc.SetCellValue("E58", _overallCalculationViewModel.Overall.pressure_drop_tube_side_total_P);
            Doc.SetCellValue("C62", _overallCalculationViewModel.Overall.pressure_drop_shell_side_modules_V);
            Doc.SetCellValue("E62", _overallCalculationViewModel.Overall.pressure_drop_shell_side_modules_P);
            Doc.SetCellValue("C63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_V);
            Doc.SetCellValue("D63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_pV);
            Doc.SetCellValue("E63", _overallCalculationViewModel.Overall.pressure_drop_shell_side_inlet_nozzles_P);

            Doc.SetCellValue("C64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_V);
            Doc.SetCellValue("D64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_pV);
            Doc.SetCellValue("E64", _overallCalculationViewModel.Overall.pressure_drop_shell_side_outlet_nozzles_P);
            //Doc.SetCellValue("E65", _overallCalculationViewModel.Overall.win);
            Doc.SetCellValue("E68", _overallCalculationViewModel.Overall.pressure_drop_shell_side_total_p);
            Doc.SetCellValue("C72", _overallCalculationViewModel.Overall.vibrations_inlet_span_length);
            Doc.SetCellValue("D72", _overallCalculationViewModel.Overall.vibrations_central_span_length);
            Doc.SetCellValue("E72", _overallCalculationViewModel.Overall.vibrations_outlet_span_length);
            Doc.SetCellValue("C73", _overallCalculationViewModel.Overall.vibrations_inlet_span_length_tema_lb);
            Doc.SetCellValue("D73", _overallCalculationViewModel.Overall.vibrations_central_span_length_tema_lb);
            Doc.SetCellValue("E73", _overallCalculationViewModel.Overall.vibrations_outlet_span_length_tema_lb);
            Doc.SetCellValue("C74", _overallCalculationViewModel.Overall.vibrations_inlet_tubes_natural_frequency);
            Doc.SetCellValue("D74", _overallCalculationViewModel.Overall.vibrations_central_tubes_natural_frequency);
            Doc.SetCellValue("E74", _overallCalculationViewModel.Overall.vibrations_outlet_tubes_natural_frequency);
            Doc.SetCellValue("C75", _overallCalculationViewModel.Overall.vibrations_inlet_shell_acoustic_frequency_gases);
            Doc.SetCellValue("D75", _overallCalculationViewModel.Overall.vibrations_central_shell_acoustic_frequency_gases);
            Doc.SetCellValue("E75", _overallCalculationViewModel.Overall.vibrations_outlet_shell_acoustic_frequency_gases);

            Doc.SetCellValue("C77", _overallCalculationViewModel.Overall.vibrations_inlet_cross_flow_velocity);
            Doc.SetCellValue("D77", _overallCalculationViewModel.Overall.vibrations_central_cross_flow_velocity);
            Doc.SetCellValue("E77", _overallCalculationViewModel.Overall.vibrations_outlet_cross_flow_velocity);
            Doc.SetCellValue("C78", _overallCalculationViewModel.Overall.vibrations_inlet_cricical_velocity);
            Doc.SetCellValue("D78", _overallCalculationViewModel.Overall.vibrations_central_cricical_velocity);
            Doc.SetCellValue("E78", _overallCalculationViewModel.Overall.vibrations_outlet_cricical_velocity);
            Doc.SetCellValue("C79", _overallCalculationViewModel.Overall.vibrations_inlet_average_cross_flow_velocity_ratio);
            Doc.SetCellValue("D79", _overallCalculationViewModel.Overall.vibrations_central_average_cross_flow_velocity_ratio);
            Doc.SetCellValue("E79", _overallCalculationViewModel.Overall.vibrations_outlet_average_cross_flow_velocity_ratio);

            Doc.SetCellValue("C81", _overallCalculationViewModel.Overall.vibrations_inlet_vortex_shedding_ratio);
            Doc.SetCellValue("D81", _overallCalculationViewModel.Overall.vibrations_central_vortex_shedding_ratio);
            Doc.SetCellValue("E81", _overallCalculationViewModel.Overall.vibrations_outlet_vortex_shedding_ratio);
            Doc.SetCellValue("C82", _overallCalculationViewModel.Overall.vibrations_inlet_turbulent_buffeting_ratio);
            Doc.SetCellValue("D82", _overallCalculationViewModel.Overall.vibrations_central_turbulent_buffeting_ratio);
            Doc.SetCellValue("E82", _overallCalculationViewModel.Overall.vibrations_outlet_turbulent_buffeting_ratio);

            Doc.SetCellValue("C84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_inlet == 1 ? "Yes" : "No");
            Doc.SetCellValue("D84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_central == 1 ? "Yes" : "No");
            Doc.SetCellValue("E84", _overallCalculationViewModel.Overall.acoustic_vibration_exist_outlet == 1 ? "Yes" : "No");

        }

        #endregion

        #region TEMA SHEET
        private static void AddTemaSheetData()
        {
            Doc.AddWorksheet("TEMA Sheet");
            //Doc.SetCellStyle("A1", "Q62", BorderCellsStyle());
            Doc.SetColumnWidth("A", 4);
            Doc.SetColumnWidth("B", 20);
            Doc.SetColumnWidth("C", 4);
            Doc.SetColumnWidth("D", 11);
            Doc.SetColumnWidth("E", 11);
            Doc.SetColumnWidth("F", 5);
            Doc.SetColumnWidth("G", 2);
            Doc.SetColumnWidth("H", 5);
            Doc.SetColumnWidth("I", 5);
            Doc.SetColumnWidth("J", 2);
            Doc.SetColumnWidth("K", 5);
            Doc.SetColumnWidth("L", 5);
            Doc.SetColumnWidth("M", 2);
            Doc.SetColumnWidth("N", 5);
            Doc.SetColumnWidth("O", 5);
            Doc.SetColumnWidth("P", 2);
            Doc.SetColumnWidth("Q", 5);

            SetTemaHeader("A1", "Q1", "Customer and revision information");
            Doc.SetCellValue("A2", "1");
            Doc.MergeWorksheetCells("B2", "C2");
            Doc.SetCellValue("B2", "Customer");
            Doc.MergeWorksheetCells("D2", "K2");
            Doc.SetCellValue("D2", _projectPageViewModel.ProjectInfo.customer);

            Doc.SetCellValue("A3", "2");
            Doc.MergeWorksheetCells("B3", "C3");
            Doc.SetCellValue("B3", "Revision Nr");
            Doc.MergeWorksheetCells("D3", "K3");
            Doc.SetCellValue("D3", _projectPageViewModel.ProjectInfo.revision.ToString());

            Doc.SetCellValue("A4", "3");
            Doc.MergeWorksheetCells("B4", "C4");
            Doc.SetCellValue("B4", "Date");
            Doc.MergeWorksheetCells("D4", "K4");
            Doc.SetCellValue("D4", "");

            Doc.SetCellValue("A5", "4");
            Doc.MergeWorksheetCells("B5", "C5");
            Doc.SetCellValue("B5", "Author");
            Doc.MergeWorksheetCells("D5", "K5");
            Doc.SetCellValue("D5", _projectPageViewModel.ProjectInfo.name);

            Doc.MergeWorksheetCells("L2", "Q6");

            SetTemaHeader("A6", "K6", "Heat exchanger summary");
            Doc.SetCellValue("A7", "5");
            Doc.SetCellValue("B7", "Module");
            Doc.MergeWorksheetCells("C7", "D7");

            Doc.SetCellValue("C7", _geometryPageViewModel.Geometry.name);

            Doc.SetCellValue("A8", "6");
            Doc.SetCellValue("B8", "Area / Module");
            Doc.SetCellValue("C8", "m²");
            Doc.SetCellValue("D8", _geometryPageViewModel.Geometry.area_module);

            Doc.SetCellValue("E7", "Nr.");
            Doc.MergeWorksheetCells("F7", "H7");
            Doc.SetCellValue("F7", _geometryPageViewModel.Geometry.nr_baffles);

            Doc.SetCellValue("E8", "Total area");
            Doc.MergeWorksheetCells("F8", "H8");
            Doc.SetCellValue("F8", "m²");

            Doc.MergeWorksheetCells("I7", "N7");
            Doc.SetCellValue("I7", "Parallel Lines (Tubes/Shell)");

            Doc.MergeWorksheetCells("I8", "Q8");
            Doc.SetCellValue("I8", _overallCalculationViewModel.Overall.surface_area_required);

            SetTemaHeader("A9", "Q9", "Heat exchanger performance");
            Doc.MergeWorksheetCells("B10", "D10");
            Doc.MergeWorksheetCells("F10", "H10");
            Doc.SetCellValue("F10", "Tube In");
            Doc.MergeWorksheetCells("I10", "K10");
            Doc.SetCellValue("I10", "Tube Out");
            Doc.MergeWorksheetCells("L10", "N10");
            Doc.SetCellValue("L10", "Shell In");
            Doc.MergeWorksheetCells("O10", "Q10");
            Doc.SetCellValue("O10", "Shell Out");

            Doc.MergeWorksheetCells("B11", "D11");
            
            Doc.SetCellValue("B11", "Fluid Name");
            Doc.MergeWorksheetCells("F11", "K11");
            Doc.SetCellValue("F11", _tubesFluidViewModel.Product.name);
            Doc.MergeWorksheetCells("L11", "Q11");
            Doc.SetCellValue("L11", _shellFluidViewModel.Product.name);

            SetTemaHeader("A30", "Q30", "Construction data");
            Doc.MergeWorksheetCells("B31", "C31");
            Doc.SetCellValue("D31", "Shell side");
            Doc.SetCellValue("E31", "Tubes side");

            TemaUnits();
            TemaNames();
            TemaHBValues();

        }

        private static void TemaUnits()
        {
            Doc.SetCellValue("A10", "7");
            Doc.SetCellValue("A11", "8");
            Doc.SetCellValue("A12", "9");
            Doc.SetCellValue("A13", "10");
            Doc.SetCellValue("A14", "11");
            Doc.SetCellValue("A15", "12");
            Doc.SetCellValue("A16", "13");
            Doc.SetCellValue("A17", "14");
            Doc.SetCellValue("A18", "15");
            Doc.SetCellValue("A19", "16");
            Doc.SetCellValue("A20", "17");
            Doc.SetCellValue("A21", "18");
            Doc.SetCellValue("A22", "19");
            Doc.SetCellValue("A23", "20");
            Doc.SetCellValue("A24", "21");
            Doc.SetCellValue("A25", "22");
            Doc.SetCellValue("A26", "23");
            Doc.SetCellValue("A27", "24");
            Doc.SetCellValue("A28", "25");
            Doc.SetCellValue("A29", "26");

            Doc.SetCellValue("A31", "27");
            Doc.SetCellValue("A32", "28");
            Doc.SetCellValue("A33", "29");
            Doc.SetCellValue("A34", "30");
            Doc.SetCellValue("A35", "31");
            Doc.SetCellValue("A36", "32");
            Doc.SetCellValue("A37", "33");
            Doc.SetCellValue("A38", "34");
            Doc.SetCellValue("A39", "35");
            Doc.SetCellValue("A40", "36");
            Doc.SetCellValue("A41", "37");
            Doc.SetCellValue("A42", "38");
            Doc.SetCellValue("A43", "39");
            Doc.SetCellValue("A44", "40");
            Doc.SetCellValue("A45", "41");
            Doc.SetCellValue("A46", "42");
            Doc.SetCellValue("A47", "43");
            Doc.SetCellValue("A48", "44");
            Doc.SetCellValue("A49", "45");
            Doc.SetCellValue("A50", "46");
            Doc.SetCellValue("A51", "47");

            Doc.SetCellValue("E12", "kg/hr");
            Doc.SetCellValue("E13", "kg/hr");
            Doc.SetCellValue("E14", "kg/hr");
            Doc.SetCellValue("E15", "°C");
            Doc.SetCellValue("E16", "kg/m³");
            Doc.SetCellValue("E17", "kcal/kg·°C");
            Doc.SetCellValue("E18", "kcal/m·hr·°C");
            Doc.SetCellValue("E19", "cP");
            Doc.SetCellValue("E20", "kcal/kg");
            Doc.SetCellValue("E21", "bar-a");
            Doc.SetCellValue("E22", "m/s");
            Doc.SetCellValue("E23", "bar-a");
            Doc.SetCellValue("E24", "bar-a");
            Doc.SetCellValue("E25", "°C");
            Doc.SetCellValue("E26", "kcal/hr");
            Doc.SetCellValue("E27", "kcal/hr·m²·°C");
            Doc.SetCellValue("E28", "hr·m²·°C/kcal");
            Doc.SetCellValue("E29", "kcal/hr·m²·°C");

            Doc.SetCellValue("C32", "bar-a");
            Doc.SetCellValue("C33", "bar-a");
            Doc.SetCellValue("C34", "°C");
            Doc.SetCellValue("C36", "mm");
            Doc.SetCellValue("C37", "mm");
            Doc.SetCellValue("C38", "mm");
            Doc.SetCellValue("C40", "mm");
            Doc.SetCellValue("C41", "I");
            Doc.SetCellValue("C42", "mm");
            Doc.SetCellValue("C43", "mm");
            Doc.SetCellValue("C48", "%");
            Doc.SetCellValue("C49", "Y/N");
            Doc.SetCellValue("C50", "Y/N");

        }

        private static void TemaNames()
        {
            Doc.MergeWorksheetCells("B12", "D12");
            Doc.SetCellValue("B12", "Flow total");

            Doc.MergeWorksheetCells("B13", "D13");
            Doc.SetCellValue("B13", "Gas phase");

            Doc.MergeWorksheetCells("B14", "D14");
            Doc.SetCellValue("B14", "Liquid phase");

            Doc.MergeWorksheetCells("B15", "D15");
            Doc.SetCellValue("B15", "Temperature");

            Doc.MergeWorksheetCells("B16", "D16");
            Doc.SetCellValue("B16", "Density");

            Doc.MergeWorksheetCells("B17", "D17");
            Doc.SetCellValue("B17", "Specific heat");

            Doc.MergeWorksheetCells("B18", "D18");
            Doc.SetCellValue("B18", "Therm. Cond.");

            Doc.MergeWorksheetCells("B19", "D19");
            Doc.SetCellValue("B19", "Viscosity");

            Doc.MergeWorksheetCells("B20", "D20");
            Doc.SetCellValue("B20", "Latent heat");

            Doc.MergeWorksheetCells("B21", "D21");
            Doc.SetCellValue("B21", "Vapour pressure");

            Doc.MergeWorksheetCells("B22", "D22");
            Doc.SetCellValue("B22", "Velocity");

            Doc.MergeWorksheetCells("B23", "D23");
            Doc.SetCellValue("B23", "Max pressure drop");

            Doc.MergeWorksheetCells("B24", "D24");
            Doc.SetCellValue("B24", "Calculated pressure drop");

            Doc.MergeWorksheetCells("B25", "D25");
            Doc.SetCellValue("B25", "LMTD corrected");

            Doc.MergeWorksheetCells("B26", "D26");
            Doc.SetCellValue("B26", "Duty");

            Doc.MergeWorksheetCells("B27", "D27");
            Doc.SetCellValue("B27", "Global fouled coefficient");

            Doc.MergeWorksheetCells("B28", "D28");
            Doc.SetCellValue("B28", "Fouling factor");

            Doc.MergeWorksheetCells("B29", "D29");
            Doc.SetCellValue("B29", "Effective heat transfer coefficient");

            Doc.SetCellValue("B32", "Design pressure");
            Doc.SetCellValue("B33", "Test pressure");
            Doc.SetCellValue("B34", "Design temperature");
            Doc.MergeWorksheetCells("B35", "C35");
            Doc.SetCellValue("B35", "Nr. Passes");
            Doc.SetCellValue("B36", "Corrosion Allowance");
            Doc.SetCellValue("B37", "OD");
            Doc.SetCellValue("B38", "Thickness");
            Doc.MergeWorksheetCells("B39", "C39");
            Doc.SetCellValue("B39", "Nr. Tubes");
            Doc.SetCellValue("B40", "Tube length");
            Doc.SetCellValue("B41", "Volume");
            Doc.SetCellValue("B42", "Inlet connection (OD)");
            Doc.SetCellValue("B43", "Outlet connection (OD)");
            Doc.MergeWorksheetCells("B44", "C44");
            Doc.SetCellValue("B44", "Shell nozzle orientation");
            Doc.MergeWorksheetCells("B45", "C45");
            Doc.SetCellValue("B45", "Construction material");
            Doc.MergeWorksheetCells("B46", "C46");
            Doc.SetCellValue("B46", "Gasket material");
            Doc.MergeWorksheetCells("B47", "C47");
            Doc.SetCellValue("B47", "Nr. Baffles");
            Doc.SetCellValue("B48", "Baffle cut (% ID)");
            Doc.SetCellValue("B49", "Impingement protection");
            Doc.SetCellValue("B50", "Expansion joint fitted");
            Doc.MergeWorksheetCells("B51", "C51");
            Doc.SetCellValue("B51", "TEMA class");
            Doc.MergeWorksheetCells("F31", "Q51");
        }

        private static void TemaHBValues()
        {
            Doc.MergeWorksheetCells("F12", "K12");
            Doc.SetCellValue("F12", _heatBalanceViewModel.Calculation.flow_tube);
            Doc.MergeWorksheetCells("L12", "Q12");
            Doc.SetCellValue("L12", _heatBalanceViewModel.Calculation.flow_shell);

            Doc.MergeWorksheetCells("F13", "H13");
            Doc.SetCellValue("F13", _heatBalanceViewModel.Calculation.gas_density_tube_inlet);
            Doc.MergeWorksheetCells("I13", "K13");
            Doc.SetCellValue("I13", _heatBalanceViewModel.Calculation.gas_density_tube_outlet);
            Doc.MergeWorksheetCells("L13", "N13");
            Doc.SetCellValue("L13", _heatBalanceViewModel.Calculation.gas_density_shell_inlet);
            Doc.MergeWorksheetCells("O13", "Q13");
            Doc.SetCellValue("O13", _heatBalanceViewModel.Calculation.gas_density_shell_outlet);

            Doc.MergeWorksheetCells("F14", "H14");
            Doc.SetCellValue("F14", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet);
            Doc.MergeWorksheetCells("I14", "K14");
            Doc.SetCellValue("I14", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet);
            Doc.MergeWorksheetCells("L14", "N14");
            Doc.SetCellValue("L14", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet);
            Doc.MergeWorksheetCells("O14", "Q14");
            Doc.SetCellValue("O14", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet);

            Doc.MergeWorksheetCells("F15", "H15");
            Doc.SetCellValue("F15", _heatBalanceViewModel.Calculation.temperature_tube_inlet);
            Doc.MergeWorksheetCells("I15", "K15");
            Doc.SetCellValue("I15", _heatBalanceViewModel.Calculation.temperature_tube_outlet);
            Doc.MergeWorksheetCells("L15", "N15");
            Doc.SetCellValue("L15", _heatBalanceViewModel.Calculation.temperature_shell_inlet);
            Doc.MergeWorksheetCells("O15", "Q15");
            Doc.SetCellValue("O15", _heatBalanceViewModel.Calculation.temperature_shell_outlet);

            Doc.MergeWorksheetCells("F16", "H16");
            Doc.SetCellValue("F16", _heatBalanceViewModel.Calculation.liquid_density_tube_inlet);
            Doc.MergeWorksheetCells("I16", "K16");
            Doc.SetCellValue("I16", _heatBalanceViewModel.Calculation.liquid_density_tube_outlet);
            Doc.MergeWorksheetCells("L16", "N16");
            Doc.SetCellValue("L16", _heatBalanceViewModel.Calculation.liquid_density_shell_inlet);
            Doc.MergeWorksheetCells("O16", "Q16");
            Doc.SetCellValue("O16", _heatBalanceViewModel.Calculation.liquid_density_shell_outlet);

            Doc.MergeWorksheetCells("F17", "H17");
            Doc.SetCellValue("F17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_inlet);
            Doc.MergeWorksheetCells("I17", "K17");
            Doc.SetCellValue("I17", _heatBalanceViewModel.Calculation.liquid_specific_heat_tube_outlet);
            Doc.MergeWorksheetCells("L17", "N17");
            Doc.SetCellValue("L17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_inlet);
            Doc.MergeWorksheetCells("O17", "Q17");
            Doc.SetCellValue("O17", _heatBalanceViewModel.Calculation.liquid_specific_heat_shell_outlet);

            Doc.MergeWorksheetCells("F18", "H18");
            Doc.SetCellValue("F18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_inlet);
            Doc.MergeWorksheetCells("I18", "K18");
            Doc.SetCellValue("I18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_tube_outlet);
            Doc.MergeWorksheetCells("L18", "N18");
            Doc.SetCellValue("L18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_inlet);
            Doc.MergeWorksheetCells("O18", "Q18");
            Doc.SetCellValue("O18", _heatBalanceViewModel.Calculation.liquid_thermal_conductivity_shell_outlet);

            Doc.MergeWorksheetCells("F19", "H19");
            Doc.SetCellValue("F19", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_inlet);
            Doc.MergeWorksheetCells("I19", "K19");
            Doc.SetCellValue("I19", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_tube_outlet);
            Doc.MergeWorksheetCells("L19", "N19");
            Doc.SetCellValue("L19", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_inlet);
            Doc.MergeWorksheetCells("O19", "Q19");
            Doc.SetCellValue("O19", _heatBalanceViewModel.Calculation.gas_dynamic_viscosity_shell_outlet);

            Doc.MergeWorksheetCells("F20", "H20");
            Doc.SetCellValue("F20", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_inlet);
            Doc.MergeWorksheetCells("I20", "K20");
            Doc.SetCellValue("I20", _heatBalanceViewModel.Calculation.liquid_latent_heat_tube_outlet);
            Doc.MergeWorksheetCells("L20", "N20");
            Doc.SetCellValue("L20", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_inlet);
            Doc.MergeWorksheetCells("O20", "Q20");
            Doc.SetCellValue("O20", _heatBalanceViewModel.Calculation.liquid_latent_heat_shell_outlet);

            Doc.MergeWorksheetCells("F21", "H21");
            Doc.SetCellValue("F21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_inlet);
            Doc.MergeWorksheetCells("I21", "K21");
            Doc.SetCellValue("I21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_tube_outlet);
            Doc.MergeWorksheetCells("L21", "N21");
            Doc.SetCellValue("L21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_inlet);
            Doc.MergeWorksheetCells("O21", "Q21");
            Doc.SetCellValue("O21", _heatBalanceViewModel.Calculation.gas_vapour_pressure_shell_outlet);

            Doc.MergeWorksheetCells("F22", "H22");
            Doc.SetCellValue("F22", _overallCalculationViewModel.Overall.fluid_velocity_tube_inlet);
            Doc.MergeWorksheetCells("I22", "K22");
            Doc.SetCellValue("I22", _overallCalculationViewModel.Overall.fluid_velocity_tube_outlet);
            Doc.MergeWorksheetCells("L22", "N22");
            Doc.SetCellValue("L22", _overallCalculationViewModel.Overall.fluid_velocity_shell_inlet);
            Doc.MergeWorksheetCells("O22", "Q22");
            Doc.SetCellValue("O22", _overallCalculationViewModel.Overall.fluid_velocity_shell_outlet);

            Doc.MergeWorksheetCells("F23", "K23");
            //Doc.SetCellValue("F23", _overallCalculationViewModel.Overall.Flulinf);
            Doc.MergeWorksheetCells("J23", "Q23");

            Doc.MergeWorksheetCells("F24", "K24");
            //Doc.SetCellValue("F23", _overallCalculationViewModel.Overall.Flulinf);
            Doc.MergeWorksheetCells("J24", "Q24");

            Doc.MergeWorksheetCells("F25", "Q25");
            Doc.SetCellValue("F25", _overallCalculationViewModel.Overall.LMTD);

            Doc.MergeWorksheetCells("F26", "Q26");
            Doc.SetCellValue("F26", _overallCalculationViewModel.Overall.duty_tube);

            Doc.MergeWorksheetCells("F27", "Q27");
            Doc.SetCellValue("F27", _overallCalculationViewModel.Overall.k_global_fouled);

            Doc.MergeWorksheetCells("F28", "K28");
            Doc.SetCellValue("F28", _overallCalculationViewModel.Overall.fouling_factor_tube);

            Doc.MergeWorksheetCells("L28", "Q28");
            Doc.SetCellValue("L28", _overallCalculationViewModel.Overall.fouling_factor_shell);

            Doc.MergeWorksheetCells("F29", "Q28");
            Doc.SetCellValue("F29", _overallCalculationViewModel.Overall.k_effective);
            Doc.SetCellValue("D35", _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);
            Doc.SetCellValue("E35", _geometryPageViewModel.Geometry.tube_plate_layout_number_of_passes);
            Doc.SetCellValue("D37", _geometryPageViewModel.Geometry.outer_diameter_shell_side);
            Doc.SetCellValue("E37", _geometryPageViewModel.Geometry.outer_diameter_tubes_side);
            Doc.SetCellValue("D38", _geometryPageViewModel.Geometry.thickness_shell_side);
            Doc.SetCellValue("E38", _geometryPageViewModel.Geometry.thickness_tubes_side);
            Doc.SetCellValue("E39", _geometryPageViewModel.Geometry.tube_plate_layout_max_nr_tubes);
            Doc.SetCellValue("E40", _geometryPageViewModel.Geometry.tube_inner_length);

            Doc.SetCellValue("D41", _geometryPageViewModel.Geometry.volume_module_shell_side);
            Doc.SetCellValue("E41", _geometryPageViewModel.Geometry.volume_module_tubes_side);
            //Doc.SetCellValue("D42", _geometryPageViewModel.Geometry.);
            //Doc.SetCellValue("E42", _geometryPageViewModel.Geometry.volume_module_tubes_side);

            SLPicture pic = new SLPicture(@$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Apora\\geometry_image.png");
            pic.ResizeInPixels(300, 300);
            pic.SetPosition(30, 6);
            Doc.InsertPicture(pic);
        }


        private static void SetTemaHeader(string start, string end, string text)

        {
            Doc.MergeWorksheetCells(start, end);
            Doc.SetCellStyle(start, BoldTemaHeaderTextStyle());
            Doc.SetCellValue(start, text);
            

        }

        #endregion

    }
}
